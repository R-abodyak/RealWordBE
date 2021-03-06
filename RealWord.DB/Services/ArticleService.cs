using AutoMapper;
using RealWord.DB.Entities;
using RealWord.DB.Extension;
using RealWord.DB.Models.RequestDtos;
using RealWord.DB.Models.ResponseDtos;
using RealWord.DB.Repositories;
using RealWordBE.Authentication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RealWord.DB.Services
{
    public class ArticleService:BaseRepository, IArticleService
    {
        private readonly ILikeRepository _likeRepository;
        private readonly IMapper _mapper;
        private readonly IArticleRebository _articleRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IArticleTagRebository _articleTagRepository;
        private readonly IUserRepository _userRepository;

        public ArticleService(ApplicationDbContext applicationDbContext ,IArticleRebository articleRebository ,
            ITagRepository tagRepository ,ILikeRepository likeRepository ,
            IArticleTagRebository articleTagRepository ,IUserRepository userRepository ,IMapper mapper) : base(applicationDbContext)
        {
            _articleRepository = articleRebository;
            _tagRepository = tagRepository;
            _likeRepository = likeRepository;
            _articleTagRepository = articleTagRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }



        public async Task<Status> CreateArticleWithTag(Article article ,List<Tag> tags)
        {

            using var transaction =
                   await _context.Database.BeginTransactionAsync();

            await _articleRepository.AddArticle(article);

            await _tagRepository.AddTags(tags);
            await SaveChangesAsync();

            await _articleTagRepository.AddTagsToArticle(article.Slug ,tags);
            await SaveChangesAsync();
            await transaction.CommitAsync();
            return Status.Completed;

            //catch( Exception )
            //{
            //    return Status.Invalid;
            //}

        }
        public async Task<ArticleResponseDto> GetAricleResponseAsync(IProfileService profileService ,Article articleDB ,String userId ,String CurrentUserName)
        {

            if( articleDB == null || articleDB.Slug == null ) return null;
            var articleResponseDto = _mapper.Map<ArticleResponseDto>(articleDB);

            articleResponseDto.CreatedDate = _articleRepository.GetCreatedDate(articleDB.Slug).ToUniversalIso8601();
            articleResponseDto.UpdatedDate = _articleRepository.GetUpdatedDate(articleDB.Slug).ToUniversalIso8601();
            articleResponseDto.Favorited = _likeRepository.IsArticleLikedByUser(articleDB.ArticleId ,userId);
            articleResponseDto.FavoritesCount = _likeRepository.CountLikes(articleDB.ArticleId ,userId);
            var Tags = _articleTagRepository.GetTagsOfArticle(articleDB.Slug);
            List<string> tagsNames = new List<string>();
            foreach( var tag in Tags ) { tagsNames.Add(tag.Name); }
            articleResponseDto.TagList = tagsNames;
            var AuthoruserName = _articleRepository.GetAuthorofArticle(articleDB.Slug).UserName;
            articleResponseDto.Author = await profileService.GetProfileAsync(CurrentUserName ,AuthoruserName);


            return articleResponseDto;



        }
        public async Task UpdateArticle(string slug ,ArticleForUpdateDto UpdatedArticle)
        {
            var articleDB = _articleRepository.GetArticleBySlug(slug);
            if( articleDB == null ) return;

            _mapper.Map<ArticleForUpdateDto ,Article>(UpdatedArticle ,articleDB);
            if( UpdatedArticle.Title != null ) articleDB.Slug = articleDB.Title.Replace(" " ,"_");
            await SaveChangesAsync();

        }
        public async Task DeleteArticle(string slug)
        {
            var articleDB = _articleRepository.GetArticleBySlug(slug);
            if( articleDB == null ) return;
            _articleRepository.DeleteArticle(articleDB);
            await SaveChangesAsync();

        }
        public bool IsArticleAuthor(String slug ,string currentUserId)
        {
            User Author = _articleRepository.GetAuthorofArticle(slug);
            if( Author == null ) return false;
            return Author.Id == currentUserId ? true : false;
        }
        public Article GetArticle(string slug)
        {

            var articleDB = _articleRepository.GetArticleBySlug(slug);
            return articleDB;

        }


        public async Task<IEnumerable<Article>> ListArticlesWithFilters(int limit ,int offset ,string tag ,string favorited ,string author)
        {
            try
            {
                Tag tag1;
                if( limit == 0 ) limit = 20;
                int tagId = 0; User Author, favoritedUser; string favoritedUserId = null; string authorId = null;
                if( tag != null ) { tag1 = await _tagRepository.GetTagByName(tag); if( tag1 != null ) tagId = tag1.TagId; }
                if( author != null )
                {
                    Author = await _userRepository.GetUserByUsernameAsync(author);
                    if( Author != null ) authorId = Author.Id;
                }
                if( favorited != null )
                {
                    favoritedUser = await _userRepository.GetUserByUsernameAsync(favorited);
                    favoritedUserId = favoritedUser.Id;
                }

                var result = _articleRepository.ListArticlesWithFilters(limit ,offset ,tagId ,favoritedUserId ,authorId);
                return result;
            }
            catch( Exception ) { return null; }

        }
        public IEnumerable<Article> FeedArticles(IFollowerRepository followerRepository ,string userId ,int limit = 20 ,int offset = 0)
        {
            var followers = followerRepository.GetFollowers(userId);
            var result = followerRepository.GetArticlesOfFolowers(followers ,limit = 20 ,offset = 0);
            return result;

        }
    }
}

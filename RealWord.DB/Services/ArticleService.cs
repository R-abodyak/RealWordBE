using AutoMapper;
using RealWord.DB.Entities;
using RealWord.DB.Models.RequestDtos;
using RealWord.DB.Models.ResponseDtos;
using RealWord.DB.Repositories;
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


        public ArticleService(ApplicationDbContext applicationDbContext ,IArticleRebository articleRebository ,ITagRepository tagRepository ,ILikeRepository likeRepository ,IArticleTagRebository articleTagRepository ,IMapper mapper) : base(applicationDbContext)
        {
            _articleRepository = articleRebository;
            _tagRepository = tagRepository;
            _likeRepository = likeRepository;
            _articleTagRepository = articleTagRepository;
            _mapper = mapper;
        }



        public async Task CreateArticleWithTag(Article article ,List<Tag> tags)
        {
            using var transaction =
                   await _context.Database.BeginTransactionAsync();
            await _articleRepository.AddArticle(article);

            await _tagRepository.AddTags(tags);
            await SaveChangesAsync();

            await _articleTagRepository.AddTagsToArticle(article.Slug ,tags);
            await SaveChangesAsync();
            await transaction.CommitAsync();

        }
        public async Task<ArticleResponseDto> GetAricleResponseAsync(IProfileService profileService ,string slug ,String userId ,String CurrentUserName)
        {
            var articleDB = _articleRepository.GetArticleBySlug(slug);
            if( articleDB == null ) return null;
            var articleResponseDto = _mapper.Map<ArticleResponseDto>(articleDB);

            articleResponseDto.CreatedDate = _articleRepository.GetCreatedDate(articleDB.Slug);
            articleResponseDto.UpdatedDate = _articleRepository.GetUpdatedDate(articleDB.Slug);
            articleResponseDto.Favorited = _likeRepository.IsArticleLikedByUser(articleDB.ArticleId ,userId);
            articleResponseDto.FavoritesCount = _likeRepository.CountLikes(articleDB.ArticleId ,userId);
            var Tags = _articleTagRepository.GetTagsOfArticle(slug);
            List<string> tagsNames = new List<string>();
            foreach( var tag in Tags ) { tagsNames.Add(tag.Name); }
            articleResponseDto.TagList = tagsNames;
            var AuthoruserName = _articleRepository.GetAuthorofArticle(slug).UserName;
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
        public bool IsValidSlug(string slug)
        {

            var articleDB = _articleRepository.GetArticleBySlug(slug);
            if( articleDB == null ) return false;
            return true;

        }


    }
}

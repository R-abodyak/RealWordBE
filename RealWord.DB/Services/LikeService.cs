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
    public class LikeService:BaseRepository, ILikeService
    {

        private readonly ILikeRepository _likeRepository;
        private readonly IMapper _mapper;
        private readonly IArticleRebository _articleRepository;


        public LikeService(ApplicationDbContext applicationDbContext ,IArticleRebository articleRebository ,ITagRepository tagRepository ,ILikeRepository likeRepository ,IArticleTagRebository articleTagRepository ,IMapper mapper) : base(applicationDbContext)
        {
            _articleRepository = articleRebository;

            _likeRepository = likeRepository;

            _mapper = mapper;
        }

        public async Task<Status> CreateLikeAsync(string slug ,string UserId)
        {
            var article = _articleRepository.GetArticleBySlug(slug);
            if( article == null ) return Status.Invalid;
            var isLiked = _likeRepository.IsArticleLikedByUser(article.ArticleId ,UserId);
            if( isLiked ) return Status.Duplicate;
            _likeRepository.CreateLike(article.ArticleId ,UserId);
            await SaveChangesAsync();
            return Status.Completed;

        }
        public async Task<Status> DeleteLikeAsync(string slug ,string UserId)
        {
            var article = _articleRepository.GetArticleBySlug(slug);
            if( article == null ) return Status.Invalid;
            var isLiked = _likeRepository.IsArticleLikedByUser(article.ArticleId ,UserId);
            if( !isLiked ) return Status.Duplicate;
            _likeRepository.DeleteLike(article.ArticleId ,UserId);
            await SaveChangesAsync();
            return Status.Completed;

        }
    }
}

﻿using AutoMapper;
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
    public class ArticleService:BaseRepository
    {
        private readonly ILikeRepository _likeRepository;
        private readonly IMapper _mapper;
        private IArticleRebository _articleRepository { get; set; }
        private ITagRepository _tagRepository { get; set; }
        public ArticleService(ApplicationDbContext applicationDbContext ,IArticleRebository articleRebository ,ITagRepository tagRepository ,ILikeRepository likeRepository ,IMapper mapper) : base(applicationDbContext)
        {
            _articleRepository = articleRebository;
            _tagRepository = tagRepository;
            _likeRepository = likeRepository;
            _mapper = mapper;
        }



        public async Task CreateArticleWithTag(Article article ,List<Tag> tags)
        {
            using var transaction =
                   await _context.Database.BeginTransactionAsync();
            await _articleRepository.AddArticle(article);
            await SaveChangesAsync();
            await _tagRepository.AddTags(tags);
            await SaveChangesAsync();

            await _articleRepository.AddTagsToArticle(article.Slug ,tags);
            await SaveChangesAsync();
            await transaction.CommitAsync();

        }
        public ArticleResponseDto GetAricleResponse(string slug ,String userId)
        {
            var articleDB = _articleRepository.GetArticleBySlug(slug);
            if( articleDB == null ) return null;
            var articleResponseDto = _mapper.Map<ArticleResponseDto>(articleDB);

            articleResponseDto.CreatedDate = _articleRepository.GetCreatedDate(articleDB.Slug);
            articleResponseDto.UpdatedDate = _articleRepository.GetUpdatedDate(articleDB.Slug);
            articleResponseDto.Favorited = _likeRepository.IsArticleLikedByUser(articleDB.ArticleId ,userId);
            articleResponseDto.FavoritesCount = _likeRepository.CountLikes(articleDB.ArticleId ,userId);
            var Tags = _tagRepository.GetTagsOfArticle(slug);
            List<string> tagsNames = new List<string>();
            foreach( var tag in Tags ) { tagsNames.Add(tag.Name); }
            articleResponseDto.TagList = tagsNames;
            articleResponseDto.Author = new ProfileResponseDto()
            {
                UserName = _articleRepository.GetAuthorofArticle(slug).UserName
            };

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

        public bool HasPermissionToUpdate(String slug ,string currentUserId)
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

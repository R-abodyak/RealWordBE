using Microsoft.AspNetCore.Mvc;
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
    public interface IArticleService
    {
        Task<Status> CreateArticleWithTag(Article article ,List<Tag> tags);
        Task<ArticleResponseDto> GetAricleResponseAsync([FromServices] IProfileService profileService ,Article article ,String userId ,string CurrentUserName);
        Task UpdateArticle(string slug ,ArticleForUpdateDto UpdatedArticle);
        Task DeleteArticle(string slug);
        bool IsArticleAuthor(String slug ,string currentUserId);
        Article GetArticle(string slug);
        Task<IEnumerable<Article>> ListArticlesWithFilters(int limit ,int offset ,string tag ,string favorited ,string author);
        IEnumerable<Article> FeedArticles([FromServices] IFollowerRepository followerRepository ,string userId ,int limit ,int offset);
    }
}

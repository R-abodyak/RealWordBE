using Microsoft.AspNetCore.Mvc;
using RealWord.DB.Entities;
using RealWord.DB.Models.RequestDtos;
using RealWord.DB.Models.ResponseDtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RealWord.DB.Services
{
    public interface IArticleService
    {
        Task CreateArticleWithTag(Article article ,List<Tag> tags);
        Task<ArticleResponseDto> GetAricleResponseAsync([FromServices] IProfileService profileService ,string slug ,String userId ,string CurrentUserName);
        Task UpdateArticle(string slug ,ArticleForUpdateDto UpdatedArticle);
        Task DeleteArticle(string slug);
        bool IsArticleAuthor(String slug ,string currentUserId);
        bool IsValidSlug(string slug);
        Task<IEnumerable<Article>> ListArticlesWithFilters(int limit ,int offset ,string tag ,string favorited ,string author);
    }
}

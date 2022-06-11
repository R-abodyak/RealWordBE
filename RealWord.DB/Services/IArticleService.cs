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
        ArticleResponseDto GetAricleResponse(string slug ,String userId);
        Task UpdateArticle(string slug ,ArticleForUpdateDto UpdatedArticle);
        Task DeleteArticle(string slug);
        bool IsArticleAuthor(String slug ,string currentUserId);
        bool IsValidSlug(string slug);
    }
}

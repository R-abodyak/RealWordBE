using RealWord.DB.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealWord.DB.Repositories
{
    public interface IArticleRebository
    {
        Task AddArticle(Article article);
        Task AddTagsToArticle(Article article ,List<Tag> tagList);
        Article GetArticleBySlug(string title);
        Task SaveChangesAsync();

    }
}
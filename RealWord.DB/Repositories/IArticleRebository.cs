using RealWord.DB.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealWord.DB.Repositories
{
    public interface IArticleRebository
    {
        Task AddArticle(Article article);
        Task AddTags(List<Tag> tagList);
        Task AddTagsToArticle(Article article ,List<Tag> tagList);
        Article GetArticleByTitle(string title);
        Task SaveChangesAsync();

    }
}
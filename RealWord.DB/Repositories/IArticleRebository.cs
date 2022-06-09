using RealWord.DB.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealWord.DB.Repositories
{
    public interface IArticleRebository
    {
        Task CreateArticle(Article article ,List<Tag> tagList);
        Task SaveChangesAsync();

    }
}
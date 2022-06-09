using RealWord.DB.Entities;
using System.Collections.Generic;

namespace RealWord.DB.Repositories
{
    public interface IArticleRebository
    {
        void CreateArticle(Article article ,List<Tag> tagList)
    }
}
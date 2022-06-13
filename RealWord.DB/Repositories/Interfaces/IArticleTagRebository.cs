using RealWord.DB.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RealWord.DB.Repositories
{
    public interface IArticleTagRebository
    {
        Task AddTagsToArticle(string slug ,List<Tag> tagList);
        IEnumerable<Tag> GetTagsOfArticle(string slug);


    }
}

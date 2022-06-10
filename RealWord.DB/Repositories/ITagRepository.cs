using RealWord.DB.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RealWord.DB.Repositories
{
    public interface ITagRepository
    {
        Task AddTags(List<Tag> tagList);
        IEnumerable<Tag> GetTagsOfArticle(string slug);


    }
}

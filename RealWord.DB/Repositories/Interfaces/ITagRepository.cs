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
        Task<List<Tag>> GetAllTagsAsync();
        Task<Tag> GetTagByName(string name);


    }
}

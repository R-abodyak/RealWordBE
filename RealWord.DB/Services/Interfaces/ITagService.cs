using RealWord.DB.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RealWord.DB.Services
{
    public interface ITagService
    {
        Task<List<Tag>> GetAllTagsAsync();
    }
}

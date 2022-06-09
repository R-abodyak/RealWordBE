using System;
using System.Collections.Generic;
using System.Text;

namespace RealWord.DB.Repositories
{
    public interface ITagRepository
    {
        Task AddTags(List<Tag> tagList);


    }
}

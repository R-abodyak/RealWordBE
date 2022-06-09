using RealWord.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealWord.DB.Repositories
{
    public class TagRepository:BaseRepository, ITagRepository
    {
        public TagRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
        public async Task AddTags(List<Tag> tagList)
        {
            foreach( var tag in tagList )
            {
                var tagDB = _context.Tags.Where(t => t.Name == tag.Name).FirstOrDefault();
                if( tagDB == null )
                    await _context.Tags.AddAsync(tag);
            }

        }
    }
}

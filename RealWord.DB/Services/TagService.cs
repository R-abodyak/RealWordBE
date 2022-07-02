using RealWord.DB.Entities;
using RealWord.DB.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RealWord.DB.Services
{
    public class TagService:BaseRepository, ITagService
    {
        private readonly ITagRepository _tagRepository;

        public TagService(ApplicationDbContext applicationDbContext ,ITagRepository tagRepository) : base(applicationDbContext)
        {
            _tagRepository = tagRepository;
        }

        public async Task<List<Tag>> GetAllTagsAsync()
        {
            var result = await _tagRepository.GetAllTagsAsync();
            await SaveChangesAsync();
            return result;
        }
    }
}

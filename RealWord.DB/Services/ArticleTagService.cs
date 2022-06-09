using RealWord.DB.Entities;
using RealWord.DB.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RealWord.DB.Services
{
    public class ArticleTagService:BaseRepository
    {
        private IArticleRebository _articleRepository { get; set; }
        private ITagRepository _tagRepository { get; set; }
        public ArticleTagService(ApplicationDbContext applicationDbContext ,IArticleRebository articleRebository ,ITagRepository tagRepository) : base(applicationDbContext)
        {
            _articleRepository = articleRebository;
            _tagRepository = tagRepository;
        }



        public async Task CreateArticleWithTag(Article article ,List<Tag> tags)
        {
            using var transaction =
                   await _context.Database.BeginTransactionAsync();
            await _articleRepository.AddArticle(article);
            await _articleRepository.SaveChangesAsync();
            await _tagRepository.AddTags(tags);
            await _articleRepository.SaveChangesAsync();

            await _articleRepository.AddTagsToArticle(article ,tags);
            await _articleRepository.SaveChangesAsync();
            await transaction.CommitAsync();

        }

    }
}

using RealWord.DB.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RealWord.DB.Repositories
{
    public class ArticleTagService:BaseRepository
    {

        public ArticleTagService(ApplicationDbContext applicationDbContext ,IArticleRebository articleRebository) : base(applicationDbContext)
        {
            _articleRepository = articleRebository;

        }

        private IArticleRebository _articleRepository { get; set; }


        public async Task CreateArticleWithTag(Article article ,List<Tag> tags)
        {
            using var transaction =
                   await _context.Database.BeginTransactionAsync();
            await _articleRepository.AddArticle(article);
            await _articleRepository.SaveChangesAsync();
            await _articleRepository.AddTags(tags);
            await _articleRepository.SaveChangesAsync();

            await _articleRepository.AddTagsToArticle(article ,tags);
            await _articleRepository.SaveChangesAsync();
            await transaction.CommitAsync();

        }

    }
}

using RealWord.DB.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RealWord.DB.Repositories
{
    public class ArticleRebository:BaseRepository, IArticleRebository
    {
        public ArticleRebository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }

        public async Task CreateArticle(Article article ,List<Tag> tagList)
        {
            var result = await _context.Articles.AddAsync(article);
            var JoinList = new List<ArticleTag>();
            foreach( Tag tag in tagList )
            {
                var tagDB = await _context.Tags.FindAsync(tag.TagId);
                if( tagDB == null ) _context.Tags.Add(tag);
                JoinList.Add(new ArticleTag() { Article = article ,Tag = tag });
            }
            // resul


        }


    }
}

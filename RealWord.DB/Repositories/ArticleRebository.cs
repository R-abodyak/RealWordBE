using RealWord.DB.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealWord.DB.Repositories
{
    public class ArticleRebository:BaseRepository, IArticleRebository
    {
        public ArticleRebository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }

        public void CreateArticle(Article article ,List<Tag> tagList)
        {
            _context.Articles.Add(article);
            var JoinList = new List<ArticleTag>();
            foreach( Tag tag in tagList )
            {
                var tagDB = _context.Tags.FindAsync(tag.TagId);
                if( tagDB == null ) _context.Tags.Add(tag);
                JoinList.Add(new ArticleTag() { Article = article ,Tag = tag });
            }


        }
    }
}

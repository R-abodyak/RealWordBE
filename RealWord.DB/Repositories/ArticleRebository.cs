using RealWord.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealWord.DB.Repositories
{
    public class ArticleRebository:BaseRepository, IArticleRebository
    {
        public ArticleRebository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }

        public async Task AddArticle(Article article)
        {
            var result = await _context.Articles.AddAsync(article);


        }

        public async Task AddTagsToArticle(Article article ,List<Tag> tagList)
        {
            article = GetArticleByTitle(article.Title);
            var JoinList = new List<ArticleTag>();
            foreach( var tag in tagList )
            {
                var tagg = _context.Tags.Where(t => t.Name == tag.Name).FirstOrDefault();
                JoinList.Add(new ArticleTag() { Article = article ,Tag = tagg });
            };
            //}
            await _context.AddRangeAsync(JoinList);
        }

        public Article GetArticleByTitle(string tittle)
        {
            return _context.Articles.Where(a => a.Title == tittle).First();
        }


    }
}

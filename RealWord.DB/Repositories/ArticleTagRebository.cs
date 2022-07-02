using RealWord.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealWord.DB.Repositories
{
    public class ArticleTagRebository:BaseRepository, IArticleTagRebository
    {
        public ArticleTagRebository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
        public async Task AddTagsToArticle(string slug ,List<Tag> tagList)
        {
            var article = _context.Articles.Where(a => a.Slug == slug).FirstOrDefault(); var JoinList = new List<ArticleTag>();
            foreach( var tag in tagList )
            {
                var tagg = _context.Tags.Where(t => t.Name == tag.Name).FirstOrDefault();
                JoinList.Add(new ArticleTag() { Article = article ,Tag = tagg });
            };
            //}
            await _context.AddRangeAsync(JoinList);
        }
        public IEnumerable<Tag> GetTagsOfArticle(string slug)
        {

            var articleTags = _context.Articles.Where(a => a.Slug == slug).Select(a => a.ArticleTags.Select(a => a.Tag)).First();
            return articleTags;
        }
    }
}

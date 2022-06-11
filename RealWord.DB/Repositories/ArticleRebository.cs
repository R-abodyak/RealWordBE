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



        public Article GetArticleBySlug(string slug)
        {
            var article = _context.Articles.Where(a => a.Slug == slug).FirstOrDefault();
            return article;

        }
        public DateTime GetCreatedDate(String slug)
        {
            var article = GetArticleBySlug(slug);
            return (DateTime)_context.Entry(article).Property("CreatedDate").CurrentValue;
        }
        public DateTime GetUpdatedDate(String slug)
        {
            var article = GetArticleBySlug(slug);
            return (DateTime)_context.Entry(article).Property("UpdatedDate").CurrentValue;
        }

        public User GetAuthorofArticle(String slug)
        {
            var article = GetArticleBySlug(slug);
            if( article == null ) return null;
            var userId = article.UserId;
            return _context.Users.Find(userId);

        }
        public void DeleteArticle(Article article)
        {

            _context.Articles.Remove(article);
        }

    }
}

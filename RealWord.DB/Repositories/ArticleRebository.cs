using Microsoft.EntityFrameworkCore;
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
        public IEnumerable<Article> ListArticlesWithFilters(int limit ,int offset ,int tagId ,string favoritedUserId ,string authorId)
        {
            var articles = _context.Articles.AsNoTracking().
                Include(a => a.ArticleTags).
                Include(a => a.Likes).ToList();
            // var articles = _context.Articles.ToList();
            if( authorId != null )
            {
                articles = articles.Where(a => a.UserId == authorId).ToList();
            }

            if( tagId > 0 )
            {

                articles = articles.Where
                (a => a.ArticleTags.Where(a => a.TagId == tagId).Count() > 0).ToList();

            }
            if( favoritedUserId != null )
            {
                articles = articles.Where
                    (a => a.Likes.Where(a => a.User_id == favoritedUserId).Count() > 0).ToList();
            }

            return articles.OrderByDescending(b => EF.Property<DateTime>(b ,"CreatedDate")).Skip(offset).Take(limit);
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

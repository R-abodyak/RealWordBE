using RealWord.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealWord.DB.Repositories
{
    internal class CommentRebository:BaseRepository, ICommentRebository
    {
        public CommentRebository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }

        public async Task AddComment(Comment comment)
        {
            await _context.AddAsync(comment);
        }

        public IEnumerable<Comment> GetComments(String slug)
        {
            var article = _context.Articles.Where(a => a.Slug == slug).FirstOrDefault();
            if( article == null ) return null;
            var comments = article.Comments.ToList();
            return comments;
        }

        public void RemoveComment(Comment comment)
        {
            _context.Remove(comment);
        }
    }
}

using RealWord.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealWord.DB.Repositories
{
    public class CommentRebository:BaseRepository, ICommentRebository
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
            var comments = _context.Comments.Select(c => c).Where(a => a.Article == article).ToList();
            return comments;
        }
        public async Task<Comment> GetComment(int id)
        {
            return await _context.Comments.FindAsync(id);
        }
        public async Task<DateTime> GetCreatedDate(int id)
        {
            var comment = await GetComment(id);
            return (DateTime)_context.Entry(comment).Property("CreatedDate").CurrentValue;
        }
        public async Task<DateTime> GetUpdatedDateAsync(int id)
        {
            var comment = await GetComment(id);
            return (DateTime)_context.Entry(comment).Property("UpdatedDate").CurrentValue;

        }
        public void RemoveComment(Comment comment)
        {
            _context.Remove(comment);
        }

    }
}

using RealWord.DB.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RealWord.DB.Repositories
{
    public interface ICommentRebository
    {
        public Task AddComment(Comment comment);
        public IEnumerable<Comment> GetComments(string slug);
        Task<Comment> GetComment(int id);
        Task<DateTime> GetUpdatedDateAsync(int id);
        Task<DateTime> GetCreatedDate(int id);
        public void RemoveComment(Comment comment);
    }
}

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
        public IEnumerable<Comment> GetComments(Article article);
        public void RemoveComment(Comment comment);
    }
}

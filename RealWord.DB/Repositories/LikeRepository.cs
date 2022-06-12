using RealWord.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RealWord.DB.Repositories
{
    public class LikeRepository:BaseRepository, ILikeRepository
    {
        public LikeRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
        public bool IsArticleLikedByUser(int ArticleId ,string UserId)
        {
            try
            {
                var result = _context.Likes.Where(l => l.User_id == UserId).Where(s => s.ArticleId == ArticleId).FirstOrDefault();
                if( result == null ) return false;
                return true;
            }
            catch( Exception ) { return false; }
        }
        public int CountLikes(int ArticleId ,string UserId)
        {
            try
            {
                var result = _context.Articles.Find(ArticleId).Likes.Where(l => l.User_id == UserId).Count();
                return result;

            }
            catch( Exception ) { return 0; }
        }


        void ILikeRepository.CreateLike(int ArticleId ,string UserId)
        {
            Like obj = new Like() { ArticleId = ArticleId ,User_id = UserId };
            _context.Likes.AddAsync(obj);
        }

        void ILikeRepository.DeleteLike(int ArticleId ,string UserId)
        {
            var entity = _context.Likes.Where(a => a.ArticleId == ArticleId && a.User_id == UserId).FirstOrDefault();
            _context.Remove(entity);
        }
    }
}

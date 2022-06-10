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
                var result = _context.Articles.Find(ArticleId).Likes.Where(l => l.User_id == UserId).FirstOrDefault();
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

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace RealWord.DB.Repositories
{
    public interface ILikeRepository
    {
        bool IsArticleLikedByUser(int ArticleId ,string UserId);
        int CountLikes(int ArticleId ,string UserId);

        void CreateLike(int ArticleId ,string UserId);
        void DeleteLike(int ArticleId ,string UserId);
    }
}

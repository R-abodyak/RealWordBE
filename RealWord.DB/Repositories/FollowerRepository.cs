using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace RealWord.DB.Repositories
{
    public class FollowerRepository:BaseRepository, IFollowerRepository
    {
        public FollowerRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
        public bool IsFollowing(string SrcId ,string DstId)
        {
            return _context.Users.Where(u => u.Id == SrcId).Select(u => u.followers.Where(f => f.followerId == DstId)).Any();

        }
    }
}

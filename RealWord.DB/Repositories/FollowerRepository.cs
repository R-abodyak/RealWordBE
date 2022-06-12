using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using RealWord.DB.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
            return _context.Users.Where(u => u.Id == SrcId).Select(u => u.followers.Where(f => f.followerId == DstId)).FirstOrDefault().Count() != 0;
        }
        public async Task<Folower> CreateFollow(string SrcId ,string DstId)
        {
            Folower f1 = new Folower();
            f1.UserId = SrcId; f1.followerId = DstId;
            var state = await _context.AddAsync(f1);
            return f1;
        }

        public void RemoveFollow(string SrcId ,string DstId)
        {
            Folower f1 = new Folower();
            f1.UserId = SrcId; f1.followerId = DstId;
            _context.Remove(f1);
            _context.SaveChanges();
        }
        public async Task<List<Folower>> GetFollowers(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            return user.followers;

        }
    }
}

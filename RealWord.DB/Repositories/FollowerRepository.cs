using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using RealWord.DB.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;


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
        public List<Folower> GetFollowers(string userId)
        {
            var user = _context.Users.Include(a => a.followers).Where(U => U.Id == userId).FirstOrDefault();
            return user.followers;

        }

        public List<Article> GetArticlesOfFolowers(List<Folower> followers ,int limit ,int offset)
        {

            if( followers == null ) return null;

            List<Article> articleslist = new List<Article>();
            var articles = _context.Articles
                .OrderByDescending(b => EF.Property<DateTime>(b ,"CreatedDate")).ToList();

            var x = articles.Where(a => a.UserId == followers.Select(F => F.followerId).Where(F => F == a.UserId).FirstOrDefault()).ToList();


            var result = x.Skip(offset).Take(limit).ToList();
            return result;

            //


        }
    }
}

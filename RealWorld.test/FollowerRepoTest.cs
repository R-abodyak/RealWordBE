using Microsoft.EntityFrameworkCore;
using Moq;
using RealWord.DB;
using RealWord.DB.Entities;
using RealWord.DB.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace RealWorld.test
{
    public class FollowerRepoTest
    {
        private DbContextOptions<ApplicationDbContext> dbContextOptions;
        ApplicationDbContext context;
        FollowerRepository followerRepostory;
        public FollowerRepoTest()
        {
            var dbName = $"RealWordDb_{DateTime.Now.ToFileTimeUtc()}";
            dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;
            context = new ApplicationDbContext(dbContextOptions);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            followerRepostory = new FollowerRepository(context);

        }

        [Fact]
        public void IsFollowingMethodTest()
        {


            User u1 = new User() { Id = "1000" };
            User u2 = new User() { Id = "2000" };
            context.Users.Add(u1);
            context.Users.Add(u2);
            Folower f1 = new Folower() { User = u1 ,UserId = u1.Id ,follower = u2 ,followerId = u2.Id };
            context.Add(f1);
            context.SaveChanges();
            var result = followerRepostory.IsFollowing(u1.Id ,u2.Id);
            Assert.True(result);

        }
        [Fact]
        public async Task CreateFollowMethodTest()
        {


            User u1 = new User() { Id = "1000" };
            User u2 = new User() { Id = "2000" };
            context.Users.Add(u1);
            context.Users.Add(u2);

            context.SaveChanges();
            await followerRepostory.CreateFollow(u1.Id ,u2.Id);
            context.SaveChanges();

            var result = followerRepostory.IsFollowing(u1.Id ,u2.Id);
            Assert.True(result);


        }
    }
}

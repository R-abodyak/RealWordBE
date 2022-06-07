using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RealWord.DB.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealWord.DB
{
    public class ApplicationDbContext:IdentityDbContext<User>
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            builder.Entity<Folower>()
            .HasOne(f => f.User)
            .WithMany(f => f.followers)
            .HasForeignKey(f => f.UserId);

            builder.Entity<Folower>()
                .HasOne(f => f.follower)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ArticleTag>()
                .HasOne(f => f.Article)
                .WithMany(f => f.ArticleTags);
            builder.Entity<ArticleTag>()
            .HasOne(f => f.Tag)
            .WithMany(f => f.ArticleTags);

            //composite primry key
            builder.Entity<ArticleTag>().HasKey(l => new { l.ArticleId ,l.TagId });
            builder.Entity<Folower>().HasKey(l => new { l.UserId ,l.followerId });
            builder.Entity<Comment>().HasKey(l => new { l.ArticleId ,l.User_id });
            builder.Entity<Like>().HasKey(l => new { l.ArticleId ,l.User_id });

            //shadow property
            builder.Entity<Article>()
           .Property<DateTime>("CreatedDate");
            builder.Entity<Article>()
          .Property<DateTime>("UpdatedDate");
            builder.Entity<Comment>()
          .Property<DateTime>("CreatedDate");
            builder.Entity<Comment>()
          .Property<DateTime>("UpdatedDate");
            builder.Entity<Like>()
          .Property<DateTime>("CreatedDate");

        }
    }
}

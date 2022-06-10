using RealWord.DB.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealWord.DB.Repositories
{
    public interface IArticleRebository
    {
        Task AddArticle(Article article);
        Task AddTagsToArticle(string slug ,List<Tag> tagList);
        Article GetArticleBySlug(string slug);
        DateTime GetCreatedDate(string articleSlug);
        DateTime GetUpdatedDate(string articleSlug);


        Task SaveChangesAsync();

    }
}
﻿using RealWord.DB.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealWord.DB.Repositories
{
    public interface IArticleRebository
    {
        Task AddArticle(Article article);
        Article GetArticleBySlug(string slug);
        DateTime GetCreatedDate(string articleSlug);
        DateTime GetUpdatedDate(string articleSlug);
        User GetAuthorofArticle(String slug);
        void DeleteArticle(Article article);


        Task SaveChangesAsync();

    }
}
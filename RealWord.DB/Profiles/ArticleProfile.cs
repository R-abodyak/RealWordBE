using AutoMapper;
using RealWord.DB.Entities;
using RealWord.DB.Models.RequestDtos;
using RealWord.DB.Models.ResponseDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealWord.DB.Profiles
{
    public class ArticleProfile:Profile
    {
        public ArticleProfile()
        {
            CreateMap<ArticleDto ,Article>();
            CreateMap<Article ,ArticleResponseDto>();

        }
    }
}

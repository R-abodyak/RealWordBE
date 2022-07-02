using AutoMapper;
using RealWord.DB.Entities;
using RealWord.DB.Models.ResponseDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealWord.DB.Profiles
{
    public class ProfileProfile:Profile
    {
        public ProfileProfile()
        {
            CreateMap<User ,ProfileResponseDto>();
        }
    }
}

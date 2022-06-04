using AutoMapper;
using RealWord.DB.Entities;
using RealWord.DB.Models;
using RealWord.DB.Models.Response_Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealWord.DB.Profiles
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<UserForRegisterDto ,User>();
            CreateMap<User ,UserResponseDto>();
        }
    }
}

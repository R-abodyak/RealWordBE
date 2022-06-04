using AutoMapper;
using RealWord.DB.Entities;
using RealWord.DB.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealWord.DB.Profiles
{
    internal class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<UserForRegisterDto ,ApplicationUser>();
        }
    }
}

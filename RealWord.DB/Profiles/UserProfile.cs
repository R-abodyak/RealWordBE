using AutoMapper;
using RealWord.DB.Entities;
using RealWord.DB.Models;
using RealWord.DB.Models.Request_Dtos;
using RealWord.DB.Models.RequestDtos;
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
            CreateMap<RegisterDto ,User>();
            CreateMap<LoginDto ,User>();
            CreateMap<User ,UserResponseDto>();
            CreateMap<User ,UserResponseDto>();
            //for update with  Null values .if value is Null then don't map it 
            CreateMap<UserForUpdateDto ,User>().ForAllMembers(x => x.Condition(
                      (src ,dest ,sourceValue) => sourceValue != null));

        }
    }
}

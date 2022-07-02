using AutoMapper;
using RealWord.DB.Entities;
using RealWord.DB.Models.RequestDtos;
using RealWord.DB.Models.ResponseDtos;

namespace RealWord.DB.Profiles
{
    public class TagProfile:Profile
    {
        public TagProfile()
        {
            CreateMap<Tag ,TagResponseDto>();


        }
    }
}

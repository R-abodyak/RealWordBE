using AutoMapper;
using RealWord.DB.Entities;
using RealWord.DB.Models.RequestDtos;
using RealWord.DB.Models.ResponseDtos;

namespace RealWord.DB.Profiles
{
    public class CommentProfile:Profile
    {
        public CommentProfile()
        {
            CreateMap<CommentDto ,Comment>();
            CreateMap<Comment ,CommentResponseDto>();

        }
    }
}

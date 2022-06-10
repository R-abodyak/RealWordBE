using AutoMapper;
using RealWord.DB.Models.ResponseDtos;
using RealWord.DB.Repositories;
using RealWordBE.Authentication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RealWord.DB.Services
{
    public class ProfileService:BaseRepository
    {
        private readonly IFollowerRepository _followerRepository;

        public readonly IUserRepository _userReposotory;

        private readonly IMapper _mapper;

        public ProfileService(ApplicationDbContext applicationDbContext ,IFollowerRepository followerRepository ,IUserRepository userReposotory ,IMapper mapper) : base(applicationDbContext)
        {
            _followerRepository = followerRepository;
            _userReposotory = userReposotory;

            _mapper = mapper;
        }

        public async Task<ProfileResponseDto> GetProfileAsync(string SrcId ,string username)
        {
            var dstUser = await _userReposotory.GetUserByUsernameAsync(username);
            if( dstUser == null ) return null;
            var profile = _mapper.Map<ProfileResponseDto>(dstUser);


            if( SrcId != null )
                profile.Following = _followerRepository.IsFollowing(SrcId ,dstUser.Id);
            return profile;
        }
    }
}
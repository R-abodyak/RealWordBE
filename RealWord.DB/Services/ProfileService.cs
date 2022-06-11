using AutoMapper;
using RealWord.DB.Entities;
using RealWord.DB.Models.ResponseDtos;
using RealWord.DB.Repositories;
using RealWordBE.Authentication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RealWord.DB.Services
{
    public class ProfileService:BaseRepository, IProfileService
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

        public async Task<ProfileResponseDto> GetProfileAsync(string SrcUserName ,string DestinationUsername)
        {
            User SrcUser = null;

            if( SrcUserName != null ) SrcUser = await _userReposotory.GetUserByUsernameAsync(SrcUserName);

            var dstUser = await _userReposotory.GetUserByUsernameAsync(DestinationUsername);
            if( dstUser == null ) return null;

            var profile = _mapper.Map<ProfileResponseDto>(dstUser);


            if( SrcUser != null )
                profile.Following = _followerRepository.IsFollowing(SrcUser.Id ,dstUser.Id);

            return profile;
        }

        public async Task<FollowResult> FollowUser(string SrcUserName ,string DestinationUserName)
        {
            var dstUser = await _userReposotory.GetUserByUsernameAsync(DestinationUserName);
            if( dstUser == null ) { return FollowResult.Invalid; }

            var SrcUser = await _userReposotory.GetUserByUsernameAsync(SrcUserName);

            if( _followerRepository.IsFollowing(SrcUser.Id ,dstUser.Id) )
            {
                return FollowResult.Duplicate;
            }
            await _followerRepository.CreateFollow(SrcUser.Id ,dstUser.Id);
            await _followerRepository.SaveChangesAsync();
            return FollowResult.Completed;

        }
        public async Task<FollowResult> UnFollowUser(string SrcUserName ,string DestinationUserName)
        {
            var dstUser = await _userReposotory.GetUserByUsernameAsync(DestinationUserName);
            if( dstUser == null ) { return FollowResult.Invalid; }

            var SrcUser = await _userReposotory.GetUserByUsernameAsync(SrcUserName);

            try { _followerRepository.RemoveFollow(SrcUser.Id ,dstUser.Id); }
            catch( Exception )
            {
                if( !_followerRepository.IsFollowing(SrcUser.Id ,dstUser.Id) )
                {
                    return FollowResult.Duplicate;
                }
            }
            await _followerRepository.SaveChangesAsync();
            return FollowResult.Completed;

        }

    }
}

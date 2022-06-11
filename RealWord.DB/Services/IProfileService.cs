using RealWord.DB.Models.ResponseDtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RealWord.DB.Services
{
    public interface IProfileService
    {
        Task<ProfileResponseDto> GetProfileAsync(string SrcUserName ,string DestinationUsername);
        Task<Status> FollowUser(string SrcUserName ,string DestinationUserName);
        Task<Status> UnFollowUser(string SrcUserName ,string DestinationUserName);
    }
}

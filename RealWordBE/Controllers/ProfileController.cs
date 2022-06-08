using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealWord.DB.Entities;
using RealWord.DB.Models;
using RealWord.DB.Models.Request_Dtos.Outer_Dtos;
using RealWord.DB.Models.RequestDtos;
using RealWord.DB.Models.RequestDtos.OuterDtos;
using RealWord.DB.Models.Response_Dtos;
using RealWord.DB.Models.ResponseDtos;
using RealWord.DB.Repositories;
using RealWordBE.Authentication;
using RealWordBE.Authentication.Logout;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
namespace RealWordBE.Controllers
{
    [Route("api/profiles/{username}")]

    [ApiController]
    public class ProfileController:ControllerBase
    {

        private readonly IUserRepository _userReposotory;
        private readonly IMapper _mapper;
        private readonly IFollowerRepository _followerRepository;

        public ProfileController(IUserRepository userRepository ,IFollowerRepository folloewrRepository ,IMapper mapper)
        {
            _userReposotory = userRepository;
            _mapper = mapper;
            _followerRepository = folloewrRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetProfile(string username)
        {

            var dstUser = await _userReposotory.GetUserByUsernameAsync(username);
            if( dstUser == null ) return BadRequest(
                new Error()
                {
                    Status = "404" ,
                    Tittle = "Bad Request" ,
                    ErrorMessage = "Invalid User Name "
                });
            var profile = _mapper.Map<ProfileResponseDto>(dstUser);
            var SrcId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            profile.Following = _followerRepository.IsFollowing(SrcId ,dstUser.Id);

            return Ok(profile);
        }
        //[Authorize]
        //[HttpPost("follow")]
        //public async Task<IActionResult> FollowUser(string username)
        //{

        //    var dstUser = await _userReposotory.GetUserByUsernameAsync(username);
        //    if( dstUser == null ) return BadRequest(
        //        new Error()
        //        {
        //            Status = "404" ,
        //            Tittle = "Bad Request" ,
        //            ErrorMessage = "Invalid User Name "
        //        });
        //    var SrcId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;

        //    profile.Following = _followerRepository.IsFollowing(SrcId ,dstUser.Id);

        //    return Ok(profile);
        //}

    }
}

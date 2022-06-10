using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealWord.DB.Models;
using RealWord.DB.Models.ResponseDtos;
using RealWord.DB.Repositories;
using RealWordBE.Authentication;
using System;
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
        [HttpGet(Name = "Profile")]
        public async Task<ActionResult<ProfileResponseDto>> GetProfile(string username)
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

            if( SrcId != null )
                profile.Following = _followerRepository.IsFollowing(SrcId ,dstUser.Id);

            return Ok(profile);
        }
        [Authorize]
        [HttpPost("follow")]
        public async Task<ActionResult<ProfileResponseDto>> FollowUser(string username)
        {

            var dstUser = await _userReposotory.GetUserByUsernameAsync(username);
            if( dstUser == null ) return BadRequest(
                new Error()
                {
                    Status = "404" ,
                    Tittle = "Bad Request" ,
                    ErrorMessage = "Invalid User Name "
                });
            var SrcId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            if( _followerRepository.IsFollowing(SrcId ,dstUser.Id) )
            {
                return BadRequest(
                new Error()
                {
                    Status = "404" ,
                    Tittle = "Bad Request" ,
                    ErrorMessage = $"User with user name {username} is aleady followed "
                });
            }
            await _followerRepository.CreateFollow(SrcId ,dstUser.Id);
            await _followerRepository.SaveChangesAsync();

            var profile = new ProfileResponseDto();
            profile.UserName = username;

            return RedirectToRoute("Profile" ,new { username = username });


        }
        [Authorize]
        [HttpDelete("follow")]
        public async Task<ActionResult<ProfileResponseDto>> UnFollowUser(string username)
        {

            var dstUser = await _userReposotory.GetUserByUsernameAsync(username);
            if( dstUser == null ) return BadRequest(
                new Error()
                {
                    Status = "404" ,
                    Tittle = "Bad Request" ,
                    ErrorMessage = "Invalid User Name "
                });
            var SrcId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;


            try { _followerRepository.RemoveFollow(SrcId ,dstUser.Id); }
            catch( Exception )
            {
                if( !_followerRepository.IsFollowing(SrcId ,dstUser.Id) )
                {
                    return BadRequest(
                    new Error()
                    {
                        Status = "404" ,
                        Tittle = "Bad Request" ,
                        ErrorMessage = $"User with user name {username} is aleady Unfollowed "
                    });
                }
            }
            await _followerRepository.SaveChangesAsync();

            return RedirectToRoute("Profile" ,new { username = username });

        }

    }
}

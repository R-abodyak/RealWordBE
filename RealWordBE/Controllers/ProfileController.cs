using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealWord.DB.Models;
using RealWord.DB.Models.ResponseDtos;
using RealWord.DB.Repositories;
using RealWord.DB.Services;
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
        private readonly IProfileService _profileService;
        private readonly IMapper _mapper;
        private readonly IFollowerRepository _followerRepository;

        public ProfileController(IProfileService profileService ,IMapper mapper)
        {
            _profileService = profileService;
            _mapper = mapper;
        }
        [HttpGet(Name = "Profile")]
        public async Task<ActionResult<ProfileResponseDto>> GetProfile(string username)
        {
            var SrcUserName = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username")?.Value;
            var profile = await _profileService.GetProfileAsync(SrcUserName ,username);
            if( profile == null )
            {
                return BadRequest(
                      new Error()
                      {
                          Status = "404" ,
                          Tittle = "Bad Request" ,
                          ErrorMessage = "Invalid User Name "
                      });
            }

            return Ok(profile);
        }
        [Authorize]
        [HttpPost("follow")]
        public async Task<ActionResult<ProfileResponseDto>> FollowUser(string username)
        {
            var SrcUserName = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username")?.Value;

            var status = await _profileService.FollowUser(SrcUserName ,username);
            if( status == FollowResult.Invalid )
                return BadRequest(
                new Error()
                {
                    Status = "404" ,
                    Tittle = "Bad Request" ,
                    ErrorMessage = "Invalid User Name "
                });


            if( status == FollowResult.Duplicate )
            {
                return BadRequest(
                new Error()
                {
                    Status = "404" ,
                    Tittle = "Bad Request" ,
                    ErrorMessage = $"User with user name {username} is already followed "
                });
            }


            var profile = new ProfileResponseDto();
            profile.UserName = username;

            return RedirectToRoute("Profile" ,new { username = username });


        }
        [Authorize]
        [HttpDelete("follow")]
        public async Task<ActionResult<ProfileResponseDto>> UnFollowUser(string username)
        {

            var SrcUserName = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username")?.Value;

            var status = await _profileService.UnFollowUser(SrcUserName ,username);
            if( status == FollowResult.Invalid )
                return BadRequest(
                new Error()
                {
                    Status = "404" ,
                    Tittle = "Bad Request" ,
                    ErrorMessage = "Invalid User Name "
                });


            if( status == FollowResult.Duplicate )
            {
                return BadRequest(
                new Error()
                {
                    Status = "404" ,
                    Tittle = "Bad Request" ,
                    ErrorMessage = $"User with user name {username} is already followed "
                });
            }


            var profile = new ProfileResponseDto();
            profile.UserName = username;

            return RedirectToRoute("Profile" ,new { username = username });


        }

    }
}

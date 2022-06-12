using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealWord.DB.Models;
using RealWord.DB.Models.ResponseDtos;
using RealWord.DB.Models.ResponseDtos.OuterResponseDto;
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

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;

        }
        [HttpGet(Name = "Profile")]
        public async Task<ActionResult<ProfileResponseOuterDto>> GetProfile(string username)
        {
            var SrcUserName = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username")?.Value;
            var profile = await _profileService.GetProfileAsync(SrcUserName ,username);
            if( profile == null )
            {
                return BadRequest(
                      new Error()
                      {
                          Status = "400" ,
                          Tittle = "Bad Request" ,
                          ErrorMessage = "Invalid User Name "
                      });
            }
            var response = new ProfileResponseOuterDto() { Profile = profile };

            return Ok(response);
        }
        [Authorize]
        [HttpPost("follow")]
        public async Task<ActionResult<ProfileResponseOuterDto>> FollowUser(string username)
        {
            var SrcUserName = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username")?.Value;

            var status = await _profileService.FollowUser(SrcUserName ,username);
            if( status == Status.Invalid )
                return BadRequest(
                new Error()
                {
                    Status = "400" ,
                    Tittle = "Bad Request" ,
                    ErrorMessage = "Invalid User Name "
                });


            if( status == Status.Duplicate )
            {
                return BadRequest(
                new Error()
                {
                    Status = "400" ,
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
        public async Task<ActionResult<ProfileResponseOuterDto>> UnFollowUser(string username)
        {

            var SrcUserName = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username")?.Value;

            var status = await _profileService.UnFollowUser(SrcUserName ,username);
            if( status == Status.Invalid )
                return BadRequest(
                new Error()
                {
                    Status = "400" ,
                    Tittle = "Bad Request" ,
                    ErrorMessage = "Invalid User Name "
                });


            if( status == Status.Duplicate )
            {
                return BadRequest(
                new Error()
                {
                    Status = "400" ,
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

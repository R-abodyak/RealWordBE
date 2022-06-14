using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealWord.DB.Models;
using RealWord.DB.Models.ResponseDtos;
using RealWord.DB.Models.ResponseDtos.OuterResponseDto;
using RealWord.DB.Repositories;
using RealWord.DB.Services;
using RealWordBE.Authentication;
using RealWordBE.Authentication.Logout;
using System;
using System.Linq;
using System.Threading.Tasks;
namespace RealWordBE.Controllers
{
    [Route("api/profiles/{username}")]

    [ApiController]
    public class ProfileController:ControllerBase
    {
        private readonly ITokenManager _tokenManager;
        private readonly IProfileService _profileService;

        public ProfileController(ITokenManager tokenmanager ,IProfileService profileService)
        {
            _tokenManager = tokenmanager;
            _profileService = profileService;

        }
        [HttpGet(Name = "Profile")]
        public async Task<ActionResult<ProfileResponseOuterDto>> GetProfile(string username)
        {
            var token = _tokenManager.GetCurrentTokenAsync();
            string CurrentUserName = null;
            if( token != string.Empty )
            {
                var tokens = _tokenManager.ExtractClaims(token);
                CurrentUserName = tokens.Claims.First(claim => claim.Type == "username").Value;
            }
            var profile = await _profileService.GetProfileAsync(CurrentUserName ,username);
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
        // [Authorize]
        [HttpPost("follow")]
        public async Task<ActionResult<ProfileResponseOuterDto>> FollowUser(string username)
        {
            var token = _tokenManager.GetCurrentTokenAsync();
            if( token == string.Empty ) return Unauthorized();
            var tokens = _tokenManager.ExtractClaims(token);

            var SrcUserName = tokens.Claims.First(claim => claim.Type == "username").Value;


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
            profile.Username = username;

            return RedirectToRoute("Profile" ,new { username = username });


        }
        //[Authorize]
        [HttpDelete("follow")]
        public async Task<ActionResult<ProfileResponseOuterDto>> UnFollowUser(string username)
        {
            var token = _tokenManager.GetCurrentTokenAsync();
            if( token == string.Empty ) return Unauthorized();
            var tokens = _tokenManager.ExtractClaims(token);

            var SrcUserName = tokens.Claims.First(claim => claim.Type == "username").Value;

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
            profile.Username = username;

            return RedirectToRoute("Profile" ,new { username = username });


        }

    }
}

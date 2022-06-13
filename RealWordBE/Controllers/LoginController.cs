using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using RealWord.DB.Entities;
using RealWord.DB.Models;
using RealWord.DB.Models.Request_Dtos.Outer_Dtos;
using RealWord.DB.Models.RequestDtos;
using RealWord.DB.Models.RequestDtos.OuterDtos;
using RealWord.DB.Models.Response_Dtos;
using RealWord.DB.Models.ResponseDtos.OuterResponseDto;
using RealWordBE.Authentication;
using RealWordBE.Authentication.Logout;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace RealWordBE.Controllers
{
    [Route("api")]
    [ApiController]
    public class LoginController:ControllerBase
    {
        private readonly IUserRepository _userReposotory;
        private readonly IMapper _mapper;
        private readonly ITokenManager _tokenManager;


        public LoginController(IUserRepository userRepository ,IMapper mapper ,ITokenManager tokenManager)
        {
            _userReposotory = userRepository;
            _mapper = mapper;
            _tokenManager = tokenManager;
        }
        [HttpPost("users")]
        public async Task<ActionResult> RegisterAsync(RegisterOuterDto model)
        {
            var userDto = model.registerDto;
            var user = _mapper.Map<User>(userDto);
            var result = await _userReposotory.RegisterAsync(user);
            if( result == "Success" )
            {
                var response = _mapper.Map<UserResponseDto>(user);
                var outerResponse = new UserResponseOuterDto() { User = response };
                return Ok(outerResponse);
            }
            else
                return BadRequest(new Error
                {
                    Tittle = "Bad Request" ,
                    Status = "400" ,
                    ErrorMessage = result
                });
        }
        [HttpPost("users/login")]
        public async Task<IActionResult> LoginAsync(LoginOuterDto model)
        {
            var userDto = model.LoginDto;

            var userEntity = await _userReposotory.AuthenticateUser(userDto.Email ,userDto.Password);

            if( userEntity == null )
            {
                return Unauthorized(new Error
                {
                    Tittle = "Unauthorized" ,
                    Status = "401" ,
                    ErrorMessage = "Invalid email or Password"
                });
            }
            else
            {
                var token = await _tokenManager.CreateJwtToken(userEntity);
                var response = _mapper.Map<UserResponseDto>(userEntity);
                response.Token = token;
                var outerResponse = new UserResponseOuterDto() { User = response };
                return Ok(outerResponse);
            }

        }

        [HttpPost("users/logout")]
        [Authorize]
        public IActionResult CancelAccessToken()
        {
            _tokenManager.DeactivateCurrentAsync();

            return NoContent();
        }
        [HttpGet("user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var token = _tokenManager.GetCurrentTokenAsync();
            if( token == string.Empty ) return Unauthorized();

            var tokens = _tokenManager.ExtractClaims(token);
            var email = tokens.Claims.First(claim => claim.Type == "emailaddress").Value;
            // var email2 = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailaddress")?.Value;
            var user = await _userReposotory.GetUserByEmailAsync(email);
            var userResponseDto = _mapper.Map<UserResponseDto>(user);

            userResponseDto.Token = token;
            var outerResponse = new UserResponseOuterDto() { User = userResponseDto };
            return Ok(outerResponse);

        }
        [HttpPut("user")]
        //[Authorize]
        public async Task<IActionResult> UpdateCurrentUser(UserForUpdateOuterDto userForUpdateOuter)
        {
            var token = _tokenManager.GetCurrentTokenAsync();
            if( token == string.Empty ) return Unauthorized();

            var tokens = _tokenManager.ExtractClaims(token);
            var email = tokens.Claims.First(claim => claim.Type == "emailaddress").Value;
            // var email = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailaddress")?.Value;
            var currentUser = await _userReposotory.GetUserByEmailAsync(email);
            if( currentUser == null ) return Unauthorized();

            var userDto = userForUpdateOuter.userForUpdateDto;
            _mapper.Map<UserForUpdateDto ,User>(userDto ,currentUser);
            await _userReposotory.UpdateUser(currentUser);
            //update user email or user name make  token claims become invalid ,token should be expired
            _tokenManager.DeactivateCurrentAsync();

            var userResponseDto = _mapper.Map<UserResponseDto>(currentUser);
            userResponseDto.Token = null;
            var outerResponse = new UserResponseOuterDto() { User = userResponseDto };
            return Ok(outerResponse);


        }

    }


}

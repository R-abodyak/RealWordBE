using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealWord.DB.Entities;
using RealWord.DB.Models;
using RealWord.DB.Models.Request_Dtos.Outer_Dtos;
using RealWord.DB.Models.Response_Dtos;
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
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ITokenManager _tokenManager;


        public LoginController(IUserService userService ,IMapper mapper ,ITokenManager tokenManager)
        {
            _userService = userService;
            _mapper = mapper;
            _tokenManager = tokenManager;
        }
        [HttpPost("users")]
        public async Task<ActionResult> RegisterAsync(RegisterOuterDto model)
        {
            var userDto = model.registerDto;
            var user = _mapper.Map<User>(userDto);
            var result = await _userService.RegisterAsync(user);
            if( result == "Success" )
            {
                var response = _mapper.Map<UserResponseDto>(user);
                //TO DO  redirect to get 
                return Ok(response);
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

            var userEntity = await _userService.AuthenticateUser(userDto.Email ,userDto.Password);

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
                var token = await _userService.CreateJwtToken(userEntity);
                var respone = _mapper.Map<UserResponseDto>(userEntity);
                respone.Token = token;
                return Ok(respone);
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
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var email = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailaddress")?.Value;
            var user = await _userService.GetUserByEmailAsync(email);
            var userResponseDto = _mapper.Map<UserResponseDto>(user);
            var token = _tokenManager.GetCurrentTokenAsync();
            if( token == null ) return Unauthorized();
            userResponseDto.Token = token;
            return Ok(userResponseDto);

        }

    }


}

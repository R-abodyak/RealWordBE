using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealWord.DB.Entities;
using RealWord.DB.Models;
using RealWord.DB.Models.Request_Dtos.Outer_Dtos;
using RealWord.DB.Models.Response_Dtos;
using RealWordBE.Authentication;
using System.Threading.Tasks;

namespace RealWordBE.Controllers
{
    [Route("api")]
    [ApiController]
    public class LoginController:ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public LoginController(IUserService userService ,IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
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
    }


}

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealWord.DB.Entities;
using RealWord.DB.Models;
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
            var userDto = model.userForRegisterDto;
            var user = _mapper.Map<User>(userDto);
            var result = await _userService.RegisterAsync(user);
            if( result == "Success" )
            {
                var response = _mapper.Map<UserResponseDto>(user);
                //TO DO  redirect to get  user is not that:(
                return Ok(response);
            }
            else
                return Ok(result);
        }
    }


}

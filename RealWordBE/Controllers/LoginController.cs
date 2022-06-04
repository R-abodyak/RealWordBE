using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealWord.DB.Entities;
using RealWord.DB.Models;
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
            var user = _mapper.Map<ApplicationUser>(userDto);
            var result = await _userService.RegisterAsync(user);
            if( result == "Success" )
                //TO DO  redirect to get 
                return Ok(model);
            else
                return Ok(result);
        }
    }


}

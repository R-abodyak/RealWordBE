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
        public LoginController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("users")]
        public async Task<ActionResult> RegisterAsync(RegisterOuterDto model)
        {
            var user = model.applicationUser;
            var result = await _userService.RegisterAsync(user);
            return Ok(result);
        }
    }


}

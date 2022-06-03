using Microsoft.AspNetCore.Mvc;
using RealWord.DB.Entities;
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
        public async Task<ActionResult> RegisterAsync(ApplicationUser model)
        {
            var result = await _userService.RegisterAsync(model);
            return Ok(result);
        }
    }


}

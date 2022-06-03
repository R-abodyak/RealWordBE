using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using RealWord.DB.Entities;

namespace RealWordBE.Authentication
{
    public class UserService:IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;
        public UserService(UserManager<ApplicationUser> userManager ,IOptions<JWT> jwt)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
        }
    }
}

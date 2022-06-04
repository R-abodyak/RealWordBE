using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RealWord.DB.Entities;
using RealWord.DB.Models;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RealWordBE.Authentication
{
    public class UserService:IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JWT _jwt;
        public UserService(UserManager<ApplicationUser> userManager ,IOptions<JWT> jwt)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
        }
        public async Task<string> RegisterAsync(ApplicationUser user)
        {
            var X = user;
            var userWithSameEmail = await _userManager.FindByEmailAsync(user.Email);
            if( userWithSameEmail == null )
            {
                var result = await _userManager.CreateAsync(user ,user.Password);
                if( result.Succeeded )
                {

                    return "Success";
                }
                return result.Errors.Select(q => q.Description).First();
            }
            else
            {
                return $"Email {user.Email } is already registered.";
            }
        }


    }
}

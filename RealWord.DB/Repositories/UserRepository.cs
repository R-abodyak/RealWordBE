using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RealWord.DB.Entities;
using RealWord.DB.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RealWordBE.Authentication
{
    public class UserRepository:IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly JWT _jwt;
        public UserRepository(UserManager<User> userManager ,IOptions<JWT> jwt)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
        }

        public async Task<User> AuthenticateUser(string email ,string password)
        {
            var userEntity = await _userManager.FindByEmailAsync(email);
            if( userEntity == null ) return null;
            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
            var x = ValidatePassword(userEntity ,password ,passwordHasher);
            if( x )
            {
                return userEntity;
            }
            return null;
        }

        private bool ValidatePassword(User user ,string password ,IPasswordHasher<User> passwordHasher)
        {

            var x = passwordHasher.VerifyHashedPassword(user ,user.PasswordHash ,password) != PasswordVerificationResult.Failed;
            return x;

        }

        public async Task<string> CreateJwtToken(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);

            var claims = new[]
            {
                new Claim("username", user.UserName),
                new Claim("emailaddress", user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey ,SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer ,
                audience: _jwt.Audience ,
                claims: claims ,
                expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes) ,
                signingCredentials: signingCredentials);
            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        }
        public async Task<string> RegisterAsync(User user)
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

        async Task<User> IUserRepository.GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user;
        }
        async Task<User> IUserRepository.GetUserByUsernameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            return user;
        }

        async Task IUserRepository.UpdateUser(User currentUser)
        {

            var result = await _userManager.UpdateAsync(currentUser);
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;

using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Identity;
using RealWord.DB.Entities;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace RealWordBE.Authentication.Logout
{
    public class TokenManager:ITokenManager
    {
        private IMemoryCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly JWT _jwt;

        public TokenManager(IMemoryCache cache ,
                IHttpContextAccessor httpContextAccessor ,
                IOptions<JWT> jwt ,UserManager<User> userManager

            )
        {
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _jwt = jwt.Value;

        }

        public bool IsCurrentActiveToken()
            => IsActiveAsync(GetCurrentTokenAsync());

        public void DeactivateCurrentAsync()
            => DeactivateAsync(GetCurrentTokenAsync());

        public bool IsActiveAsync(string token)
        {
            var token2 = GetKey(token);
            return !_cache.TryGetValue("key" ,out token2);

        }

        public void DeactivateAsync(string token)
        {
            var cacheExpiryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(5) ,
                Priority = CacheItemPriority.High ,
                SlidingExpiration = TimeSpan.FromMinutes(2) ,
                Size = 1024 ,
            };
            var token2 = GetKey(token);
            _cache.Set("key" ,token2 ,cacheExpiryOptions);
            //var x = _cache.Count;

        }



        public string GetCurrentTokenAsync()
        {
            //var authorizationHeader = _httpContextAccessor
            //    .HttpContext.Request.Headers["authorization"];
            StringValues authorizationHeader1 = "";
            var result = _httpContextAccessor
               .HttpContext.Request.Headers.TryGetValue("authorization" ,out authorizationHeader1);
            var x = authorizationHeader1 == StringValues.Empty
                   ? string.Empty
                   : authorizationHeader1.Single().Split(" ").Last();
            return x;
        }

        private static string GetKey(string token)
            => $"tokens:{token}:deactivated";

        public JwtSecurityToken ExtractClaims(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = jsonToken as JwtSecurityToken;
            return tokenS;
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
    }

}
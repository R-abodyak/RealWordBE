using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Caching.Memory;

namespace RealWordBE.Authentication.Logout
{
    public class TokenManager:ITokenManager
    {
        private MemoryCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenManager(MemoryCache cache ,
                IHttpContextAccessor httpContextAccessor

            )
        {
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;

        }

        public bool IsCurrentActiveToken()
            => IsActiveAsync(GetCurrentAsync());

        public void DeactivateCurrentAsync()
            => DeactivateAsync(GetCurrentAsync());

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
            var x = _cache.Count;
            x = 3;

        }



        private string GetCurrentAsync()
        {
            var authorizationHeader = _httpContextAccessor
                .HttpContext.Request.Headers["authorization"];

            var x = authorizationHeader == StringValues.Empty
                   ? string.Empty
                   : authorizationHeader.Single().Split(" ").Last();
            return x;
        }

        private static string GetKey(string token)
            => $"tokens:{token}:deactivated";


    }

}
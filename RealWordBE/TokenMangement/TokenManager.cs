using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;

using Microsoft.Extensions.Caching.Memory;

namespace RealWordBE.Authentication.Logout
{
    public class TokenManager:ITokenManager
    {
        private IMemoryCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenManager(IMemoryCache cache ,
                IHttpContextAccessor httpContextAccessor

            )
        {
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;

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
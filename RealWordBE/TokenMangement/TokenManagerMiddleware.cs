using Microsoft.AspNetCore.Http;
using RealWordBE.Authentication.Logout;
using System.Net;
using System.Threading.Tasks;

namespace DoctorWho2.Authintication
{
    public class TokenManagerMiddleware:IMiddleware
    {
        private readonly ITokenManager _tokenManager;

        public TokenManagerMiddleware(ITokenManager tokenManager)
        {
            _tokenManager = tokenManager;
        }

        public async Task InvokeAsync(HttpContext context ,RequestDelegate next)
        {
            if( _tokenManager.IsCurrentActiveToken() )
            {
                await next(context);

                return;
            }
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
    }
}

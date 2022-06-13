using RealWord.DB.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace RealWordBE.Authentication.Logout
{
    public interface ITokenManager
    {
        void DeactivateAsync(string token);
        void DeactivateCurrentAsync();
        bool IsActiveAsync(string token);
        bool IsCurrentActiveToken();
        public string GetCurrentTokenAsync();
        Task<string> CreateJwtToken(User user);
        JwtSecurityToken ExtractClaims(string token);
    }
}

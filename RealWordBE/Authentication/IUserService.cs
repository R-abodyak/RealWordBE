using RealWord.DB.Entities;
using System.Threading.Tasks;

namespace RealWordBE.Authentication
{
    public interface IUserService
    {
        Task<string> RegisterAsync(ApplicationUser user);
    }
}

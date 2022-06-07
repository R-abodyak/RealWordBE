using RealWord.DB.Entities;
using System.Threading.Tasks;

namespace RealWordBE.Authentication
{
    public interface IUserRepository
    {
        Task<string> RegisterAsync(User user);
        Task<User> AuthenticateUser(string email ,string password);
        Task<string> CreateJwtToken(User user);
        Task<User> GetUserByEmailAsync(string email);
        Task UpdateUser(User User);
        Task<User> GetUserByUsernameAsync(string userName);
    }
}

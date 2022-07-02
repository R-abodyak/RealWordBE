using RealWord.DB.Entities;
using System.Threading.Tasks;

namespace RealWordBE.Authentication
{
    public interface IUserRepository
    {
        Task<string> RegisterAsync(User user ,string password);
        Task<User> AuthenticateUser(string email ,string password);

        Task<User> GetUserByEmailAsync(string email);
        Task UpdateUser(User User);
        Task<User> GetUserByUsernameAsync(string userName);
    }
}

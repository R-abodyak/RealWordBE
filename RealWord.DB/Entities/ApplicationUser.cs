using Microsoft.AspNetCore.Identity;

namespace RealWord.DB.Entities
{
    public class User:IdentityUser
    {
        public string Password { get; set; }
        public string Image { get; set; }
        public string Bio { get; set; }

    }
}

using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace RealWord.DB.Entities
{
    public class User:IdentityUser
    {
        public string Password { get; set; }
        public string Image { get; set; }
        public string Bio { get; set; }
        public List<Folower> followers { get; set; }
    }
}

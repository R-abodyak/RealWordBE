using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace RealWord.DB.Models.Response_Dtos
{
    public class UserResponseDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string UserName { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }

    }
}

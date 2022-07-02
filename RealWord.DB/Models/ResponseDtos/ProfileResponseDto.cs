using System;
using System.Collections.Generic;
using System.Text;

namespace RealWord.DB.Models.ResponseDtos
{
    public class ProfileResponseDto
    {
        public string Username { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }
        public bool Following { get; set; }
    }
}

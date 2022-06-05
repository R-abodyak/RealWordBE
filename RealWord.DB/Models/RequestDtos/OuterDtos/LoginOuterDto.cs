using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace RealWord.DB.Models.Request_Dtos.Outer_Dtos
{
    public class LoginOuterDto
    {
        [JsonPropertyName("user")]
        public LoginDto LoginDto { get; set; }
    }
}

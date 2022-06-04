using RealWord.DB.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace RealWord.DB.Models
{
    public class RegisterOuterDto
    {
        [JsonPropertyName("user")]

        //public ApplicationUser applicationUser { get; set; }
        public UserForRegisterDto userForRegisterDto { get; set; }
    }
}

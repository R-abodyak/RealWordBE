using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace RealWord.DB.Models.RequestDtos
{
    public class UserForUpdateDto
    {

        [JsonPropertyName("username")]
        public string UserName { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }
        [Url]
        [JsonPropertyName("image")]
        public string Image { get; set; }

        [JsonPropertyName("bio")]
        public string Bio { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace RealWord.DB.Models
{
    public class RegisterDto
    {
        [Required]
        [JsonPropertyName("username")]
        public string UserName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [Required]
        [JsonPropertyName("password")]
        public string Password { get; set; }

    }
}

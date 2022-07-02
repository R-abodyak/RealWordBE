using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace RealWord.DB.Models.RequestDtos
{
    public class CommentDto
    {
        [JsonPropertyName("body")]
        [Required]
        public string CommentMsg { get; set; }
    }
}

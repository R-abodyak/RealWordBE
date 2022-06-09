using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace RealWord.DB.Models.RequestDtos
{
    public class ArticleDto
    {
        [Required]
        [JsonPropertyName("title")]

        public string Title { get; set; }

        [Required]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [Required]
        [JsonPropertyName("body")]
        public string Body { get; set; }
        public List<string> Tags { get; set; }
    }
}

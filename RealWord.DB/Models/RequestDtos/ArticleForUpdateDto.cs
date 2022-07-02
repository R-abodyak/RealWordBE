using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace RealWord.DB.Models.RequestDtos
{
    public class ArticleForUpdateDto
    {
        [JsonPropertyName("title")]

        public string Title { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("body")]
        public string Body { get; set; }
    }
}

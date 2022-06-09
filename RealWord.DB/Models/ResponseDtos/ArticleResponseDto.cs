using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace RealWord.DB.Models.ResponseDtos
{
    public class ArticleResponseDto
    {
        public string Slug { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }

        public List<string> TagList { get; set; }
        [JsonPropertyName("createdAt")]
        public DateTime CreatedDate { get; set; }

        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedDate { get; set; }
    }
}
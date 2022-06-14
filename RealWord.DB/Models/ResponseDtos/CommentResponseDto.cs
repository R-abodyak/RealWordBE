using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace RealWord.DB.Models.ResponseDtos
{
    public class CommentResponseDto
    {
        [JsonPropertyName("id")]

        public int CommentId { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedDate { get; set; }

        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedDate { get; set; }

        [JsonPropertyName("body")]

        public string CommentMsg { get; set; }


        public ProfileResponseDto Author { get; set; }
    }
}

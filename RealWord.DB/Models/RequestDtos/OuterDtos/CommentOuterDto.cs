using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace RealWord.DB.Models.RequestDtos.OuterDtos
{
    public class CommentOuterDto
    {
        [JsonPropertyName("comment")]

        public CommentDto CommentDto { get; set; }
    }
}

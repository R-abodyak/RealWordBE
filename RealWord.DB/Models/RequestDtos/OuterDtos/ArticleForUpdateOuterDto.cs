using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace RealWord.DB.Models.RequestDtos.OuterDtos
{
    public class ArticleForUpdateOuterDto
    {
        [JsonPropertyName("article")]

        public ArticleForUpdateDto articleForUpdateDto { get; set; }
    }
}

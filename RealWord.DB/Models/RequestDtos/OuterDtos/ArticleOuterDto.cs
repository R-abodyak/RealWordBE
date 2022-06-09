using System.Text.Json.Serialization;

namespace RealWord.DB.Models.RequestDtos.OuterDtos
{
    public class ArticleOuterDto
    {
        [JsonPropertyName("article")]

        public ArticleDto ArticleDto { get; set; }
    }
}

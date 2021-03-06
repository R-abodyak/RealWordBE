
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealWord.DB.Entities
{
    public class ArticleTag
    {

        public int ArticleId { get; set; }

        public int TagId { get; set; }
        public Article Article { get; set; }
        public Tag Tag { get; set; }
    }
}

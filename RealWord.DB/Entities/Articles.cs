using System;
using System.Collections.Generic;
using System.Text;

namespace RealWord.DB.Entities
{
    public class Articles
    {
        public int ArticlId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


    }
}

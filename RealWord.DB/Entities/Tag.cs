using System;
using System.Collections.Generic;
using System.Text;

namespace RealWord.DB.Entities
{
    public class Tag
    {
        public int TagId { get; set; }
        public string Name { get; set; }
        public ICollection<ArticleTag> ArticleTags { get; set; }

    }
}

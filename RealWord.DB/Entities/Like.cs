using System;
using System.Collections.Generic;
using System.Text;

namespace RealWord.DB.Entities
{
    public class Like
    {
        public int ArticleId { get; set; }

        public int User_id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

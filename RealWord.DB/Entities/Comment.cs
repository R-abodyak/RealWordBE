using System;
using System.Collections.Generic;
using System.Text;

namespace RealWord.DB.Entities
{
    public class Comment
    {
        public int ArticleId { get; set; }

        public int User_id { get; set; }
        public string CommentMsg { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RealWord.DB.Entities
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        public int ArticleId { get; set; }
        public string User_id { get; set; }
        public string CommentMsg { get; set; }
        public Article Article { get; set; }
        public User User { get; set; }


    }
}

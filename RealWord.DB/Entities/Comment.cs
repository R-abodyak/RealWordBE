﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RealWord.DB.Entities
{
    public class Comment
    {
        public int ArticleId { get; set; }
        public int User_id { get; set; }
        public string CommentMsg { get; set; }
        public Article Article { get; set; }
        public User User { get; set; }


    }
}

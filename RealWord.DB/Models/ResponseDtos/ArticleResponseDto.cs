using System;
using System.Collections.Generic;
using System.Text;

namespace RealWord.DB.Models.ResponseDtos
{
    public class ArticleResponseDto
    {
        public string Slug { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }

        public List<string> TagList { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
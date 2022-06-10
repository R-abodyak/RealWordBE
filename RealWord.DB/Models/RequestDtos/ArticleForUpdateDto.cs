using System;
using System.Collections.Generic;
using System.Text;

namespace RealWord.DB.Models.RequestDtos
{
    public class ArticleForUpdateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
    }
}

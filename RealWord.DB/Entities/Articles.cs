using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RealWord.DB.Entities
{
    public class Article
    {
        [Key]
        public int ArticleId { get; set; }
        [Required]

        public string Title { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public string Body { get; set; }

        [ForeignKey("AuthorId")]
        public string UserId { get; set; }
        public User User { get; set; }
        public ICollection<ArticleTag> ArticleTags { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Comment> Comments { get; set; }





    }
}

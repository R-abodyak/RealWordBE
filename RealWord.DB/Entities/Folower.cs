using System;
using System.Collections.Generic;
using System.Text;

namespace RealWord.DB.Entities
{
    public class Folower
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public string followerId { get; set; }
        public User follower { get; set; }
    }
}

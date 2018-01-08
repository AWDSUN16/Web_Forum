using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_Forum.Entities
{
    public class Post
    {
        public int PostId { get; set; }
        public string Content { get; set; }
        public DateTime DateOfCreation { get; set; }

        public string PosterId { get; set; }

        public ApplicationUser Poster { get; set; }
    }
}

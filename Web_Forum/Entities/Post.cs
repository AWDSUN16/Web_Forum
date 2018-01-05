using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_Forum.Entities
{
    public class Post
    {
        public string Content { get; set; }
        public int Id { get; set; }
        public string DateOfCreation { get; set; }
        public string CreatedBy { get; set; }
    }
}

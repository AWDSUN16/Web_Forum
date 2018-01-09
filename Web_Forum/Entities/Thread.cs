using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_Forum.Entities
{
    public class Thread
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string DateOfLastUpdate { get; set; }
        public string DateOfCreation { get; set; }
        public int AmountOfReplies { get; set; }
        public int AmountOfViews { get; set; }
    }
}

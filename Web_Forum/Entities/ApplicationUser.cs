﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_Forum.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public List<Post> Posts { get; set; }
    }
}

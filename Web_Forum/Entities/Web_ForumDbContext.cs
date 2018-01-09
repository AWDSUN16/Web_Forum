using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Web_Forum.Entities
{
 

    public class Web_ForumDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Thread> Threads { get; set; }
        public DbSet<Post> Posts { get; set; }
        public Web_ForumDbContext(DbContextOptions<Web_ForumDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}

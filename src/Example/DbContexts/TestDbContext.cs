using Example.Entities;

using kr.bbon.Data;

using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.DbContexts
{
    public class TestDbContext : AppDbContextBase
    {
        public TestDbContext(DbContextOptions<TestDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<User> Users { get; set; }
    }
}

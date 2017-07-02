using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace postgres1
{
    public class GeneralContext : DbContext
    {
        public GeneralContext(DbContextOptions options)
            : base(options)
        { }

        public DbSet<Value> Values { get; set; }
    }
}
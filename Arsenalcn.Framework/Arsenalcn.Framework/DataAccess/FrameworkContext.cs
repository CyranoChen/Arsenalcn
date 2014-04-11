using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

using Arsenalcn.Framework.Models;

namespace Arsenalcn.Framework.DataAccess
{
    public class FrameworkContext : DbContext
    {
        public FrameworkContext()
            : base("FrameworkContext")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Organization> Organizations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
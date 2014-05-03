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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // 去除生成数据库的表名不为复数
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }        
        
        public DbSet<User> Users { get; set; }
        public DbSet<Organization> Organizations { get; set; }

    }
}
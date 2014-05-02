using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

using Arsenalcn.Framework.Entity;

namespace Arsenalcn.Framework.DataAccess
{
    public class FrameworkDataContext : DbContext
    {
        public FrameworkDataContext()
        {
            Configuration.ProxyCreationEnabled = false;

            #if(Debug)
            Database.SetInitializer(new FrameworkDataInitializer());
            #endif
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // 去除生成数据库的表名不为复数
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Bid>()
                .HasRequired(x => x.Auction)
                .WithMany()
                .WillCascadeOnDelete(false);
        }

        public DbSet<Auction> Auctions { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
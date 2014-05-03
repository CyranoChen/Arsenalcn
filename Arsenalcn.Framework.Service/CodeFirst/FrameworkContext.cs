using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;

using Arsenalcn.Framework.Entity;
using Arsenalcn.Framework.DataAnnotations;

namespace Arsenalcn.Framework.DataAccess.CodeFirst
{
    public class FrameworkContext : DbContext
    {
        //public DbSet<Auction> Auctions { get; set; }
        //public DbSet<Bid> Bids { get; set; }
        //public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        //public DbSet<Account> Accounts { get; set; }

        public FrameworkContext()
            : base("FrameworkContextCodeFirst")
        {
            //Configuration.ProxyCreationEnabled = false;

            Database.SetInitializer(new FrameworkInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // 去除生成数据库的表名不为复数
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // 防止每次连接数据库访问元数据表 EF6后不使用
            //modelBuilder.Conventions.Remove<IncludeMetadataConvention>(); 

            // 将EF的数据库级联删除关闭
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<User>()
                .HasMany(u => u.Accounts).WithMany(a => a.Users);
        }
    }
}
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
            // ȥ���������ݿ�ı�����Ϊ����
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // ��ֹÿ���������ݿ����Ԫ���ݱ� EF6��ʹ��
            //modelBuilder.Conventions.Remove<IncludeMetadataConvention>(); 

            // ��EF�����ݿ⼶��ɾ���ر�
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<User>()
                .HasMany(u => u.Accounts).WithMany(a => a.Users);
        }
    }
}
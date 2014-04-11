using System;
using System.Collections.Generic;
using System.Data.Entity;

using Arsenalcn.Framework.Models;

namespace Arsenalcn.Framework.DataAccess
{
    public class FrameworkInitializer : DropCreateDatabaseIfModelChanges<FrameworkContext>
    {
        protected override void Seed(FrameworkContext context)
        {
            var users = new List<User>
            {
            new User{Name="Cyrano", CreateTime=DateTime.Now},
            new User{Name="Vicky", CreateTime=DateTime.Now},
            new User{Name="Catherine", CreateTime=DateTime.Now},
            new User{Name="Casper", CreateTime=DateTime.Now},
            new User{Name="Lisa", CreateTime=DateTime.Now},
            };

            users.ForEach(u => context.Users.Add(u));
            context.SaveChanges();

            var organizations = new List<Organization>
            {
            new Organization{Name="Wonders", CreateTime=DateTime.Now},
            new Organization{Name="Wicresoft", CreateTime=DateTime.Now},
            new Organization{Name="Microsoft", CreateTime=DateTime.Now},
            new Organization{Name="Google", CreateTime=DateTime.Now},
            new Organization{Name="Shmetro", CreateTime=DateTime.Now},
            };

            organizations.ForEach(o => context.Organizations.Add(o));
            context.SaveChanges();
        }
    }
}
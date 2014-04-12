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

            var users = new List<User>
            {
            new User{Name="Cyrano", CreateTime=DateTime.Now,OrganizationID=1},
            new User{Name="Vicky", CreateTime=DateTime.Now,OrganizationID=1},
            new User{Name="Catherine", CreateTime=DateTime.Now,OrganizationID=1},
            new User{Name="Casper", CreateTime=DateTime.Now,OrganizationID=1},
            new User{Name="Lisa", CreateTime=DateTime.Now,OrganizationID=1},
            };

            users.ForEach(u => context.Users.Add(u));
            context.SaveChanges();
        }
    }
}
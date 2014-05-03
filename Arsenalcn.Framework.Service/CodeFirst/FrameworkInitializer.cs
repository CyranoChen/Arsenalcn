using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;

using Arsenalcn.Framework.Entity;

namespace Arsenalcn.Framework.DataAccess.CodeFirst
{
    public class FrameworkInitializer
        : DropCreateDatabaseIfModelChanges<FrameworkContext>
    {
        protected override void Seed(FrameworkContext db)
        {
            db.Users.Add(new User
            {
                Name = "cyrano",
                DisplayName = "Cyrano Chen",
                Mobile = "13818059707",
                Email = "cyrano@arsenalcn.com",
            });

            db.Users.Add(new User
            {
                Name = "vicky",
                DisplayName = "Vicky Ling",
                Mobile = "15026699918",
                Email = "vicky@arsenal.cn",
            });

            //db.SaveChanges();
        }
    }
}
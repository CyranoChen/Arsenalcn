using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Arsenalcn.Framework.DataAccess.ModelFirst;

namespace Arsenalcn.Framework.Models
{
    public class UserModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string DisplayName { get; set; }
        public string Gender { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
}
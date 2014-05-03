using Arsenalcn.Framework.DataAnnotations;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arsenalcn.Framework.Entity
{
    [MetadataType(typeof(Account.Metadata))]
    public class Account : Entity<long>
    {
        public Account()
        {
            Users = new Collection<User>();
        }

        [Unique]
        public string Name { get; set; }

        public string Alias { get; set; }

        public string Password { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public class Metadata
        {
            [Required, StringLength(50, MinimumLength = 1)]
            public object Name;

            [StringLength(50)]
            public object Alias;

            [Required]
            public object Password;
        }
    }
}

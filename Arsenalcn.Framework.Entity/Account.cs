using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Arsenalcn.Framework.Entity
{
    public class Account : EntityBase
    {
        public Account()
        {
            Users = new Collection<User>();
        }


        public string Alias { get; set; }

        public string Password { get; set; }

        public virtual ICollection<User> Users { get; set; }

        //public class Metadata
        //{
        //    //[Required, StringLength(50, MinimumLength = 1)]
        //    //public object Name;

        //    [StringLength(50)]
        //    public object Alias;

        //    [Required]
        //    public object Password;
        //}
    }
}

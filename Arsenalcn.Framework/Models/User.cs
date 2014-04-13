using System;
using System.Collections.Generic;

namespace Arsenalcn.Framework.Models
{
    public class User
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.None)] // 自动生成主键值
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
        public int OrganizationID { get; set; }

        //public virtual ICollection<User> Users { get; set; }
        //public virtual ICollection<Organization> Organizations { get; set; }
    }
}
using System;

namespace Arsenalcn.Framework.Entity
{
    public class EntityBase
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        //public DateTime CreateTime { get; set; }

        //public DateTime UpdateTime { get; set; }

        public string Remark { get; set; }

        public string Key { get; set; }
    }
}

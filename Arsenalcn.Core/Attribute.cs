using System;
using System.ComponentModel.DataAnnotations;

namespace Arsenalcn.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AttrDbTable : Attribute
    {
        public string Name;
        public string Key;
        public string Sort;

        public AttrDbTable(string name)
        {
            Name = name;
            Key = "ID";
            Sort = string.Empty;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class AttrDbColumn : Attribute
    {
        public string Name;
        public bool Key;

        public AttrDbColumn(string name)
        {
            Name = name;
            Key = false;
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class UniqueAttribute : RequiredAttribute { }
}

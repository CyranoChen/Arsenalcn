using System;
using System.ComponentModel.DataAnnotations;

namespace Arsenalcn.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DbTable : Attribute
    {
        public string Name;
        public string Key;
        public string Sort;

        public DbTable(string name)
        {
            Name = name;
            Key = "ID";
            Sort = string.Empty;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class DbColumn : Attribute
    {
        public string Name;
        public bool Key;

        public DbColumn(string name)
        {
            Name = name;
            Key = false;
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class UniqueAttribute : RequiredAttribute { }
}

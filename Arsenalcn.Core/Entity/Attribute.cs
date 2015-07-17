using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class UniqueAttribute : RequiredAttribute { }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class DomainAttribute : ValidationAttribute
    {
        public IEnumerable<string> Values { get; private set; }

        public DomainAttribute(string value)
        {
            this.Values = new string[] { value };
        }

        public DomainAttribute(params string[] values)
        {
            this.Values = values;
        }

        public override bool IsValid(object value)
        {
            if (null == value)
            {
                return true;
            }
            return this.Values.Any(item => value.ToString() == item);
        }

        public override string FormatErrorMessage(string name)
        {
            string[] values = this.Values.Select(value => string.Format("'{0}'", value)).ToArray();
            return string.Format(base.ErrorMessageString, name, string.Join(",", values));
        }
    }
}

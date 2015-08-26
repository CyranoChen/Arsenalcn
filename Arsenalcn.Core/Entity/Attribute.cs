using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Arsenalcn.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DbSchema : Attribute
    {
        public string Name;
        public string Key;
        public string Sort;

        public DbSchema(string name)
        {
            Name = name;
            Key = "ID";
            Sort = string.Empty;
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class DbColumn : Attribute
    {
        public string Name;
        public bool IsKey;
        public string Key;
        public string ForeignKey;

        public DbColumn(string name)
        {
            Name = name;
            IsKey = false;
            Key = "ID";
            ForeignKey = string.Empty;
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class UniqueAttribute : RequiredAttribute { }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class DomainAttribute : ValidationAttribute
    {
        public IEnumerable<string> Values { get; }

        public DomainAttribute(string value)
        {
            Values = new[] { value };
        }

        public DomainAttribute(params string[] values)
        {
            Values = values;
        }

        public override bool IsValid(object value)
        {
            if (null == value)
            {
                return true;
            }
            return Values.Any(item => value.ToString() == item);
        }

        public override string FormatErrorMessage(string name)
        {
            var values = Values.Select(value => $"'{value}'").ToArray();
            return string.Format(ErrorMessageString, name, string.Join(",", values));
        }
    }
}

using System;

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
        public bool IsKey;
        public int Length;
        public bool IsNull;

        public AttrDbColumn(string name)
        {
            Name = name;
            IsKey = false;
            Length = 0;
            IsNull = true;
        }
    }

}

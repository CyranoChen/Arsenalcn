using System;
using System.Data;

namespace Arsenalcn.ClubSys.Entity
{
    public class ClubSysObject
    {
        private static readonly string AdditionalDataColumnName = "additional";

        protected ClubSysObject()
        {
        }

        protected ClubSysObject(DataRow dr)
        {
            var properties = GetType().GetProperties();

            foreach (var pi in properties)
            {
                var attributes = pi.GetCustomAttributes(typeof (ClubSysDbColumnAttribute), true);

                if (attributes.Length != 1)
                    continue;
                var dbColumn = (ClubSysDbColumnAttribute) attributes[0];

                var columnName = dbColumn.ColumnName;

                if (dr[columnName] == DBNull.Value)
                    pi.SetValue(this, null, null);
                else
                    pi.SetValue(this, dr[columnName], null);
            }

            if (dr.Table.Columns.Contains(AdditionalDataColumnName))
            {
                AdditionalData = dr[AdditionalDataColumnName];
            }
        }

        public object AdditionalData { get; set; }

        public object AdditionalData2 { get; set; } = null;
    }

    public class ClubSysDbColumnAttribute : Attribute
    {
        public ClubSysDbColumnAttribute(string columnName)
        {
            ColumnName = columnName;
        }

        public string ColumnName { get; }
    }
}
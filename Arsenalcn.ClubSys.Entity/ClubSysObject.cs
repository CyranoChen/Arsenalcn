using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Reflection;

namespace Arsenalcn.ClubSys.Entity
{
    public class ClubSysObject
    {
        private static readonly string AdditionalDataColumnName = "additional";

        private object _additionalData = null;
        public object AdditionalData
        {
            get
            {
                return _additionalData;
            }
            set
            {
                _additionalData = value;
            }
        }

        private object _additionalData2 = null;
        public object AdditionalData2
        {
            get
            {
                return _additionalData2;
            }
            set
            {
                _additionalData2 = value;
            }
        }

        protected ClubSysObject()
        {
        }

        protected ClubSysObject(DataRow dr)
        {
            PropertyInfo[] properties = this.GetType().GetProperties();

            foreach (PropertyInfo pi in properties)
            {
                object[] attributes = pi.GetCustomAttributes(typeof(ClubSysDbColumnAttribute), true);

                if (attributes.Length != 1)
                    continue;
                else
                {
                    ClubSysDbColumnAttribute dbColumn = (ClubSysDbColumnAttribute)attributes[0];

                    string columnName = dbColumn.ColumnName;

                    if( dr[columnName] == DBNull.Value )
                        pi.SetValue(this, null, null);
                    else
                        pi.SetValue(this, dr[columnName], null);
                }
            }

            if ( dr.Table.Columns.Contains(AdditionalDataColumnName) )
            {
                AdditionalData = dr[AdditionalDataColumnName];
            }
        }
    }

    public class ClubSysDbColumnAttribute : Attribute
    {
        private string _columnName;

        public ClubSysDbColumnAttribute(string columnName)
        {
            _columnName = columnName;
        }

        public string ColumnName
        {
            get
            {
                return _columnName;
            }
        }
    }
}

using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;

using Microsoft.ApplicationBlocks.Data;

namespace Arsenalcn.Core
{
    public static class DataAccess
    {
        public static string ConnectString;
        static DataAccess()
        {
            ConnectString = ConfigurationManager.ConnectionStrings["Arsenalcn.ConnectionString"].ConnectionString;
        }

        public static DataSet ExecuteDataset(string sql, SqlParameter[] para = null)
        {
            Contract.Requires(!string.IsNullOrEmpty(sql));

            return SqlHelper.ExecuteDataset(ConnectString, CommandType.Text, sql, para);
        }

        public static void ExecuteNonQuery(string sql, SqlParameter[] para = null, SqlTransaction trans = null)
        {
            Contract.Requires(!string.IsNullOrEmpty(sql));

            if (trans != null)
            {
                SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, para);
            }
            else
            {
                SqlHelper.ExecuteNonQuery(ConnectString, CommandType.Text, sql, para);
            }
        }
    }
}

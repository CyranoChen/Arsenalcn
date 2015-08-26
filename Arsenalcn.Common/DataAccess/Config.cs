using System.Data;
using System.Data.SqlClient;

using Microsoft.ApplicationBlocks.Data;

namespace Arsenalcn.Common.DataAccess
{
    public class Config
    {
        public static DataRow GetConfigByID(string configSystem, string configKey)
        {
            var sql = "SELECT ConfigSystem, ConfigKey, ConfigValue FROM dbo.Arsenalcn_Config WHERE ConfigSystem = @configSystem AND ConfigKey = @configKey";

            SqlParameter[] para = { new SqlParameter("@configSystem", configSystem), new SqlParameter("@configKey", configKey) };

            var ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, para);

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0].Rows[0];
        }

        public static void UpdateConfig(string configSystem, string configKey, string configValue, SqlTransaction trans)
        {
            var sql = "UPDATE dbo.Arsenalcn_Config SET ConfigValue = @configValue WHERE ConfigSystem = @configSystem AND ConfigKey = @configKey";

            SqlParameter[] para = { new SqlParameter("@configSystem", configSystem), new SqlParameter("@configKey", configKey), new SqlParameter("@configValue", configValue) };

            if (trans != null)
                SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, para);
            else
                SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static void InsertConfig(string configSystem, string configKey, string configValue, SqlTransaction trans)
        {
            var sql = "INSERT INTO dbo.Arsenalcn_Config (ConfigSystem, ConfigKey, ConfigValue) VALUES (@configSystem, @configKey, @configValue)";

            SqlParameter[] para = { new SqlParameter("@configSystem", configSystem), new SqlParameter("@configKey", configKey), new SqlParameter("@configValue", configValue) };

            if (trans != null)
                SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, para);
            else
                SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static void DeleteConfig(string configSystem, string configKey)
        {
            var sql = "DELETE dbo.Arsenalcn_Config WHERE ConfigSystem = @configSystem AND ConfigKey = @configKey";

            SqlParameter[] para = { new SqlParameter("@configSystem", configSystem), new SqlParameter("@configKey", configKey) };

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static DataTable GetConfigs()
        {
            var sql = @"SELECT * FROM dbo.Arsenalcn_Config ORDER BY ConfigSystem, ConfigKey";

            var ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }
    }
}

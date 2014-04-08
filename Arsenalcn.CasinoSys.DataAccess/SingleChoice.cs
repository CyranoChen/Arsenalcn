using System;
using System.Data;
using System.Data.SqlClient;

using Microsoft.ApplicationBlocks.Data;

namespace Arsenalcn.CasinoSys.DataAccess
{
    public class SingleChoice
    {
        public static DataRow GetSingleChoice(Guid casinoItemGuid)
        {
            string sql = "SELECT * FROM dbo.AcnCasino_SingleChoice WHERE CasinoItemGuid = @guid";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@guid", casinoItemGuid));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0].Rows[0];
        }

        public static void InsertSingleChoice(Guid casinoItemGuid, bool floatingRate, SqlTransaction trans)
        {
            string sql = "INSERT INTO dbo.AcnCasino_SingleChoice VALUES (@guid, @floatingRate, null)";

            if (trans != null)
            {
                SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, new SqlParameter("@guid", casinoItemGuid), new SqlParameter("@floatingRate", floatingRate));

            }
            else
            {
                SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@guid", casinoItemGuid), new SqlParameter("@floatingRate", floatingRate));
            }
        }
    }
}

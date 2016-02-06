using System;
using System.Data;
using System.Data.SqlClient;
using Arsenalcn.Common;
using Microsoft.ApplicationBlocks.Data;

namespace Arsenalcn.CasinoSys.DataAccess
{
    public class MatchResult
    {
        public static DataRow GetMatchResult(Guid casinoItemGuid)
        {
            var sql = "SELECT * FROM dbo.AcnCasino_MatchResult WHERE CasinoItemGuid = @guid";

            var ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql,
                new SqlParameter("@guid", casinoItemGuid));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            return ds.Tables[0].Rows[0];
        }

        public static void InsertMatchResult(Guid casinoItemGuid, SqlTransaction trans)
        {
            var sql = "INSERT INTO dbo.AcnCasino_MatchResult VALUES (@guid, null, null)";

            if (trans != null)
            {
                SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, new SqlParameter("@guid", casinoItemGuid));
            }
            else
            {
                SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql,
                    new SqlParameter("@guid", casinoItemGuid));
            }
        }

        public static void UpdateMatchResult(Guid casinoItemGuid, short home, short away, SqlTransaction trans)
        {
            var sql = "UPDATE dbo.AcnCasino_MatchResult SET Home = @home, Away = @away WHERE CasinoItemGuid = @guid";

            SqlParameter[] para =
            {
                new SqlParameter("@home", home), new SqlParameter("@away", away),
                new SqlParameter("@guid", casinoItemGuid)
            };

            if (trans != null)
                SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, para);
            else
                SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }
    }
}
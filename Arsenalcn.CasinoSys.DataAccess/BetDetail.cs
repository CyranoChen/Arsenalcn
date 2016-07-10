using System.Data;
using System.Data.SqlClient;
using Arsenalcn.Common;
using Microsoft.ApplicationBlocks.Data;

namespace Arsenalcn.CasinoSys.DataAccess
{
    public static class BetDetail
    {
        public static DataTable GetBetDetailByBetId(int id)
        {
            var sql = "SELECT * FROM AcnCasino_BetDetail WHERE BetID = @id";

            var ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql,
                new SqlParameter("@id", id));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            return ds.Tables[0];
        }

        public static void InsertBetDetail(int id, string detailName, string detailValue, SqlTransaction trans)
        {
            var sql = "INSERT INTO AcnCasino_BetDetail VALUES (@id, @detailName, @detailValue)";

            SqlParameter[] para =
            {
                new SqlParameter("@id", id),
                new SqlParameter("@detailName", detailName),
                new SqlParameter("@detailValue", detailValue)
            };

            if (trans != null)
                SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, para);
            else
                SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static void CleanBetDetail(SqlTransaction trans)
        {
            var sql = "DELETE FROM AcnCasino_BetDetail WHERE BetID NOT IN (SELECT [ID] FROM AcnCasino_Bet)";

            if (trans != null)
                SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql);
            else
                SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql);
        }
    }
}
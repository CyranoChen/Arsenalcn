using System.Data;
using System.Data.SqlClient;

using Arsenalcn.Common;
using Microsoft.ApplicationBlocks.Data;

namespace Arsenalcn.CasinoSys.DataAccess
{
    public class BetDetail
    {
        public static DataTable GetBetDetailByBetID(int betID)
        {
            var sql = "SELECT * FROM AcnCasino_BetDetail WHERE BetID = @betID";

            var ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@betID", betID));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }

        public static void InsertBetDetail(int betID, string detailName, string detailValue, SqlTransaction trans)
        {
            var sql = "INSERT INTO AcnCasino_BetDetail VALUES (@betID, @detailName, @detailValue)";

            SqlParameter[] para = { new SqlParameter("@betID", betID), new SqlParameter("@detailName", detailName), new SqlParameter("@detailValue", detailValue) };

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

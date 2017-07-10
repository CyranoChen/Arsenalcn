using System;
using System.Data;
using System.Data.SqlClient;
using Arsenalcn.Common;
using Microsoft.ApplicationBlocks.Data;

namespace Arsenalcn.CasinoSys.DataAccess
{
    public static class ChoiceOption
    {
        public static DataTable GetChoiceOptions(Guid itemGuid)
        {
            var sql = "SELECT * FROM AcnCasino_ChoiceOption WHERE CasinoItemGuid = @guid ORDER BY OptionOrder";

            var ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql,
                new SqlParameter("@guid", itemGuid));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            return ds.Tables[0];
        }

        public static void InsertChoiceOption(Guid itemGuid, string display, string value, float rate,
            int order, SqlTransaction trans)
        {
            var sql = "INSERT INTO AcnCasino_ChoiceOption VALUES (@guid, @order, @display, @value, @rate)";

            //Decimal.ToSingle(Math.Round(new decimal(optionRate), 2));

            SqlParameter[] para =
            {
                new SqlParameter("@guid", itemGuid), new SqlParameter("@order", order),
                new SqlParameter("@display", display), new SqlParameter("@value", value),
                new SqlParameter("@rate", rate)
            };

            if (trans != null)
                SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, para);
            else
                SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        //public static void CleanChoiceOption(SqlTransaction trans)
        //{
        //    var sql =
        //        "DELETE FROM AcnCasino_ChoiceOption WHERE (CasinoItemGuid NOT IN (SELECT CasinoItemGuid FROM AcnCasino_CasinoItem))";

        //    if (trans != null)
        //        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql);
        //    else
        //        SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql);
        //}

        public static float GetOptionTotalBet(Guid itemGuid, string name)
        {
            var sql = @"SELECT ISNULL(SUM(BETAmount), 0) FROM dbo.AcnCasino_Bet bet
                        INNER JOIN dbo.AcnCasino_BetDetail detail
                        ON bet.[ID] = detail.[BetID]
                        WHERE bet.CasinoItemGuid = @guid AND detail.DetailName = @name";

            var obj = SqlHelper.ExecuteScalar(SQLConn.GetConnection(), CommandType.Text, sql,
                new SqlParameter("@guid", itemGuid), new SqlParameter("@name", name));

            return obj.Equals(DBNull.Value) ? 0f : Convert.ToSingle(obj);
        }

        public static int GetOptionTotalCount(Guid itemGuid, string name)
        {
            var sql = @"SELECT COUNT(*) FROM dbo.AcnCasino_Bet bet
                        INNER JOIN dbo.AcnCasino_BetDetail detail
                        ON bet.[ID] = detail.[BetID]
                        WHERE bet.CasinoItemGuid = @guid AND detail.DetailName = @name";

            var obj = SqlHelper.ExecuteScalar(SQLConn.GetConnection(), CommandType.Text, sql,
                new SqlParameter("@guid", itemGuid), new SqlParameter("@name", name));

            return obj.Equals(DBNull.Value) ? 0 : Convert.ToInt32(obj);
        }
    }
}
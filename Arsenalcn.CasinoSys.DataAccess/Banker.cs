using System;
using System.Data;
using System.Data.SqlClient;
using Arsenalcn.Common;
using Microsoft.ApplicationBlocks.Data;

namespace Arsenalcn.CasinoSys.DataAccess
{
    public class Banker
    {
        public static DataRow GetBankerByID(Guid bankerID)
        {
            var sql = "SELECT * FROM dbo.AcnCasino_Banker WHERE [ID] = @bankerID";

            var ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql,
                new SqlParameter("@bankerID", bankerID));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            return ds.Tables[0].Rows[0];
        }

        public static void UpdateBankerCash(Guid bankerID, float cash, SqlTransaction trans)
        {
            var sql = "UPDATE dbo.AcnCasino_Banker SET Cash = @cash WHERE [ID] = @bankerID";

            if (trans == null)
                SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql,
                    new SqlParameter("@cash", cash), new SqlParameter("@bankerID", bankerID));
            else
                SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, new SqlParameter("@cash", cash),
                    new SqlParameter("@bankerID", bankerID));
        }

        public static DataTable GetAllBankers()
        {
            var sql = "SELECT * FROM dbo.AcnCasino_Banker ORDER BY Cash DESC";

            var ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            return ds.Tables[0];
        }

        public static DataTable GetAllBankers(bool isActive)
        {
            var sql = "SELECT * FROM dbo.AcnCasino_Banker WHERE IsActive = @isActive ORDER BY Cash DESC";

            var ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql,
                new SqlParameter("@isActive", isActive));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            return ds.Tables[0];
        }
    }
}
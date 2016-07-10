using System;
using System.Data;
using System.Data.SqlClient;
using Arsenalcn.Common;
using Microsoft.ApplicationBlocks.Data;

namespace Arsenalcn.CasinoSys.DataAccess
{
    public static class Banker
    {
        public static DataRow GetBankerById(Guid key)
        {
            var sql = "SELECT * FROM dbo.AcnCasino_Banker WHERE [ID] = @key";

            var ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql,
                new SqlParameter("@key", key));

            if (ds.Tables[0].Rows.Count == 0) { return null; }

            return ds.Tables[0].Rows[0];
        }

        public static void UpdateBankerCash(Guid key, float cash, SqlTransaction trans)
        {
            var sql = "UPDATE dbo.AcnCasino_Banker SET Cash = @cash WHERE [ID] = @key";

            if (trans == null)
                SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql,
                    new SqlParameter("@cash", cash), new SqlParameter("@key", key));
            else
                SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, new SqlParameter("@cash", cash),
                    new SqlParameter("@key", key));
        }

        //public static DataTable GetAllBankers()
        //{
        //    var sql = "SELECT * FROM dbo.AcnCasino_Banker ORDER BY Cash DESC";

        //    var ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

        //    if (ds.Tables[0].Rows.Count == 0)
        //        return null;
        //    return ds.Tables[0];
        //}

        //public static DataTable GetAllBankers(bool isActive)
        //{
        //    var sql = "SELECT * FROM dbo.AcnCasino_Banker WHERE IsActive = @isActive ORDER BY Cash DESC";

        //    var ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql,
        //        new SqlParameter("@isActive", isActive));

        //    if (ds.Tables[0].Rows.Count == 0)
        //        return null;
        //    return ds.Tables[0];
        //}
    }
}
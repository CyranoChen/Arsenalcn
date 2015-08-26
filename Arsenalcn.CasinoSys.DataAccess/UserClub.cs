using System;
using System.Data;
using System.Data.SqlClient;

using Arsenalcn.Common;
using Microsoft.ApplicationBlocks.Data;

namespace Arsenalcn.CasinoSys.DataAccess
{
    public class UserClub
    {
        public static DataRow GetUserClubHistoryInfo(int userID, DateTime betTime)
        {
            var sql = @"SELECT dbo.AcnClub_Club.FullName AS ClubName, dbo.AcnClub_RelationUserClub.* FROM dbo.AcnClub_Club 
                                  INNER JOIN dbo.AcnClub_RelationUserClub ON dbo.AcnClub_Club.ClubUid = dbo.AcnClub_RelationUserClub.ClubUid 
                                  WHERE (dbo.AcnClub_RelationUserClub.UserID = @userID) AND (dbo.AcnClub_RelationUserClub.FromDate < @betTime) AND 
                                  (ISNULL(dbo.AcnClub_RelationUserClub.ToDate, GETDATE()) > @betTime)";

            var ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@userID", userID), new SqlParameter("@betTime", betTime));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0].Rows[0];
        }

        public static DataTable GetAllClubs()
        {
            var sql = "SELECT * FROM AcnClub_Club WHERE IsActive = 1 ORDER BY RankScore DESC";

            var ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }
    }
}

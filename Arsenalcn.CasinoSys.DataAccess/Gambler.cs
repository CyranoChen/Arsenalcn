using System;
using System.Data;
using System.Data.SqlClient;
using Arsenalcn.Common;
using Microsoft.ApplicationBlocks.Data;

namespace Arsenalcn.CasinoSys.DataAccess
{
    public static class Gambler
    {
        //public static DataRow GetGamblerByID(int gID)
        //{
        //    var sql = "SELECT * FROM dbo.AcnCasino_Gambler WHERE ID = @gID";

        //    var ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql,
        //        new SqlParameter("@gID", gID));

        //    if (ds.Tables[0].Rows.Count == 0)
        //        return null;
        //    return ds.Tables[0].Rows[0];
        //}

        public static DataRow GetGamblerByUserId(int id, SqlTransaction trans = null)
        {
            var sql = "SELECT * FROM dbo.AcnCasino_Gambler WHERE UserID = @id";

            DataSet ds;

            if (trans == null)
            {
                ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql,
                    new SqlParameter("@id", id));
            }
            else
            {
                ds = SqlHelper.ExecuteDataset(trans, CommandType.Text, sql, new SqlParameter("@id", id));
            }

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            return ds.Tables[0].Rows[0];
        }

        public static void UpdateGambler(int gID, int userID, string userName, float cash, float totalBet, int win,
            int lose, int? rpBonus, int? contestRank,
            int totalRank, int? banker, DateTime joinDate, bool isActive, string description, string remark,
            SqlTransaction trans = null)
        {
            var sql =
                @"UPDATE dbo.AcnCasino_Gambler SET UserID = @userID, UserName = @userName, Cash = @cash, TotalBet = @totalBet, 
                                  Win = @win, Lose = @lose, RPBonus = @rpBonus, ContestRank = @contestRank, TotalRank = @totalRank, Banker = @banker,
                                  JoinDate = @joinDate, IsActive = @isActive, [Description] = @description, Remark = @remark WHERE ID = @gID";

            SqlParameter[] para =
            {
                new SqlParameter("@gID", gID),
                new SqlParameter("@userID", userID),
                new SqlParameter("@userName", userName),
                new SqlParameter("@cash", cash),
                new SqlParameter("@totalBet", totalBet),
                new SqlParameter("@win", win),
                new SqlParameter("@lose", lose),
                new SqlParameter("@rpBonus", !rpBonus.HasValue ? DBNull.Value : (object) rpBonus.Value),
                new SqlParameter("@contestRank", !contestRank.HasValue ? DBNull.Value : (object) contestRank.Value),
                new SqlParameter("@totalRank", totalRank),
                new SqlParameter("@banker", !banker.HasValue ? DBNull.Value : (object) banker.Value),
                new SqlParameter("@joinDate", joinDate),
                new SqlParameter("@isActive", isActive),
                new SqlParameter("@description", description),
                new SqlParameter("@remark", remark)
            };

            if (trans == null)
            {
                SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
            }
            else
            {
                SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, para);
            }
        }

        public static int InsertGambler(int gID, int userID, string userName, float cash, float totalBet, int win,
            int lose, int? rpBonus, int? contestRank,
            int totalRank, int? banker, DateTime joinDate, bool isActive, string description, string remark,
            SqlTransaction trans = null)
        {
            var sql =
                @"INSERT INTO dbo.AcnCasino_Gambler (UserID, UserName, Cash, TotalBet, Win, Lose, RPBonus, ContestRank, 
                                 TotalRank, Banker, JoinDate, IsActive, Description, Remark)  
                                 VALUES (@userID, @userName, @cash, @totalBet, @win, @lose, @rpBonus, @ContestRank, 
                                 @totalRank, @banker, @joinDate, @isActive, @description, @remark);
                                 SELECT SCOPE_IDENTITY();";

            SqlParameter[] para =
            {
                new SqlParameter(),
                new SqlParameter("@userID", userID),
                new SqlParameter("@userName", userName),
                new SqlParameter("@cash", cash),
                new SqlParameter("@totalBet", totalBet),
                new SqlParameter("@win", win),
                new SqlParameter("@lose", lose),
                new SqlParameter("@rpBonus", !rpBonus.HasValue ? DBNull.Value : (object) rpBonus.Value),
                new SqlParameter("@contestRank", !contestRank.HasValue ? DBNull.Value : (object) contestRank.Value),
                new SqlParameter("@totalRank", totalRank),
                new SqlParameter("@banker", !banker.HasValue ? DBNull.Value : (object) banker.Value),
                new SqlParameter("@joinDate", joinDate),
                new SqlParameter("@isActive", isActive),
                new SqlParameter("@description", description),
                new SqlParameter("@remark", remark)
            };

            if (trans == null)
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(SQLConn.GetConnection(), CommandType.Text, sql, para));
            }

            return Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sql, para));
        }

        //public static void DeleteGambler(int gID, SqlTransaction trans = null)
        //{
        //    var sql = "DELETE dbo.AcnCasino_Gambler WHERE ID = @gID";

        //    SqlParameter[] para = {new SqlParameter("@gID", gID)};

        //    if (trans == null)
        //    {
        //        SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        //    }
        //    else
        //    {
        //        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, para);
        //    }
        //}

        public static DataTable GetGamblers()
        {
            var sql =
                @"SELECT  ID, UserID, UserName, Cash, TotalBet, Win, Lose, RPBonus, ContestRank, TotalRank, Banker, JoinDate, IsActive, Description, Remark  
                                  FROM AcnCasino_Gambler Order By TotalBet DESC, Cash DESC, WIN DESC, LOSE DESC, ID";

            var ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            return ds.Tables[0];
        }

        public static int GetGamblerCount()
        {
            var sql = "SELECT COUNT(ID) FROM AcnCasino_Gambler";

            var obj = SqlHelper.ExecuteScalar(SQLConn.GetConnection(), CommandType.Text, sql);

            return obj.Equals(DBNull.Value) ? 0 : Convert.ToInt32(obj);
        }

        public static DataTable GetGamblerProfitView()
        {
            var sql = @"SELECT BetInfo.*, RPInfo.RPBet, RPInfo.RPBonus FROM
                                    (SELECT UserID, UserName, 
                                                COUNT(CASE IsWin WHEN 1 THEN 1 ELSE NULL END) AS Win, 
                                                COUNT(CASE IsWin WHEN 0 THEN 0 ELSE NULL END) AS Lose, 
                                                COUNT(distinct CAST(CasinoItemGuid AS CHAR(50))) AS MatchBet, 
                                                SUM(ISNULL(Earning, 0)) AS Earning, 
                                                SUM(ISNULL(BetAmount, 0)) AS TotalBet
                                    FROM dbo.vw_AcnCasino_BetInfo WHERE (Earning IS NOT NULL) AND (BetAmount IS NOT NULL) AND (ItemType = 2) 
                                    GROUP BY UserID, UserName) AS BetInfo
                                LEFT OUTER JOIN
                                    (SELECT UserID, UserName, 
                                                COUNT(ID) AS RPBet, 
                                                COUNT(CASE EarningDesc WHEN 'RP+1' THEN 1 ELSE NULL END) AS RPBonus
                                    FROM dbo.vw_AcnCasino_BetInfo WHERE (Earning = 0) AND (BetAmount IS NULL) AND (ItemType = 1) 
                                    GROUP BY UserID, UserName) AS RPInfo
                                ON BetInfo.UserID = RPInfo.UserID AND BetInfo.UserName = RPInfo.UserName
                                ORDER BY BetInfo.TotalBet DESC";

            var ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            return ds.Tables[0];
        }

        public static DataTable GetGamblerProfitView(Guid leagueGuid)
        {
            var sql = @"SELECT BetInfo.*, RPInfo.RPBet, RPInfo.RPBonus FROM 
                                    (SELECT UserID, UserName, 
                                                COUNT(CASE IsWin WHEN 1 THEN 1 ELSE NULL END) AS Win, 
                                                COUNT(CASE IsWin WHEN 0 THEN 0 ELSE NULL END) AS Lose, 
                                                COUNT(distinct CAST(CasinoItemGuid AS CHAR(50))) AS MatchBet, 
                                                SUM(ISNULL(Earning, 0)) AS Earning, 
                                                SUM(ISNULL(BetAmount, 0)) AS TotalBet
                                    FROM dbo.vw_AcnCasino_BetInfo 
                                    WHERE (Earning IS NOT NULL) AND (BetAmount IS NOT NULL) AND (ItemType = 2) AND (LeagueGuid = @leagueGuid)
                                    GROUP BY UserID, UserName) AS BetInfo
                                LEFT OUTER JOIN
                                    (SELECT UserID, UserName, 
                                                COUNT(ID) AS RPBet, 
                                                COUNT(CASE EarningDesc WHEN 'RP+1' THEN 1 ELSE NULL END) AS RPBonus
                                    FROM dbo.vw_AcnCasino_BetInfo 
                                    WHERE (Earning = 0) AND (BetAmount IS NULL) AND (ItemType = 1) AND (LeagueGuid = @leagueGuid)
                                    GROUP BY UserID, UserName) AS RPInfo
                                ON BetInfo.UserID = RPInfo.UserID AND BetInfo.UserName = RPInfo.UserName
                                ORDER BY BetInfo.TotalBet DESC";

            var ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql,
                new SqlParameter("@leagueGuid", leagueGuid));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            return ds.Tables[0];
        }

        public static float GetGamblerTotalBetByUserID(int userid)
        {
            var sql = @"SELECT SUM(ISNULL(BetAmount, 0)) AS TotalBet FROM dbo.vw_AcnCasino_BetInfo 
                                  WHERE (Earning IS NOT NULL) AND (BetAmount IS NOT NULL) AND (ItemType = 2) AND (UserID = @userid)";

            var obj = SqlHelper.ExecuteScalar(SQLConn.GetConnection(), CommandType.Text, sql,
                new SqlParameter("@userid", userid));

            return obj.Equals(DBNull.Value) ? 0f : Convert.ToSingle(obj);
        }

        public static float GetGamblerTotalBetByUserID(int userid, Guid leagueGuid)
        {
            var sql = @"SELECT SUM(ISNULL(BetAmount, 0)) AS TotalBet FROM dbo.vw_AcnCasino_BetInfo 
                                  WHERE (Earning IS NOT NULL) AND (BetAmount IS NOT NULL) AND (ItemType = 2)  AND (UserID = @userid) AND (LeagueGuid = @leagueGuid)";

            var obj = SqlHelper.ExecuteScalar(SQLConn.GetConnection(), CommandType.Text, sql,
                new SqlParameter("@userid", userid), new SqlParameter("@leagueGuid", leagueGuid));

            return obj.Equals(DBNull.Value) ? 0f : Convert.ToSingle(obj);
        }

        //public static int GetGamblerRPByUserID(int userid)
        //{
        //    var sql =
        //        @"SELECT COUNT(*) AS RPBonus FROM dbo.AcnCasino_Bet WHERE Earning = 0 AND EarningDesc = 'RP+1' AND IsWin = 1 AND UserID = @userid";

        //    var obj = SqlHelper.ExecuteScalar(SQLConn.GetConnection(), CommandType.Text, sql,
        //        new SqlParameter("@userid", userid));

        //    return obj.Equals(DBNull.Value) ? 0 : Convert.ToInt32(obj);
        //}

        //public static int GetGamblerRPByUserID(int userid, Guid leagueGuid)
        //{
        //    var sql =
        //        @"SELECT COUNT(*) AS RPBonus FROM dbo.vw_AcnCasino_BetInfo WHERE Earning = 0 AND EarningDesc = 'RP+1' AND IsWin = 1  AND (ItemType = 1) AND UserID = @userid AND LeagueGuid = @guid";

        //    var obj = SqlHelper.ExecuteScalar(SQLConn.GetConnection(), CommandType.Text, sql,
        //        new SqlParameter("@userid", userid), new SqlParameter("@guid", leagueGuid));

        //    return obj.Equals(DBNull.Value) ? 0 : Convert.ToInt32(obj);
        //}
    }
}
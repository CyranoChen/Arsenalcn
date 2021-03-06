﻿using System;
using System.Data;
using System.Data.SqlClient;
using Arsenalcn.Common;
using Microsoft.ApplicationBlocks.Data;

namespace Arsenalcn.CasinoSys.DataAccess
{
    public static class Bet
    {
        public static DataRow GetBetById(int key)
        {
            var sql = "SELECT * FROM AcnCasino_Bet WHERE [ID] = @key";

            var ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql,
                new SqlParameter("@key", key));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            return ds.Tables[0].Rows[0];
        }

        public static int InsertBet(int userid, string username, Guid casinoItemGuid, float? betAmount, float? betRate,
            SqlTransaction trans)
        {
            var sql =
                "INSERT INTO AcnCasino_Bet VALUES (@userid, @username, @casinoItemGuid, @betAmount, GETDATE(), @betRate, NULL, NULL, NULL); SELECT SCOPE_IDENTITY();";

            SqlParameter[] para =
            {
                new SqlParameter("@userid", userid), new SqlParameter("@username", username),
                new SqlParameter("@casinoItemGuid", casinoItemGuid), new SqlParameter("@betAmount", betAmount),
                new SqlParameter("@betRate", betRate)
            };

            object obj;

            if (trans != null)
            {
                obj = SqlHelper.ExecuteScalar(trans, CommandType.Text, sql, para);
            }
            else
            {
                obj = SqlHelper.ExecuteScalar(SQLConn.GetConnection(), CommandType.Text, sql, para);
            }

            return Convert.ToInt32(obj);
        }

        public static void UpdateBet(int key, bool isWin, float earning, string earningDesc, SqlTransaction trans)
        {
            var sql =
                "UPDATE AcnCasino_Bet SET IsWin = @isWin, Earning = @earning, EarningDesc = @earningDesc WHERE [ID] = @key";

            SqlParameter[] para =
            {
                new SqlParameter("@isWin", isWin), new SqlParameter("@earning", earning),
                new SqlParameter("@earningDesc", earningDesc), new SqlParameter("@key", key)
            };

            if (trans != null)
                SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, para);
            else
                SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static void DeleteBetByMatchGuid(Guid matchGuid, SqlTransaction trans)
        {
            var sql =
                "DELETE AcnCasino_Bet WHERE CasinoItemGuid in (SELECT CasinoItemGuid FROM dbo.AcnCasino_CasinoItem WHERE MatchGuid = @matchGuid)";

            SqlParameter[] para = {new SqlParameter("@matchGuid", matchGuid)};

            if (trans != null)
                SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, para);
            else
                SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static void DeleteBetById(int key, SqlTransaction trans)
        {
            var sql = "DELETE AcnCasino_Bet WHERE [ID] = @key";

            SqlParameter[] para = {new SqlParameter("@key", key) };

            if (trans != null)
                SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, para);
            else
                SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        //public static void CleanBet(SqlTransaction trans)
        //{
        //    var sql =
        //        "DELETE FROM dbo.AcnCasino_Bet WHERE (CasinoItemGuid NOT IN (SELECT CasinoItemGuid FROM dbo.AcnCasino_CasinoItem))";

        //    if (trans != null)
        //        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql);
        //    else
        //        SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql);
        //}

        public static float GetUserMatchTotalBet(int userid, Guid matchGuid)
        {
            var sql = @"SELECT ISNULL(SUM(betAmount), 0) FROM dbo.AcnCasino_Bet
                        WHERE UserID = @userid AND CasinoItemGuid IN (SELECT CasinoItemGuid FROM dbo.AcnCasino_CasinoItem WHERE MatchGuid = @guid)";

            var obj = SqlHelper.ExecuteScalar(SQLConn.GetConnection(), CommandType.Text, sql,
                new SqlParameter("@userid", userid), new SqlParameter("@guid", matchGuid));

            return obj.Equals(DBNull.Value) ? 0f : Convert.ToSingle(obj);
        }

        public static DataTable GetUserMatchAllBet(int userid, Guid matchGuid)
        {
            var sql = @"SELECT * FROM dbo.AcnCasino_Bet WHERE UserID = @userid
                        AND CasinoItemGuid IN (SELECT CasinoItemGuid FROM dbo.AcnCasino_CasinoItem WHERE MatchGuid = @guid) ORDER BY BetTime Desc";

            var ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql,
                new SqlParameter("@userid", userid), new SqlParameter("@guid", matchGuid));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            return ds.Tables[0];
        }

        public static DataTable GetUserCasinoItemAllBet(int userid, Guid itemGuid)
        {
            var sql = @"SELECT * FROM dbo.AcnCasino_Bet WHERE UserID = @userid
                        AND CasinoItemGuid = @guid ORDER BY BetTime Desc";

            var ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql,
                new SqlParameter("@userid", userid), new SqlParameter("@guid", itemGuid));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            return ds.Tables[0];
        }

        public static DataTable GetMatchAllBet(Guid matchGuid)
        {
            var sql = @"SELECT * FROM dbo.AcnCasino_Bet
                        WHERE CasinoItemGuid IN (SELECT CasinoItemGuid FROM dbo.AcnCasino_CasinoItem WHERE MatchGuid = @guid) ORDER BY BetTime Desc";

            var ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql,
                new SqlParameter("@guid", matchGuid));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            return ds.Tables[0];
        }

        public static DataTable GetBetByCasinoItemGuid(Guid itemGuid, SqlTransaction trans)
        {
            var sql = "SELECT * FROM dbo.AcnCasino_Bet WHERE CasinoItemGuid = @guid ORDER BY BetTime desc";

            DataSet ds;

            if (trans == null)
                ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql,
                    new SqlParameter("@guid", itemGuid));
            else
                ds = SqlHelper.ExecuteDataset(trans, CommandType.Text, sql, new SqlParameter("@guid", itemGuid));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            return ds.Tables[0];
        }

        //public static float GetTotalEarningByCasinoItemGuid(Guid itemGuid)
        //{
        //    var sql =
        //        "SELECT ISNULL(SUM(Bet), 0) - ISNULL(SUM(Earning), 0) AS TotalEarning FROM dbo.AcnCasino_Bet WHERE CasinoItemGuid = @itemGuid";

        //    var obj = SqlHelper.ExecuteScalar(SQLConn.GetConnection(), CommandType.Text, sql,
        //        new SqlParameter("@itemGuid", itemGuid));

        //    return obj.Equals(DBNull.Value) ? 0f : Convert.ToSingle(obj);
        //}

        //public static float GetTotalEarningByBankerGuid(Guid bankerGuid)
        //{
        //    var sql =
        //        @"SELECT ISNULL(SUM(dbo.AcnCasino_Bet.Bet), 0) - ISNULL(SUM(dbo.AcnCasino_Bet.Earning), 0) AS BankerCash
        //                   FROM dbo.AcnCasino_CasinoItem INNER JOIN dbo.AcnCasino_Bet ON dbo.AcnCasino_CasinoItem.CasinoItemGuid = dbo.AcnCasino_Bet.CasinoItemGuid
        //                   WHERE (dbo.AcnCasino_CasinoItem.BankerID = @bankerGuid)";

        //    var obj = SqlHelper.ExecuteScalar(SQLConn.GetConnection(), CommandType.Text, sql,
        //        new SqlParameter("@bankerGuid", bankerGuid));

        //    return obj.Equals(DBNull.Value) ? 0f : Convert.ToSingle(obj);
        //}

        public static DataTable GetAllBetByTimeDiff(int timeDiff)
        {
            var sql =
                "SELECT * FROM dbo.AcnCasino_Bet WHERE DATEADD(DAY, @diff, BetTime) >= GETDATE() ORDER BY BetTime DESC";

            var ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql,
                new SqlParameter("@diff", timeDiff));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            return ds.Tables[0];
        }

        //public static float GetUserTotalBetCash(int userid)
        //{
        //    var sql = "SELECT ISNULL(SUM(Bet), 0) FROM dbo.AcnCasino_Bet WHERE UserID = @userid";

        //    var obj = SqlHelper.ExecuteScalar(SQLConn.GetConnection(), CommandType.Text, sql,
        //        new SqlParameter("@userid", userid));

        //    return obj.Equals(DBNull.Value) ? 0f : Convert.ToSingle(obj);
        //}

        public static float GetUserTotalWinCash(int userid)
        {
            var sql = "SELECT ISNULL(SUM(Earning), 0) FROM dbo.AcnCasino_Bet WHERE UserID = @userid";

            var obj = SqlHelper.ExecuteScalar(SQLConn.GetConnection(), CommandType.Text, sql,
                new SqlParameter("@userid", userid));

            return obj.Equals(DBNull.Value) ? 0f : Convert.ToSingle(obj);
        }

        //public static int GetUserTotalWinLoseCount(int userid, bool isWin)
        //{
        //    var sql = "SELECT COUNT(*) FROM dbo.AcnCasino_Bet WHERE UserID = @userid AND IsWin = @isWin";

        //    var obj = SqlHelper.ExecuteScalar(SQLConn.GetConnection(), CommandType.Text, sql,
        //        new SqlParameter("@userid", userid), new SqlParameter("@isWin", isWin));

        //    return Convert.ToInt16(obj);
        //}

        public static float GetMatchTopBet(Guid matchGuid)
        {
            var sql = @"SELECT MAX(BetAmount) AS topBet FROM dbo.AcnCasino_Bet WHERE CasinoItemGuid IN 
                        (SELECT CasinoItemGuid FROM dbo.AcnCasino_CasinoItem WHERE MatchGuid = @guid)";

            var obj = SqlHelper.ExecuteScalar(SQLConn.GetConnection(), CommandType.Text, sql,
                new SqlParameter("@guid", matchGuid));

            if (Convert.IsDBNull(obj) || obj == null)
                return 0;

            return obj.Equals(DBNull.Value) ? 0f : Convert.ToSingle(obj);
        }

        public static float GetMatchTopEarning(Guid matchGuid)
        {
            var sql = @"SELECT MAX(Earning) AS topBet FROM dbo.AcnCasino_Bet WHERE CasinoItemGuid IN 
                        (SELECT CasinoItemGuid FROM dbo.AcnCasino_CasinoItem WHERE MatchGuid = @guid)";

            var obj = SqlHelper.ExecuteScalar(SQLConn.GetConnection(), CommandType.Text, sql,
                new SqlParameter("@guid", matchGuid));

            if (Convert.IsDBNull(obj) || obj == null)
                return 0;

            return obj.Equals(DBNull.Value) ? 0f : Convert.ToSingle(obj);
        }

        public static DataTable GetUserBetHistoryView(int userid)
        {
            //            string sql = @"SELECT Bet.*, teamH.TeamDisplayName as HomeDisplay, teamA.TeamDisplayName as AwayDisplay,
            //                        teamH.Ground, teamH.Capacity, match.*
            //                        FROM dbo.AcnCasino_Bet bet
            //                        INNER JOIN dbo.AcnCasino_CasinoItem item
            //                        ON bet.CasinoItemGuid = item.CasinoItemGuid
            //                        INNER JOIN dbo.AcnCasino_Match match
            //                        ON match.MatchGuid = item.MatchGuid
            //                        INNER JOIN arsenal_team teamH
            //                        ON match.home = teamH.TeamGuid
            //                        INNER JOIN arsenal_team teamA
            //                        ON match.away = teamA.TeamGuid
            //                        WHERE UserID = @userid AND item.MatchGuid IS NOT NULL
            //                        ORDER BY BetTime desc";

            var sql = @"SELECT bet.*, match.*, item.*
                        FROM dbo.AcnCasino_Bet bet
                        INNER JOIN dbo.AcnCasino_CasinoItem item
                        ON bet.CasinoItemGuid = item.CasinoItemGuid
                        INNER JOIN dbo.AcnCasino_Match match
                        ON match.MatchGuid = item.MatchGuid
                        WHERE UserID = @userid AND item.MatchGuid IS NOT NULL
                        ORDER BY BetTime desc";

            var ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql,
                new SqlParameter("@userid", userid));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            return ds.Tables[0];
        }

        public static DataTable GetUserBetMatch(int userid)
        {
//            string sql = @"SELECT match.*, teamH.TeamDisplayName as HomeDisplay, teamA.TeamDisplayName as AwayDisplay,
//                        teamH.Ground, teamH.Capacity
//                        FROM (SELECT DISTINCT item.MatchGuid
//                        FROM dbo.AcnCasino_Bet bet
//                        INNER JOIN dbo.AcnCasino_CasinoItem item
//                        ON bet.CasinoItemGuid = item.CasinoItemGuid AND item.Earning IS NOT NULL
//                        WHERE UserID = @userid AND MatchGuid IS NOT NULL) betMatch
//                        INNER JOIN dbo.AcnCasino_Match match
//                        ON match.MatchGuid = betMatch.MatchGuid AND match.ResultHome IS NOT NULL AND match.ResultAway IS NOT NULL
//                        INNER JOIN arsenal_team teamH
//                        ON match.home = teamH.TeamGuid
//                        INNER JOIN arsenal_team teamA
//                        ON match.away = teamA.TeamGuid
//                        ORDER BY match.PlayTime desc";

            var sql = @"SELECT match.*
                        FROM (SELECT DISTINCT item.MatchGuid
                        FROM dbo.AcnCasino_Bet bet
                        INNER JOIN dbo.AcnCasino_CasinoItem item
                        ON bet.CasinoItemGuid = item.CasinoItemGuid AND item.Earning IS NOT NULL
                        WHERE UserID = @userid AND MatchGuid IS NOT NULL) betMatch
                        INNER JOIN dbo.AcnCasino_Match match
                        ON match.MatchGuid = betMatch.MatchGuid AND match.ResultHome IS NOT NULL AND match.ResultAway IS NOT NULL
                        ORDER BY match.PlayTime desc";

            var ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql,
                new SqlParameter("@userid", userid));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            return ds.Tables[0];
        }

        //public static DateTime GetLastBetTime()
        //{
        //    var sql =
        //        @"SELECT TOP 1 BetTime FROM AcnCasino_Bet WHERE (Bet IS NOT NULL) AND (Earning IS NOT NULL) ORDER BY BetTime";
        //    try
        //    {
        //        return Convert.ToDateTime(SqlHelper.ExecuteScalar(SQLConn.GetConnection(), CommandType.Text, sql));
        //    }
        //    catch
        //    {
        //        return DateTime.MinValue;
        //    }
        //}
    }
}
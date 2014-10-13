using System;
using System.Data;
using System.Data.SqlClient;

using Arsenalcn.Common;
using Microsoft.ApplicationBlocks.Data;

namespace Arsenalcn.CasinoSys.DataAccess
{
    public class Bet
    {
        public static DataRow GetBetByID(int betID)
        {
            string sql = "SELECT * FROM AcnCasino_Bet WHERE [ID] = @betID";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@betID", betID));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0].Rows[0];
        }

        public static int InsertBet(int userid, string username, Guid casinoItemGuid, float? bet, float? betRate, SqlTransaction trans)
        {
            string sql = "INSERT INTO AcnCasino_Bet VALUES (@userid, @username, @casinoItemGuid, @bet, GETDATE(), @betRate, NULL, NULL, NULL); SELECT SCOPE_IDENTITY();";

            SqlParameter[] para = { new SqlParameter("@userid", userid), new SqlParameter("@username", username), new SqlParameter("@casinoItemGuid", casinoItemGuid), new SqlParameter("@bet", bet), new SqlParameter("@betRate", betRate) };

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

        public static void UpdateBet(int betID, bool isWin, float earning, string earningDesc, SqlTransaction trans)
        {
            string sql = "UPDATE AcnCasino_Bet SET IsWin = @isWin, Earning = @earning, EarningDesc = @earningDesc WHERE [ID] = @betID";

            SqlParameter[] para = { new SqlParameter("@isWin", isWin), new SqlParameter("@earning", earning), new SqlParameter("@earningDesc", earningDesc), new SqlParameter("@betID", betID) };

            if (trans != null)
                SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, para);
            else
                SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static void DeleteBetByMatchGuid(Guid matchGuid, SqlTransaction trans)
        {
            string sql = "DELETE AcnCasino_Bet WHERE CasinoItemGuid in (SELECT CasinoItemGuid FROM dbo.AcnCasino_CasinoItem WHERE MatchGuid = @matchGuid)";

            SqlParameter[] para = { new SqlParameter("@matchGuid", matchGuid) };

            if (trans != null)
                SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, para);
            else
                SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static void DeleteBetByID(int betID, SqlTransaction trans)
        {
            string sql = "DELETE AcnCasino_Bet WHERE [ID] = @betID";

            SqlParameter[] para = { new SqlParameter("@betID", betID) };

            if (trans != null)
                SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, para);
            else
                SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static void CleanBet(SqlTransaction trans)
        {
            string sql = "DELETE FROM dbo.AcnCasino_Bet WHERE (CasinoItemGuid NOT IN (SELECT CasinoItemGuid FROM dbo.AcnCasino_CasinoItem))";

            if (trans != null)
                SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql);
            else
                SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql);
        }

        public static float GetUserMatchTotalBet(int userid, Guid matchGuid)
        {
            string sql = @"SELECT ISNULL(SUM(bet), 0) FROM dbo.AcnCasino_Bet
                        WHERE UserID = @userid AND CasinoItemGuid IN (SELECT CasinoItemGuid FROM dbo.AcnCasino_CasinoItem WHERE MatchGuid = @guid)";

            Object obj = SqlHelper.ExecuteScalar(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@userid", userid), new SqlParameter("@guid", matchGuid));

            return obj.Equals(DBNull.Value) ? 0f : Convert.ToSingle(obj);
        }

        public static DataTable GetUserMatchAllBet(int userid, Guid matchGuid)
        {
            string sql = @"SELECT * FROM dbo.AcnCasino_Bet WHERE UserID = @userid
                        AND CasinoItemGuid IN (SELECT CasinoItemGuid FROM dbo.AcnCasino_CasinoItem WHERE MatchGuid = @guid) ORDER BY BetTime Desc";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@userid", userid), new SqlParameter("@guid", matchGuid));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }

        public static DataTable GetUserCasinoItemAllBet(int userid, Guid itemGuid)
        {
            string sql = @"SELECT * FROM dbo.AcnCasino_Bet WHERE UserID = @userid
                        AND CasinoItemGuid = @guid ORDER BY BetTime Desc";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@userid", userid), new SqlParameter("@guid", itemGuid));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }

        public static DataTable GetMatchAllBet(Guid matchGuid)
        {
            string sql = @"SELECT * FROM dbo.AcnCasino_Bet
                        WHERE CasinoItemGuid IN (SELECT CasinoItemGuid FROM dbo.AcnCasino_CasinoItem WHERE MatchGuid = @guid) ORDER BY BetTime Desc";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@guid", matchGuid));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }

        public static DataTable GetBetByCasinoItemGuid(Guid itemGuid, SqlTransaction trans)
        {
            string sql = "SELECT * FROM dbo.AcnCasino_Bet WHERE CasinoItemGuid = @guid ORDER BY BetTime desc";

            DataSet ds;

            if (trans == null)
                ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@guid", itemGuid));
            else
                ds = SqlHelper.ExecuteDataset(trans, CommandType.Text, sql, new SqlParameter("@guid", itemGuid));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }

        public static float GetTotalEarningByCasinoItemGuid(Guid itemGuid)
        {
            string sql = "SELECT ISNULL(SUM(Bet), 0) - ISNULL(SUM(Earning), 0) AS TotalEarning FROM dbo.AcnCasino_Bet WHERE CasinoItemGuid = @itemGuid";

            Object obj = SqlHelper.ExecuteScalar(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@itemGuid", itemGuid));

            return obj.Equals(DBNull.Value) ? 0f : Convert.ToSingle(obj);
        }

        public static float GetTotalEarningByBankerGuid(Guid bankerGuid)
        {
            string sql = @"SELECT ISNULL(SUM(dbo.AcnCasino_Bet.Bet), 0) - ISNULL(SUM(dbo.AcnCasino_Bet.Earning), 0) AS BankerCash
                           FROM dbo.AcnCasino_CasinoItem INNER JOIN dbo.AcnCasino_Bet ON dbo.AcnCasino_CasinoItem.CasinoItemGuid = dbo.AcnCasino_Bet.CasinoItemGuid
                           WHERE (dbo.AcnCasino_CasinoItem.BankerID = @bankerGuid)";

            Object obj = SqlHelper.ExecuteScalar(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@bankerGuid", bankerGuid));

            return obj.Equals(DBNull.Value) ? 0f : Convert.ToSingle(obj);
        }

        public static DataTable GetAllBetByTimeDiff(int timeDiff)
        {
            string sql = "SELECT * FROM dbo.AcnCasino_Bet WHERE DATEADD(DAY, @diff, BetTime) >= GETDATE() ORDER BY BetTime DESC";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@diff", timeDiff));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }

        public static float GetUserTotalBetCash(int userid)
        {
            string sql = "SELECT ISNULL(SUM(Bet), 0) FROM dbo.AcnCasino_Bet WHERE UserID = @userid";

            Object obj = SqlHelper.ExecuteScalar(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@userid", userid));

            return obj.Equals(DBNull.Value) ? 0f : Convert.ToSingle(obj);
        }

        public static float GetUserTotalWinCash(int userid)
        {
            string sql = "SELECT ISNULL(SUM(Earning), 0) FROM dbo.AcnCasino_Bet WHERE UserID = @userid";

            Object obj = SqlHelper.ExecuteScalar(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@userid", userid));

            return obj.Equals(DBNull.Value) ? 0f : Convert.ToSingle(obj);
        }

        public static int GetUserTotalWinLoseCount(int userid, bool isWin)
        {
            string sql = "SELECT COUNT(*) FROM dbo.AcnCasino_Bet WHERE UserID = @userid AND IsWin = @isWin";

            Object obj = SqlHelper.ExecuteScalar(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@userid", userid), new SqlParameter("@isWin", isWin));

            return Convert.ToInt16(obj);
        }

        public static float GetMatchTopBet(Guid matchGuid)
        {
            string sql = @"SELECT MAX(Bet) AS topBet FROM dbo.AcnCasino_Bet WHERE CasinoItemGuid IN 
                        (SELECT CasinoItemGuid FROM dbo.AcnCasino_CasinoItem WHERE MatchGuid = @guid)";

            Object obj = SqlHelper.ExecuteScalar(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@guid", matchGuid));

            if (Convert.IsDBNull(obj) || obj == null)
                return 0;

            return obj.Equals(DBNull.Value) ? 0f : Convert.ToSingle(obj);
        }

        public static float GetMatchTopEarning(Guid matchGuid)
        {
            string sql = @"SELECT MAX(Earning) AS topBet FROM dbo.AcnCasino_Bet WHERE CasinoItemGuid IN 
                        (SELECT CasinoItemGuid FROM dbo.AcnCasino_CasinoItem WHERE MatchGuid = @guid)";

            Object obj = SqlHelper.ExecuteScalar(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@guid", matchGuid));

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

            string sql = @"SELECT Bet.*, match.*
                        FROM dbo.AcnCasino_Bet bet
                        INNER JOIN dbo.AcnCasino_CasinoItem item
                        ON bet.CasinoItemGuid = item.CasinoItemGuid
                        INNER JOIN dbo.AcnCasino_Match match
                        ON match.MatchGuid = item.MatchGuid
                        WHERE UserID = @userid AND item.MatchGuid IS NOT NULL
                        ORDER BY BetTime desc";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@userid", userid));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
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

            string sql = @"SELECT match.*
                        FROM (SELECT DISTINCT item.MatchGuid
                        FROM dbo.AcnCasino_Bet bet
                        INNER JOIN dbo.AcnCasino_CasinoItem item
                        ON bet.CasinoItemGuid = item.CasinoItemGuid AND item.Earning IS NOT NULL
                        WHERE UserID = @userid AND MatchGuid IS NOT NULL) betMatch
                        INNER JOIN dbo.AcnCasino_Match match
                        ON match.MatchGuid = betMatch.MatchGuid AND match.ResultHome IS NOT NULL AND match.ResultAway IS NOT NULL
                        ORDER BY match.PlayTime desc";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@userid", userid));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }

        public static DateTime GetLastBetTime()
        {
            string sql = @"SELECT TOP 1 BetTime FROM AcnCasino_Bet WHERE (Bet IS NOT NULL) AND (Earning IS NOT NULL) ORDER BY BetTime";
            try
            {
                return Convert.ToDateTime(SqlHelper.ExecuteScalar(SQLConn.GetConnection(), CommandType.Text, sql));
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
    }
}

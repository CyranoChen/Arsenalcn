using System;
using System.Data;
using System.Data.SqlClient;

using Microsoft.ApplicationBlocks.Data;

namespace Arsenalcn.CasinoSys.DataAccess
{
    public class Gambler
    {
        public static DataRow GetGamblerByUserID(int userid, SqlTransaction trans)
        {
            string sql = "SELECT * FROM AcnCasino_Gambler WHERE UserID = @userid";

            DataSet ds;
            if (trans == null)
                ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@userid", userid));
            else
                ds = SqlHelper.ExecuteDataset(trans, CommandType.Text, sql, new SqlParameter("@userid", userid));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0].Rows[0];
        }

        public static DataTable GetGambler()
        {
            string sql = "SELECT * FROM AcnCasino_Gambler Order By TotalBet DESC, Cash DESC, WIN DESC, LOSE DESC, ID";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }

        public static DataTable GetGambler(string username)
        {
            string sql = @"SELECT * FROM AcnCasino_Gambler WHERE UserName LIKE @username 
                                 Order By TotalBet DESC, Cash DESC, WIN DESC, LOSE DESC, ID";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@username", '%' + username + '%'));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }

        public static void InsertGambler(int userid, string username)
        {
            string sql = "INSERT INTO AcnCasino_Gambler VALUES (@userid, @username, 0, 0, 0, 0, 0, 1)";

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@userid", userid), new SqlParameter("@username", username));
        }

        public static void UpdateGambler(int userid, float cash, float totalBet, int win, int lose, int totalBanker, bool isActive, SqlTransaction trans)
        {
            string sql = "UPDATE AcnCasino_Gambler SET Cash = @cash, TotalBet = @totalBet, Win = @win, Lose = @lose, TotalBanker = @totalBanker, isActive = @isActive WHERE UserID = @userid";

            SqlParameter[] para = { new SqlParameter("@userid", userid), new SqlParameter("@cash", cash), new SqlParameter("@totalBet", totalBet), new SqlParameter("@win", win), new SqlParameter("@lose", lose), new SqlParameter("@totalBanker", totalBanker), new SqlParameter("@isActive", isActive) };

            if (trans != null)
            {
                SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, para);
            }
            else
            {
                SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
            }
        }

        public static int GetGamblerCount()
        {
            string sql = "SELECT COUNT(*) FROM AcnCasino_Gambler WHERE IsActive = 1";

            Object obj = SqlHelper.ExecuteScalar(SQLConn.GetConnection(), CommandType.Text, sql);

            return Convert.ToInt32(obj);
        }

        public static DataTable GetGamblerProfitView()
        {
            string sql = @"SELECT * FROM dbo.AcnCasino_Gambler gambler INNER JOIN 
                        (SELECT UserID, SUM(ISNULL(Earning, 0)) AS Earning, SUM(ISNULL(Bet, 0)) AS TotalBet
                        FROM dbo.AcnCasino_Bet WHERE Earning IS NOT NULL AND Bet IS NOT NULL
                        GROUP BY UserID) bet ON gambler.UserID = bet.UserID";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }

        public static DataTable GetGamblerProfitView(Guid leagueGuid)
        {
            string sql = @"SELECT UserID, UserName,COUNT(CASE IsWin WHEN 1 THEN 1 ELSE NULL END) AS Win, COUNT(CASE IsWin WHEN 0 THEN 0 ELSE NULL END) AS Lose, 
                           SUM(ISNULL(Earning, 0)) AS Earning, SUM(ISNULL(Bet, 0)) AS TotalBet
                           FROM dbo.vw_AcnCasino_BetInfo WHERE (Earning IS NOT NULL) AND (LeagueGuid = @guid)
                           GROUP BY UserID, UserName";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@guid", leagueGuid));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }

        public static int GetGamblerRPByUserID(int userid)
        {
            string sql = @"SELECT COUNT(*) AS RPBonus FROM dbo.AcnCasino_Bet WHERE Earning = 0 AND EarningDesc = 'RP+1' AND IsWin = 1 AND UserID = @userid";

            Object obj = SqlHelper.ExecuteScalar(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@userid", userid));

            return Convert.ToInt32(obj);
        }

        public static int GetGamblerRPByUserID(int userid, Guid leagueGuid)
        {
            string sql = @"SELECT COUNT(*) AS RPBonus FROM dbo.vw_AcnCasino_BetInfo WHERE Earning = 0 AND EarningDesc = 'RP+1' AND IsWin = 1 AND UserID = @userid AND LeagueGuid = @guid";

            Object obj = SqlHelper.ExecuteScalar(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@userid", userid), new SqlParameter("@guid", leagueGuid));

            return Convert.ToInt32(obj);
        }
    }
}

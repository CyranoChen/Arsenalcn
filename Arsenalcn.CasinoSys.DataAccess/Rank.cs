using System;
using System.Data;
using System.Data.SqlClient;

using Microsoft.ApplicationBlocks.Data;

namespace Arsenalcn.CasinoSys.DataAccess
{
    public class Rank
    {
        public static DataTable GetAllRanks(int rankYear, int rankMonth)
        {
            string sql = "SELECT * FROM AcnCasino_Rank WHERE RankYear = @rankYear and RankMonth = @rankMonth";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@rankYear", rankYear), new SqlParameter("@rankMonth", rankMonth));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }

        public static void InsertRank(int rankYear, int rankMonth, int winnerUserID, string winnerUserName, float winnerProfit, float winnerTotalBet, int loserUserID, string loserUserName, float loserProfit, float loserTotalBet, int rpUserID, string rpUserName, int rpAmount)
        {
            string sql = @"INSERT INTO AcnCasino_Rank VALUES (
                                @rankYear, @rankMonth, @winnerUserID, @winnerUserName, @winnerProfit, @winnerTotalBet, 
                                @loserUserID, @loserUserName, @loserProfit, @loserTotalBet, @rpUserID, @rpUserName, @rpAmount)";

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql,
                new SqlParameter("@rankYear", rankYear),
                new SqlParameter("@rankMonth", rankMonth),
                new SqlParameter("@winnerUserID", winnerUserID),
                new SqlParameter("@winnerUserName", winnerUserName),
                new SqlParameter("@winnerProfit", winnerProfit),
                new SqlParameter("@winnerTotalBet", winnerTotalBet),
                new SqlParameter("@loserUserID", loserUserID),
                new SqlParameter("@loserUserName", loserUserName),
                new SqlParameter("@loserProfit", loserProfit),
                new SqlParameter("@loserTotalBet", loserTotalBet),
                new SqlParameter("@rpUserID", rpUserID),
                new SqlParameter("@rpUserName", rpUserName),
                new SqlParameter("@rpAmount", rpAmount));
        }

        public static void UpdateRank(int rankYear, int rankMonth, int winnerUserID, string winnerUserName, float winnerProfit, float winnerTotalBet, int loserUserID, string loserUserName, float loserProfit, float loserTotalBet, int rpUserID, string rpUserName, int rpAmount)
        {
            string sql = @"UPDATE AcnCasino_Rank SET WinnerUserID = @winnerUserID, WinnerUserName = @winnerUserName, 
                                WinnerProfit = @winnerProfit, WinnerTotalBet = @winnerTotalBet, LoserUserID = @loserUserID, 
                                LoserUserName = @loserUserName, LoserProfit = @loserProfit, LoserTotalBet = @loserTotalBet,
                                RPUserID = @rpUserID, RPUserName = @rpUserName, RPAmount = @rpAmount  
                                WHERE RankYear = @RankYear AND RankMonth = @RankMonth";

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql,
                new SqlParameter("@RankYear", rankYear),
                new SqlParameter("@RankMonth", rankMonth),
                new SqlParameter("@WinnerUserID", winnerUserID),
                new SqlParameter("@WinnerUserName", winnerUserName),
                new SqlParameter("@WinnerProfit", winnerProfit),
                new SqlParameter("@WinnerTotalBet", winnerTotalBet),
                new SqlParameter("@LoserUserID", loserUserID),
                new SqlParameter("@LoserUserName", loserUserName),
                new SqlParameter("@LoserProfit", loserProfit),
                new SqlParameter("@LoserTotalBet", loserTotalBet),
                new SqlParameter("@rpUserID", rpUserID),
                new SqlParameter("@rpUserName", rpUserName),
                new SqlParameter("@rpAmount", rpAmount));
        }

        public static DataTable GetTopGamblerMonthly()
        {
            string sql = @"SELECT * FROM dbo.AcnCasino_Rank ORDER BY RankYear DESC, RankMonth DESC";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        public static DataTable GetTopGamblerMonthly(bool isWinner, DateTime today)
        {
            string sql = string.Empty;
            string strOrder = string.Empty;

            DateTime monthStart = today.AddDays(1 - today.Day);
            DateTime nextStart = monthStart.AddMonths(1);

            if (isWinner)
                strOrder = "DESC";

            sql = string.Format(@"SELECT TOP 1 UserID, UserName, SUM(ISNULL(Earning, 0) - ISNULL(Bet, 0)) AS profit, SUM(ISNULL(Bet,0)) AS TotalBet
                                  FROM dbo.AcnCasino_Bet WHERE (Earning IS NOT NULL) AND (Bet IS NOT NULL) AND (BetTime >= '{0}') AND (BetTime < '{1}')
                                  GROUP BY UserID, UserName ORDER BY Profit {2}, TotalBet DESC", monthStart, nextStart, strOrder);

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        public static DataTable GetTopGamblerMonthly(bool isWinner, DateTime today, bool isRP)
        {
            string sql = string.Empty;
            string strOrder = string.Empty;

            DateTime monthStart = today.AddDays(1 - today.Day);
            DateTime nextStart = monthStart.AddMonths(1);

            if (isWinner)
                strOrder = "DESC";

            sql = string.Format(@"SELECT TOP 1 dbo.AcnCasino_Gambler.UserID, dbo.AcnCasino_Gambler.UserName, COUNT(*) AS RPBonus
                         FROM dbo.AcnCasino_Gambler INNER JOIN dbo.AcnCasino_Bet ON dbo.AcnCasino_Gambler.UserID = dbo.AcnCasino_Bet.UserID
                         WHERE (dbo.AcnCasino_Bet.Earning = 0) AND (dbo.AcnCasino_Bet.EarningDesc = 'RP+1') AND (dbo.AcnCasino_Bet.IsWin = 1) AND (BetTime >= '{0}') AND (BetTime < '{1}')
                         GROUP BY dbo.AcnCasino_Gambler.UserID, dbo.AcnCasino_Gambler.UserName ORDER BY RPBonus {2}", monthStart, nextStart, strOrder);

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        public static DataTable GetTopGamblerProfit(out int months)
        {
            DateTime iDay = DateTime.Today;
            months = 0;
            do
            {
                DateTime monthStart = iDay.AddDays(1 - iDay.Day);
                DateTime nextStart = monthStart.AddMonths(1);

                string sql = string.Format(@"SELECT TOP 5 * FROM dbo.AcnCasino_Gambler gambler INNER JOIN 
                        (SELECT UserID, SUM(ISNULL(Earning, 0) - ISNULL(Bet, 0)) AS profit FROM dbo.AcnCasino_Bet 
                        WHERE (Earning IS NOT NULL) AND (Bet IS NOT NULL) AND (BetTime >= '{0}') AND (BetTime < '{1}')
                        GROUP BY UserID) bet ON gambler.UserID = bet.UserID ORDER BY Profit DESC", monthStart, nextStart);

                DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

                if (ds.Tables[0].Rows.Count > 0 || months < -12)
                    return ds.Tables[0];
                else
                {
                    months--;
                    iDay = monthStart.AddMonths(-1);
                }
            }
            while (true);
        }

        public static DataTable GetTopGamblerTotalBet(out int months)
        {
            DateTime iDay = DateTime.Today;
            months = 0;
            do
            {
                DateTime monthStart = iDay.AddDays(1 - iDay.Day);
                DateTime nextStart = monthStart.AddMonths(1);

                string sql = string.Format(@"SELECT TOP 5 * FROM dbo.AcnCasino_Gambler gambler INNER JOIN
                        (SELECT UserID, SUM(ISNULL(Bet, 0)) AS TotalBetMonthly FROM dbo.AcnCasino_Bet 
                        WHERE (Bet IS NOT NULL) AND (BetTime >= '{0}') AND (BetTime < '{1}')
                        GROUP BY UserID) bet ON gambler.UserID = bet.UserID ORDER BY TotalBetMonthly DESC", monthStart, nextStart);

                DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

                if (ds.Tables[0].Rows.Count > 0 || months < -12)
                    return ds.Tables[0];
                else
                {
                    months--;
                    iDay = monthStart.AddMonths(-1);
                }
            }
            while (true);
        }

        public static DataTable GetTopGamblerRP(out int months)
        {
            DateTime iDay = DateTime.Today;
            months = 0;
            do
            {
                DateTime monthStart = iDay.AddDays(1 - iDay.Day);
                DateTime nextStart = monthStart.AddMonths(1);

                string sql = string.Format(@"SELECT TOP 5 dbo.AcnCasino_Gambler.UserID, dbo.AcnCasino_Gambler.UserName, COUNT(*) AS RPBonus
                         FROM dbo.AcnCasino_Gambler INNER JOIN dbo.AcnCasino_Bet ON dbo.AcnCasino_Gambler.UserID = dbo.AcnCasino_Bet.UserID
                         WHERE (dbo.AcnCasino_Bet.Earning = 0) AND (dbo.AcnCasino_Bet.EarningDesc = 'RP+1') AND (dbo.AcnCasino_Bet.IsWin = 1) AND (BetTime >= '{0}') AND (BetTime < '{1}')
                         GROUP BY dbo.AcnCasino_Gambler.UserID, dbo.AcnCasino_Gambler.UserName ORDER BY RPBonus DESC", monthStart, nextStart);

                DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

                if (ds.Tables[0].Rows.Count > 0 || months < -12)
                    return ds.Tables[0];
                else
                {
                    months--;
                    iDay = monthStart.AddMonths(-1);
                }
            }
            while (true);
        }
    }
}

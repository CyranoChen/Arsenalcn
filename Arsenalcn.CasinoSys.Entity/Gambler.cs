using System;
using System.Data;
using System.Data.SqlClient;

using Discuz.Entity;
using Discuz.Forum;

namespace Arsenalcn.CasinoSys.Entity
{
    public class Gambler
    {
        public Gambler() { }

        public Gambler(int userid, SqlTransaction trans)
        {
            DataRow dr = DataAccess.Gambler.GetGamblerByUserID(userid, trans);

            if (dr == null)
            {
                //create a gambler for this user
                ShortUserInfo sUser = AdminUsers.GetShortUserInfo(userid);

                DataAccess.Gambler.InsertGambler(userid, sUser.Username.Trim());

                //reload dr
                dr = DataAccess.Gambler.GetGamblerByUserID(userid, null);
            }

            InitGambler(dr);
        }

        private void InitGambler(DataRow dr)
        {
            if (dr != null)
            {
                UserID = Convert.ToInt32(dr["UserID"]);
                UserName = Convert.ToString(dr["UserName"]);
                Cash = Convert.ToSingle(dr["Cash"]);
                TotalBet = Convert.ToSingle(dr["TotalBet"]);
                Win = Convert.ToInt32(dr["Win"]);
                Lose = Convert.ToInt32(dr["Lose"]);
                TotalBanker = Convert.ToInt32(dr["TotalBanker"]);
                IsActive = Convert.ToBoolean(dr["IsActive"]);
            }
            else
                throw new Exception("Unable to init Gambler.");
        }

        public void Update(SqlTransaction trans)
        {
            DataAccess.Gambler.UpdateGambler(this.UserID, Cash, TotalBet, Win, Lose, TotalBanker, IsActive, trans);
        }

        public static DataTable GetGambler(string username)
        {
            if (!string.IsNullOrEmpty(username))
                return DataAccess.Gambler.GetGambler(username);
            else
                return DataAccess.Gambler.GetGambler();
        }

        public static DataTable GetGamblerProfitView(Guid leagueGuid)
        {
            if (leagueGuid != Guid.Empty)
                return DataAccess.Gambler.GetGamblerProfitView(leagueGuid);
            else
                return DataAccess.Gambler.GetGamblerProfitView();
        }

        public static int GetGamblerRPByUserID(int userID, Guid leagueGuid)
        {
            if (leagueGuid != Guid.Empty)
                return DataAccess.Gambler.GetGamblerRPByUserID(userID, leagueGuid);
            else
                return DataAccess.Gambler.GetGamblerRPByUserID(userID);
        }

        public static void GamblerStatistics()
        {
            DataTable dt = DataAccess.Gambler.GetGambler();

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Gambler gambler = new Gambler((int)dr["UserID"], null);
                    gambler.TotalBet = DataAccess.Bet.GetUserTotalBetCash(gambler.UserID);
                    gambler.Win = DataAccess.Bet.GetUserTotalWinLoseCount(gambler.UserID, true);
                    gambler.Lose = DataAccess.Bet.GetUserTotalWinLoseCount(gambler.UserID, false);

                    if (gambler.Cash > 0)
                        gambler.IsActive = true;
                    else
                        gambler.IsActive = false;

                    gambler.Update(null);
                }
            }
        }

        public static void GamblerStatistics(int userID)
        {
            Gambler gambler = new Gambler(userID, null);
            gambler.TotalBet = DataAccess.Bet.GetUserTotalBetCash(gambler.UserID);
            gambler.Win = DataAccess.Bet.GetUserTotalWinLoseCount(gambler.UserID, true);
            gambler.Lose = DataAccess.Bet.GetUserTotalWinLoseCount(gambler.UserID, false);

            if (gambler.Cash > 0)
                gambler.IsActive = true;
            else
                gambler.IsActive = false;

            gambler.Update(null);
        }

        public static void TopGamblerMonthlyStatistics()
        {
            DateTime iDay = DateTime.Today;

            DateTime lastBetTime = DataAccess.Bet.GetLastBetTime();
            string sql = string.Empty;

            while (!(iDay.Year <= lastBetTime.Year && iDay.Month < lastBetTime.Month))
            {
                DataTable dtWinner = DataAccess.Rank.GetTopGamblerMonthly(true, iDay);
                DataTable dtLoser = DataAccess.Rank.GetTopGamblerMonthly(false, iDay);
                DataTable dtRP = DataAccess.Rank.GetTopGamblerMonthly(true, iDay, true);

                if (dtWinner != null && dtLoser != null && dtRP != null)
                {
                    DataTable dtRanks = DataAccess.Rank.GetAllRanks(iDay.Year, iDay.Month);

                    if (dtRanks != null)
                    {
                        //insert
                        DataAccess.Rank.UpdateRank(iDay.Year, iDay.Month, Convert.ToInt32(dtWinner.Rows[0]["UserID"]), dtWinner.Rows[0]["UserName"].ToString().Trim(), Convert.ToSingle(dtWinner.Rows[0]["profit"]), Convert.ToSingle(dtWinner.Rows[0]["TotalBet"]), Convert.ToInt32(dtLoser.Rows[0]["UserID"]), dtLoser.Rows[0]["UserName"].ToString().Trim(), Convert.ToSingle(dtLoser.Rows[0]["profit"]), Convert.ToSingle(dtLoser.Rows[0]["TotalBet"]), Convert.ToInt32(dtRP.Rows[0]["UserID"]), dtRP.Rows[0]["UserName"].ToString().Trim(), Convert.ToInt32(dtRP.Rows[0]["RPBonus"]));
                    }
                    else
                    {
                        //update
                        DataAccess.Rank.InsertRank(iDay.Year, iDay.Month, Convert.ToInt32(dtWinner.Rows[0]["UserID"]), dtWinner.Rows[0]["UserName"].ToString().Trim(), Convert.ToSingle(dtWinner.Rows[0]["profit"]), Convert.ToSingle(dtWinner.Rows[0]["TotalBet"]), Convert.ToInt32(dtLoser.Rows[0]["UserID"]), dtLoser.Rows[0]["UserName"].ToString().Trim(), Convert.ToSingle(dtLoser.Rows[0]["profit"]), Convert.ToSingle(dtLoser.Rows[0]["TotalBet"]), Convert.ToInt32(dtRP.Rows[0]["UserID"]), dtRP.Rows[0]["UserName"].ToString().Trim(), Convert.ToInt32(dtRP.Rows[0]["RPBonus"]));
                    }
                }
                iDay = iDay.AddMonths(-1);
            }
        }

        public int UserID
        { get; private set; }

        public string UserName
        { get; private set; }

        public float Cash
        { get; set; }

        public float TotalBet
        { get; set; }

        public int Win
        { get; set; }

        public int Lose
        { get; set; }

        public int TotalBanker
        { get; set; }

        public bool IsActive
        { get; set; }
    }
}

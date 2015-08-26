using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using Arsenalcn.Common;

using Discuz.Forum;

namespace Arsenalcn.CasinoSys.Entity
{
    public class Bet
    {
        public Bet() { }

        public Bet(int betID)
        {
            var dr = DataAccess.Bet.GetBetByID(betID);

            if (dr != null)
                InitBet(dr);
        }

        public Bet(DataRow dr)
        {
            InitBet(dr);
        }

        private void InitBet(DataRow dr)
        {
            if (dr != null)
            {
                ID = Convert.ToInt32(dr["ID"]);
                UserID = Convert.ToInt32(dr["UserID"]);
                UserName = Convert.ToString(dr["UserName"]);
                CasinoItemGuid = (Guid)dr["CasinoItemGuid"];

                if (Convert.IsDBNull(dr["Bet"]))
                    BetAmount = null;
                else
                    BetAmount = Convert.ToSingle(dr["Bet"]);


                BetTime = (DateTime)dr["BetTime"];

                if (Convert.IsDBNull(dr["BetRate"]))
                    BetRate = null;
                else
                    BetRate = Convert.ToSingle(dr["BetRate"]);

                if (Convert.IsDBNull(dr["IsWin"]))
                    IsWin = null;
                else
                    IsWin = Convert.ToBoolean(dr["IsWin"]);

                if (Convert.IsDBNull(dr["Earning"]))
                    Earning = null;
                else
                    Earning = Convert.ToSingle(dr["Earning"]);

                if (Convert.IsDBNull(dr["EarningDesc"]))
                    EarningDesc = null;
                else
                    EarningDesc = Convert.ToString(dr["EarningDesc"]);
            }
            else
                throw new Exception("Unable to init Bet.");
        }

        public void Update(SqlTransaction trans)
        {
            DataAccess.Bet.UpdateBet(ID, IsWin.Value, Earning.Value, EarningDesc, trans);
        }

        public void Insert(MatchResultBetDetail matchResult)
        {
            using (var conn = SQLConn.GetConnection())
            {
                conn.Open();
                var trans = conn.BeginTransaction();
                try
                {
                    if (BetCheck())
                    {
                        //update gambler statistics
                        var gambler = new Gambler(UserID, trans);
                        gambler.TotalBet += BetAmount.GetValueOrDefault(0f);

                        if (BetAmount.HasValue)
                            gambler.Cash -= BetAmount.GetValueOrDefault(0f);
                        gambler.Update(trans);

                        var banker = new Banker(Entity.CasinoItem.GetCasinoItem(CasinoItemGuid).BankerID);
                        banker.Cash += BetAmount.GetValueOrDefault(0f);
                        banker.Update(trans);

                        var betID = DataAccess.Bet.InsertBet(UserID, UserName, CasinoItemGuid, BetAmount, BetRate, trans);
                        matchResult.Save(betID, trans);

                        trans.Commit();
                    }
                    else
                        throw new Exception("Failed to create bet (MatchResult).");
                }
                catch
                {
                    trans.Rollback();
                }

                //conn.Close();
            }
        }

        public void Insert(string optionValue)
        {
            using (var conn = SQLConn.GetConnection())
            {
                conn.Open();
                var trans = conn.BeginTransaction();
                try
                {
                    if (BetCheck(trans))
                    {
                        //update gambler statistics
                        var gambler = new Gambler(UserID, trans);
                        gambler.TotalBet += BetAmount.GetValueOrDefault(0f);

                        if (BetAmount.HasValue)
                            gambler.Cash -= BetAmount.GetValueOrDefault(0f);
                        gambler.Update(trans);

                        var banker = new Banker(Entity.CasinoItem.GetCasinoItem(CasinoItemGuid).BankerID);
                        banker.Cash += BetAmount.GetValueOrDefault(0f);
                        banker.Update(trans);

                        var betID = DataAccess.Bet.InsertBet(UserID, UserName, CasinoItemGuid, BetAmount, BetRate, trans);
                        Entity.MatchChoiceOption.SaveMatchChoiceOption(betID, optionValue, trans);

                        trans.Commit();
                    }
                    else
                        throw new Exception("Failed to create bet (SingleChoice).");
                }
                catch
                {
                    trans.Rollback();
                }

                //conn.Close();
            }
        }

        public static void CleanNoCasinoItemBet()
        {
            using (var conn = SQLConn.GetConnection())
            {
                conn.Open();
                var trans = conn.BeginTransaction();
                try
                {
                    DataAccess.Bet.CleanBet(trans);
                    DataAccess.BetDetail.CleanBetDetail(trans);
                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();
                }

                //conn.Close();
            }
        }

        public static void ReturnBet(int betID)
        {
            using (var conn = SQLConn.GetConnection())
            {
                conn.Open();
                var trans = conn.BeginTransaction();
                try
                {
                    var bet = new Bet(betID);

                    if (bet.BetAmount.HasValue && bet.BetAmount >= 0f)
                    {
                        var betAmount = Convert.ToSingle(bet.BetAmount);

                        var banker = new Banker(CasinoItem.GetCasinoItem(bet.CasinoItemGuid).BankerID);
                        var gambler = new Gambler(bet.UserID, trans);

                        gambler.Cash += betAmount;
                        gambler.TotalBet -= betAmount;
                        banker.Cash -= betAmount;

                        gambler.Update(trans);
                        banker.Update(trans);
                    }
                    else if (!bet.BetAmount.HasValue && !bet.Earning.HasValue && bet.EarningDesc == "RP+1")
                    {
                        AdminUsers.UpdateUserExtCredits(bet.UserID, 4, -1);
                    }

                    DataAccess.Bet.DeleteBetByID(betID, trans);
                    DataAccess.BetDetail.CleanBetDetail(trans);
                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();
                }

                //conn.Close();
            }
        }

        public bool BetCheck(SqlTransaction trans = null)
        {
            //check close time
            var item = Entity.CasinoItem.GetCasinoItem(CasinoItemGuid);

            if (item == null)
                return false;

            if (DateTime.Now > item.CloseTime)
                return false;

            if (BetAmount.HasValue && BetAmount <= 0)
                return false;

            //check user account
            if (BetAmount.HasValue)
            {
                var gamber = new Gambler(UserID, trans);
                if (gamber.Cash < BetAmount.Value)
                    return false;
            }

            return true;
        }

        public static List<Bet> GetUserMatchAllBet(int userid, Guid matchGuid)
        {
            var dt = DataAccess.Bet.GetUserMatchAllBet(userid, matchGuid);
            var betList = new List<Bet>();

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    betList.Add(new Bet(dr));
                }
            }

            return betList;
        }

        public static DataTable GetUserMatchAllBetTable(int userid, Guid matchGuid)
        {
            return DataAccess.Bet.GetUserMatchAllBet(userid, matchGuid);
        }

        public static List<Bet> GetUserCasinoItemAllBet(int userid, Guid casinoItemGuid)
        {
            var dt = DataAccess.Bet.GetUserCasinoItemAllBet(userid, casinoItemGuid);
            var betList = new List<Bet>();

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    betList.Add(new Bet(dr));
                }
            }

            return betList;
        }

        public static List<Bet> GetBetByCasinoItemGuid(Guid guid, SqlTransaction trans)
        {
            var dt = DataAccess.Bet.GetBetByCasinoItemGuid(guid, trans);
            var betList = new List<Bet>();

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    betList.Add(new Bet(dr));
                }
            }

            return betList;
        }

        public static List<Bet> GetMatchAllBet(Guid matchGuid)
        {
            var dt = DataAccess.Bet.GetMatchAllBet(matchGuid);
            var betList = new List<Bet>();

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    betList.Add(new Bet(dr));
                }
            }

            return betList;
        }

        public static DataTable GetMatchAllBetTable(Guid matchGuid)
        {
            return DataAccess.Bet.GetMatchAllBet(matchGuid);
        }

        public static List<Bet> GetAllBetByTimeDiff(int timeDiff)
        {
            var dt = DataAccess.Bet.GetAllBetByTimeDiff(timeDiff);
            var betList = new List<Bet>();

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    betList.Add(new Bet(dr));
                }
            }

            return betList;
        }

        public static DataTable GetUserBetHistoryView(int userID)
        {
            return DataAccess.Bet.GetUserBetHistoryView(userID);
        }

        public static DataTable GetUserBetMatch(int userID)
        {
            return DataAccess.Bet.GetUserBetMatch(userID);
        }

        public static float GetUserMatchTotalBet(int userID, Guid matchGuid)
        {
            return DataAccess.Bet.GetUserMatchTotalBet(userID, matchGuid);
        }

        public static float GetUserTotalWinCash(int userID)
        {
            return DataAccess.Bet.GetUserTotalWinCash(userID);
        }

        public static float GetMatchTopBet(Guid matchGuid)
        {
            return DataAccess.Bet.GetMatchTopBet(matchGuid);
        }

        public static float GetMatchTopEarning(Guid matchGuid)
        {
            return DataAccess.Bet.GetMatchTopEarning(matchGuid);
        }

        public int ID
        { get; set; }

        public int UserID
        { get; set; }

        public string UserName
        { get; set; }

        public Guid CasinoItemGuid
        { get; set; }

        public float? BetAmount
        { get; set; }

        public DateTime BetTime
        { get; private set; }

        public float? BetRate
        { get; set; }

        public bool? IsWin
        { get; set; }

        public float? Earning
        { get; set; }

        public string EarningDesc
        { get; set; }
    }
}

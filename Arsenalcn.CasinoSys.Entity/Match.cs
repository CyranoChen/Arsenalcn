using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using Discuz.Forum;

namespace Arsenalcn.CasinoSys.Entity
{
    public class Match
    {
        public Match() { }

        public Match(Guid matchGuid)
        {
            DataRow dr = DataAccess.Match.GetMatchByID(matchGuid);

            if (dr != null)
                InitMatch(dr);
        }

        private Match(DataRow dr)
        {
            InitMatch(dr);
        }

        private void InitMatch(DataRow dr)
        {
            if (dr != null)
            {
                MatchGuid = (Guid)dr["MatchGuid"];
                Home = (Guid)dr["Home"];
                Away = (Guid)dr["Away"];
                LeagueGuid = (Guid)dr["LeagueGuid"];

                if (!Convert.IsDBNull(dr["GroupGuid"]))
                    GroupGuid = (Guid)dr["GroupGuid"];
                else
                    GroupGuid = null;

                if (!Convert.IsDBNull(dr["ResultHome"]))
                    ResultHome = Convert.ToInt16(dr["ResultHome"]);
                else
                    ResultHome = null;

                if (!Convert.IsDBNull(dr["ResultAway"]))
                    ResultAway = Convert.ToInt16(dr["ResultAway"]);
                else
                    ResultAway = null;

                PlayTime = (DateTime)dr["PlayTime"];
                LeagueName = dr["LeagueName"].ToString();

                if (!Convert.IsDBNull(dr["Round"]))
                    Round = Convert.ToInt16(dr["Round"]);
                else
                    Round = null;
            }
            else
                throw new Exception("Unable to init Match.");
        }

        public void Update()
        {
            DataAccess.Match.UpdateMatch(MatchGuid, Home, Away, ResultHome, ResultAway, PlayTime, LeagueGuid, LeagueName, Round, GroupGuid);
        }

        public void Insert(int userID, string username, float winRate, float drawRate, float loseRate)
        {
            using (SqlConnection conn = DataAccess.SQLConn.GetConnection())
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    DataAccess.Match.InsertMatch(MatchGuid, Home, Away, PlayTime, LeagueGuid, LeagueName, Round, GroupGuid, trans);
                        
                    //add matchResult
                    MatchResult itemMatchResult = (MatchResult)Entity.CasinoItem.CreateInstance(CasinoItem.CasinoType.MatchResult);
                    itemMatchResult.MatchGuid = MatchGuid;
                    itemMatchResult.CreateTime = DateTime.Now;
                    itemMatchResult.PublishTime = DateTime.Now;
                    itemMatchResult.CloseTime = PlayTime;
                    itemMatchResult.BankerID = Banker.DefaultBankerID;
                    itemMatchResult.BankerName = new Banker(Banker.DefaultBankerID).BankerName;
                    itemMatchResult.Earning = null;
                    itemMatchResult.OwnerID = userID;
                    itemMatchResult.OwnerUserName = username;

                    itemMatchResult.Save(trans);

                    //add singleChoice
                    SingleChoice itemSingleChoice = (SingleChoice)Entity.CasinoItem.CreateInstance(CasinoItem.CasinoType.SingleChoice);
                    itemSingleChoice.MatchGuid = MatchGuid;
                    itemSingleChoice.CreateTime = DateTime.Now;
                    itemSingleChoice.PublishTime = DateTime.Now;
                    itemSingleChoice.CloseTime = PlayTime;
                    itemSingleChoice.BankerID = Banker.DefaultBankerID;
                    itemSingleChoice.BankerName = new Banker(Entity.Banker.DefaultBankerID).BankerName;
                    itemSingleChoice.Earning = null;
                    itemSingleChoice.OwnerID = userID;
                    itemSingleChoice.OwnerUserName = username;
                    itemSingleChoice.FloatingRate = false;

                    itemSingleChoice.Options.Add(new ChoiceOption() { OptionDisplay = "主队胜", OptionValue = MatchChoiceOption.HomeWinValue, OptionRate = winRate, OrderID = 1 });
                    itemSingleChoice.Options.Add(new ChoiceOption() { OptionDisplay = "双方平", OptionValue = MatchChoiceOption.DrawValue, OptionRate = drawRate, OrderID = 2 });
                    itemSingleChoice.Options.Add(new ChoiceOption() { OptionDisplay = "客队胜", OptionValue = MatchChoiceOption.AwayWinValue, OptionRate = loseRate, OrderID = 3 });

                    itemSingleChoice.Save(trans);

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
                conn.Close();
            }
        }

        public void Delete()
        {
            DataAccess.CasinoItem.DeleteCasinoItem(MatchGuid);
            DataAccess.Match.DeleteMatch(MatchGuid);
        }

        public static List<Match> GetMatchs()
        {
            DataTable dt = DataAccess.Match.GetMatchs();
            List<Match> list = new List<Match>();

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new Match(dr));
                }
            }

            return list;
        }

        public static Guid GetRandomOpenMatch()
        {
            return DataAccess.Match.GetRandomOpenMatch();
        }

        public void ReturnBet()
        {
            using (SqlConnection conn = DataAccess.SQLConn.GetConnection())
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    Guid? casinoItemGuid = DataAccess.CasinoItem.GetCasinoItemGuidByMatch(MatchGuid, CasinoItem.CasinoType.SingleChoice.ToString(), trans);
                    if (casinoItemGuid.HasValue)
                    {
                        CasinoItem item = CasinoItem.GetCasinoItem(casinoItemGuid.Value);
                        Banker banker = new Banker(item.BankerID);

                        DataTable dtMatchBet = DataAccess.Bet.GetMatchAllBet(MatchGuid);

                        float TotalBet = 0f;

                        foreach (DataRow dr in dtMatchBet.Rows)
                        {
                            if (!Convert.IsDBNull(dr["Bet"]))
                            {
                                Entity.Gambler gambler = new Entity.Gambler(Convert.ToInt32(dr["UserID"]), trans);
                                gambler.Cash += Convert.ToSingle(dr["Bet"]);
                                gambler.TotalBet -= Convert.ToSingle(dr["Bet"]);
                                gambler.Update(trans);

                                TotalBet += Convert.ToSingle(dr["Bet"]);
                            }
                        }

                        banker.Cash -= TotalBet;
                        banker.Update(trans);

                        DataAccess.Bet.DeleteBetByMatchGuid(MatchGuid, trans);
                        DataAccess.BetDetail.CleanBetDetail(trans);

                        trans.Commit();
                    }
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
                conn.Close();
            }
        }

        public void CalcBonus()
        {
            if (string.IsNullOrEmpty(ResultHome.ToString()) || string.IsNullOrEmpty(ResultAway.ToString()))
            {
                throw new Exception("You can not calc bonus without a match result");
            }

            using (SqlConnection conn = DataAccess.SQLConn.GetConnection())
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();

                try
                {
                    Guid? itemGuid = DataAccess.CasinoItem.GetCasinoItemGuidByMatch(MatchGuid, CasinoItem.CasinoType.SingleChoice.ToString(), trans);

                    if (itemGuid.HasValue)
                    {
                        //single choice bonus
                        Entity.CasinoItem item = Entity.CasinoItem.GetCasinoItem(itemGuid.Value);
                        Entity.Banker banker = new Entity.Banker(item.BankerID);

                        float totalEarning = 0f;

                        List<Entity.Bet> betList = Entity.Bet.GetBetByCasinoItemGuid(itemGuid.Value, trans);

                        foreach (Entity.Bet bet in betList)
                        {
                            DataTable dt = DataAccess.BetDetail.GetBetDetailByBetID(bet.ID);

                            if (dt != null)
                            {
                                Entity.Gambler gambler = new Arsenalcn.CasinoSys.Entity.Gambler(bet.UserID, trans);

                                if (bet.IsWin == null)
                                {
                                    bool isWin = false;

                                    DataRow dr = dt.Rows[0];

                                    if (dr["DetailName"].ToString() == MatchChoiceOption.HomeWinValue && ResultHome > ResultAway)
                                        isWin = true;
                                    else if (dr["DetailName"].ToString() == MatchChoiceOption.DrawValue && ResultHome == ResultAway)
                                        isWin = true;
                                    else if (dr["DetailName"].ToString() == MatchChoiceOption.AwayWinValue && ResultHome < ResultAway)
                                        isWin = true;

                                    bet.IsWin = isWin;

                                    totalEarning += bet.BetAmount.Value;

                                    if (isWin)
                                    {
                                        bet.Earning = bet.BetAmount * bet.BetRate;
                                        bet.EarningDesc = string.Format("{0:N2}", bet.Earning.Value);

                                        totalEarning -= bet.Earning.Value;

                                        //add gambler cash

                                        gambler.Cash += bet.Earning.Value;
                                        gambler.Win++;

                                        banker.Cash -= bet.Earning.Value;
                                    }
                                    else
                                    {
                                        gambler.Lose++;

                                        bet.Earning = 0;
                                        bet.EarningDesc = string.Empty;
                                    }
                                }

                                bet.Update(trans);
                                gambler.Update(trans);
                            }
                        }

                        banker.Update(trans);

                        item.Earning = totalEarning;
                        item.Save(trans);
                    }

                    itemGuid = DataAccess.CasinoItem.GetCasinoItemGuidByMatch(MatchGuid, CasinoItem.CasinoType.MatchResult.ToString(), trans);

                    if (itemGuid.HasValue)
                    {
                        //match result bonus
                        List<Entity.Bet> betList = Entity.Bet.GetBetByCasinoItemGuid(itemGuid.Value, trans);

                        Entity.CasinoItem item = Entity.CasinoItem.GetCasinoItem(itemGuid.Value);
                        item.Earning = 0;
                        item.Save(trans);

                        foreach (Entity.Bet bet in betList)
                        {
                            Entity.Gambler gambler = new Arsenalcn.CasinoSys.Entity.Gambler(bet.UserID, trans);

                            DataTable dt = DataAccess.BetDetail.GetBetDetailByBetID(bet.ID);

                            Entity.MatchResultBetDetail betDetail = new MatchResultBetDetail(dt);

                            if (bet.IsWin == null)
                            {
                                if (betDetail.Home == ResultHome && betDetail.Away == ResultAway)
                                {
                                    //win
                                    bet.IsWin = true;
                                    bet.Earning = 0;
                                    bet.EarningDesc = "RP+1";

                                    gambler.Win++;

                                    //update user rp

                                    AdminUsers.UpdateUserExtCredits(bet.UserID, 4, 1);
                                }
                                else
                                {
                                    //lose
                                    bet.IsWin = false;
                                    bet.Earning = 0;
                                    bet.EarningDesc = string.Empty;

                                    gambler.Lose++;
                                }
                            }

                            bet.Update(trans);
                            gambler.Update(trans);
                        }
                    }

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
                conn.Close();
            }
        }

        public static void UpdateMatchResult(Guid casinoItem, short home, short away)
        {
            DataAccess.MatchResult.UpdateMatchResult(casinoItem, home, away, null);
        }

        public Guid MatchGuid
        { get; set; }

        public Guid Home
        { get; set; }

        public Guid Away
        { get; set; }

        public short? ResultHome
        { get; set; }

        public short? ResultAway
        { get; set; }

        public DateTime PlayTime
        { get; set; }

        public Guid LeagueGuid
        { get; set; }

        public string LeagueName
        { get; set; }

        public int? Round
        { get; set; }

        public Guid? GroupGuid
        { get; set; }
    }
}

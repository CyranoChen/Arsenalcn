using System;
using System.Collections.Generic;
using System.Data;
using Arsenalcn.Common;
using Discuz.Forum;

namespace Arsenalcn.CasinoSys.Entity
{
    public class Match
    {
        public Match() { }

        public Match(Guid matchGuid)
        {
            var dr = DataAccess.Match.GetMatchByID(matchGuid);

            if (dr != null)
                InitMatch(dr);
        }

        private Match(DataRow dr)
        {
            InitMatch(dr);
        }

        public Guid MatchGuid { get; set; }

        public Guid Home { get; set; }

        public Guid Away { get; set; }

        public short? ResultHome { get; set; }

        public short? ResultAway { get; set; }

        public DateTime PlayTime { get; set; }

        public Guid LeagueGuid { get; set; }

        public string LeagueName { get; set; }

        public int? Round { get; set; }

        public Guid? GroupGuid { get; set; }

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
            DataAccess.Match.UpdateMatch(MatchGuid, Home, Away, ResultHome, ResultAway, PlayTime, LeagueGuid, LeagueName,
                Round, GroupGuid);
        }

        public void Insert(int userId, string username, float winRate, float drawRate, float loseRate)
        {
            using (var conn = SQLConn.GetConnection())
            {
                conn.Open();
                var trans = conn.BeginTransaction();
                try
                {
                    DataAccess.Match.InsertMatch(MatchGuid, Home, Away, PlayTime, LeagueGuid, LeagueName, Round,
                        GroupGuid, trans);

                    //add matchResult
                    var itemMatchResult = (MatchResult)CasinoItem.CreateInstance(CasinoType.MatchResult);
                    itemMatchResult.MatchGuid = MatchGuid;
                    itemMatchResult.CreateTime = DateTime.Now;
                    itemMatchResult.PublishTime = DateTime.Now;
                    itemMatchResult.CloseTime = PlayTime;
                    itemMatchResult.BankerID = Banker.DefaultBankerID;
                    itemMatchResult.BankerName = new Banker(Banker.DefaultBankerID).BankerName;
                    itemMatchResult.Earning = null;
                    itemMatchResult.OwnerID = userId;
                    itemMatchResult.OwnerUserName = username;

                    itemMatchResult.Save(trans);

                    //add singleChoice
                    var itemSingleChoice = (SingleChoice)CasinoItem.CreateInstance(CasinoType.SingleChoice);
                    itemSingleChoice.MatchGuid = MatchGuid;
                    itemSingleChoice.CreateTime = DateTime.Now;
                    itemSingleChoice.PublishTime = DateTime.Now;
                    itemSingleChoice.CloseTime = PlayTime;
                    itemSingleChoice.BankerID = Banker.DefaultBankerID;
                    itemSingleChoice.BankerName = new Banker(Banker.DefaultBankerID).BankerName;
                    itemSingleChoice.Earning = null;
                    itemSingleChoice.OwnerID = userId;
                    itemSingleChoice.OwnerUserName = username;
                    itemSingleChoice.FloatingRate = false;

                    itemSingleChoice.Options.Add(new ChoiceOption
                    {
                        OptionDisplay = "主队胜",
                        OptionValue = MatchChoiceOption.HomeWinValue,
                        OptionRate = winRate,
                        OrderID = 1
                    });
                    itemSingleChoice.Options.Add(new ChoiceOption
                    {
                        OptionDisplay = "双方平",
                        OptionValue = MatchChoiceOption.DrawValue,
                        OptionRate = drawRate,
                        OrderID = 2
                    });
                    itemSingleChoice.Options.Add(new ChoiceOption
                    {
                        OptionDisplay = "客队胜",
                        OptionValue = MatchChoiceOption.AwayWinValue,
                        OptionRate = loseRate,
                        OrderID = 3
                    });

                    itemSingleChoice.Save(trans);

                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();
                }
            }
        }

        public void Delete()
        {
            DataAccess.CasinoItem.DeleteCasinoItem(MatchGuid);
            DataAccess.Match.DeleteMatch(MatchGuid);
        }

        public static List<Match> GetMatchs()
        {
            var dt = DataAccess.Match.GetMatchs();
            var list = new List<Match>();

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new Match(dr));
                }
            }

            return list;
        }

        public void ReturnBet()
        {
            using (var conn = SQLConn.GetConnection())
            {
                conn.Open();
                var trans = conn.BeginTransaction();
                try
                {
                    var casinoItemGuid = DataAccess.CasinoItem.GetCasinoItemGuidByMatch(MatchGuid,
                        (int)CasinoType.SingleChoice, trans);
                    if (casinoItemGuid.HasValue)
                    {
                        var item = CasinoItem.GetCasinoItem(casinoItemGuid.Value);
                        var banker = new Banker(item.BankerID);

                        var dtMatchBet = DataAccess.Bet.GetMatchAllBet(MatchGuid);

                        var totalBet = 0f;

                        foreach (DataRow dr in dtMatchBet.Rows)
                        {
                            if (!Convert.IsDBNull(dr["Bet"]))
                            {
                                var gambler = new Gambler(Convert.ToInt32(dr["UserID"]), trans);
                                gambler.Cash += Convert.ToSingle(dr["Bet"]);
                                gambler.TotalBet -= Convert.ToSingle(dr["Bet"]);
                                gambler.Update(trans);

                                totalBet += Convert.ToSingle(dr["Bet"]);
                            }
                        }

                        banker.Cash -= totalBet;
                        banker.Update(trans);

                        DataAccess.Bet.DeleteBetByMatchGuid(MatchGuid, trans);
                        DataAccess.BetDetail.CleanBetDetail(trans);

                        trans.Commit();
                    }
                }
                catch
                {
                    trans.Rollback();
                }

                //conn.Close();
            }
        }

        public void CalcBonus()
        {
            if (string.IsNullOrEmpty(ResultHome.ToString()) || string.IsNullOrEmpty(ResultAway.ToString()))
            {
                throw new Exception("You can not calc bonus without a match result");
            }

            using (var conn = SQLConn.GetConnection())
            {
                conn.Open();
                var trans = conn.BeginTransaction();

                try
                {
                    var itemGuid = DataAccess.CasinoItem.GetCasinoItemGuidByMatch(MatchGuid,
                        (int)CasinoType.SingleChoice, trans);

                    if (itemGuid.HasValue)
                    {
                        //single choice bonus
                        var item = CasinoItem.GetCasinoItem(itemGuid.Value);
                        var banker = new Banker(item.BankerID);

                        var totalEarning = 0f;

                        var betList = Bet.GetBetByCasinoItemGuid(itemGuid.Value, trans);

                        foreach (var bet in betList)
                        {
                            var dt = DataAccess.BetDetail.GetBetDetailByBetId(bet.ID);

                            if (dt != null)
                            {
                                var gambler = new Gambler(bet.UserID, trans);

                                if (bet.IsWin == null)
                                {
                                    var isWin = false;

                                    var dr = dt.Rows[0];

                                    if (dr["DetailName"].ToString() == MatchChoiceOption.HomeWinValue &&
                                        ResultHome > ResultAway)
                                        isWin = true;
                                    else if (dr["DetailName"].ToString() == MatchChoiceOption.DrawValue &&
                                             ResultHome == ResultAway)
                                        isWin = true;
                                    else if (dr["DetailName"].ToString() == MatchChoiceOption.AwayWinValue &&
                                             ResultHome < ResultAway)
                                        isWin = true;

                                    bet.IsWin = isWin;

                                    if (bet.BetAmount.HasValue)
                                    {
                                        totalEarning += bet.BetAmount.Value;

                                        if (isWin)
                                        {
                                            bet.Earning = bet.BetAmount * bet.BetRate;

                                            if (bet.Earning != null)
                                            {
                                                bet.EarningDesc = $"{bet.Earning.Value:N2}";

                                                totalEarning -= bet.Earning.Value;

                                                //add gambler cash

                                                gambler.Cash += bet.Earning.Value;
                                                gambler.Win++;

                                                banker.Cash -= bet.Earning.Value;
                                            }
                                        }
                                        else
                                        {
                                            gambler.Lose++;

                                            bet.Earning = 0;
                                            bet.EarningDesc = string.Empty;
                                        }
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

                    itemGuid = DataAccess.CasinoItem.GetCasinoItemGuidByMatch(MatchGuid, (int)CasinoType.MatchResult,
                        trans);

                    if (itemGuid.HasValue)
                    {
                        //match result bonus
                        var betList = Bet.GetBetByCasinoItemGuid(itemGuid.Value, trans);

                        var item = CasinoItem.GetCasinoItem(itemGuid.Value);
                        item.Earning = 0;
                        item.Save(trans);

                        foreach (var bet in betList)
                        {
                            var gambler = new Gambler(bet.UserID, trans);

                            var dt = DataAccess.BetDetail.GetBetDetailByBetId(bet.ID);

                            var betDetail = new MatchResultBetDetail(dt);

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

                                    Users.UpdateUserExtCredits(bet.UserID, 4, 1);
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
                catch
                {
                    trans.Rollback();
                }
            }
        }

        public static void UpdateMatchResult(Guid casinoItem, short home, short away)
        {
            DataAccess.MatchResult.UpdateMatchResult(casinoItem, home, away, null);
        }
    }
}
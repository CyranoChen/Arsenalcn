using System;
using System.Collections.Generic;
using System.Data;

namespace Arsenalcn.CasinoSys.Entity
{
    public class CasinoGambler
    {
        public CasinoGambler()
        {
        }

        private CasinoGambler(DataRow dr)
        {
            InitCasinoGambler(dr);
        }

        private void InitCasinoGambler(DataRow dr)
        {
            if (dr != null)
            {
                UserID = Convert.ToInt32(dr["UserID"]);
                UserName = dr["UserName"].ToString();
                Win = Convert.ToInt32(dr["Win"]);
                Lose = Convert.ToInt32(dr["Lose"]);
                MatchBet = Convert.ToInt32(dr["MatchBet"]);
                Earning = Convert.ToSingle(dr["Earning"]);
                TotalBet = Convert.ToSingle(dr["TotalBet"]);

                if (!Convert.IsDBNull(dr["RPBet"]))
                    RPBet = Convert.ToInt32(dr["RPBet"]);
                else
                    RPBet = null;

                if (!Convert.IsDBNull(dr["RPBonus"]))
                    RPBonus = Convert.ToInt32(dr["RPBonus"]);
                else
                    RPBonus = null;

                Profit = Earning - TotalBet;

                if (TotalBet > 0f)
                {
                    ProfitRate = Profit / TotalBet * 100;
                }
                else
                {
                    ProfitRate = 0;
                }

                if (RPBet.HasValue && RPBet.Value > 0
                    && RPBonus.HasValue && RPBonus.Value >= 0)
                {
                    RPRate = RPBonus / RPBet * 100;
                }
                else
                {
                    RPRate = null;
                }
            }
            else
                throw new Exception("Unable to init CasinoGambler.");
        }

        public static List<CasinoGambler> GetCasinoGamblers()
        {
            var dt = DataAccess.Gambler.GetGamblerProfitView();
            var list = new List<CasinoGambler>();

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new CasinoGambler(dr));
                }
            }

            return list;
        }

        public static List<CasinoGambler> GetCasinoGamblers(Guid leagueGuid)
        {
            var dt = DataAccess.Gambler.GetGamblerProfitView(leagueGuid);
            var list = new List<CasinoGambler>();

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new CasinoGambler(dr));
                }
            }

            // 进入最终名次排行榜的标准（评选要求）同时满足以下3个条件：
            //1、赛季中必须投注博采币次数达到5个单场及以上（反复多次投注同一场比赛只能算是1次）；
            //2、赛季中参与累计投注量达到5,000菠菜币及以上；
            //3、赛季中并且获得RP+3及以上，即猜对本赛季3场以上的比赛比分。
            if (ConfigGlobal.ContestLimitIgnore) { return list; }

            return list.FindAll(cg => cg.MatchBet >= ConfigGlobal.RankCondition[0]
                && cg.TotalBet >= ConfigGlobal.RankCondition[1] && cg.RPBonus >= ConfigGlobal.RankCondition[2]);
        }

        public static List<CasinoGambler> SortCasinoGambler(List<CasinoGambler> list, string orderKeyword)
        {
            if (orderKeyword.Equals("ProfitRate", StringComparison.OrdinalIgnoreCase))
            {
                list.Sort(delegate (CasinoGambler cg1, CasinoGambler cg2)
                {
                    return !cg2.ProfitRate.Equals(cg1.ProfitRate)
                        ? cg2.ProfitRate.CompareTo(cg1.ProfitRate)
                        : cg2.Profit.CompareTo(cg1.Profit);
                });
            }
            else if (orderKeyword.Equals("TotalBet", StringComparison.OrdinalIgnoreCase))
            {
                list.Sort(delegate (CasinoGambler cg1, CasinoGambler cg2)
                {
                    return !cg2.TotalBet.Equals(cg1.TotalBet)
                        ? cg2.TotalBet.CompareTo(cg1.TotalBet)
                        : cg2.Profit.CompareTo(cg1.Profit);
                });
            }
            else if (orderKeyword.Equals("RPBonus", StringComparison.OrdinalIgnoreCase))
            {
                list.Sort(delegate (CasinoGambler cg1, CasinoGambler cg2)
                {
                    if (!cg1.RPBonus.HasValue && !cg2.RPBonus.HasValue)
                    {
                        return cg2.Profit.CompareTo(cg1.Profit);
                    }
                    if (cg1.RPBonus.HasValue && !cg2.RPBonus.HasValue)
                    {
                        return -1;
                    }
                    // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                    if (!cg1.RPBonus.HasValue && cg2.RPBonus.HasValue)
                    {
                        return 1;
                    }
                    return !cg2.RPBonus.Value.Equals(cg1.RPBonus.Value)
                        ? cg2.RPBonus.Value - cg1.RPBonus.Value
                        : cg2.Profit.CompareTo(cg1.Profit);
                });
            }
            else
            {
                list.Sort(delegate (CasinoGambler cg1, CasinoGambler cg2) { return cg2.Profit.CompareTo(cg1.Profit); });
            }

            //    orderClause = string.Format("{0} DESC, Profit DESC, RPBonus DESC, TotalBet DESC", orderKeyword);
            //else if (orderKeyword == "TotalBet")
            //    orderClause = string.Format("{0} DESC, Profit DESC, RPBonus DESC, ProfitRate DESC", orderKeyword);
            //else if (orderKeyword == "RPBonus")
            //    orderClause = string.Format("{0} DESC, Profit DESC, ProfitRate DESC, TotalBet DESC", orderKeyword);
            //else
            //    orderClause = "Profit DESC, RPBonus DESC, ProfitRate DESC, TotalBet DESC";

            var rank = 1;

            foreach (var cg in list)
            {
                cg.Rank = rank++;
                cg.Credit = cg.Rank;

                //dr["Profit"] = Convert.ToSingle(dr["Earning"]) - Convert.ToSingle(dr["TotalBet"]);
                //if (Convert.ToSingle(dr["TotalBet"]) > 0f)
                //    dr["ProfitRate"] = Convert.ToSingle(dr["Profit"]) / Convert.ToSingle(dr["TotalBet"]) * 100;
                //else
                //    dr["ProfitRate"] = 0;

                //int RPBonus = Entity.Gambler.GetGamblerRPByUserID(Convert.ToInt32(dr["UserID"]), CurrentLeague);

                //if (RPBonus > 0)
                //    dr["RPBonus"] = RPBonus;
            }

            return list;
        }

        public static List<CasinoGambler> SortCasinoGambler(List<CasinoGambler> list)
        {
            if (list != null && list.Count > 0)
            {
                var dictCasinoGambler = new Dictionary<int, int>();
                string[] arrayOrderKeyword = { "ProfitRate", "TotalBet", "RPBonus", "Profit" };

                // Sort List<CasinoGambler> by different orderKeywords
                foreach (var s in arrayOrderKeyword)
                {
                    var sortedList = SortCasinoGambler(list, s);

                    // Insert CasinoGambler instance into Dictionary or Add the rank of exist instance
                    if (sortedList != null && sortedList.Count > 0)
                    {
                        foreach (var cg in sortedList)
                        {
                            if (dictCasinoGambler.ContainsKey(cg.UserID))
                            {
                                dictCasinoGambler[cg.UserID] += cg.Rank;
                            }
                            else
                            {
                                dictCasinoGambler.Add(cg.UserID, cg.Rank);
                            }
                        }
                    }
                }

                // Update the Rank of every list Gamblers
                foreach (var cg in list)
                {
                    cg.Credit = dictCasinoGambler[cg.UserID];
                }

                // Sort the final list
                list.Sort(
                    (cg1, cg2) => cg2.Credit != null && cg1.Credit != null && !cg1.Credit.Value.Equals(cg2.Credit.Value)
                        ? cg1.Credit.Value - cg2.Credit.Value
                        : cg2.Profit.CompareTo(cg1.Profit));

                var rank = 1;
                foreach (var cg in list)
                {
                    cg.Rank = rank++;
                }
            }

            return list;
        }

        #region Members and Properties

        public int UserID { get; set; }

        public string UserName { get; set; }

        public int Win { get; set; }

        public int Lose { get; set; }

        public int MatchBet { get; set; }

        public float Earning { get; set; }

        public float TotalBet { get; set; }

        public int? RPBet { get; set; }

        public int? RPBonus { get; set; }

        public float Profit { get; set; }

        public float ProfitRate { get; set; }

        public float? RPRate { get; set; }

        public int Rank { get; set; }

        public int? Credit { get; set; }

        #endregion
    }
}
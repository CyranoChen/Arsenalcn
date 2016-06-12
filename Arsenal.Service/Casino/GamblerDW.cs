using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Arsenalcn.Core;
using DataReaderMapper;
using DataReaderMapper.Mappers;

namespace Arsenal.Service.Casino
{
    // ReSharper disable once InconsistentNaming
    public class GamblerDW
    {
        private static void CreateMap()
        {
            var map = Mapper.CreateMap<IDataReader, GamblerDW>();

            map.ForMember(d => d.Profit,
                opt => opt.MapFrom(s => (double)s.GetValue("Earning") - (double)s.GetValue("TotalBet")));

            map.ForMember(d => d.ProfitRate, opt => opt.ResolveUsing(s =>
            {
                var earning = (double)s.GetValue("Earning");
                var totalBet = (double)s.GetValue("TotalBet");

                if (totalBet > 0)
                {
                    return (earning - totalBet) / totalBet * 100;
                }
                else
                {
                    return 0f;
                }
            }));
        }

        public static GamblerDW Single(int key, Guid leagueGuid)
        {
            var sql = @"SELECT BetInfo.*, RPInfo.RPBet, RPInfo.RPBonus FROM
                                        (SELECT UserID, UserName, 
                                                    COUNT(CASE IsWin WHEN 1 THEN 1 ELSE NULL END) AS Win, 
                                                    COUNT(CASE IsWin WHEN 0 THEN 0 ELSE NULL END) AS Lose, 
                                                    COUNT(distinct CAST(CasinoItemGuid AS CHAR(50))) AS MatchBet, 
                                                    SUM(ISNULL(Earning, 0)) AS Earning, 
                                                    SUM(ISNULL(Bet, 0)) AS TotalBet
                                        FROM dbo.vw_AcnCasino_BetInfo 
                                        WHERE (UserID = @key) AND (Earning IS NOT NULL) AND (Bet IS NOT NULL) AND (ItemType = 2) AND (LeagueGuid = @leagueGuid)
                                        GROUP BY UserID, UserName) AS BetInfo
                                    LEFT OUTER JOIN
                                        (SELECT UserID, UserName, 
                                                    COUNT(ID) AS RPBet, 
                                                    COUNT(CASE EarningDesc WHEN 'RP+1' THEN 1 ELSE NULL END) AS RPBonus
                                        FROM dbo.vw_AcnCasino_BetInfo 
                                        WHERE (UserID = @key) AND (Earning = 0) AND (Bet IS NULL) AND (ItemType = 1) AND (LeagueGuid = @leagueGuid)
                                        GROUP BY UserID, UserName) AS RPInfo
                                    ON BetInfo.UserID = RPInfo.UserID AND BetInfo.UserName = RPInfo.UserName
                                    ORDER BY BetInfo.TotalBet DESC";

            SqlParameter[] para = { new SqlParameter("@key", key), new SqlParameter("@leagueGuid", leagueGuid) };

            var ds = DataAccess.ExecuteDataset(sql, para);

            var dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                using (var reader = dt.CreateDataReader())
                {
                    Mapper.Initialize(cfg =>
                    {
                        MapperRegistry.Mappers.Insert(0,
                            new DataReaderMapper.DataReaderMapper { YieldReturnEnabled = false });

                        CreateMap();
                    });

                    return Mapper.Map<IDataReader, IEnumerable<GamblerDW>>(reader).FirstOrDefault();
                }
            }

            return null;
        }

        public static List<GamblerDW> All(Guid leagueGuid)
        {
            var sql = @"SELECT BetInfo.*, RPInfo.RPBet, RPInfo.RPBonus FROM
                                        (SELECT UserID, UserName, 
                                                    COUNT(CASE IsWin WHEN 1 THEN 1 ELSE NULL END) AS Win, 
                                                    COUNT(CASE IsWin WHEN 0 THEN 0 ELSE NULL END) AS Lose, 
                                                    COUNT(distinct CAST(CasinoItemGuid AS CHAR(50))) AS MatchBet, 
                                                    SUM(ISNULL(Earning, 0)) AS Earning, 
                                                    SUM(ISNULL(Bet, 0)) AS TotalBet
                                        FROM dbo.vw_AcnCasino_BetInfo 
                                        WHERE (Earning IS NOT NULL) AND (Bet IS NOT NULL) AND (ItemType = 2) AND (LeagueGuid = @leagueGuid)
                                        GROUP BY UserID, UserName) AS BetInfo
                                    LEFT OUTER JOIN
                                        (SELECT UserID, UserName, 
                                                    COUNT(ID) AS RPBet, 
                                                    COUNT(CASE EarningDesc WHEN 'RP+1' THEN 1 ELSE NULL END) AS RPBonus
                                        FROM dbo.vw_AcnCasino_BetInfo 
                                        WHERE (Earning = 0) AND (Bet IS NULL) AND (ItemType = 1) AND (LeagueGuid = @leagueGuid)
                                        GROUP BY UserID, UserName) AS RPInfo
                                    ON BetInfo.UserID = RPInfo.UserID AND BetInfo.UserName = RPInfo.UserName
                                    ORDER BY BetInfo.TotalBet DESC";

            SqlParameter[] para = { new SqlParameter("@leagueGuid", leagueGuid) };

            var ds = DataAccess.ExecuteDataset(sql, para);

            var dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                using (var reader = dt.CreateDataReader())
                {
                    Mapper.Initialize(cfg =>
                    {
                        MapperRegistry.Mappers.Insert(0,
                            new DataReaderMapper.DataReaderMapper { YieldReturnEnabled = false });

                        CreateMap();
                    });

                    return Mapper.Map<IDataReader, IEnumerable<GamblerDW>>(reader).ToList();
                }
            }

            return null;
        }

        public static double GetGamblerBetLimit(int userId, Guid leagueGuid)
        {
            IRepository repo = new Repository();

            var gambler = repo.Query<Gambler>(x => x.UserID == userId).FirstOrDefault();
            var cash = gambler?.Cash ?? 0f;

            if (leagueGuid.Equals(ConfigGlobal_AcnCasino.DefaultLeagueID))
            {
                var g = Single(userId, leagueGuid);

                // 如果没有投过注，或投注量小于标准，判断是否在下半赛区
                if (g == null || g.TotalBet < ConfigGlobal_AcnCasino.TotalBetStandard)
                {
                    var singleBetLimit = ConfigGlobal_AcnCasino.SingleBetLimit;

                    return cash < singleBetLimit ? cash : singleBetLimit;
                }
            }

            return cash;
        }

        public static List<GamblerDW> SortRank(List<GamblerDW> list, string orderKeyword)
        {
            if (orderKeyword.Equals("ProfitRate", StringComparison.OrdinalIgnoreCase))
            {
                list = list.OrderByDescending(x => x.ProfitRate)
                    .ThenByDescending(x => x.Profit)
                    .ThenByDescending(x => x.RPBonus)
                    .ThenByDescending(x => x.TotalBet)
                    .ThenBy(x => x.UserID)
                    .ToList();
            }
            else if (orderKeyword.Equals("TotalBet", StringComparison.OrdinalIgnoreCase))
            {
                list = list.OrderByDescending(x => x.TotalBet)
                    .ThenByDescending(x => x.ProfitRate)
                    .ThenByDescending(x => x.Profit)
                    .ThenByDescending(x => x.RPBonus)
                    .ThenBy(x => x.UserID)
                    .ToList();
            }
            else if (orderKeyword.Equals("RPBonus", StringComparison.OrdinalIgnoreCase))
            {
                list = list.OrderByDescending(x => x.RPBonus)
                    .ThenByDescending(x => x.ProfitRate)
                    .ThenByDescending(x => x.Profit)
                    .ThenByDescending(x => x.TotalBet)
                    .ThenBy(x => x.UserID)
                    .ToList();
            }
            else
            {
                list = list.OrderByDescending(x => x.Profit)
                    .ThenByDescending(x => x.ProfitRate)
                    .ThenByDescending(x => x.RPBonus)
                    .ThenByDescending(x => x.TotalBet)
                    .ThenBy(x => x.UserID)
                    .ToList();
            }

            var rank = 1;

            foreach (var g in list)
            {
                g.Rank = rank++;
                g.Credit = g.Rank;
            }

            return list;
        }

        public static List<GamblerDW> SortRank(List<GamblerDW> list)
        {
            if (list != null && list.Count > 0)
            {
                var dictRank = new Dictionary<int, int>();
                string[] arrayOrderKeyword = { "ProfitRate", "TotalBet", "RPBonus", "Profit" };

                // Sort List<CasinoGambler> by different orderKeywords
                foreach (var s in arrayOrderKeyword)
                {
                    var sortedList = SortRank(list, s);

                    // Insert CasinoGambler instance into Dictionary or Add the rank of exist instance
                    if (sortedList != null && sortedList.Count > 0)
                    {
                        foreach (var g in sortedList)
                        {
                            if (dictRank.ContainsKey(g.UserID))
                            {
                                dictRank[g.UserID] += g.Rank;
                            }
                            else
                            {
                                dictRank.Add(g.UserID, g.Rank);
                            }
                        }
                    }
                }

                // Update the Rank of every list Gamblers
                foreach (var g in list)
                {
                    g.Credit = dictRank[g.UserID];
                }

                // Sort the final list
                list.Sort(
                    (g1, g2) => g2.Credit != null && g1.Credit != null && !g1.Credit.Value.Equals(g2.Credit.Value)
                        ? g1.Credit.Value - g2.Credit.Value
                        : g2.Profit.CompareTo(g1.Profit));

                var rank = 1;
                foreach (var g in list)
                {
                    g.Rank = rank++;
                }
            }

            return list;
        }

        #region Members and Properties

        [DbColumn("UserID", IsKey = true)]
        public int UserID { get; set; }

        [DbColumn("UserName")]
        public string UserName { get; set; }

        [DbColumn("Win")]
        public int Win { get; set; }

        [DbColumn("Lose")]
        public int Lose { get; set; }

        [DbColumn("MatchBet")]
        public int MatchBet { get; set; }

        [DbColumn("Earning")]
        public double Earning { get; set; }

        [DbColumn("TotalBet")]
        public double TotalBet { get; set; }

        [DbColumn("RPBet")]
        public int RPBet { get; set; }

        [DbColumn("RPBonus")]
        public int RPBonus { get; set; }

        public double Profit { get; set; }

        public double ProfitRate { get; set; }

        public int Rank { get; set; }

        public int? Credit { get; set; }

        #endregion
    }
}
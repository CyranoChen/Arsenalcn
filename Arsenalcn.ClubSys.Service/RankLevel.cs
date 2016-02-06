using System.Collections.Generic;
using System.Data;
using Arsenalcn.ClubSys.Entity;
using Arsenalcn.Common;
using Microsoft.ApplicationBlocks.Data;

namespace Arsenalcn.ClubSys.Service
{
    public class RankLevel
    {
        private static RankLevel _rankLevel;

        private RankLevel()
        {
            AllRanks = GetSysRanks();
        }

        public List<Rank> AllRanks { get; }

        public Rank GetRank(int clubFortune)
        {
            foreach (var rank in AllRanks)
            {
                if (clubFortune <= rank.MaxClubFortune)
                    return rank;
            }

            return AllRanks[AllRanks.Count - 1];
        }

        public static RankLevel GetInstance()
        {
            if (_rankLevel == null)
            {
                lock (typeof (RankLevel))
                {
                    if (_rankLevel == null)
                        _rankLevel = new RankLevel();
                }
            }

            return _rankLevel;
        }

        internal static List<Rank> GetSysRanks()
        {
            var sql = "SELECT * FROM dbo.AcnClub_ConfigRank	ORDER BY RankLevelID";

            var ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
                return new List<Rank>();
            var list = new List<Rank>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(new Rank(dr));
            }
            return list;
        }
    }
}
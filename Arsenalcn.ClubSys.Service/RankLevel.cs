using System.Collections.Generic;
using System.Data;

using Microsoft.ApplicationBlocks.Data;

using Arsenalcn.ClubSys.Entity;
using Arsenalcn.Common;

namespace Arsenalcn.ClubSys.Service
{
    public class RankLevel
    {
        private static RankLevel _rankLevel = null;

        private List<Rank> _ranks;
        private RankLevel()
        {
            _ranks = GetSysRanks();
        }

        public Rank GetRank(int clubFortune)
        {
            foreach (Rank rank in _ranks)
            {
                if (clubFortune <= rank.MaxClubFortune)
                    return rank;
            }

            return _ranks[_ranks.Count - 1];
        }

        public List<Rank> AllRanks
        {
            get
            {
                return _ranks;
            }
        }

        public static RankLevel GetInstance()
        {
            if (_rankLevel == null)
            {
                lock (typeof(RankLevel))
                {
                    if (_rankLevel == null)
                        _rankLevel = new RankLevel();
                }
            }

            return _rankLevel;
        }

        internal static List<Rank> GetSysRanks()
        {
            string sql = "SELECT * FROM dbo.AcnClub_ConfigRank	ORDER BY RankLevelID";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
                return new List<Rank>();
            else
            {
                List<Rank> list = new List<Rank>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(new Rank(dr));
                }
                return list;
            }
        }
    }
}

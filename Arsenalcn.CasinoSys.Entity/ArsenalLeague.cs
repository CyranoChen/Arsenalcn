using System;
using System.Collections.Generic;
using System.Data;

namespace Arsenalcn.CasinoSys.Entity
{
    public class League
    {
        private League(DataRow dr)
        {
            InitLeague(dr);
        }

        private void InitLeague(DataRow dr)
        {
            if (dr != null)
            {
                ID = (Guid) dr["LeagueGuid"];
                LeagueName = dr["LeagueName"].ToString();
                LeagueOrgName = dr["LeagueOrgName"].ToString();
                LeagueSeason = dr["LeagueSeason"].ToString();
                LeagueTime = Convert.ToDateTime(dr["LeagueTime"]);
                LeagueLogo = dr["LeagueLogo"].ToString();
                LeagueOrder = Convert.ToInt32(dr["LeagueOrder"]);
                IsActive = Convert.ToBoolean(dr["IsActive"]);

                LeagueNameInfo = LeagueName + LeagueSeason;
            }
            else
                throw new Exception("Unable to init League.");
        }

        public static List<League> GetLeagues()
        {
            var dt = DataAccess.League.GetLeagues();
            var list = new List<League>();

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new League(dr));
                }
            }

            return list;
        }

        public static class Cache
        {
            public static List<League> LeagueList;
            public static List<League> LeagueListActive;

            static Cache()
            {
                InitCache();
            }

            public static void RefreshCache()
            {
                InitCache();
            }

            private static void InitCache()
            {
                LeagueList = GetLeagues();
                LeagueListActive = LeagueList.FindAll(x => x.IsActive);
            }

            public static League Load(Guid guid)
            {
                return LeagueList.Find(l => l.ID.Equals(guid));
            }

            public static List<League> GetSeasonsByLeagueGuid(Guid guid)
            {
                return LeagueList.FindAll(l => l.LeagueName.Equals(Load(guid).LeagueName));
            }
        }

        #region Members and Properties

        public Guid ID { get; set; }

        public string LeagueName { get; set; }

        public string LeagueOrgName { get; set; }

        public string LeagueSeason { get; set; }

        public DateTime LeagueTime { get; set; }

        public string LeagueLogo { get; set; }

        public int LeagueOrder { get; set; }

        public bool IsActive { get; set; }

        public int TeamCountInfo { get; set; }

        public string LeagueNameInfo { get; set; }

        #endregion
    }
}
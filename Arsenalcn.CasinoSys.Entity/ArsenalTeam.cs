using System;
using System.Collections.Generic;
using System.Data;

namespace Arsenalcn.CasinoSys.Entity
{
    public class Team
    {
        private Team(DataRow dr)
        {
            InitTeam(dr);
        }

        private void InitTeam(DataRow dr)
        {
            if (dr != null)
            {
                ID = (Guid) dr["TeamGuid"];
                TeamEnglishName = dr["TeamEnglishName"].ToString();
                TeamDisplayName = dr["TeamDisplayName"].ToString();
                TeamLogo = dr["TeamLogo"].ToString();
                Ground = dr["Ground"].ToString();

                if (Convert.IsDBNull(dr["Capacity"]))
                    Capacity = null;
                else
                    Capacity = Convert.ToInt32(dr["Capacity"]);

                Manager = dr["Manager"].ToString();
            }
            else
                throw new Exception("Unable to init Team.");
        }

        public static List<Team> GetTeams()
        {
            var dt = DataAccess.Team.GetTeams();
            var list = new List<Team>();

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new Team(dr));
                }
            }

            return list;
        }

        public static class Cache
        {
            public static List<Team> TeamList;

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
                TeamList = GetTeams();
            }

            public static Team Load(Guid guid)
            {
                return TeamList.Find(x => x.ID.Equals(guid));
            }

            public static List<Team> GetTeamsByLeagueGuid(Guid guid)
            {
                return TeamList.FindAll(x =>
                    new RelationLeagueTeam {TeamGuid = x.ID, LeagueGuid = guid}.Any());
            }
        }

        #region Members and Properties

        public Guid ID { get; set; }

        public string TeamEnglishName { get; set; }

        public string TeamDisplayName { get; set; }

        public string TeamLogo { get; set; }

        public string Ground { get; set; }

        public int? Capacity { get; set; }

        public string Manager { get; set; }

        public Guid LeagueGuid { get; set; }

        public int LeagueCountInfo { get; set; }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Data;

namespace Arsenal.Entity
{
    public class Team
    {
        public Team() { }

        private Team(DataRow dr)
        {
            InitTeam(dr);
        }

        private void InitTeam(DataRow dr)
        {
            if (dr != null)
            {
                TeamGuid = (Guid)dr["TeamGuid"];
                TeamEnglishName = dr["TeamEnglishName"].ToString();
                TeamDisplayName = dr["TeamDisplayName"].ToString();
                TeamLogo = dr["TeamLogo"].ToString();
                TeamNickName = dr["TeamNickName"].ToString();
                Founded = dr["Founded"].ToString();
                Ground = dr["Ground"].ToString();

                if (Convert.IsDBNull(dr["Capacity"]))
                    Capacity = null;
                else
                    Capacity = Convert.ToInt32(dr["Capacity"]);

                Chairman = dr["Chairman"].ToString();
                Manager = dr["Manager"].ToString();
                //LeagueGuid = (Guid)dr["LeagueGuid"];

                // Generate League Count Info
                List<RelationLeagueTeam> list = RelationLeagueTeam.GetRelationLeagueTeams().FindAll(delegate(RelationLeagueTeam rlt) { return rlt.TeamGuid.Equals(TeamGuid); });
                if (list != null && list.Count > 0)
                    LeagueCountInfo = list.Count;
                else
                    LeagueCountInfo = 0;
            }
            else
                throw new Exception("Unable to init Team.");
        }

        public void Select()
        {
            DataRow dr = DataAccess.Team.GetTeamByID(TeamGuid);

            if (dr != null)
                InitTeam(dr);
        }

        public void Update()
        {
            DataAccess.Team.UpdateTeam(TeamGuid, TeamEnglishName, TeamDisplayName, TeamLogo, TeamNickName, Founded, Ground, Capacity, Chairman, Manager, LeagueGuid);
        }

        public void Insert()
        {
            DataAccess.Team.InsertTeam(TeamGuid, TeamEnglishName, TeamDisplayName, TeamLogo, TeamNickName, Founded, Ground, Capacity, Chairman, Manager, LeagueGuid);
        }

        public void Delete()
        {
            DataAccess.Team.DeleteTeam(TeamGuid);
        }

        public static List<Team> GetTeams()
        {
            DataTable dt = DataAccess.Team.GetTeams();
            List<Team> list = new List<Team>();

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
                return TeamList.Find(delegate(Team t) { return t.TeamGuid.Equals(guid); });
            }

            public static List<Team> TeamList;
        }

        #region Members and Properties

        public Guid TeamGuid
        { get; set; }

        public string TeamEnglishName
        { get; set; }

        public string TeamDisplayName
        { get; set; }

        public string TeamLogo
        { get; set; }

        public string TeamNickName
        { get; set; }

        public string Founded
        { get; set; }

        public string Ground
        { get; set; }

        public int? Capacity
        { get; set; }

        public string Chairman
        { get; set; }

        public string Manager
        { get; set; }

        public Guid LeagueGuid
        { get; set; }

        public int LeagueCountInfo
        { get; set; }

        #endregion
    }
}

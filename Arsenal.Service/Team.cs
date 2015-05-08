using System;
using System.Collections.Generic;
using System.Data;

using Arsenalcn.Core;

namespace Arsenal.Service
{
    [AttrDbTable("Arsenal_Team", Key = "TeamGuid")]
    public class Team : Entity
    {
        public Team() { }

        public Team(DataRow dr)
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
                LeagueGuid = (Guid)dr["LeagueGuid"];

                // Generate League Count Info
                List<RelationLeagueTeam> list = new Entity().All<RelationLeagueTeam>().FindAll(rlt => rlt.TeamGuid.Equals(TeamGuid));

                if (list != null && list.Count > 0)
                    LeagueCountInfo = list.Count;
                else
                    LeagueCountInfo = 0;
            }
            else
                throw new Exception("Unable to init Team.");
        }

        //public void Select()
        //{
        //    DataRow dr = DataAccess.Team.GetTeamByID(TeamGuid);

        //    if (dr != null)
        //        InitTeam(dr);
        //}

        //public void Update()
        //{
        //    DataAccess.Team.UpdateTeam(TeamGuid, TeamEnglishName, TeamDisplayName, TeamLogo, TeamNickName, Founded, Ground, Capacity, Chairman, Manager, LeagueGuid);
        //}

        //public void Insert()
        //{
        //    DataAccess.Team.InsertTeam(TeamGuid, TeamEnglishName, TeamDisplayName, TeamLogo, TeamNickName, Founded, Ground, Capacity, Chairman, Manager, LeagueGuid);
        //}

        //public void Delete()
        //{
        //    DataAccess.Team.DeleteTeam(TeamGuid);
        //}

        //public static List<Team> GetTeams()
        //{
        //    DataTable dt = DataAccess.Team.GetTeams();
        //    List<Team> list = new List<Team>();

        //    if (dt != null)
        //    {
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            list.Add(new Team(dr));
        //        }
        //    }

        //    return list;
        //}

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
                TeamList = new Team().All<Team>();
            }

            public static Team Load(Guid guid)
            {
                return TeamList.Find(t => t.TeamGuid.Equals(guid));
            }

            public static List<Team> GetTeamsByLeagueGuid(Guid guid)
            {
                return TeamList.FindAll(t => RelationLeagueTeam.Exist(t.TeamGuid, guid));
            }

            public static List<Team> TeamList;
        }

        #region Members and Properties

        [AttrDbColumn("TeamGuid", IsKey = true)]
        public Guid TeamGuid
        { get; set; }

        [AttrDbColumn("TeamEnglishName")]
        public string TeamEnglishName
        { get; set; }

        [AttrDbColumn("TeamDisplayName")]
        public string TeamDisplayName
        { get; set; }

        [AttrDbColumn("TeamLogo")]
        public string TeamLogo
        { get; set; }

        [AttrDbColumn("TeamNickName")]
        public string TeamNickName
        { get; set; }

        [AttrDbColumn("Founded")]
        public string Founded
        { get; set; }

        [AttrDbColumn("Ground")]
        public string Ground
        { get; set; }

        [AttrDbColumn("Capacity")]
        public int? Capacity
        { get; set; }

        [AttrDbColumn("Chairman")]
        public string Chairman
        { get; set; }

        [AttrDbColumn("Manager")]
        public string Manager
        { get; set; }

        [AttrDbColumn("LeagueGuid")]
        public Guid LeagueGuid
        { get; set; }

        public int LeagueCountInfo
        { get; set; }

        #endregion
    }
}

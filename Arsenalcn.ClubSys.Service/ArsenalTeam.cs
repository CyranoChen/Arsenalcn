using System;
using System.Collections.Generic;
using System.Data;

namespace Arsenalcn.ClubSys.Service
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
                ID = (Guid)dr["TeamGuid"];
                TeamEnglishName = dr["TeamEnglishName"].ToString();
                TeamDisplayName = dr["TeamDisplayName"].ToString();
                TeamLogo = dr["TeamLogo"].ToString();
                Ground = dr["Ground"].ToString();

                if (Convert.IsDBNull(dr["Capacity"]))
                    Capacity = null;
                else
                    Capacity = Convert.ToInt32(dr["Capacity"]);

                Manager = dr["Manager"].ToString();

                //LeagueGuid = (Guid)dr["LeagueGuid"];
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

        #region Members and Properties

        public Guid ID
        { get; set; }

        public string TeamEnglishName
        { get; set; }

        public string TeamDisplayName
        { get; set; }

        public string TeamLogo
        { get; set; }

        public string Ground
        { get; set; }

        public int? Capacity
        { get; set; }

        public string Manager
        { get; set; }

        public Guid LeagueGuid
        { get; set; }

        public int LeagueCountInfo
        { get; set; }

        #endregion

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
                return TeamList.Find(x => x.ID.Equals(guid));
            }

            public static List<Team> GetTeamsByLeagueGuid(Guid guid)
            {
                return TeamList.FindAll(x =>
                    new RelationLeagueTeam() { TeamGuid = x.ID, LeagueGuid = guid }.Any());
            }

            public static List<Team> TeamList;
        }


        //public static class Cache
        //{
        //    static Cache()
        //    {
        //        InitCache();
        //    }

        //    public static void RefreshCache()
        //    {
        //        InitCache();
        //    }

        //    private static void InitCache()
        //    {
        //        var svc = RemoteServiceProvider.GetWebService();
        //        var arrayTeams = svc.GetTeams();

        //        if (TeamList != null)
        //        { TeamList.Clear(); }
        //        else
        //        { TeamList = new List<Arsenal.Team>(); }

        //        if (arrayTeams != null && arrayTeams.Length > 0)
        //        {
        //            foreach (Arsenal.Team t in arrayTeams)
        //            {
        //                TeamList.Add(t);
        //            }
        //        }
        //    }

        //    public static Arsenal.Team Load(Guid guid)
        //    {
        //        return TeamList.Find(delegate(Arsenal.Team t) { return t.ID.Equals(guid); });
        //    }

        //    public static List<Arsenal.Team> GetTeamsByLeagueGuid(Guid guid)
        //    {
        //        var svc = RemoteServiceProvider.GetWebService();
        //        var arrayTeams = svc.GetTeamsByLeagueGuid(guid);
        //        var list = new List<Arsenal.Team>();

        //        if (arrayTeams != null && arrayTeams.Length > 0)
        //        {
        //            foreach (Arsenal.Team t in arrayTeams)
        //            {
        //                list.Add(t);
        //            }
        //        }

        //        return list;
        //    }

        //    public static List<Arsenal.Team> TeamList;
        //}
    }
}

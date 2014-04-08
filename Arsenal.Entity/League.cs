using System;
using System.Collections.Generic;
using System.Data;

namespace Arsenal.Entity
{
    public class League
    {
        public League() { }

        private League(DataRow dr)
        {
            InitLeague(dr);
        }

        private void InitLeague(DataRow dr)
        {
            if (dr != null)
            {
                LeagueGuid = (Guid)dr["LeagueGuid"];
                LeagueName = dr["LeagueName"].ToString();
                LeagueOrgName = dr["LeagueOrgName"].ToString();
                LeagueSeason = dr["LeagueSeason"].ToString();
                LeagueTime = (DateTime)dr["LeagueTime"];
                LeagueLogo = dr["LeagueLogo"].ToString();
                LeagueOrder = Convert.ToInt16(dr["LeagueOrder"]);
                IsActive = Convert.ToBoolean(dr["IsActive"]);

                // Generate League Count Info
                List<RelationLeagueTeam> list = RelationLeagueTeam.GetRelationLeagueTeams().FindAll(delegate(RelationLeagueTeam rlt) { return rlt.LeagueGuid.Equals(LeagueGuid); });
                if (list != null && list.Count > 0)
                    TeamCountInfo = list.Count;
                else
                    TeamCountInfo = 0;

                // Generate League Name Info
                LeagueNameInfo = LeagueName + LeagueSeason;
            }
            else
                throw new Exception("Unable to init League.");
        }

        public void Select()
        {
            DataRow dr = DataAccess.League.GetLeagueByID(LeagueGuid);

            if (dr != null)
                InitLeague(dr);
        }

        public void Update()
        {
            DataAccess.League.UpdateLeague(LeagueGuid, LeagueName, LeagueOrgName, LeagueSeason, LeagueTime, LeagueLogo, LeagueOrder, IsActive);
        }

        public void Insert()
        {
            DataAccess.League.InsertLeague(LeagueGuid, LeagueName, LeagueOrgName, LeagueSeason, LeagueTime, LeagueLogo, LeagueOrder, IsActive);
        }

        public void Delete()
        {
            DataAccess.League.DeleteLeague(LeagueGuid);
        }

        public static List<League> GetLeagues()
        {
            DataTable dt = DataAccess.League.GetLeagues();
            List<League> list = new List<League>();

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
                LeagueList_Active = LeagueList.FindAll(delegate(League l) { return l.IsActive; });
            }

            public static League Load(Guid guid)
            {
                return LeagueList.Find(delegate(League l) { return l.LeagueGuid.Equals(guid); });
            }

            public static List<League> LeagueList;
            public static List<League> LeagueList_Active;
        }

        //public static DataTable GetLeague(bool isActive)
        //{
        //    return DataAccess.League.GetAllLeagues(isActive);
        //}

        //public static DataTable GetLeagueAllSeason(Guid leagueGuid)
        //{
        //    return DataAccess.League.GetLeagueAllSeason(leagueGuid);
        //}

        #region Members and Properties

        public Guid LeagueGuid
        { get; set; }

        public string LeagueName
        { get; set; }

        public string LeagueOrgName
        { get; set; }

        public string LeagueSeason
        { get; set; }

        public DateTime LeagueTime
        { get; set; }

        public string LeagueLogo
        { get; set; }

        public int LeagueOrder
        { get; set; }

        public bool IsActive
        { get; set; }

        public int TeamCountInfo
        { get; set; }

        public string LeagueNameInfo
        { get; set; }

        #endregion
    }
}

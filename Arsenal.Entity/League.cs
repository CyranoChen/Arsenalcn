using System;
using System.Collections.Generic;
using System.Data;

using Arsenalcn.Core;

namespace Arsenal.Service
{
    [AttrDbTable("Arsenal_League", Key = "LeagueGuid")]
    public class League : Entity
    {
        public League() : base() { }

        public League(DataRow dr)
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
                List<RelationLeagueTeam> list = new RelationLeagueTeam().All<RelationLeagueTeam>().FindAll(rlt => rlt.LeagueGuid.Equals(LeagueGuid));
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

        //public void Select()
        //{
        //    DataRow dr = DataAccess.League.GetLeagueByID(LeagueGuid);

        //    if (dr != null)
        //        InitLeague(dr);
        //}

        //public void Update()
        //{
        //    DataAccess.League.UpdateLeague(LeagueGuid, LeagueName, LeagueOrgName, LeagueSeason, LeagueTime, LeagueLogo, LeagueOrder, IsActive);
        //}

        //public void Insert()
        //{
        //    DataAccess.League.InsertLeague(LeagueGuid, LeagueName, LeagueOrgName, LeagueSeason, LeagueTime, LeagueLogo, LeagueOrder, IsActive);
        //}

        //public void Delete()
        //{
        //    DataAccess.League.DeleteLeague(LeagueGuid);
        //}

        //public static List<League> GetLeagues()
        //{
        //    DataTable dt = DataAccess.League.GetLeagues();
        //    List<League> list = new List<League>();

        //    if (dt != null)
        //    {
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            list.Add(new League(dr));
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
                LeagueList = new League().All<League>();
                LeagueList_Active = LeagueList.FindAll(l => l.IsActive);
            }

            public static League Load(Guid guid)
            {
                return LeagueList.Find(l => l.LeagueGuid.Equals(guid));
            }

            public static List<League> LeagueList;
            public static List<League> LeagueList_Active;
        }

        #region Members and Properties
        [AttrDbColumn("LeagueGuid", IsKey = true)]
        public Guid LeagueGuid
        { get; set; }

        [AttrDbColumn("LeagueName")]
        public string LeagueName
        { get; set; }

        [AttrDbColumn("LeagueOrgName")]
        public string LeagueOrgName
        { get; set; }

        [AttrDbColumn("LeagueSeason")]
        public string LeagueSeason
        { get; set; }

        [AttrDbColumn("LeagueTime")]
        public DateTime LeagueTime
        { get; set; }

        [AttrDbColumn("LeagueLogo")]
        public string LeagueLogo
        { get; set; }

        [AttrDbColumn("LeagueOrder")]
        public int LeagueOrder
        { get; set; }

        [AttrDbColumn("IsActive")]
        public bool IsActive
        { get; set; }

        public int TeamCountInfo
        { get; set; }

        public string LeagueNameInfo
        { get; set; }

        #endregion
    }
}

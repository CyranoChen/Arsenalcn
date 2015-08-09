using System;
using System.Collections.Generic;
using System.Data;

namespace Arsenalcn.CasinoSys.Entity
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
                ID = (Guid)dr["LeagueGuid"];
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

        #region Members and Properties

        public Guid ID
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
                LeagueList_Active = LeagueList.FindAll(x => x.IsActive);
            }

            public static League Load(Guid guid)
            {
                return LeagueList.Find(delegate (League l) { return l.ID.Equals(guid); });
            }

            public static List<League> GetSeasonsByLeagueGuid(Guid guid)
            {
                return LeagueList.FindAll(delegate (League l) { return l.LeagueName.Equals(Load(guid).LeagueName); });
            }

            public static List<League> LeagueList;
            public static List<League> LeagueList_Active;
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
        //        var arrayLeagues = svc.GetLeagues();

        //        if (LeagueList != null)
        //        { LeagueList.Clear(); }
        //        else
        //        { LeagueList = new List<Arsenal.League>(); }

        //        if (arrayLeagues != null && arrayLeagues.Length > 0)
        //        {
        //            foreach (Arsenal.League l in arrayLeagues)
        //            {
        //                LeagueList.Add(l);
        //            }
        //        }

        //        LeagueList_Active = LeagueList.FindAll(delegate(Arsenal.League l) { return l.IsActive; });
        //    }

        //    public static Arsenal.League Load(Guid guid)
        //    {
        //        return LeagueList.Find(delegate(Arsenal.League l) { return l.ID.Equals(guid); });
        //        //return LeagueList.Find(l => l.LeagueGuid.Equals(guid));
        //    }

        //    public static List<Arsenal.League> GetSeasonsByLeagueGuid(Guid guid)
        //    {
        //        return LeagueList.FindAll(delegate(Arsenal.League l) { return l.LeagueName.Equals(Load(guid).LeagueName); });
        //    }

        //    public static List<Arsenal.League> LeagueList;
        //    public static List<Arsenal.League> LeagueList_Active;
        //}
    }
}

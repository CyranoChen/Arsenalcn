using System;
using System.Collections.Generic;
using System.Data;

using Arsenalcn.Core;

namespace Arsenal.Service
{
    [AttrDbTable("Arsenal_Match", Key = "MatchGuid")]
    public class Match : Entity
    {
        public Match() { }

        public Match(DataRow dr)
        {
            InitMatch(dr);
        }

        private void InitMatch(DataRow dr)
        {
            if (dr != null)
            {
                MatchGuid = (Guid)dr["MatchGuid"];
                TeamGuid = (Guid)dr["TeamGuid"];
                TeamName = dr["TeamName"].ToString();
                IsHome = Convert.ToBoolean(dr["IsHome"]);

                if (!Convert.IsDBNull(dr["ResultHome"]))
                    ResultHome = Convert.ToInt16(dr["ResultHome"]);
                else
                    ResultHome = null;

                if (!Convert.IsDBNull(dr["ResultAway"]))
                    ResultAway = Convert.ToInt16(dr["ResultAway"]);
                else
                    ResultAway = null;

                PlayTime = (DateTime)dr["PlayTime"];

                if (!Convert.IsDBNull(dr["LeagueGuid"]))
                    LeagueGuid = (Guid)dr["LeagueGuid"];
                else
                    LeagueGuid = null;

                LeagueName = dr["LeagueName"].ToString();

                if (!Convert.IsDBNull(dr["Round"]))
                    Round = Convert.ToInt16(dr["Round"]);
                else
                    Round = null;

                if (!Convert.IsDBNull(dr["GroupGuid"]))
                    GroupGuid = (Guid)dr["GroupGuid"];
                else
                    GroupGuid = null;

                if (!Convert.IsDBNull(dr["CasinoMatchGuid"]))
                    CasinoMatchGuid = (Guid)dr["CasinoMatchGuid"];
                else
                    CasinoMatchGuid = null;

                ReportImageURL = dr["ReportImageURL"].ToString();
                ReportURL = dr["ReportURL"].ToString();
                TopicURL = dr["TopicURL"].ToString();
                IsActive = Convert.ToBoolean(dr["IsActive"]);
                Remark = dr["Remark"].ToString();

                #region Generate Match ResultInfo

                if (ResultHome.HasValue && ResultAway.HasValue)
                {
                    if (IsHome)
                        ResultInfo = ResultHome.Value.ToString() + "：" + ResultAway.Value.ToString();
                    else
                        ResultInfo = ResultAway.Value.ToString() + "：" + ResultHome.Value.ToString();
                }
                else
                    ResultInfo = string.Empty;

                #endregion
            }
            else
                throw new Exception("Unable to init Match.");
        }

        //public void Select()
        //{
        //    DataRow dr = DataAccess.Match.GetMatchByID(MatchGuid);

        //    if (dr != null)
        //        InitMatch(dr);
        //}

        //public void Update()
        //{
        //    DataAccess.Match.UpdateMatch(MatchGuid, TeamGuid, TeamName, IsHome, ResultHome, ResultAway, PlayTime, LeagueGuid.Value, LeagueName, Round, GroupGuid, CasinoMatchGuid, ReportImageURL, ReportURL, TopicURL, IsActive, Remark);
        //}

        //public void Insert()
        //{
        //    DataAccess.Match.InsertMatch(MatchGuid, TeamGuid, TeamName, IsHome, ResultHome, ResultAway, PlayTime, LeagueGuid.Value, LeagueName, Round, GroupGuid, CasinoMatchGuid, ReportImageURL, ReportURL, TopicURL, IsActive, Remark);
        //}

        //public void Delete()
        //{
        //    DataAccess.Match.DeleteMatch(MatchGuid);
        //}

        //public static List<Match> GetMatchs()
        //{
        //    DataTable dt = DataAccess.Match.GetMatchs();
        //    List<Match> list = new List<Match>();

        //    if (dt != null)
        //    {
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            list.Add(new Match(dr));
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
                MatchList = new Match().All<Match>(); ;
            }

            public static Match Load(Guid guid)
            {
                return MatchList.Find(m => m.MatchGuid.Equals(guid));
            }

            public static List<Match> MatchList;
        }

        #region Members and Properties

        [AttrDbColumn("MatchGuid", IsKey = true)]
        public Guid MatchGuid
        { get; set; }

        [AttrDbColumn("TeamGuid")]
        public Guid TeamGuid
        { get; set; }

        [AttrDbColumn("TeamName")]
        public string TeamName
        { get; set; }

        [AttrDbColumn("IsHome")]
        public Boolean IsHome
        { get; set; }

        [AttrDbColumn("ResultHome")]
        public int? ResultHome
        { get; set; }

        [AttrDbColumn("ResultAway")]
        public int? ResultAway
        { get; set; }

        public string ResultInfo
        { get; set; }

        [AttrDbColumn("PlayTime")]
        public DateTime PlayTime
        { get; set; }

        [AttrDbColumn("LeagueGuid")]
        public Guid? LeagueGuid
        { get; set; }

        [AttrDbColumn("LeagueName")]
        public string LeagueName
        { get; set; }

        [AttrDbColumn("Round")]
        public int? Round
        { get; set; }

        [AttrDbColumn("GroupGuid")]
        public Guid? GroupGuid
        { get; set; }

        [AttrDbColumn("CasinoMatchGuid")]
        public Guid? CasinoMatchGuid
        { get; set; }

        [AttrDbColumn("ReportImageURL")]
        public string ReportImageURL
        { get; set; }

        [AttrDbColumn("ReportURL")]
        public string ReportURL
        { get; set; }

        [AttrDbColumn("TopicURL")]
        public string TopicURL
        { get; set; }

        [AttrDbColumn("IsActive")]
        public Boolean IsActive
        { get; set; }

        [AttrDbColumn("Remark")]
        public string Remark
        { get; set; }

        #endregion
    }
}

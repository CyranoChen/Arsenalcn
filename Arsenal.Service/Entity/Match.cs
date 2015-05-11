using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Arsenalcn.Core;

namespace Arsenal.Service
{
    [AttrDbTable("Arsenal_Match", Key = "MatchGuid", Sort = "PlayTime DESC")]
    public class Match : Entity<Guid>
    {
        public Match() : base() { }

        public Match(DataRow dr)
            : base(dr)
        {
            #region Generate Match ResultInfo

            if (ResultHome.HasValue && ResultAway.HasValue)
            {
                var _strResult = "{0} : {1}";

                if (IsHome)
                    ResultInfo = string.Format(_strResult, ResultHome.Value.ToString(), ResultAway.Value.ToString());
                else
                    ResultInfo = string.Format(_strResult, ResultAway.Value.ToString(), ResultHome.Value.ToString());
            }
            else
            { ResultInfo = string.Empty; }

            #endregion
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
                IRepository repo = new Repository();

                MatchList = repo.All<Match>().ToList();
            }

            public static Match Load(Guid guid)
            {
                return MatchList.Find(x => x.ID.Equals(guid));
            }

            public static List<Match> MatchList;
        }

        #region Members and Properties

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

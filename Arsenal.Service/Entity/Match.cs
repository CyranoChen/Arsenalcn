using System;
using System.Collections.Generic;
using Arsenalcn.Core;
using Arsenalcn.Core.Dapper;

namespace Arsenal.Service
{
    [DbSchema("Arsenal_Match", Key = "MatchGuid", Sort = "PlayTime DESC")]
    public class Match : Entity<Guid>
    {
        public override void Inital()
        {
            if (ResultHome.HasValue && ResultAway.HasValue)
            {
                ResultInfo = IsHome ? $"{ResultHome.Value} : {ResultAway.Value}" : $"{ResultAway.Value} : {ResultHome.Value}";
            }
            else
            {
                ResultInfo = null;
            }
        }

        public static class Cache
        {
            public static List<Match> MatchList;

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

                MatchList = repo.All<Match>();
            }

            public static Match Load(Guid guid)
            {
                return MatchList.Find(x => x.ID.Equals(guid));
            }
        }

        #region Members and Properties

        [DbColumn("TeamGuid")]
        public Guid TeamGuid { get; set; }

        [DbColumn("TeamName")]
        public string TeamName { get; set; }

        [DbColumn("IsHome")]
        public bool IsHome { get; set; }

        [DbColumn("ResultHome")]
        public short? ResultHome { get; set; }

        [DbColumn("ResultAway")]
        public short? ResultAway { get; set; }

        public string ResultInfo { get; set; }

        [DbColumn("PlayTime")]
        public DateTime PlayTime { get; set; }

        [DbColumn("LeagueGuid")]
        public Guid? LeagueGuid { get; set; }

        [DbColumn("LeagueName")]
        public string LeagueName { get; set; }

        [DbColumn("Round")]
        public short? Round { get; set; }

        [DbColumn("GroupGuid")]
        public Guid? GroupGuid { get; set; }

        [DbColumn("CasinoMatchGuid")]
        public Guid? CasinoMatchGuid { get; set; }

        [DbColumn("ReportImageURL")]
        public string ReportImageURL { get; set; }

        [DbColumn("ReportURL")]
        public string ReportURL { get; set; }

        [DbColumn("TopicURL")]
        public string TopicURL { get; set; }

        [DbColumn("IsActive")]
        public bool IsActive { get; set; }

        [DbColumn("Remark")]
        public string Remark { get; set; }

        #endregion
    }
}
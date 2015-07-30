using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Arsenalcn.Core;

namespace Arsenal.Service
{
    [DbSchema("Arsenal_Match", Key = "MatchGuid", Sort = "PlayTime DESC")]
    public class Match : Entity<Guid>
    {
        public Match() : base() { }

        public static void CreateMap()
        {
            var map = AutoMapper.Mapper.CreateMap<IDataReader, Match>();

            map.ForMember(d => d.ID, opt => opt.MapFrom(s => (Guid)s.GetValue("MatchGuid")));

            map.ForMember(d => d.ResultInfo, opt => opt.ResolveUsing(s =>
            {
                short? resultHome = (short?)s.GetValue("ResultHome");
                short? resultAway = (short?)s.GetValue("ResultAway");
                bool IsHome = (bool)s.GetValue("IsHome");

                if (resultHome.HasValue && resultAway.HasValue)
                {
                    var _strResult = "{0} : {1}";

                    if (IsHome)
                        return string.Format(_strResult, resultHome.Value.ToString(), resultAway.Value.ToString());
                    else
                        return string.Format(_strResult, resultAway.Value.ToString(), resultHome.Value.ToString());
                }
                else
                { return string.Empty; }
            }));
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

                MatchList = repo.All<Match>();
            }

            public static Match Load(Guid guid)
            {
                return MatchList.Find(x => x.ID.Equals(guid));
            }

            public static List<Match> MatchList;
        }

        #region Members and Properties

        [DbColumn("TeamGuid")]
        public Guid TeamGuid
        { get; set; }

        [DbColumn("TeamName")]
        public string TeamName
        { get; set; }

        [DbColumn("IsHome")]
        public Boolean IsHome
        { get; set; }

        [DbColumn("ResultHome")]
        public short? ResultHome
        { get; set; }

        [DbColumn("ResultAway")]
        public short? ResultAway
        { get; set; }

        public string ResultInfo
        { get; set; }

        [DbColumn("PlayTime")]
        public DateTime PlayTime
        { get; set; }

        [DbColumn("LeagueGuid")]
        public Guid? LeagueGuid
        { get; set; }

        [DbColumn("LeagueName")]
        public string LeagueName
        { get; set; }

        [DbColumn("Round")]
        public short? Round
        { get; set; }

        [DbColumn("GroupGuid")]
        public Guid? GroupGuid
        { get; set; }

        [DbColumn("CasinoMatchGuid")]
        public Guid? CasinoMatchGuid
        { get; set; }

        [DbColumn("ReportImageURL")]
        public string ReportImageURL
        { get; set; }

        [DbColumn("ReportURL")]
        public string ReportURL
        { get; set; }

        [DbColumn("TopicURL")]
        public string TopicURL
        { get; set; }

        [DbColumn("IsActive")]
        public Boolean IsActive
        { get; set; }

        [DbColumn("Remark")]
        public string Remark
        { get; set; }

        #endregion
    }
}

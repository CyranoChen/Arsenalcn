using System;
using System.Collections.Generic;
using Arsenalcn.Core;

namespace Arsenal.Service.Casino
{
    [DbSchema("AcnCasino_BetView", Sort = "BetTime DESC")]
    public class BetView : Dao
    {
        //public static void CreateMap()
        //{
        //    var map = Mapper.CreateMap<IDataReader, BetView>();

        //    #region BetView.CasinoItem

        //    var cMap = Mapper.CreateMap<IDataReader, CasinoItem>();
        //    cMap.ForMember(d => d.ID, opt => opt.MapFrom(s => s.GetValue("CasinoItemGuid")));
        //    cMap.ForMember(d => d.ItemType, opt => opt.MapFrom(s =>
        //        (CasinoType) Enum.Parse(typeof (CasinoType), s.GetValue("ItemType").ToString())));
        //    cMap.ForMember(d => d.Earning, opt => opt.MapFrom(s => s.GetValue("c_Earning")));

        //    map.ForMember(d => d.CasinoItem, opt => opt.MapFrom(s => Mapper.Map<CasinoItem>(s)));

        //    #endregion

        //    #region BetView.League

        //    var lMap = Mapper.CreateMap<IDataReader, League>();
        //    lMap.ForMember(d => d.ID, opt => opt.MapFrom(s => s.GetValue("LeagueGuid")));

        //    map.ForMember(d => d.League, opt => opt.MapFrom(s => Mapper.Map<League>(s)));

        //    #endregion

        //    #region BetView.Match

        //    var mMap = Mapper.CreateMap<IDataReader, Match>();
        //    mMap.ForMember(d => d.ID, opt => opt.MapFrom(s => s.GetValue("MatchGuid")));

        //    map.ForMember(d => d.Match, opt => opt.MapFrom(s => Mapper.Map<Match>(s)));

        //    #endregion

        //    #region BetView.Team

        //    var tMap = Mapper.CreateMap<IDataReader, Team>();

        //    map.ForMember(d => d.Home, opt => opt.ResolveUsing(s =>
        //    {
        //        var home = new Team
        //        {
        //            ID = (Guid) s.GetValue("h_TeamGuid"),
        //            TeamEnglishName = s.GetValue("h_TeamEnglishName").ToString(),
        //            TeamDisplayName = s.GetValue("h_TeamDisplayName").ToString(),
        //            TeamLogo = s.GetValue("h_TeamLogo").ToString()
        //        };

        //        return home;
        //    }));

        //    map.ForMember(d => d.Away, opt => opt.ResolveUsing(s =>
        //    {
        //        var away = new Team
        //        {
        //            ID = (Guid) s.GetValue("a_TeamGuid"),
        //            TeamEnglishName = s.GetValue("a_TeamEnglishName").ToString(),
        //            TeamDisplayName = s.GetValue("a_TeamDisplayName").ToString(),
        //            TeamLogo = s.GetValue("a_TeamLogo").ToString()
        //        };

        //        return away;
        //    }));

        //    #endregion
        //}

        #region Members and Properties

        [DbColumn("ID", IsKey = true)]
        public int ID { get; set; }

        [DbColumn("UserID")]
        public int UserID { get; set; }

        [DbColumn("UserName")]
        public string UserName { get; set; }

        [DbColumn("BetAmount")]
        public double? BetAmount { get; set; }

        [DbColumn("BetTime")]
        public DateTime BetTime { get; private set; }

        [DbColumn("BetRate")]
        public double? BetRate { get; set; }

        [DbColumn("IsWin")]
        public bool? IsWin { get; set; }

        [DbColumn("Earning")]
        public double? Earning { get; set; }

        [DbColumn("EarningDesc")]
        public string EarningDesc { get; set; }

        // Complex Object
        [DbColumn("c", Key = "CasinoItemGuid")]
        public CasinoItem CasinoItem { get; set; }

        [DbColumn("m", Key = "MatchGuid")]
        public Match Match { get; set; }

        [DbColumn("h", Key = "TeamGuid")]
        public Team Home { get; set; }

        [DbColumn("a", Key = "TeamGuid")]
        public Team Away { get; set; }

        [DbColumn("l", Key = "LeagueGuid")]
        public League League { get; set; }

        [DbColumn("@BetDetail", ForeignKey = "BetID")]
        public IEnumerable<BetDetail> BetDetails { get; set; }

        #endregion
    }
}
using System;
using Arsenalcn.Core;

namespace Arsenal.Service.Casino
{
    [DbSchema("AcnCasino_CouponView", Sort = "PlayTime")]
    public class CouponView : Viewer
    {
        //public static void CreateMap()
        //{
        //    var map = Mapper.CreateMap<IDataReader, CouponView>();

        //    //map.ForMember(d => d.UserID, opt => opt.MapFrom(s => s.GetValue("UserID")));
        //    map.ForMember(d => d.BetResultHome, opt => opt.MapFrom(s => Convert.ToInt16(s.GetValue("BetResultHome"))));
        //    map.ForMember(d => d.BetResultAway, opt => opt.MapFrom(s => Convert.ToInt16(s.GetValue("BetResultAway"))));

        //    #region CouponView.Bet

        //    var bMap = Mapper.CreateMap<IDataReader, Bet>();

        //    map.ForMember(d => d.Bet, opt => opt.MapFrom(s => Mapper.Map<Bet>(s)));

        //    #endregion

        //    #region CouponView.League

        //    var lMap = Mapper.CreateMap<IDataReader, League>();
        //    lMap.ForMember(d => d.ID, opt => opt.MapFrom(s => s.GetValue("LeagueGuid")));

        //    map.ForMember(d => d.League, opt => opt.MapFrom(s => Mapper.Map<League>(s)));

        //    #endregion

        //    #region CouponView.Team

        //    var tMap = Mapper.CreateMap<IDataReader, Team>();

        //    map.ForMember(d => d.Home, opt => opt.ResolveUsing(s =>
        //    {
        //        var home = new Team
        //        {
        //            ID = (Guid)s.GetValue("h_TeamGuid"),
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
        //            ID = (Guid)s.GetValue("a_TeamGuid"),
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

        [DbColumn("MatchGuid")]
        public Guid MatchGuid { get; set; }

        [DbColumn("BetResultHome")]
        public short BetResultHome { get; set; }

        [DbColumn("BetResultAway")]
        public short BetResultAway { get; set; }

        [DbColumn("PlayTime")]
        public DateTime PlayTime { get; set; }

        [DbColumn("LeagueName")]
        public string LeagueName { get; set; }

        [DbColumn("Round")]
        public short? Round { get; set; }

        // Complex Object

        //[DbColumn("b", Key = "ID")]
        //public Bet Bet { get; set; }

        [DbColumn("h", Key = "TeamGuid")]
        public HomeTeam Home { get; set; }

        [DbColumn("a", Key = "TeamGuid")]
        public AwayTeam Away { get; set; }

        [DbColumn("l", Key = "LeagueGuid")]
        public League League { get; set; }

        #endregion
    }
}

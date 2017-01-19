using System;
using Arsenalcn.Core;

namespace Arsenal.Service.Casino
{
    [DbSchema("AcnCasino_BonusView", Key = "MatchGuid", Sort = "PlayTime DESC")]
    public class BonusView : Dao
    {
        //public static void CreateMap()
        //{
        //    var map = Mapper.CreateMap<IDataReader, BonusView>();

        //    map.ForMember(d => d.UserID, opt => opt.MapFrom(s => s.GetValue("UserID")));

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

        [DbColumn("MatchGuid")]
        public Guid MatchGuid { get; set; }

        [DbColumn("UserID")]
        public int UserID { get; set; }

        [DbColumn("UserName")]
        public string UserName { get; set; }

        [DbColumn("Win")]
        public int? Win { get; set; }

        [DbColumn("Lose")]
        public int? Lose { get; set; }

        [DbColumn("Earning")]
        public double? Earning { get; set; }

        [DbColumn("TotalBet")]
        public double? TotalBet { get; set; }

        [DbColumn("RPBonus")]
        public int? RPBonus { get; set; }

        [DbColumn("PlayTime")]
        public DateTime PlayTime { get; set; }

        [DbColumn("LeagueName")]
        public string LeagueName { get; set; }

        [DbColumn("Round")]
        public short? Round { get; set; }

        // Complex Object

        [DbColumn("h", Key = "TeamGuid")]
        public Team Home { get; set; }

        [DbColumn("a", Key = "TeamGuid")]
        public Team Away { get; set; }

        [DbColumn("l", Key = "LeagueGuid")]
        public League League { get; set; }

        #endregion
    }
}

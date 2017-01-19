using System;
using System.Collections.Generic;
using Arsenalcn.Core;

namespace Arsenal.Service.Casino
{
    [DbSchema("AcnCasino_MatchView", Sort = "PlayTime DESC")]
    public class MatchView : Dao
    {
        //public static void CreateMap()
        //{
        //    var map = Mapper.CreateMap<IDataReader, MatchView>();

        //    #region MatchView.CasinoItem

        //    var cMap = Mapper.CreateMap<IDataReader, CasinoItem>();
        //    cMap.ForMember(d => d.ID, opt => opt.MapFrom(s => s.GetValue("CasinoItemGuid")));
        //    cMap.ForMember(d => d.ItemType, opt => opt.MapFrom(s =>
        //        (CasinoType) Enum.Parse(typeof (CasinoType), s.GetValue("ItemType").ToString())));

        //    map.ForMember(d => d.CasinoItem, opt => opt.MapFrom(s => Mapper.Map<CasinoItem>(s)));

        //    #endregion

        //    #region MatchView.League

        //    var lMap = Mapper.CreateMap<IDataReader, League>();
        //    lMap.ForMember(d => d.ID, opt => opt.MapFrom(s => s.GetValue("LeagueGuid")));

        //    map.ForMember(d => d.League, opt => opt.MapFrom(s => Mapper.Map<League>(s)));

        //    #endregion

        //    #region MatchView.Group

        //    var gMap = Mapper.CreateMap<IDataReader, Group>();
        //    gMap.ForMember(d => d.ID, opt =>
        //    {
        //        opt.Condition(s => s.GetValue("GroupGuid") != null);
        //        opt.MapFrom(s => s.GetValue("GroupGuid"));
        //    });

        //    map.ForMember(d => d.Group, opt =>
        //    {
        //        opt.Condition(s => s.GetValue("GroupGuid") != null);
        //        opt.MapFrom(s => Mapper.Map<Group>(s));
        //    });

        //    #endregion

        //    #region MatchView.Team

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
        public Guid ID { get; set; }

        [DbColumn("ResultHome")]
        public short? ResultHome { get; set; }

        [DbColumn("ResultAway")]
        public short? ResultAway { get; set; }

        [DbColumn("PlayTime")]
        public DateTime PlayTime { get; set; }

        [DbColumn("LeagueName")]
        public string LeagueName { get; set; }

        [DbColumn("Round")]
        public short? Round { get; set; }

        // Complex Object
        [DbColumn("c", Key = "CasinoItemGuid")]
        public CasinoItem CasinoItem { get; set; }

        [DbColumn("h", Key = "TeamGuid")]
        public Team Home { get; set; }

        [DbColumn("a", Key = "TeamGuid")]
        public Team Away { get; set; }

        [DbColumn("l", Key = "LeagueGuid")]
        public League League { get; set; }

        [DbColumn("g", Key = "GroupGuid")]
        public Group Group { get; set; }

        [DbColumn("@ChoiceOption", ForeignKey = "CasinoItemGuid")]
        public IEnumerable<ChoiceOption> ChoiceOptions { get; set; }

        #endregion
    }
}
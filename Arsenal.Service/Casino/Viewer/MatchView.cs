using System;
using System.Data;

using Arsenalcn.Core;
using System.Collections.Generic;

namespace Arsenal.Service.Casino
{
    [DbSchema("AcnCasino_MatchView", Sort = "m_PlayTime DESC")]
    public class MatchView : Viewer
    {
        public MatchView() : base() { }

        public static void CreateMap()
        {
            var map = AutoMapper.Mapper.CreateMap<IDataReader, MatchView>();

            map.ForMember(d => d.ResultHome, opt => opt.MapFrom(s => s.GetValue("m_ResultHome")));
            map.ForMember(d => d.ResultAway, opt => opt.MapFrom(s => s.GetValue("m_ResultAway")));
            map.ForMember(d => d.PlayTime, opt => opt.MapFrom(s => s.GetValue("m_PlayTime")));
            map.ForMember(d => d.CasinoItem, opt => opt.MapFrom(s => AutoMapper.Mapper.DynamicMap<CasinoItem>(s)));
        }

        #region Members and Properties

        [DbColumn("ID", IsKey = true)]
        public Guid ID
        { get; set; }

        [DbColumn("m_ResultHome")]
        public short? ResultHome
        { get; set; }

        [DbColumn("m_ResultAway")]
        public short? ResultAway
        { get; set; }

        [DbColumn("m_PlayTime")]
        public DateTime PlayTime
        { get; set; }

        [DbColumn("m_LeagueName")]
        public string LeagueName
        { get; set; }

        [DbColumn("m_Round")]
        public int? Round
        { get; set; }

        // Complex Object
        [DbColumn("c", Key = "c_CasinoItemGuid")]
        public CasinoItem CasinoItem
        { get; set; }

        [DbColumn("h", Key = "h_TeamGuid")]
        public Team Home
        { get; set; }

        [DbColumn("a", Key = "a_TeamGuid")]
        public Team Away
        { get; set; }

        [DbColumn("l", Key = "l_LeagueGuid")]
        public League League
        { get; set; }

        [DbColumn("g", Key = "g_GroupGuid")]
        public Group Group
        { get; set; }

        [DbColumn("@ChoiceOption", ForeignKey = "CasinoItemGuid")]
        public IEnumerable<ChoiceOption> ListChoiceOption
        { get; set; }

        #endregion
    }
}

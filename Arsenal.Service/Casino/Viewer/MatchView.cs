using System;
using System.Collections.Generic;
using Arsenalcn.Core;

namespace Arsenal.Service.Casino
{
    [DbSchema("AcnCasino_MatchView", Sort = "PlayTime DESC")]
    public class MatchView : Viewer
    {
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

        [DbColumn("h", Key = "HomeTeamGuid")]
        public HomeTeam Home { get; set; }

        [DbColumn("a", Key = "AwayTeamGuid")]
        public AwayTeam Away { get; set; }

        [DbColumn("g", Key = "GroupGuid")]
        public Group Group { get; set; }

        [DbColumn("l", Key = "LeagueGuid")]
        public League League { get; set; }

        [DbColumn("@ChoiceOption", ForeignKey = "CasinoItemGuid")]
        public IEnumerable<ChoiceOption> ChoiceOptions { get; set; }

        #endregion
    }
}
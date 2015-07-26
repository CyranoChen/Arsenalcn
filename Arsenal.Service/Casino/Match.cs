﻿using System;
using System.Data;

using Arsenalcn.Core;

namespace Arsenal.Service.Casino
{
    [DbSchema("AcnCasino_Match", Key = "MatchGuid", Sort = "PlayTime DESC, LeagueName")]
    public class Match : Entity<Guid>
    {
        public Match() : base() { }

        public Match(DataRow dr) : base(dr) { }

        #region Members and Properties

        [DbColumn("Home")]
        public Guid Home
        { get; set; }

        [DbColumn("Away")]
        public Guid Away
        { get; set; }

        [DbColumn("ResultHome")]
        public short? ResultHome
        { get; set; }

        [DbColumn("ResultAway")]
        public short? ResultAway
        { get; set; }

        [DbColumn("PlayTime")]
        public DateTime PlayTime
        { get; set; }

        [DbColumn("LeagueGuid")]
        public Guid LeagueGuid
        { get; set; }

        [DbColumn("LeagueName")]
        public string LeagueName
        { get; set; }

        [DbColumn("Round")]
        public int? Round
        { get; set; }

        [DbColumn("GroupGuid")]
        public Guid? GroupGuid
        { get; set; }

        #endregion
    }
}
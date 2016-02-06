using System;
using System.Data;
using Arsenalcn.Core;
using DataReaderMapper;

namespace Arsenal.Service.Casino
{
    [DbSchema("AcnCasino_Match", Key = "MatchGuid", Sort = "PlayTime DESC, LeagueName")]
    public class Match : Entity<Guid>
    {
        public static void CreateMap()
        {
            var map = Mapper.CreateMap<IDataReader, Match>();

            map.ForMember(d => d.ID, opt => opt.MapFrom(s => (Guid) s.GetValue("MatchGuid")));
        }

        #region Members and Properties

        [DbColumn("Home")]
        public Guid Home { get; set; }

        [DbColumn("Away")]
        public Guid Away { get; set; }

        [DbColumn("ResultHome")]
        public short? ResultHome { get; set; }

        [DbColumn("ResultAway")]
        public short? ResultAway { get; set; }

        [DbColumn("PlayTime")]
        public DateTime PlayTime { get; set; }

        [DbColumn("LeagueGuid")]
        public Guid LeagueGuid { get; set; }

        [DbColumn("LeagueName")]
        public string LeagueName { get; set; }

        [DbColumn("Round")]
        public short? Round { get; set; }

        [DbColumn("GroupGuid")]
        public Guid? GroupGuid { get; set; }

        #endregion
    }
}
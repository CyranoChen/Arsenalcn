using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Arsenalcn.Core;
using DataReaderMapper;

namespace Arsenal.Service
{
    [DbSchema("Arsenal_League", Key = "LeagueGuid", Sort = "LeagueOrder, LeagueOrgName")]
    public class League : Entity<Guid>
    {
        public static void CreateMap()
        {
            var map = Mapper.CreateMap<IDataReader, League>();

            map.ForMember(d => d.ID, opt => opt.MapFrom(s => (Guid) s.GetValue("LeagueGuid")));

            map.ForMember(d => d.LeagueNameInfo, opt => opt.MapFrom(s =>
                string.Format("{0}{1}", s.GetValue("LeagueName").ToString(), s.GetValue("LeagueSeason").ToString())));

            map.ForMember(d => d.TeamCountInfo, opt => opt.MapFrom(s =>
                RelationLeagueTeam.Cache.RelationLeagueTeamList.Count(x =>
                    x.LeagueGuid.Equals((Guid) s.GetValue("LeagueGuid")))));
        }

        public static class Cache
        {
            public static List<League> LeagueList;
            public static List<League> LeagueList_Active;

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

                LeagueList = repo.All<League>();
                LeagueList_Active = LeagueList.FindAll(x => x.IsActive);
            }

            public static League Load(Guid guid)
            {
                return LeagueList.Find(x => x.ID.Equals(guid));
            }
        }

        #region Members and Properties

        [DbColumn("LeagueName")]
        public string LeagueName { get; set; }

        [DbColumn("LeagueOrgName")]
        public string LeagueOrgName { get; set; }

        [DbColumn("LeagueSeason")]
        public string LeagueSeason { get; set; }

        [DbColumn("LeagueTime")]
        public DateTime LeagueTime { get; set; }

        [DbColumn("LeagueLogo")]
        public string LeagueLogo { get; set; }

        [DbColumn("LeagueOrder")]
        public int LeagueOrder { get; set; }

        [DbColumn("IsActive")]
        public bool IsActive { get; set; }

        public int TeamCountInfo { get; set; }

        public string LeagueNameInfo { get; set; }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Arsenalcn.Core;

namespace Arsenal.Service
{
    [AttrDbTable("Arsenal_League", Key = "LeagueGuid", Sort = "LeagueOrder, LeagueOrgName")]
    public class League : Entity<Guid>
    {
        public League() : base() { }

        public League(DataRow dr)
            : base(dr)
        {
            // Generate League Count Info
            TeamCountInfo = RelationLeagueTeam.Cache.RelationLeagueTeamList.Count(x => x.LeagueGuid.Equals(this.ID));

            // Generate League Name Info
            LeagueNameInfo = LeagueName + LeagueSeason;
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

                LeagueList = repo.All<League>();
                LeagueList_Active = LeagueList.FindAll(x => x.IsActive);
            }

            public static League Load(Guid guid)
            {
                return LeagueList.Find(x => x.ID.Equals(guid));
            }

            public static List<League> LeagueList;
            public static List<League> LeagueList_Active;
        }

        #region Members and Properties

        [AttrDbColumn("LeagueName")]
        public string LeagueName
        { get; set; }

        [AttrDbColumn("LeagueOrgName")]
        public string LeagueOrgName
        { get; set; }

        [AttrDbColumn("LeagueSeason")]
        public string LeagueSeason
        { get; set; }

        [AttrDbColumn("LeagueTime")]
        public DateTime LeagueTime
        { get; set; }

        [AttrDbColumn("LeagueLogo")]
        public string LeagueLogo
        { get; set; }

        [AttrDbColumn("LeagueOrder")]
        public int LeagueOrder
        { get; set; }

        [AttrDbColumn("IsActive")]
        public bool IsActive
        { get; set; }

        public int TeamCountInfo
        { get; set; }

        public string LeagueNameInfo
        { get; set; }

        #endregion
    }
}

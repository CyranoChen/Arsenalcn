using System;
using System.Collections.Generic;
using System.Linq;
using Arsenalcn.Core;

namespace Arsenal.Service
{
    [DbSchema("Arsenal_League", Key = "LeagueGuid", Sort = "LeagueOrder, LeagueOrgName")]
    public class League : Entity<Guid>
    {
        public override void Inital()
        {
            LeagueNameInfo = $"{LeagueName}{LeagueSeason}";
            TeamCountInfo = RelationLeagueTeam.Cache.RelationLeagueTeamList.Count(x => x.LeagueGuid.Equals(this.ID));
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

        public override Guid ID
        {
            get
            {
                if (LeagueGuid.Equals(Guid.Empty))
                {
                    LeagueGuid = Guid.NewGuid();
                }

                return LeagueGuid;
            }
            set { LeagueGuid = value; }
        }

        private Guid LeagueGuid { get; set; }

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
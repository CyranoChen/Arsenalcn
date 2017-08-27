using System;
using System.Collections.Generic;
using System.Linq;
using Arsenalcn.Core;
using Arsenalcn.Core.Dapper;

namespace Arsenal.Service
{
    [DbSchema("Arsenal_Team", Key = "TeamGuid", Sort = "TeamEnglishName")]
    public class Team : Entity<Guid>
    {
        public override void Inital()
        {
            TeamNameInfo = $"{TeamDisplayName} ({TeamEnglishName})";
            LeagueCountInfo = RelationLeagueTeam.Cache.RelationLeagueTeamList.Count(x => x.TeamGuid.Equals(this.ID));
        }

        public static class Cache
        {
            public static List<Team> TeamList;

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

                TeamList = repo.All<Team>();
            }

            public static Team Load(Guid guid)
            {
                return TeamList.Find(x => x.ID.Equals(guid));
            }

            public static List<Team> GetTeamsByLeagueGuid(Guid guid)
            {
                return TeamList.FindAll(x =>
                    new RelationLeagueTeam { TeamGuid = x.ID, LeagueGuid = guid }.Any());
            }
        }

        #region Members and Properties

        [DbColumn("TeamEnglishName")]
        public string TeamEnglishName { get; set; }

        [DbColumn("TeamDisplayName")]
        public string TeamDisplayName { get; set; }

        [DbColumn("TeamLogo")]
        public string TeamLogo { get; set; }

        [DbColumn("TeamNickName")]
        public string TeamNickName { get; set; }

        [DbColumn("Founded")]
        public string Founded { get; set; }

        [DbColumn("Ground")]
        public string Ground { get; set; }

        [DbColumn("Capacity")]
        public int? Capacity { get; set; }

        [DbColumn("Chairman")]
        public string Chairman { get; set; }

        [DbColumn("Manager")]
        public string Manager { get; set; }

        [DbColumn("LeagueGuid")]
        public Guid LeagueGuid { get; set; }

        public int LeagueCountInfo { get; set; }

        public string TeamNameInfo { get; set; }

        #endregion
    }

    public class HomeTeam : Team
    {
        public override Guid ID => HomeTeamGuid;

        private Guid HomeTeamGuid { get; set; }

        public string HomeEnglishName { get; set; }

        public string HomeDisplayName { get; set; }

        public string HomeLogo { get; set; }
    }

    public class AwayTeam : Team
    {
        public override Guid ID => AwayTeamGuid;

        private Guid AwayTeamGuid { get; set; }

        public string AwayEnglishName { get; set; }

        public string AwayDisplayName { get; set; }

        public string AwayLogo { get; set; }
    }
}
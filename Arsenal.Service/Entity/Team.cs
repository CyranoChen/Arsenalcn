using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Arsenalcn.Core;

namespace Arsenal.Service
{
    [DbSchema("Arsenal_Team", Key = "TeamGuid", Sort = "TeamEnglishName")]
    public class Team : Entity<Guid>
    {
        public Team() : base() { }

        public static void CreateMap()
        {
            var map = AutoMapper.Mapper.CreateMap<IDataReader, Team>();

            map.ForMember(d => d.ID, opt => opt.MapFrom(s => (Guid)s.GetValue("TeamGuid")));

            map.ForMember(d => d.LeagueCountInfo, opt => opt.MapFrom(s =>
                RelationLeagueTeam.Cache.RelationLeagueTeamList.Count(x =>
                x.TeamGuid.Equals((Guid)s.GetValue("TeamGuid")))));
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

                TeamList = repo.All<Team>();
            }

            public static Team Load(Guid guid)
            {
                return TeamList.Find(x => x.ID.Equals(guid));
            }

            public static List<Team> GetTeamsByLeagueGuid(Guid guid)
            {
                return TeamList.FindAll(x =>
                    new RelationLeagueTeam() { TeamGuid = x.ID, LeagueGuid = guid }.Any());
            }

            public static List<Team> TeamList;
        }

        #region Members and Properties

        [DbColumn("TeamEnglishName")]
        public string TeamEnglishName
        { get; set; }

        [DbColumn("TeamDisplayName")]
        public string TeamDisplayName
        { get; set; }

        [DbColumn("TeamLogo")]
        public string TeamLogo
        { get; set; }

        [DbColumn("TeamNickName")]
        public string TeamNickName
        { get; set; }

        [DbColumn("Founded")]
        public string Founded
        { get; set; }

        [DbColumn("Ground")]
        public string Ground
        { get; set; }

        [DbColumn("Capacity")]
        public int? Capacity
        { get; set; }

        [DbColumn("Chairman")]
        public string Chairman
        { get; set; }

        [DbColumn("Manager")]
        public string Manager
        { get; set; }

        [DbColumn("LeagueGuid")]
        public Guid LeagueGuid
        { get; set; }

        public int LeagueCountInfo
        { get; set; }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Arsenalcn.Core;

namespace Arsenal.Service
{
    [AttrDbTable("Arsenal_Team", Key = "TeamGuid")]
    public class Team : Entity
    {
        public Team() : base() { }

        public Team(DataRow dr)
            : base(dr)
        {
            // Generate League Count Info
            LeagueCountInfo = this.All<RelationLeagueTeam>().Count(rlt => rlt.TeamGuid.Equals(TeamGuid));
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
                TeamList = new Team().All<Team>().ToList();
            }

            public static Team Load(Guid guid)
            {
                return TeamList.Find(t => t.TeamGuid.Equals(guid));
            }

            // TODO
            //public static List<Team> GetTeamsByLeagueGuid(Guid guid)
            //{
            //    return TeamList.FindAll(t => RelationLeagueTeam.Exist(t.TeamGuid, guid));
            //}

            public static List<Team> TeamList;
        }

        #region Members and Properties

        [AttrDbColumn("TeamGuid", IsKey = true)]
        public Guid TeamGuid
        { get; set; }

        [AttrDbColumn("TeamEnglishName")]
        public string TeamEnglishName
        { get; set; }

        [AttrDbColumn("TeamDisplayName")]
        public string TeamDisplayName
        { get; set; }

        [AttrDbColumn("TeamLogo")]
        public string TeamLogo
        { get; set; }

        [AttrDbColumn("TeamNickName")]
        public string TeamNickName
        { get; set; }

        [AttrDbColumn("Founded")]
        public string Founded
        { get; set; }

        [AttrDbColumn("Ground")]
        public string Ground
        { get; set; }

        [AttrDbColumn("Capacity")]
        public int? Capacity
        { get; set; }

        [AttrDbColumn("Chairman")]
        public string Chairman
        { get; set; }

        [AttrDbColumn("Manager")]
        public string Manager
        { get; set; }

        [AttrDbColumn("LeagueGuid")]
        public Guid LeagueGuid
        { get; set; }

        public int LeagueCountInfo
        { get; set; }

        #endregion
    }
}

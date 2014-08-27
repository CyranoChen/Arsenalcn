using System;
using System.Data;

namespace Arsenalcn.CasinoSys.Entity
{
    public class Team
    {
        public Team() { }

        public Team(Guid teamGuid)
        {
            DataRow dr = DataAccess.Team.GetTeamByID(teamGuid);

            if (dr != null)
                InitTeam(dr);
        }

        private void InitTeam(DataRow dr)
        {
            if (dr != null)
            {
                TeamGuid = (Guid)dr["TeamGuid"];
                TeamEnglishName = Convert.ToString(dr["TeamEnglishName"]);
                TeamDisplayName = Convert.ToString(dr["TeamDisplayName"]);
                TeamLogo = Convert.IsDBNull(dr["TeamLogo"]) ? null : Convert.ToString(dr["TeamLogo"]);
                TeamNickName = Convert.IsDBNull(dr["TeamNickName"]) ? null : Convert.ToString(dr["TeamNickName"]);
                Founded = Convert.IsDBNull(dr["Founded"]) ? null : Convert.ToString(dr["Founded"]);
                Ground = Convert.IsDBNull(dr["Ground"]) ? null : Convert.ToString(dr["Ground"]);

                if (Convert.IsDBNull(dr["Capacity"]))
                    Capacity = null;
                else
                    Capacity = Convert.ToInt32(dr["Capacity"]);

                Chairman = Convert.IsDBNull(dr["Chairman"]) ? null : Convert.ToString(dr["Chairman"]);
                Manager = Convert.IsDBNull(dr["Manager"]) ? null : Convert.ToString(dr["Manager"]);
                LeagueGuid = (Guid)dr["LeagueGuid"];
            }
            else
                throw new Exception("Unable to init Team.");
        }

        public void Update()
        {
            DataAccess.Team.UpdateTeam(TeamGuid, TeamEnglishName, TeamDisplayName, TeamLogo, TeamNickName, Founded, Ground, Capacity, Chairman, Manager, LeagueGuid);
        }

        public void Insert()
        {
            DataAccess.Team.InsertTeam(TeamGuid, TeamEnglishName, TeamDisplayName, TeamLogo, TeamNickName, Founded, Ground, Capacity, Chairman, Manager, LeagueGuid);
            DataAccess.Team.InsertRelationLeagueTeam(LeagueGuid, TeamGuid);
        }

        public static DataTable GetTeamByLeague(Guid leagueGuid)
        {
            return DataAccess.Team.GetLeagueTeams(leagueGuid);
        }

        // Relation League Team
        public static void InsertRelationLeagueTeam(Guid leagueGuid, Guid teamGuid)
        {
            DataAccess.Team.InsertRelationLeagueTeam(leagueGuid, teamGuid);
        }

        public static void DeleteRelationLeagueTeam(Guid leagueGuid, Guid teamGuid)
        {
            DataAccess.Team.DeleteRelationLeagueTeam(leagueGuid, teamGuid);
        }

        public static bool IsExistRelationLeagueTeam(Guid leagueGuid, Guid teamGuid)
        {
            return Convert.ToBoolean(DataAccess.Team.GetRelationLeagueTeamCount(leagueGuid, teamGuid) > 0);
        }

        public static int GetRelationLeagueCountByTeamGuid(Guid teamGuid)
        {
            return DataAccess.Team.GetRelationLeagueCountByTeamGuid(teamGuid);
        }

        public Guid TeamGuid
        { get; set; }

        public string TeamEnglishName
        { get; set; }

        public string TeamDisplayName
        { get; set; }

        public string TeamLogo
        { get; set; }

        public string TeamNickName
        { get; set; }

        public string Founded
        { get; set; }

        public string Ground
        { get; set; }

        public int? Capacity
        { get; set; }

        public string Chairman
        { get; set; }

        public string Manager
        { get; set; }

        public Guid LeagueGuid
        { get; set; }
    }
}

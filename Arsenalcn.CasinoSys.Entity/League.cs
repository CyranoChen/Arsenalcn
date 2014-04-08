using System;
using System.Data;

namespace Arsenalcn.CasinoSys.Entity
{
    public class League
    {
        public League() { }

        public League(Guid leagueGuid)
        {
            DataRow dr = DataAccess.League.GetLeagueByID(leagueGuid);

            if (dr != null)
                InitLeague(dr);
        }

        private void InitLeague(DataRow dr)
        {
            if (dr != null)
            {
                LeagueGuid = (Guid)dr["LeagueGuid"];
                LeagueName = dr["LeagueName"].ToString();
                LeagueOrgName = dr["LeagueOrgName"].ToString();
                LeagueSeason = dr["LeagueSeason"].ToString();
                LeagueTime = (DateTime)dr["LeagueTime"];
                LeagueLogo = dr["LeagueLogo"].ToString();
                LeagueOrder = Convert.ToInt16(dr["LeagueOrder"]);
                IsActive = Convert.ToBoolean(dr["IsActive"]);
            }
            else
                throw new Exception("Unable to init League.");
        }

        public void Insert()
        {
            DataAccess.League.InsertLeague(LeagueName, LeagueOrgName, LeagueSeason, LeagueTime, LeagueLogo, LeagueOrder, IsActive);
        }

        public void Update()
        {
            DataAccess.League.UpdateLeague(LeagueGuid, LeagueName, LeagueOrgName, LeagueSeason, LeagueTime, LeagueLogo, LeagueOrder, IsActive);
        }

        public static DataTable GetLeague()
        {
            return DataAccess.League.GetAllLeagues();
        }

        public static DataTable GetLeague(bool isActive)
        {
            return DataAccess.League.GetAllLeagues(isActive);
        }

        public static DataTable GetLeagueAllSeason(Guid leagueGuid)
        {
            return DataAccess.League.GetLeagueAllSeason(leagueGuid);
        }

        public Guid LeagueGuid
        { get; set; }

        public string LeagueName
        { get; set; }

        public string LeagueOrgName
        { get; set; }

        public string LeagueSeason
        { get; set; }

        public DateTime LeagueTime
        { get; set; }

        public string LeagueLogo
        { get; set; }

        public int LeagueOrder
        { get; set; }

        public bool IsActive
        { get; set; }
    }
}

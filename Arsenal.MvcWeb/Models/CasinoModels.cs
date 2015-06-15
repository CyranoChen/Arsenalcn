using System;
using System.Data;

using Arsenalcn.CasinoSys.Entity;

namespace Arsenal.MvcWeb.Models
{
    public class CasinoMatch : Match
    {
        public CasinoMatch(DataRow dr)
            : base(dr)
        {
            HomeTeamName = Arsenal_Team.Cache.Load(Home).TeamDisplayName;
            AwayTeamName = Arsenal_Team.Cache.Load(Away).TeamDisplayName;
        }

        public string HomeTeamName { get; set; }

        public string AwayTeamName { get; set; }
    }
}
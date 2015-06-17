using System;
using System.Data;

using Arsenalcn.CasinoSys.Entity;

namespace Arsenal.MvcWeb.Models
{
    public class CasinoMatch : Match
    {
        public CasinoMatch(Guid matchGuid)
            : base(matchGuid)
        {
            Init();
        }

        public CasinoMatch(DataRow dr)
            : base(dr)
        {
            Init();
        }

        private void Init()
        {
            HomeTeamName = Arsenal_Team.Cache.Load(Home).TeamDisplayName;
            AwayTeamName = Arsenal_Team.Cache.Load(Away).TeamDisplayName;
        }

        public string HomeTeamName { get; set; }

        public string AwayTeamName { get; set; }
    }

    public class CasinoBet : Bet
    {
        public CasinoBet(DataRow dr)
            : base(dr)
        {

        }
    }
}
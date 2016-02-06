using System;
using System.Data;

namespace Arsenalcn.CasinoSys.Entity
{
    public class RelationLeagueTeam
    {
        public RelationLeagueTeam()
        {
        }

        private RelationLeagueTeam(DataRow dr)
        {
            Init(dr);
        }

        private void Init(DataRow dr)
        {
            if (dr != null)
            {
                TeamGuid = (Guid) dr["TeamGuid"];
                LeagueGuid = (Guid) dr["LeagueGuid"];
            }
        }

        public bool Any()
        {
            return DataAccess.Team.ExistRelationLeagueTeam(TeamGuid, LeagueGuid);
        }

        #region Members and Properties

        public Guid TeamGuid { get; set; }

        public Guid LeagueGuid { get; set; }

        #endregion
    }
}
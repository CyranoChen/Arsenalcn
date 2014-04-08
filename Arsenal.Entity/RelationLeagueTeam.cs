using System;
using System.Collections.Generic;
using System.Data;

namespace Arsenal.Entity
{
    public class RelationLeagueTeam
    {
        public RelationLeagueTeam() { }

        private RelationLeagueTeam(DataRow dr)
        {
            InitRelationLeagueTeam(dr);
        }

        private void InitRelationLeagueTeam(DataRow dr)
        {
            if (dr != null)
            {
                TeamGuid = (Guid)dr["TeamGuid"];
                LeagueGuid = (Guid)dr["LeagueGuid"];
            }
            else
                throw new Exception("Unable to init RelationLeagueTeam.");
        }

        public void Select()
        {
            DataRow dr = DataAccess.RelationLeagueTeam.GetRelationLeagueTeamByID(TeamGuid, LeagueGuid);

            if (dr != null)
                InitRelationLeagueTeam(dr);
        }

        public void Insert()
        {
            DataAccess.RelationLeagueTeam.InsertRelationLeagueTeam(TeamGuid, LeagueGuid);
        }

        public void Delete()
        {
            DataAccess.RelationLeagueTeam.DeleteRelationLeagueTeam(TeamGuid, LeagueGuid);
        }

        public static List<RelationLeagueTeam> GetRelationLeagueTeams()
        {
            DataTable dt = DataAccess.RelationLeagueTeam.GetRelationLeagueTeams();
            List<RelationLeagueTeam> list = new List<RelationLeagueTeam>();

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new RelationLeagueTeam(dr));
                }
            }

            return list;
        }

        public static Boolean Exist(Guid tGuid, Guid lGuid)
        {
            DataRow dr = DataAccess.RelationLeagueTeam.GetRelationLeagueTeamByID(tGuid, lGuid);

            if (dr != null)
                return true;
            else
                return false;
        }

        public static void CleanRelationLeagueTeam()
        {
            DataAccess.RelationLeagueTeam.CleanRelationLeagueTeam();
        }

        #region Members and Properties

        public Guid TeamGuid
        { get; set; }

        public Guid LeagueGuid
        { get; set; }

        #endregion
    }
}

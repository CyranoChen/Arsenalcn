using System;
using System.Collections.Generic;
using System.Data;

using Arsenalcn.Core;

namespace Arsenal.Service
{
    [AttrDbTable("Arsenal_RelationLeagueTeam", Key = "TeamGuid, LeagueGuid")]
    public class RelationLeagueTeam : Entity
    {
        public RelationLeagueTeam() { }

        public RelationLeagueTeam(DataRow dr)
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

        //public void Select()
        //{
        //    DataRow dr = DataAccess.RelationLeagueTeam.GetRelationLeagueTeamByID(TeamGuid, LeagueGuid);

        //    if (dr != null)
        //        InitRelationLeagueTeam(dr);
        //}

        //public void Insert()
        //{
        //    DataAccess.RelationLeagueTeam.InsertRelationLeagueTeam(TeamGuid, LeagueGuid);
        //}

        //public void Delete()
        //{
        //    DataAccess.RelationLeagueTeam.DeleteRelationLeagueTeam(TeamGuid, LeagueGuid);
        //}

        //public static List<RelationLeagueTeam> GetRelationLeagueTeams()
        //{
        //    DataTable dt = DataAccess.RelationLeagueTeam.GetRelationLeagueTeams();
        //    List<RelationLeagueTeam> list = new List<RelationLeagueTeam>();

        //    if (dt != null)
        //    {
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            list.Add(new RelationLeagueTeam(dr));
        //        }
        //    }

        //    return list;
        //}

        public static Boolean Exist(Guid tGuid, Guid lGuid)
        {
            //DataRow dr = DataAccess.RelationLeagueTeam.GetRelationLeagueTeamByID(tGuid, lGuid);

            return new RelationLeagueTeam().All<RelationLeagueTeam>().Exists(rlt =>
                rlt.TeamGuid.Equals(tGuid) && rlt.LeagueGuid.Equals(lGuid));
        }

        //public static void CleanRelationLeagueTeam()
        //{
        //    DataAccess.RelationLeagueTeam.CleanRelationLeagueTeam();
        //}

        #region Members and Properties

        [AttrDbColumn("TeamGuid", IsKey = true)]
        public Guid TeamGuid
        { get; set; }

        [AttrDbColumn("LeagueGuid", IsKey = true)]
        public Guid LeagueGuid
        { get; set; }

        #endregion
    }
}

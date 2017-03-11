using System;
using System.Collections.Generic;
using System.Data;
using Arsenalcn.Core;

namespace Arsenal.Service
{
    [DbSchema("Arsenal_RelationLeagueTeam", Sort = "")]
    public class RelationLeagueTeam : Dao
    {
        public bool Any()
        {
            IRepository repo = new Repository();

            return repo.Any<RelationLeagueTeam>(x => x.TeamGuid == TeamGuid && x.LeagueGuid == LeagueGuid);
        }

        public void Delete(IDbTransaction trans = null)
        {
            IRepository repo = new Repository();

            repo.Delete<RelationLeagueTeam>(x => x.LeagueGuid == LeagueGuid && x.TeamGuid == TeamGuid, trans);
        }

        public static void Clean(IDbTransaction trans = null)
        {
            var sql =
                $@"DELETE FROM {Repository.GetTableAttr<RelationLeagueTeam>().Name} WHERE 
                     (TeamGuid NOT IN (SELECT TeamGuid FROM {Repository.GetTableAttr<Team>().Name})) 
                     OR (LeagueGuid NOT IN (SELECT LeagueGuid FROM {Repository.GetTableAttr<League>().Name}))";

            IDapperHelper dapper = new DapperHelper();

            dapper.Execute(sql, trans);
        }

        public static class Cache
        {
            public static List<RelationLeagueTeam> RelationLeagueTeamList;

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

                RelationLeagueTeamList = repo.All<RelationLeagueTeam>();
            }
        }

        #region Members and Properties

        [DbColumn("TeamGuid", IsKey = true)]
        public Guid TeamGuid { get; set; }

        [DbColumn("LeagueGuid", IsKey = true)]
        public Guid LeagueGuid { get; set; }

        #endregion
    }
}
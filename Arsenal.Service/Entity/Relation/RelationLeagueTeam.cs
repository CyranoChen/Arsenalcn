﻿using System;
using System.Collections.Generic;
using Arsenalcn.Core;
using Arsenalcn.Core.Dapper;

namespace Arsenal.Service
{
    [DbSchema("Arsenal_RelationLeagueTeam", Sort = "")]
    public class RelationLeagueTeam : Dao
    {
        public bool Any()
        {
            return Cache.RelationLeagueTeamList.Exists(x =>
                x.TeamGuid == TeamGuid && x.LeagueGuid == LeagueGuid);
        }

        public void Delete()
        {
            using (IRepository repo = new Repository())
            {
                repo.Delete<RelationLeagueTeam>(x => x.LeagueGuid == LeagueGuid && x.TeamGuid == TeamGuid);
            }
        }

        public static void Clean()
        {
            var sql =
                $@"DELETE FROM {Repository.GetTableAttr<RelationLeagueTeam>().Name} WHERE 
                     (TeamGuid NOT IN (SELECT TeamGuid FROM {Repository.GetTableAttr<Team>().Name})) 
                     OR (LeagueGuid NOT IN (SELECT LeagueGuid FROM {Repository.GetTableAttr<League>().Name}))";

            IDapperHelper dapper = DapperHelper.GetInstance();

            dapper.Execute(sql);
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
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;

using Arsenalcn.Core;

namespace Arsenal.Service
{
    [DbSchema("Arsenal_RelationLeagueTeam", Key = "", Sort = "")]
    public class RelationLeagueTeam
    {
        public RelationLeagueTeam() { }

        public RelationLeagueTeam(DataRow dr)
        {
            Contract.Requires(dr != null);

            Init(dr);
        }

        private void Init(DataRow dr)
        {
            TeamGuid = (Guid)dr["TeamGuid"];
            LeagueGuid = (Guid)dr["LeagueGuid"];
        }

        public void Single()
        {
            string sql = string.Format("SELECT * FROM {0} WHERE TeamGuid = @teamGuid AND LeagueGuid = @leagueGuid",
                Repository.GetTableAttr<RelationLeagueTeam>().Name);

            SqlParameter[] para = {
                                      new SqlParameter("@teamGuid", TeamGuid), 
                                      new SqlParameter("@leagueGuid", LeagueGuid)
                                  };

            DataSet ds = DataAccess.ExecuteDataset(sql, para);

            if (ds.Tables[0].Rows.Count > 0) { Init(ds.Tables[0].Rows[0]); }
        }

        public bool Any()
        {
            string sql = string.Format("SELECT * FROM {0} WHERE TeamGuid = @teamGuid AND LeagueGuid = @leagueGuid",
                Repository.GetTableAttr<RelationLeagueTeam>().Name);

            SqlParameter[] para = {
                                      new SqlParameter("@teamGuid", TeamGuid), 
                                      new SqlParameter("@leagueGuid", LeagueGuid)
                                  };

            DataSet ds = DataAccess.ExecuteDataset(sql, para);

            return ds.Tables[0].Rows.Count > 0;
        }

        public static List<RelationLeagueTeam> All()
        {
            var list = new List<RelationLeagueTeam>();

            string sql = string.Format("SELECT * FROM {0}", Repository.GetTableAttr<RelationLeagueTeam>().Name);

            DataSet ds = DataAccess.ExecuteDataset(sql);

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(new RelationLeagueTeam(dr));
                }
            }

            return list;
        }

        public static List<RelationLeagueTeam> QueryByLeagueGuid(Guid lGuid)
        {
            var list = new List<RelationLeagueTeam>();

            string sql = string.Format("SELECT * FROM {0} WHERE LeagueGuid = @leagueGuid",
                Repository.GetTableAttr<RelationLeagueTeam>().Name);

            SqlParameter[] para = { new SqlParameter("@leagueGuid", lGuid) };

            DataSet ds = DataAccess.ExecuteDataset(sql, para);

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(new RelationLeagueTeam(dr));
                }
            }

            return list;
        }

        public static List<RelationLeagueTeam> QueryByTeamGuid(Guid tGuid)
        {
            var list = new List<RelationLeagueTeam>();

            string sql = string.Format("SELECT * FROM {0} WHERE TeamGuid = @teamGuid",
                Repository.GetTableAttr<RelationLeagueTeam>().Name);

            SqlParameter[] para = { new SqlParameter("@teamGuid", tGuid) };

            DataSet ds = DataAccess.ExecuteDataset(sql, para);

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(new RelationLeagueTeam(dr));
                }
            }

            return list;
        }

        public void Create(SqlTransaction trans = null)
        {
            string sql = string.Format("INSERT INTO {0} (TeamGuid, LeagueGuid) VALUES (@teamGuid, @leagueGuid)",
                Repository.GetTableAttr<RelationLeagueTeam>().Name);

            SqlParameter[] para = { new SqlParameter("@teamGuid", TeamGuid), new SqlParameter("@leagueGuid", LeagueGuid) };

            DataAccess.ExecuteNonQuery(sql, para, trans);
        }

        public void Create(IEnumerable<RelationLeagueTeam> instances, SqlTransaction trans = null)
        {
            Contract.Requires(instances != null);

            foreach (var instance in instances)
            {
                instance.Create(trans);
            }
        }

        public void Delete(SqlTransaction trans = null)
        {
            string sql = string.Format("DELETE FROM {0} WHERE TeamGuid = @teamGuid AND LeagueGuid = @leagueGuid",
                Repository.GetTableAttr<RelationLeagueTeam>().Name);

            SqlParameter[] para = { new SqlParameter("@teamGuid", TeamGuid), new SqlParameter("@leagueGuid", LeagueGuid) };

            DataAccess.ExecuteNonQuery(sql, para, trans);
        }

        public static void Delete(IEnumerable<RelationLeagueTeam> instances, SqlTransaction trans = null)
        {
            Contract.Requires(instances != null);

            foreach (var instance in instances)
            {
                instance.Delete(trans);
            }
        }

        public static void Clean(SqlTransaction trans = null)
        {
            string sql = string.Format(@"DELETE FROM {0} WHERE (TeamGuid NOT IN (SELECT TeamGuid FROM {1})) OR
                               (LeagueGuid NOT IN (SELECT LeagueGuid FROM {2}))",
                               Repository.GetTableAttr<RelationLeagueTeam>().Name,
                               Repository.GetTableAttr<Team>().Name,
                               Repository.GetTableAttr<League>().Name);

            DataAccess.ExecuteNonQuery(sql, null, trans);
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
                RelationLeagueTeamList = All();
            }

            public static List<RelationLeagueTeam> RelationLeagueTeamList;
        }


        #region Members and Properties

        [DbColumn("TeamGuid", IsKey = true)]
        public Guid TeamGuid
        { get; set; }

        [DbColumn("LeagueGuid", IsKey = true)]
        public Guid LeagueGuid
        { get; set; }

        #endregion
    }
}

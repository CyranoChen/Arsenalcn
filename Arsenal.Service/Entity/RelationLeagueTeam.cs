using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;

using Arsenalcn.Core;

namespace Arsenal.Service
{
    [AttrDbTable("Arsenal_RelationLeagueTeam", Key = "", Sort = "")]
    public class RelationLeagueTeam : IRelationLeagueTeam
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

        public IQueryable<RelationLeagueTeam> All()
        {
            string sql = string.Format("SELECT * FROM {0}", Repository.GetTableAttr<RelationLeagueTeam>().Name);

            DataSet ds = DataAccess.ExecuteDataset(sql);

            if (ds.Tables[0].Rows.Count == 0) { return null; }

            DataTable dt = ds.Tables[0];

            IList<RelationLeagueTeam> list = new List<RelationLeagueTeam>();

            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new RelationLeagueTeam(dr));
            }

            return list.AsQueryable();
        }

        public IQueryable<RelationLeagueTeam> Query(Expression<Func<RelationLeagueTeam, bool>> predicate)
        {
            Contract.Requires(predicate != null);

            return All().Where(predicate);
        }

        public void Create(SqlTransaction trans = null)
        {
            string sql = string.Format("INSERT INTO {0} (TeamGuid, LeagueGuid) VALUES (@teamGuid, @leagueGuid)",
                Repository.GetTableAttr<RelationLeagueTeam>().Name);

            SqlParameter[] para = { new SqlParameter("@teamGuid", TeamGuid), new SqlParameter("@leagueGuid", LeagueGuid) };

            DataAccess.ExecuteNonQuery(sql, para);
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

            DataAccess.ExecuteNonQuery(sql, para);
        }

        public void Delete(Expression<Func<RelationLeagueTeam, bool>> predicate, SqlTransaction trans = null)
        {
            Contract.Requires(predicate != null);

            var instances = Query(predicate);

            foreach (var instance in instances)
            {
                instance.Delete(trans);
            }
        }

        public void Clean(SqlTransaction trans = null)
        {
            string sql = string.Format(@"DELETE FROM {0} WHERE (TeamGuid NOT IN (SELECT TeamGuid FROM {1})) OR
                               (LeagueGuid NOT IN (SELECT LeagueGuid FROM {2}))",
                               Repository.GetTableAttr<RelationLeagueTeam>().Name,
                               Repository.GetTableAttr<Team>().Name,
                               Repository.GetTableAttr<League>().Name);

            DataAccess.ExecuteNonQuery(sql);
        }

        #region Members and Properties

        [AttrDbColumn("TeamGuid", Key = true)]
        public Guid TeamGuid
        { get; set; }

        [AttrDbColumn("LeagueGuid", Key = true)]
        public Guid LeagueGuid
        { get; set; }

        #endregion
    }
}

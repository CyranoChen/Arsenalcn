using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;
using Arsenalcn.Core;
using DataReaderMapper;
using DataReaderMapper.Mappers;

namespace Arsenal.Service
{
    [DbSchema("Arsenal_RelationLeagueTeam", Key = "", Sort = "")]
    public class RelationLeagueTeam
    {
        private static void CreateMap()
        {
            Mapper.CreateMap<IDataReader, RelationLeagueTeam>();
        }

        public static RelationLeagueTeam Single(Guid teamGuid, Guid leagueGuid)
        {
            var sql =
                $"SELECT * FROM {Repository.GetTableAttr<RelationLeagueTeam>().Name} WHERE TeamGuid = @teamGuid AND LeagueGuid = @leagueGuid";

            SqlParameter[] para =
            {
                new SqlParameter("@teamGuid", teamGuid),
                new SqlParameter("@leagueGuid", leagueGuid)
            };

            var ds = DataAccess.ExecuteDataset(sql, para);

            var dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                using (var reader = dt.CreateDataReader())
                {
                    Mapper.Initialize(cfg =>
                    {
                        MapperRegistry.Mappers.Insert(0,
                            new DataReaderMapper.DataReaderMapper { YieldReturnEnabled = false });

                        CreateMap();
                    });

                    return Mapper.Map<IDataReader, IEnumerable<RelationLeagueTeam>>(reader).FirstOrDefault();
                }
            }

            return null;
        }

        public bool Any()
        {
            var sql =
                $"SELECT * FROM {Repository.GetTableAttr<RelationLeagueTeam>().Name} WHERE TeamGuid = @teamGuid AND LeagueGuid = @leagueGuid";

            SqlParameter[] para =
            {
                new SqlParameter("@teamGuid", TeamGuid),
                new SqlParameter("@leagueGuid", LeagueGuid)
            };

            var ds = DataAccess.ExecuteDataset(sql, para);

            return ds.Tables[0].Rows.Count > 0;
        }

        public static List<RelationLeagueTeam> All()
        {
            var list = new List<RelationLeagueTeam>();

            var sql = $"SELECT * FROM {Repository.GetTableAttr<RelationLeagueTeam>().Name}";

            var ds = DataAccess.ExecuteDataset(sql);

            var dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                using (var reader = dt.CreateDataReader())
                {
                    Mapper.Initialize(cfg =>
                    {
                        MapperRegistry.Mappers.Insert(0,
                            new DataReaderMapper.DataReaderMapper { YieldReturnEnabled = false });

                        CreateMap();
                    });

                    list = Mapper.Map<IDataReader, IEnumerable<RelationLeagueTeam>>(reader).ToList();
                }
            }

            return list;
        }

        public static List<RelationLeagueTeam> QueryByLeagueGuid(Guid leagueGuid)
        {
            var list = new List<RelationLeagueTeam>();

            var sql =
                $"SELECT * FROM {Repository.GetTableAttr<RelationLeagueTeam>().Name} WHERE LeagueGuid = @leagueGuid";

            SqlParameter[] para = { new SqlParameter("@leagueGuid", leagueGuid) };

            var ds = DataAccess.ExecuteDataset(sql, para);

            var dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                using (var reader = dt.CreateDataReader())
                {
                    Mapper.Initialize(cfg =>
                    {
                        MapperRegistry.Mappers.Insert(0,
                            new DataReaderMapper.DataReaderMapper { YieldReturnEnabled = false });

                        CreateMap();
                    });

                    list = Mapper.Map<IDataReader, IEnumerable<RelationLeagueTeam>>(reader).ToList();
                }
            }

            return list;
        }

        public static List<RelationLeagueTeam> QueryByTeamGuid(Guid teamGuid)
        {
            var list = new List<RelationLeagueTeam>();

            var sql = $"SELECT * FROM {Repository.GetTableAttr<RelationLeagueTeam>().Name} WHERE TeamGuid = @teamGuid";

            SqlParameter[] para = { new SqlParameter("@teamGuid", teamGuid) };

            var ds = DataAccess.ExecuteDataset(sql, para);

            var dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                using (var reader = dt.CreateDataReader())
                {
                    Mapper.Initialize(cfg =>
                    {
                        MapperRegistry.Mappers.Insert(0,
                            new DataReaderMapper.DataReaderMapper { YieldReturnEnabled = false });

                        CreateMap();
                    });

                    list = Mapper.Map<IDataReader, IEnumerable<RelationLeagueTeam>>(reader).ToList();
                }
            }

            return list;
        }

        public void Insert(SqlTransaction trans = null)
        {
            var sql =
                $"INSERT INTO {Repository.GetTableAttr<RelationLeagueTeam>().Name} (TeamGuid, LeagueGuid) VALUES (@teamGuid, @leagueGuid)";

            SqlParameter[] para = { new SqlParameter("@teamGuid", TeamGuid), new SqlParameter("@leagueGuid", LeagueGuid) };

            DataAccess.ExecuteNonQuery(sql, para, trans);
        }

        public void Delete(SqlTransaction trans = null)
        {
            var sql =
                $"DELETE FROM {Repository.GetTableAttr<RelationLeagueTeam>().Name} WHERE TeamGuid = @teamGuid AND LeagueGuid = @leagueGuid";

            SqlParameter[] para = { new SqlParameter("@teamGuid", TeamGuid), new SqlParameter("@leagueGuid", LeagueGuid) };

            DataAccess.ExecuteNonQuery(sql, para, trans);
        }

        public static void Clean(SqlTransaction trans = null)
        {
            var sql =
                $@"DELETE FROM {Repository.GetTableAttr<RelationLeagueTeam>().Name} WHERE 
                     (TeamGuid NOT IN (SELECT TeamGuid FROM {Repository.GetTableAttr<Team>().Name})) 
                     OR (LeagueGuid NOT IN (SELECT LeagueGuid FROM {Repository.GetTableAttr<League>().Name}))";

            DataAccess.ExecuteNonQuery(sql, null, trans);
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
                RelationLeagueTeamList = All();
            }
        }

        #region Members and Properties

        [DbColumn("TeamGuid", IsKey = true)]
        public Guid TeamGuid { get; set; }

        [DbColumn("LeagueGuid", IsKey = true)]
        public Guid LeagueGuid { get; set; }

        #endregion
    }

    public static class RelationLeagueTeamExtensions
    {
        public static int Insert(this IEnumerable<RelationLeagueTeam> source, SqlTransaction trans = null)
        {
            Contract.Requires(source != null);

            var list = source as IList<RelationLeagueTeam> ?? source.ToList();

            foreach (var instance in list)
            {
                instance.Insert(trans);
            }

            return list.Count;
        }

        public static int Delete(this IEnumerable<RelationLeagueTeam> source, SqlTransaction trans = null)
        {
            Contract.Requires(source != null);

            var list = source as IList<RelationLeagueTeam> ?? source.ToList();

            foreach (var instance in list)
            {
                instance.Delete(trans);
            }

            return list.Count;
        }
    }
}
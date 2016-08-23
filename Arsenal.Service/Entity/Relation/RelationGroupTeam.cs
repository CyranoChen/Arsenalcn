using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Arsenalcn.Core;
using DataReaderMapper;
using DataReaderMapper.Mappers;

namespace Arsenal.Service
{
    [DbSchema("Arsenal_RelationGroupTeam", Key = "", Sort = "GroupGuid, PositionNo")]
    public class RelationGroupTeam
    {
        private static void CreateMap()
        {
            Mapper.CreateMap<IDataReader, RelationGroupTeam>();
        }

        public static RelationGroupTeam Single(Guid groupGuid, Guid teamGuid)
        {
            var sql =
                $"SELECT * FROM {Repository.GetTableAttr<RelationGroupTeam>().Name} WHERE GroupGuid = @groupGuid AND TeamGuid = @teamGuid";

            SqlParameter[] para =
            {
                new SqlParameter("@groupGuid", groupGuid),
                new SqlParameter("@teamGuid", teamGuid)
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

                    return Mapper.Map<IDataReader, IEnumerable<RelationGroupTeam>>(reader).FirstOrDefault();
                }
            }

            return null;
        }

        public bool Any()
        {
            var sql =
                $"SELECT * FROM {Repository.GetTableAttr<RelationGroupTeam>().Name} WHERE GroupGuid = @groupGuid AND TeamGuid = @teamGuid";

            SqlParameter[] para =
            {
                new SqlParameter("@groupGuid", GroupGuid),
                new SqlParameter("@teamGuid", TeamGuid)
            };

            var ds = DataAccess.ExecuteDataset(sql, para);

            return ds.Tables[0].Rows.Count > 0;
        }

        public static List<RelationGroupTeam> All()
        {
            var list = new List<RelationGroupTeam>();

            var sql = $"SELECT * FROM {Repository.GetTableAttr<RelationGroupTeam>().Name}";

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

                    list = Mapper.Map<IDataReader, IEnumerable<RelationGroupTeam>>(reader).ToList();
                }
            }

            return list;
        }

        public static List<RelationGroupTeam> QueryByGroupGuid(Guid groupGuid)
        {
            var list = new List<RelationGroupTeam>();

            var sql = $"SELECT * FROM {Repository.GetTableAttr<RelationGroupTeam>().Name} WHERE GroupGuid = @groupGuid";

            SqlParameter[] para = { new SqlParameter("@groupGuid", groupGuid) };

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

                    list = Mapper.Map<IDataReader, IEnumerable<RelationGroupTeam>>(reader).ToList();
                }
            }

            return list;
        }

        public static List<RelationGroupTeam> QueryByTeamGuid(Guid teamGuid)
        {
            var list = new List<RelationGroupTeam>();

            var sql = $"SELECT * FROM {Repository.GetTableAttr<RelationGroupTeam>().Name} WHERE TeamGuid = @teamGuid";

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

                    list = Mapper.Map<IDataReader, IEnumerable<RelationGroupTeam>>(reader).ToList();
                }
            }

            return list;
        }

        public void Insert(SqlTransaction trans = null)
        {
            var sql = $@"INSERT INTO {Repository.GetTableAttr<RelationGroupTeam>().Name} 
                                (PositionNo, TotalPlayed, TotalPoints, HomeWon, HomeDraw, HomeLost, HomePoints, 
                                HomeGoalFor, HomeGoalAgainst, HomeGoalDiff, AwayWon, AwayDraw, AwayLost, AwayPoints, 
                                AwayGoalFor, AwayGoalAgainst, AwayGoalDiff, GroupGuid, TeamGuid) VALUES 
                                (@positionNo, @totalPlayed, @totalPoints, @homeWon, @homeDraw, @homeLost, @homePoints, 
                                @homeGoalFor, @homeGoalAgainst, @homeGoalDiff, @awayWon, @awayDraw, @awayLost,  @awayPoints,
                                 @awayGoalFor, @awayGoalAgainst, @awayGoalDiff, @groupGuid, @teamGuid)";

            SqlParameter[] para =
            {
                new SqlParameter("@positionNo", !PositionNo.HasValue ? DBNull.Value : (object) PositionNo.Value),
                new SqlParameter("@totalPlayed", !TotalPlayed.HasValue ? DBNull.Value : (object) TotalPlayed.Value),
                new SqlParameter("@totalPoints", !TotalPoints.HasValue ? DBNull.Value : (object) TotalPoints.Value),
                // Home
                new SqlParameter("@homeWon", !HomeWon.HasValue ? DBNull.Value : (object) HomeWon.Value),
                new SqlParameter("@homeDraw", !HomeDraw.HasValue ? DBNull.Value : (object) HomeDraw.Value),
                new SqlParameter("@homeLost", !HomeLost.HasValue ? DBNull.Value : (object) HomeLost.Value),
                new SqlParameter("@homePoints", !HomePoints.HasValue ? DBNull.Value : (object) HomePoints.Value),
                new SqlParameter("@homeGoalFor", !HomeGoalFor.HasValue ? DBNull.Value : (object) HomeGoalFor.Value),
                new SqlParameter("@homeGoalAgainst", !HomeGoalAgainst.HasValue ? DBNull.Value : (object) HomeGoalAgainst.Value),
                new SqlParameter("@homeGoalDiff", !HomeGoalDiff.HasValue ? DBNull.Value : (object) HomeGoalDiff.Value),
                // Away
                new SqlParameter("@awayWon", !AwayWon.HasValue ? DBNull.Value : (object) AwayWon.Value),
                new SqlParameter("@awayDraw", !AwayDraw.HasValue ? DBNull.Value : (object) AwayDraw.Value),
                new SqlParameter("@awayLost", !AwayLost.HasValue ? DBNull.Value : (object) AwayLost.Value),
                new SqlParameter("@awayPoints", !AwayPoints.HasValue ? DBNull.Value : (object) AwayPoints.Value),
                new SqlParameter("@awayGoalFor", !AwayGoalFor.HasValue ? DBNull.Value : (object) AwayGoalFor.Value),
                new SqlParameter("@awayGoalAgainst", !AwayGoalAgainst.HasValue ? DBNull.Value : (object) AwayGoalAgainst.Value),
                new SqlParameter("@awayGoalDiff", !AwayGoalDiff.HasValue ? DBNull.Value : (object) AwayGoalDiff.Value),
                new SqlParameter("@groupGuid", GroupGuid),
                new SqlParameter("@teamGuid", TeamGuid)
            };

            DataAccess.ExecuteNonQuery(sql, para, trans);
        }

        public void Update(SqlTransaction trans = null)
        {
            var sql = $@"UPDATE {Repository.GetTableAttr<RelationGroupTeam>().Name} 
                                SET PositionNo = @positionNo, TotalPlayed = @totalPlayed, TotalPoints = @totalPoints,
                                HomeWon = @homeWon, HomeDraw = @homeDraw, HomeLost = @homeLost, HomePoints = @homePoints,
                                HomeGoalFor = @homeGoalFor, HomeGoalAgainst = @homeGoalAgainst, HomeGoalDiff = @homeGoalDiff,
                                AwayWon = @awayWon, AwayDraw = @awayDraw, AwayLost = @awayLost, AwayPoints = @awayPoints,
                                AwayGoalFor = @awayGoalFor, AwayGoalAgainst = @awayGoalAgainst, AwayGoalDiff = @awayGoalDiff 
                                WHERE GroupGuid = @groupGuid AND TeamGuid = @teamGuid";

            SqlParameter[] para =
            {
                new SqlParameter("@positionNo", !PositionNo.HasValue ? DBNull.Value : (object) PositionNo.Value),
                new SqlParameter("@totalPlayed", !TotalPlayed.HasValue ? DBNull.Value : (object) TotalPlayed.Value),
                new SqlParameter("@totalPoints", !TotalPoints.HasValue ? DBNull.Value : (object) TotalPoints.Value),
                // Home
                new SqlParameter("@homeWon", !HomeWon.HasValue ? DBNull.Value : (object) HomeWon.Value),
                new SqlParameter("@homeDraw", !HomeDraw.HasValue ? DBNull.Value : (object) HomeDraw.Value),
                new SqlParameter("@homeLost", !HomeLost.HasValue ? DBNull.Value : (object) HomeLost.Value),
                new SqlParameter("@homePoints", !HomePoints.HasValue ? DBNull.Value : (object) HomePoints.Value),
                new SqlParameter("@homeGoalFor", !HomeGoalFor.HasValue ? DBNull.Value : (object) HomeGoalFor.Value),
                new SqlParameter("@homeGoalAgainst", !HomeGoalAgainst.HasValue ? DBNull.Value : (object) HomeGoalAgainst.Value),
                new SqlParameter("@homeGoalDiff", !HomeGoalDiff.HasValue ? DBNull.Value : (object) HomeGoalDiff.Value),
                // Away
                new SqlParameter("@awayWon", !AwayWon.HasValue ? DBNull.Value : (object) AwayWon.Value),
                new SqlParameter("@awayDraw", !AwayDraw.HasValue ? DBNull.Value : (object) AwayDraw.Value),
                new SqlParameter("@awayLost", !AwayLost.HasValue ? DBNull.Value : (object) AwayLost.Value),
                new SqlParameter("@awayPoints", !AwayPoints.HasValue ? DBNull.Value : (object) AwayPoints.Value),
                new SqlParameter("@awayGoalFor", !AwayGoalFor.HasValue ? DBNull.Value : (object) AwayGoalFor.Value),
                new SqlParameter("@awayGoalAgainst", !AwayGoalAgainst.HasValue ? DBNull.Value : (object) AwayGoalAgainst.Value),
                new SqlParameter("@awayGoalDiff", !AwayGoalDiff.HasValue ? DBNull.Value : (object) AwayGoalDiff.Value),
                // WHERE
                new SqlParameter("@groupGuid", GroupGuid),
                new SqlParameter("@teamGuid", TeamGuid)
            };

            DataAccess.ExecuteNonQuery(sql, para, trans);
        }

        public void Delete(SqlTransaction trans = null)
        {
            var sql =
                $"DELETE FROM {Repository.GetTableAttr<RelationGroupTeam>().Name} WHERE GroupGuid = @groupGuid AND TeamGuid = @teamGuid";

            SqlParameter[] para = { new SqlParameter("@groupGuid", GroupGuid), new SqlParameter("@teamGuid", TeamGuid) };

            DataAccess.ExecuteNonQuery(sql, para, trans);
        }

        public void Statistic(List<Casino.Match> list)
        {
            Inital();

            foreach (var m in list)
            {
                if (m != null)
                {
                    // 主队统计
                    if (m.Home == TeamGuid)
                    {
                        TotalPlayed++;

                        if (m.ResultHome > m.ResultAway)
                        {
                            HomeWon++;
                            HomePoints += 3;
                        }
                        else if (m.ResultHome == m.ResultAway)
                        {
                            HomeDraw++;
                            HomePoints += 1;
                        }
                        else
                            HomeLost++;

                        HomeGoalFor += m.ResultHome;
                        HomeGoalAgainst += m.ResultAway;
                    }
                    // 客队统计
                    else if (m.Away == TeamGuid)
                    {
                        TotalPlayed++;

                        if (m.ResultAway > m.ResultHome)
                        {
                            AwayWon++;
                            AwayPoints += 3;
                        }
                        else if (m.ResultAway == m.ResultHome)
                        {
                            AwayDraw++;
                            AwayPoints += 1;
                        }
                        else
                            AwayLost++;

                        AwayGoalFor += m.ResultAway;
                        AwayGoalAgainst += m.ResultHome;
                    }
                }
            }

            // 净胜球和总积分
            HomeGoalDiff = Convert.ToInt16(HomeGoalFor - HomeGoalAgainst);
            AwayGoalDiff = Convert.ToInt16(AwayGoalFor - AwayGoalAgainst);
            TotalPoints = Convert.ToInt16(HomePoints + AwayPoints);
        }

        private void Inital()
        {
            PositionNo = 0;
            TotalPlayed = 0;
            TotalPoints = 0;

            HomeWon = 0;
            HomeDraw = 0;
            HomeLost = 0;
            HomeGoalFor = 0;
            HomeGoalAgainst = 0;
            HomeGoalDiff = 0;
            HomePoints = 0;

            AwayWon = 0;
            AwayDraw = 0;
            AwayLost = 0;
            AwayGoalFor = 0;
            AwayGoalAgainst = 0;
            AwayGoalDiff = 0;
            AwayPoints = 0;
        }

        public static void Clean(SqlTransaction trans = null)
        {
            var sql =
                $@"DELETE FROM {Repository.GetTableAttr<RelationGroupTeam>().Name} WHERE 
                     (TeamGuid NOT IN (SELECT TeamGuid FROM {Repository.GetTableAttr<Team>().Name})) 
                     OR (GroupGuid NOT IN (SELECT GroupGuid FROM {Repository.GetTableAttr<Group>().Name}))";

            DataAccess.ExecuteNonQuery(sql, null, trans);
        }

        public static class Cache
        {
            public static List<RelationGroupTeam> RelationGroupTeamList;

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
                RelationGroupTeamList = All();
            }
        }

        #region Members and Properties

        [DbColumn("GroupGuid", IsKey = true)]
        public Guid GroupGuid { get; set; }

        [DbColumn("TeamGuid", IsKey = true)]
        public Guid TeamGuid { get; set; }

        [DbColumn("PositionNo")]
        public short? PositionNo { get; set; }

        [DbColumn("TotalPlayed")]
        public short? TotalPlayed { get; set; }

        [DbColumn("TotalPoints")]
        public short? TotalPoints { get; set; }

        [DbColumn("HomeWon")]
        public short? HomeWon { get; set; }

        [DbColumn("HomeDraw")]
        public short? HomeDraw { get; set; }

        [DbColumn("HomeLost")]
        public short? HomeLost { get; set; }

        [DbColumn("HomeGoalFor")]
        public short? HomeGoalFor { get; set; }

        [DbColumn("HomeGoalAgainst")]
        public short? HomeGoalAgainst { get; set; }

        [DbColumn("HomeGoalDiff")]
        public short? HomeGoalDiff { get; set; }

        [DbColumn("HomePoints")]
        public short? HomePoints { get; set; }

        [DbColumn("AwayWon")]
        public short? AwayWon { get; set; }

        [DbColumn("AwayDraw")]
        public short? AwayDraw { get; set; }

        [DbColumn("AwayLost")]
        public short? AwayLost { get; set; }

        [DbColumn("AwayGoalFor")]
        public short? AwayGoalFor { get; set; }

        [DbColumn("AwayGoalAgainst")]
        public short? AwayGoalAgainst { get; set; }

        [DbColumn("AwayGoalDiff")]
        public short? AwayGoalDiff { get; set; }

        [DbColumn("AwayPoints")]
        public short? AwayPoints { get; set; }

        #endregion
    }
}
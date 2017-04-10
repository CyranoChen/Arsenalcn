using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Arsenalcn.Core;
using Arsenalcn.Core.Dapper;

namespace Arsenal.Service
{
    [DbSchema("Arsenal_RelationGroupTeam", Sort = "GroupGuid, PositionNo")]
    public class RelationGroupTeam : Dao
    {
        public void Update()
        {
            IRepository repo = new Repository();

            repo.Update(this, x => x.GroupGuid == GroupGuid && x.TeamGuid == TeamGuid);
        }

        public void Delete()
        {
            IRepository repo = new Repository();

            repo.Delete<RelationGroupTeam>(x => x.GroupGuid == GroupGuid && x.TeamGuid == TeamGuid);
        }

        public void Statistic(IEnumerable<Casino.Match> matches)
        {
            Default();

            foreach (var m in matches.Where(m => m != null))
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

            // 净胜球和总积分
            HomeGoalDiff = Convert.ToInt16(HomeGoalFor - HomeGoalAgainst);
            AwayGoalDiff = Convert.ToInt16(AwayGoalFor - AwayGoalAgainst);
            TotalPoints = Convert.ToInt16(HomePoints + AwayPoints);
        }

        private void Default()
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

        public static void Clean(IDbTransaction trans = null)
        {
            var sql =
                $@"DELETE FROM {Repository.GetTableAttr<RelationGroupTeam>().Name} WHERE 
                     (TeamGuid NOT IN (SELECT TeamGuid FROM {Repository.GetTableAttr<Team>().Name})) 
                     OR (GroupGuid NOT IN (SELECT GroupGuid FROM {Repository.GetTableAttr<Group>().Name}))";

            IDapperHelper dapper = DapperHelper.GetInstance();

            dapper.Execute(sql, trans);
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
                IRepository repo = new Repository();

                RelationGroupTeamList = repo.All<RelationGroupTeam>();
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
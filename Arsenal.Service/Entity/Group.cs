using System;
using System.Collections.Generic;
using System.Linq;
using Arsenalcn.Core;
using CasinoMatch = Arsenal.Service.Casino.Match;

namespace Arsenal.Service
{
    [DbSchema("Arsenal_Group", Key = "GroupGuid", Sort = "GroupOrder")]
    public class Group : Entity<Guid>
    {
        public void BindMatches()
        {
            IRepository repo = new Repository();

            var list = repo.Query<CasinoMatch>(x => x.LeagueGuid == LeagueGuid);
            var teams = repo.Query<RelationGroupTeam>(x => x.GroupGuid == ID);

            list = list.FindAll(x => teams.Exists(t => t.TeamGuid == x.Home) && teams.Exists(t => t.TeamGuid == x.Away));

            if (list.Count > 0)
            {
                foreach (var m in list.Where(m => !m.GroupGuid.HasValue || m.GroupGuid != ID))
                {
                    m.GroupGuid = ID;

                    repo.Update(m);
                }
            }
        }

        public void Statistic()
        {
            IRepository repo = new Repository();

            // 取得当前分组的所有球队排名关系
            var listRgt = repo.Query<RelationGroupTeam>(x => x.GroupGuid == ID);

            // 取得当前分组的所有博彩比赛
            List<CasinoMatch> listMatch;

            if (IsTable)
            {
                // 联赛积分榜
                //@"SELECT * FROM dbo.AcnCasino_Match WHERE (ResultHome IS NOT NULL) AND (ResultAway IS NOT NULL) AND 
                //        (Home IN (SELECT TeamGuid FROM dbo.Arsenal_RelationGroupTeam AS GroupTeam1 WHERE GroupGuid = @groupGuid)) AND 
                //        (Away IN (SELECT TeamGuid FROM dbo.Arsenal_RelationGroupTeam AS GroupTeam2 WHERE GroupGuid = @groupGuid)) AND
                //        (LeagueGuid = (SELECT LeagueGuid FROM dbo.Arsenal_Group WHERE GroupGuid = @groupGuid))
                //        ORDER BY PlayTime DESC";

                listMatch = repo.Query<CasinoMatch>(x => x.ResultHome.HasValue && x.ResultAway.HasValue && x.LeagueGuid == LeagueGuid)
                    .FindAll(x => listRgt.Exists(t => t.TeamGuid == x.Home) && listRgt.Exists(t => t.TeamGuid == x.Away))
                    .OrderBy(x => x.PlayTime).ToList();
            }
            else
            {
                // 分组排名表
                //@"SELECT * FROM dbo.AcnCasino_Match WHERE (GroupGuid =@groupGuid) AND (ResultHome IS NOT NULL) AND (ResultAway IS NOT NULL) 
                //        AND (LeagueGuid = (SElECT LeagueGuid FROM dbo.Arsenal_Group WHERE GroupGuid = @groupGuid)) ORDER BY PlayTime DESC";

                listMatch = repo.Query<CasinoMatch>(x => x.ResultHome.HasValue && x.ResultAway.HasValue && x.GroupGuid == ID && x.LeagueGuid == LeagueGuid)
                    .OrderBy(x => x.PlayTime).ToList();
            }

            if (listRgt.Count > 0 && listMatch.Count > 0)
            {
                foreach (var rgt in listRgt)
                {
                    // 根据比赛更新球队的分组排名信息
                    rgt.Statistic(listMatch.FindAll(x => x.Home == rgt.TeamGuid || x.Away == rgt.TeamGuid));
                }
            }

            if (listRgt.Count > 0)
            {
                if (RankMethod == RankMethodType.VersusHist)
                {
                    // 设置双方交战比较规则
                    var comparer = new GroupRankComparer { Matches = listMatch };

                    // 根据总积分、双方交战记录、净胜球、进球数、失球数排序
                    listRgt = listRgt.OrderByDescending(x => x.TotalPoints)
                        .ThenByDescending(x => x.TeamGuid, comparer)
                        .ThenByDescending(x => x.HomeGoalDiff + x.AwayGoalDiff)
                        .ThenByDescending(x => x.HomeGoalFor + x.AwayGoalFor)
                        .ThenBy(x => x.HomeGoalAgainst + x.AwayGoalAgainst)
                        .ThenBy(x => Team.Cache.Load(x.TeamGuid).TeamEnglishName)
                        .ToList();
                }
                else
                {
                    // 根据总积分、净胜球、进球数、失球数排序
                    listRgt = listRgt.OrderByDescending(x => x.TotalPoints)
                        .ThenByDescending(x => x.HomeGoalDiff + x.AwayGoalDiff)
                        .ThenByDescending(x => x.HomeGoalFor + x.AwayGoalFor)
                        .ThenBy(x => x.HomeGoalAgainst + x.AwayGoalAgainst)
                        .ThenBy(x => Team.Cache.Load(x.TeamGuid).TeamEnglishName)
                        .ToList();
                }
                // 更新排名并持久化
                short positionNo = 0;

                foreach (var instance in listRgt)
                {
                    instance.PositionNo = ++positionNo;
                    instance.Update();
                }
            }
        }

        #region Members and Properties

        [DbColumn("GroupName")]
        public string GroupName { get; set; }

        [DbColumn("GroupOrder")]
        public int GroupOrder { get; set; }

        [DbColumn("LeagueGuid")]
        public Guid LeagueGuid { get; set; }

        [DbColumn("IsTable")]
        public bool IsTable { get; set; }

        [DbColumn("RankMethod")]
        public RankMethodType RankMethod { get; set; }

        #endregion
    }

    public class GroupRankComparer : IComparer<Guid>
    {
        public List<CasinoMatch> Matches { get; set; }

        public int Compare(Guid id1, Guid id2)
        {
            var matchResult = Matches.FindAll(x => x.ResultHome.HasValue && x.ResultAway.HasValue
                                                   && (x.Home == id1 && x.Away == id2 || x.Home == id2 && x.Away == id1));

            if (matchResult.Count > 0)
            {
                short goal1 = 0;
                short goal2 = 0;
                short goalAway1 = 0;
                short goalAway2 = 0;

                foreach (var m in matchResult)
                {
                    if (m.Home == id1 && m.ResultHome.HasValue && m.ResultAway.HasValue)
                    {
                        goal1 += m.ResultHome.Value;
                        goal2 += m.ResultAway.Value;
                        goalAway2 += m.ResultAway.Value;
                    }
                    else if (m.Home == id2 && m.ResultHome.HasValue && m.ResultAway.HasValue)
                    {
                        goal2 += m.ResultHome.Value;
                        goal1 += m.ResultAway.Value;
                        goalAway1 += m.ResultAway.Value;
                    }
                }

                if (goal1 == goal2)
                {
                    return goalAway1.CompareTo(goalAway2);
                }
                else
                {
                    return goal1.CompareTo(goal2);
                }
            }
            else
            {
                return 0;
            }
        }
    }

    public enum RankMethodType
    {
        VersusHist = 0,
        GoalDiff = 1
    }
}
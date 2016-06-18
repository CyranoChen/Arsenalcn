using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Arsenalcn.Core;
using DataReaderMapper;
using CasinoMatch = Arsenal.Service.Casino.Match;

namespace Arsenal.Service
{
    [DbSchema("Arsenal_Group", Key = "GroupGuid", Sort = "GroupOrder")]
    public class Group : Entity<Guid>
    {
        public static void CreateMap()
        {
            var map = Mapper.CreateMap<IDataReader, Group>();

            map.ForMember(d => d.ID, opt => opt.MapFrom(s => (Guid)s.GetValue("GroupGuid")));
        }

        public void Statistic()
        {
            IRepository repo = new Repository();

            // 取得当前分组的所有球队排名关系
            var listRgt = RelationGroupTeam.QueryByGroupGuid(ID);

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
                // 根据总积分、净胜球、进球数、失球数排序
                listRgt = listRgt.OrderByDescending(x => x.TotalPoints)
                    .ThenByDescending(x => x.HomeGoalDiff + x.AwayGoalDiff)
                    .ThenByDescending(x => x.HomeGoalFor + x.AwayGoalFor)
                    .ThenBy(x => x.HomeGoalAgainst + x.AwayGoalAgainst)
                    .ThenBy(x => Team.Cache.Load(x.TeamGuid).TeamEnglishName)
                    .ToList();

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

        #endregion
    }
}
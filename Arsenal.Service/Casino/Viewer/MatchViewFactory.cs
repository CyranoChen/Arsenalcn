using System.Collections.Generic;
using System.Linq;
using Arsenalcn.Core;

namespace Arsenal.Service.Casino
{
    public class MatchViewFactory : ViewerFactory, IViewerFactory<MatchView>
    {
        public MatchViewFactory()
        {
            Dapper = new DapperHelper();

            ViewerSql = @"SELECT m.MatchGuid AS ID, m.ResultHome, m.ResultAway, m.PlayTime, m.LeagueName, m.Round, 
                                      c.CasinoItemGuid, c.ItemType, c.MatchGuid, c.CloseTime, c.Earning, 
                                      h.TeamGuid AS HomeTeamGuid, h.TeamEnglishName AS HomeEnglishName, h.TeamDisplayName AS HomeDisplayName, h.TeamLogo AS HomeLogo, 
                                      a.TeamGuid AS AwayTeamGuid, a.TeamEnglishName AS AwayEnglishName, a.TeamDisplayName AS AwayDisplayName, a.TeamLogo AS AwayLogo, 
                                      g.GroupGuid, g.GroupName, g.IsTable, 
                                      l.LeagueGuid, l.LeagueOrgName, l.LeagueSeason, l.LeagueLogo 
                            FROM AcnCasino_Match AS m LEFT OUTER JOIN
                                      AcnCasino_CasinoItem AS c ON m.MatchGuid = c.MatchGuid LEFT OUTER JOIN
                                      Arsenal_League AS l ON m.LeagueGuid = l.LeagueGuid LEFT OUTER JOIN
                                      Arsenal_Team AS h ON m.Home = h.TeamGuid LEFT OUTER JOIN
                                      Arsenal_Team AS a ON m.Away = a.TeamGuid LEFT OUTER JOIN
                                      Arsenal_Group AS g ON m.GroupGuid = g.GroupGuid
                            WHERE  (c.ItemType = 2) ";

            SplitOn = "ID, CasinoItemGuid, HomeTeamGuid, AwayTeamGuid, GroupGuid, LeagueGuid";

            DbSchema = Repository.GetTableAttr<MatchView>();
        }

        public MatchView Single(object key)
        {
            return Dapper.Query<MatchView, CasinoItem, HomeTeam, AwayTeam, Group, League, MatchView>(BuildSingleSql(),
                        (x, c, h, a, g, l) =>
                        {
                            x.CasinoItem = c;
                            x.Home = h;
                            x.Away = a;
                            x.Group = g;
                            x.League = l;

                            return x;
                        }, new { key }, SplitOn).FirstOrDefault();
        }

        public MatchView Single(Criteria criteria)
        {
            return Dapper.Query<MatchView, CasinoItem, HomeTeam, AwayTeam, Group, League, MatchView>(BuildSingleSql(criteria),
                        (x, c, h, a, g, l) =>
                        {
                            x.CasinoItem = c;
                            x.Home = h;
                            x.Away = a;
                            x.Group = g;
                            x.League = l;

                            return x;
                        }, criteria?.Parameters, SplitOn).FirstOrDefault();
        }

        public List<MatchView> All()
        {
            return Dapper.Query<MatchView, CasinoItem, HomeTeam, AwayTeam, Group, League, MatchView>(BuildAllSql(),
                        (x, c, h, a, g, l) =>
                        {
                            x.CasinoItem = c;
                            x.Home = h;
                            x.Away = a;
                            x.Group = g;
                            x.League = l;

                            return x;
                        }, null, SplitOn).ToList();
        }

        public List<MatchView> All(IPager pager, string orderBy = null)
        {
            return Dapper.Query<MatchView, CasinoItem, HomeTeam, AwayTeam, Group, League, MatchView>(BuildAllSql(pager, orderBy),
                        (x, c, h, a, g, l) =>
                        {
                            x.CasinoItem = c;
                            x.Home = h;
                            x.Away = a;
                            x.Group = g;
                            x.League = l;

                            return x;
                        }, null, SplitOn).ToList();
        }

        public List<MatchView> Query(Criteria criteria)
        {
            return Dapper.Query<MatchView, CasinoItem, HomeTeam, AwayTeam, Group, League, MatchView>(BuildQuerySql(criteria),
                        (x, c, h, a, g, l) =>
                        {
                            x.CasinoItem = c;
                            x.Home = h;
                            x.Away = a;
                            x.Group = g;
                            x.League = l;

                            return x;
                        }, criteria?.Parameters, SplitOn).ToList();
        }
    }
}

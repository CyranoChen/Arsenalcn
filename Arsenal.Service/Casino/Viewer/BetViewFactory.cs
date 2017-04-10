using System.Collections.Generic;
using System.Linq;
using Arsenalcn.Core;
using Arsenalcn.Core.Dapper;

namespace Arsenal.Service.Casino
{
    public class BetViewFactory : ViewerFactory, IViewerFactory<BetView>
    {
        private IDapperHelper _dapper;

        public BetViewFactory(IDapperHelper dapper = null)
        {
            _dapper = dapper ?? DapperHelper.GetInstance();

            ViewerSql = @"SELECT b.ID, b.UserID, b.UserName, b.BetAmount, b.BetTime, b.BetRate, b.IsWin, b.Earning, b.EarningDesc, 
                                      c.CasinoItemGuid, c.ItemType, c.CloseTime, c.Earning AS c_Earning, 
                                      m.MatchGuid, m.ResultHome, m.ResultAway, m.PlayTime, m.LeagueName, m.Round, m.GroupGuid, 
                                      h.TeamGuid AS HomeTeamGuid, h.TeamEnglishName AS HomeEnglishName, h.TeamDisplayName AS HomeDisplayName, h.TeamLogo AS HomeLogo, 
                                      a.TeamGuid AS AwayTeamGuid, a.TeamEnglishName AS AwayEnglishName, a.TeamDisplayName AS AwayDisplayName, a.TeamLogo AS AwayLogo, 
                                      l.LeagueGuid, l.LeagueOrgName, l.LeagueSeason, l.LeagueLogo 
                    FROM AcnCasino_Bet AS b LEFT OUTER JOIN
                                      AcnCasino_CasinoItem AS c ON b.CasinoItemGuid = c.CasinoItemGuid LEFT OUTER JOIN
                                      AcnCasino_Match AS m ON c.MatchGuid = m.MatchGuid LEFT OUTER JOIN
                                      Arsenal_Team AS h ON m.Home = h.TeamGuid LEFT OUTER JOIN
                                      Arsenal_Team AS a ON m.Away = a.TeamGuid LEFT OUTER JOIN
                                      Arsenal_League AS l ON m.LeagueGuid = l.LeagueGuid";

            SplitOn = "ID, CasinoItemGuid, MatchGuid, HomeTeamGuid, AwayTeamGuid, LeagueGuid";

            DbSchema = Repository.GetTableAttr<BetView>();
        }

        public BetView Single(object key)
        {
            return _dapper.Query<BetView, CasinoItem, Match, HomeTeam, AwayTeam, League, BetView>(BuildSingleSql(),
                        (x, c, m, h, a, l) =>
                        {
                            x.CasinoItem = c;
                            x.Match = m;
                            x.Home = h;
                            x.Away = a;
                            x.League = l;

                            return x;
                        }, new { key }, SplitOn).FirstOrDefault();
        }

        public BetView Single(Criteria criteria)
        {
            return _dapper.Query<BetView, CasinoItem, Match, HomeTeam, AwayTeam, League, BetView>(BuildSingleSql(criteria),
                (x, c, m, h, a, l) =>
                {
                    x.CasinoItem = c;
                    x.Match = m;
                    x.Home = h;
                    x.Away = a;
                    x.League = l;

                    return x;
                }, criteria?.Parameters, SplitOn).FirstOrDefault();
        }

        public List<BetView> All()
        {
            return _dapper.Query<BetView, CasinoItem, Match, HomeTeam, AwayTeam, League, BetView>(BuildAllSql(),
                (x, c, m, h, a, l) =>
                {
                    x.CasinoItem = c;
                    x.Match = m;
                    x.Home = h;
                    x.Away = a;
                    x.League = l;

                    return x;
                }, null, SplitOn).ToList();
        }

        public List<BetView> All(IPager pager, string orderBy = null)
        {
            return _dapper.Query<BetView, CasinoItem, Match, HomeTeam, AwayTeam, League, BetView>(BuildAllSql(pager, orderBy),
                (x, c, m, h, a, l) =>
                {
                    x.CasinoItem = c;
                    x.Match = m;
                    x.Home = h;
                    x.Away = a;
                    x.League = l;

                    return x;
                }, null, SplitOn).ToList();
        }

        public List<BetView> Query(Criteria criteria)
        {
            return _dapper.Query<BetView, CasinoItem, Match, HomeTeam, AwayTeam, League, BetView>(BuildQuerySql(criteria),
                (x, c, m, h, a, l) =>
                {
                    x.CasinoItem = c;
                    x.Match = m;
                    x.Home = h;
                    x.Away = a;
                    x.League = l;

                    return x;
                }, criteria?.Parameters, SplitOn).ToList();
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using Arsenalcn.Core;
using Arsenalcn.Core.Dapper;

namespace Arsenal.Service.Casino
{
    public class CouponViewFactory : ViewerFactory, IViewerFactory<CouponView>
    {
        private IDapperHelper _dapper;

        public CouponViewFactory(IDapperHelper dapper = null)
        {
            _dapper = dapper ?? DapperHelper.GetInstance();

            ViewerSql = @"SELECT b.ID, b.UserID, b.UserName, m.MatchGuid, bdh.DetailValue AS BetResultHome, bda.DetailValue AS BetResultAway, m.PlayTime, m.Round, 
                                  h.TeamGuid AS HomeTeamGuid, h.TeamEnglishName AS HomeEnglishName, h.TeamDisplayName AS HomeDisplayName, h.TeamLogo AS HomeLogo, 
                                  a.TeamGuid AS AwayTeamGuid, a.TeamEnglishName AS AwayEnglishName, a.TeamDisplayName AS AwayDisplayName, a.TeamLogo AS AwayLogo, 
                                  l.LeagueGuid, l.LeagueOrgName, l.LeagueSeason, l.LeagueLogo
                            FROM AcnCasino_Match AS m LEFT OUTER JOIN
                                  AcnCasino_CasinoItem AS c ON m.MatchGuid = c.MatchGuid LEFT OUTER JOIN
                                  Arsenal_League AS l ON m.LeagueGuid = l.LeagueGuid LEFT OUTER JOIN
                                  Arsenal_Team AS h ON m.Home = h.TeamGuid LEFT OUTER JOIN
                                  Arsenal_Team AS a ON m.Away = a.TeamGuid LEFT OUTER JOIN
                                  AcnCasino_Bet AS b ON b.CasinoItemGuid = c.CasinoItemGuid LEFT OUTER JOIN
                                  AcnCasino_BetDetail AS bdh ON b.ID = bdh.BetID LEFT OUTER JOIN
                                  AcnCasino_BetDetail AS bda ON b.ID = bda.BetID
                            WHERE  (c.ItemType = 1) AND (c.CloseTime > GETDATE()) AND (bdh.DetailName = 'Home') AND (bda.DetailName = 'Away') ";

            SplitOn = "ID, HomeTeamGuid, AwayTeamGuid, LeagueGuid";

            DbSchema = Repository.GetTableAttr<CouponView>();
        }

        public CouponView Single(Criteria criteria)
        {
            return _dapper.Query<CouponView, HomeTeam, AwayTeam, League, CouponView>(BuildSingleSql(criteria),
                (x, h, a, l) =>
                {
                    x.Home = h;
                    x.Away = a;
                    x.League = l;

                    return x;
                }, criteria?.Parameters, SplitOn).FirstOrDefault();
        }

        public List<CouponView> All()
        {
            return _dapper.Query<CouponView, HomeTeam, AwayTeam, League, CouponView>(BuildAllSql(),
                (x, h, a, l) =>
                {
                    x.Home = h;
                    x.Away = a;
                    x.League = l;

                    return x;
                }, null, SplitOn).ToList();
        }

        public List<CouponView> All(IPager pager, string orderBy = null)
        {
            return _dapper.Query<CouponView, HomeTeam, AwayTeam, League, CouponView>(BuildAllSql(pager, orderBy),
                (x, h, a, l) =>
                {
                    x.Home = h;
                    x.Away = a;
                    x.League = l;

                    return x;
                }, null, SplitOn).ToList();
        }

        public List<CouponView> Query(Criteria criteria)
        {
            return _dapper.Query<CouponView, HomeTeam, AwayTeam, League, CouponView>(BuildQuerySql(criteria),
                (x, h, a, l) =>
                {
                    x.Home = h;
                    x.Away = a;
                    x.League = l;

                    return x;
                }, criteria?.Parameters, SplitOn).ToList();
        }
    }
}

using System.Collections.Generic;
using System.Data;
using System.Linq;
using Arsenalcn.Core;

namespace Arsenal.Service.Casino
{
    public class BonusViewFactory : ViewerFactory, IViewerFactory<BonusView>
    {
        public BonusViewFactory()
        {
            Dapper = new DapperHelper();

            ViewerSql = @"SELECT ISNULL(mr.MatchGuid, sc.MatchGuid) AS MatchGuid, ISNULL(mr.UserID, sc.UserID) AS UserID, ISNULL(mr.UserName, sc.UserName) AS UserName, 
                                  sc.Win, sc.Lose, sc.Earning, sc.TotalBet, mr.RPBonus, m.PlayTime, m.LeagueName, m.Round, 
                                  h.TeamGuid AS HomeTeamGuid, h.TeamEnglishName AS HomeEnglishName, h.TeamDisplayName AS HomeDisplayName, h.TeamLogo AS HomeLogo, 
                                  a.TeamGuid AS AwayTeamGuid, a.TeamEnglishName AS AwayEnglishName, a.TeamDisplayName AS AwayDisplayName, a.TeamLogo AS AwayLogo, 
                                  l.LeagueGuid, l.LeagueOrgName, l.LeagueSeason, l.LeagueLogo 
                        FROM     
                                (SELECT MatchGuid, UserID, UserName, COUNT(CASE IsWin WHEN 1 THEN 1 ELSE NULL END) AS Win, COUNT(CASE IsWin WHEN 0 THEN 0 ELSE NULL END) AS Lose, SUM(ISNULL(Earning, 0)) AS Earning, SUM(ISNULL(BetAmount, 0)) AS TotalBet
                                 FROM vw_AcnCasino_BetInfo AS BetSingleChoice
                                 WHERE (Earning IS NOT NULL) AND (BetAmount IS NOT NULL) AND (ItemType = 2)
                                 GROUP BY MatchGuid, UserID, UserName) AS sc 
                            FULL OUTER JOIN
                                 (SELECT MatchGuid, UserID, UserName, COUNT(CASE EarningDesc WHEN 'RP+1' THEN 1 ELSE NULL END) AS RPBonus
                                  FROM vw_AcnCasino_BetInfo AS BetMatchResult
                                  WHERE   (Earning = 0) AND (BetAmount IS NULL) AND (ItemType = 1)
                                  GROUP BY MatchGuid, UserID, UserName) AS mr 
                            ON sc.MatchGuid = mr.MatchGuid AND sc.UserID = mr.UserID AND sc.UserName = mr.UserName
                        LEFT OUTER JOIN 
                                  AcnCasino_Match AS m ON ISNULL(mr.MatchGuid, sc.MatchGuid) = m.MatchGuid LEFT OUTER JOIN
                                  Arsenal_League AS l ON m.LeagueGuid = l.LeagueGuid LEFT OUTER JOIN
                                  Arsenal_Team AS h ON m.Home = h.TeamGuid LEFT OUTER JOIN
                                  Arsenal_Team AS a ON m.Away = a.TeamGuid";

            SplitOn = "MatchGuid, HomeTeamGuid, AwayTeamGuid, LeagueGuid";

            DbSchema = Repository.GetTableAttr<BonusView>();
        }

        public BonusView Single(Criteria criteria, IDbTransaction trans = null)
        {
            return Dapper.Query<BonusView, HomeTeam, AwayTeam, League, BonusView>(BuildSingleSql(criteria),
                (x, h, a, l) =>
                {
                    x.Home = h;
                    x.Away = a;
                    x.League = l;

                    return x;
                }, criteria?.Parameters, trans, SplitOn).FirstOrDefault();
        }

        public List<BonusView> All(IDbTransaction trans = null)
        {
            return Dapper.Query<BonusView, HomeTeam, AwayTeam, League, BonusView>(BuildAllSql(),
                (x, h, a, l) =>
                {
                    x.Home = h;
                    x.Away = a;
                    x.League = l;

                    return x;
                }, null, trans, SplitOn).ToList();
        }

        public List<BonusView> All(IPager pager, string orderBy = null, IDbTransaction trans = null)
        {
            return Dapper.Query<BonusView, HomeTeam, AwayTeam, League, BonusView>(BuildAllSql(pager, orderBy),
                (x, h, a, l) =>
                {
                    x.Home = h;
                    x.Away = a;
                    x.League = l;

                    return x;
                }, null, trans, SplitOn).ToList();
        }

        public List<BonusView> Query(Criteria criteria, IDbTransaction trans = null)
        {
            return Dapper.Query<BonusView, HomeTeam, AwayTeam, League, BonusView>(BuildQuerySql(criteria),
                (x, h, a, l) =>
                {
                    x.Home = h;
                    x.Away = a;
                    x.League = l;

                    return x;
                }, criteria?.Parameters, trans, SplitOn).ToList();
        }
    }
}

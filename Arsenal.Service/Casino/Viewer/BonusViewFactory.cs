using System;
using System.Collections.Generic;
using System.Linq;
using Arsenalcn.Core;

namespace Arsenal.Service.Casino
{
    public class BonusViewFactory : IViewerFactory<BonusView>
    {
        private string _viewerSql;
        private string _splitOn;
        private DbSchema _dbSchema;

        public BonusViewFactory()
        {
            _viewerSql = @"SELECT ISNULL(mr.MatchGuid, sc.MatchGuid) AS MatchGuid, ISNULL(mr.UserID, sc.UserID) AS UserID, ISNULL(mr.UserName, sc.UserName) AS UserName, 
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

            _splitOn = "MatchGuid, HomeTeamGuid, AwayTeamGuid, LeagueGuid";

            _dbSchema = Repository.GetTableAttr<BonusView>();
        }

        public BonusView Single(Criteria criteria)
        {
            var sql = $"SELECT * FROM ({_viewerSql}) AS {_dbSchema.Name} ";

            var dapper = new DapperHelper();

            if (!string.IsNullOrEmpty(criteria?.WhereClause))
            {
                sql += " " + criteria.WhereClause;
            }

            return dapper.Query<BonusView, HomeTeam, AwayTeam, League, BonusView>(sql,
                (x, h, a, l) =>
                {
                    x.Home = h;
                    x.Away = a;
                    x.League = l;

                    return x;
                }, criteria?.Parameters, _splitOn).FirstOrDefault();
        }

        public List<BonusView> All()
        {
            var sql = $"SELECT * FROM ({_viewerSql}) AS {_dbSchema.Name} ORDER BY {_dbSchema.Sort}";

            var dapper = new DapperHelper();

            return dapper.Query<BonusView, HomeTeam, AwayTeam, League, BonusView>(sql,
                (x, h, a, l) =>
                {
                    x.Home = h;
                    x.Away = a;
                    x.League = l;

                    return x;
                }, null, _splitOn).ToList();
        }

        public List<BonusView> All(IPager page, string orderBy = null)
        {
            throw new NotImplementedException();
        }

        public List<BonusView> Query(Criteria criteria)
        {
            var sql = $"SELECT * FROM ({_viewerSql}) AS {_dbSchema.Name} ";

            var dapper = new DapperHelper();

            if (!string.IsNullOrEmpty(criteria?.WhereClause))
            {
                sql += " WHERE " + criteria.WhereClause;
                sql += " ORDER BY " + (!string.IsNullOrEmpty(criteria.OrderClause)
                    ? criteria.OrderClause : _dbSchema.Sort);
            }

            return dapper.Query<BonusView, HomeTeam, AwayTeam, League, BonusView>(sql,
                (x, h, a, l) =>
                {
                    x.Home = h;
                    x.Away = a;
                    x.League = l;

                    return x;
                }, criteria?.Parameters, _splitOn).ToList();
        }
    }
}

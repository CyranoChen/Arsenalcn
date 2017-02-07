using System;
using System.Collections.Generic;
using System.Linq;
using Arsenalcn.Core;

namespace Arsenal.Service.Casino
{
    public class BetViewFactory : IViewerFactory<BetView>
    {
        private string _viewerSql;
        private string _splitOn;
        private DbSchema _dbSchema;

        public BetViewFactory()
        {
            _viewerSql = @"SELECT b.ID, b.UserID, b.UserName, b.BetAmount, b.BetTime, b.BetRate, b.IsWin, b.Earning, b.EarningDesc, 
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

            _splitOn = "ID, CasinoItemGuid, MatchGuid, HomeTeamGuid, AwayTeamGuid, LeagueGuid";

            _dbSchema = Repository.GetTableAttr<BetView>();
        }

        public BetView Single(object key)
        {
            string sql = $"SELECT * FROM ({_viewerSql}) AS {_dbSchema.Name} WHERE {_dbSchema.Key} = @key";

            var dapper = new DapperHelper();

            return dapper.Query<BetView, CasinoItem, Match, HomeTeam, AwayTeam, League, BetView>(sql,
                        (x, c, m, h, a, l) =>
                        {
                            x.CasinoItem = c;
                            x.Match = m;
                            x.Home = h;
                            x.Away = a;
                            x.League = l;

                            return x;
                        }, new { key }, _splitOn).FirstOrDefault();
        }

        public BetView Single(Criteria criteria)
        {
            var sql = $"SELECT * FROM ({_viewerSql}) AS {_dbSchema.Name} ";

            var dapper = new DapperHelper();

            if (!string.IsNullOrEmpty(criteria?.WhereClause))
            {
                sql += " " + criteria.WhereClause;
            }

            return dapper.Query<BetView, CasinoItem, Match, HomeTeam, AwayTeam, League, BetView>(sql,
                (x, c, m, h, a, l) =>
                {
                    x.CasinoItem = c;
                    x.Match = m;
                    x.Home = h;
                    x.Away = a;
                    x.League = l;

                    return x;
                }, criteria?.Parameters, _splitOn).FirstOrDefault();
        }

        public List<BetView> All()
        {
            var sql = $"SELECT * FROM ({_viewerSql}) AS {_dbSchema.Name} ORDER BY {_dbSchema.Sort}";

            var dapper = new DapperHelper();

            return dapper.Query<BetView, CasinoItem, Match, HomeTeam, AwayTeam, League, BetView>(sql,
                (x, c, m, h, a, l) =>
                {
                    x.CasinoItem = c;
                    x.Match = m;
                    x.Home = h;
                    x.Away = a;
                    x.League = l;

                    return x;
                }, null, _splitOn).ToList();
        }

        public List<BetView> All(IPager page, string orderBy = null)
        {
            throw new NotImplementedException();
        }

        public List<BetView> Query(Criteria criteria)
        {
            var sql = $"SELECT * FROM ({_viewerSql}) AS {_dbSchema.Name} ";

            var dapper = new DapperHelper();

            if (!string.IsNullOrEmpty(criteria?.WhereClause))
            {
                sql += " WHERE " + criteria.WhereClause;
                sql += " ORDER BY " + (!string.IsNullOrEmpty(criteria.OrderClause)
                    ? criteria.OrderClause : _dbSchema.Sort);
            }

            return dapper.Query<BetView, CasinoItem, Match, HomeTeam, AwayTeam, League, BetView>(sql,
                (x, c, m, h, a, l) =>
                {
                    x.CasinoItem = c;
                    x.Match = m;
                    x.Home = h;
                    x.Away = a;
                    x.League = l;

                    return x;
                }, criteria?.Parameters, _splitOn).ToList();
        }
    }
}

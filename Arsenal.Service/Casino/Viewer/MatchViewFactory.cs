using System;
using System.Collections.Generic;
using System.Linq;
using Arsenalcn.Core;

namespace Arsenal.Service.Casino
{
    public class MatchViewFactory : IViewerFactory<MatchView>
    {
        private string _viewerSql;
        private string _splitOn;
        private DbSchema _dbSchema;

        public MatchViewFactory()
        {
            _viewerSql = @"SELECT m.MatchGuid AS ID, m.ResultHome, m.ResultAway, m.PlayTime, m.LeagueName, m.Round, 
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

            _splitOn = "ID, CasinoItemGuid, HomeTeamGuid, AwayTeamGuid, GroupGuid, LeagueGuid";

            _dbSchema = Repository.GetTableAttr<MatchView>();
        }

        public MatchView Single(object key)
        {
            string sql = $"SELECT * FROM ({_viewerSql}) AS {_dbSchema.Name} WHERE {_dbSchema.Key} = @key";

            var dapper = new DapperHelper();

            return dapper.Query<MatchView, CasinoItem, HomeTeam, AwayTeam, Group, League, MatchView>(sql,
                        (x, c, h, a, g, l) =>
                        {
                            x.CasinoItem = c;
                            x.Home = h;
                            x.Away = a;
                            x.Group = g;
                            x.League = l;

                            return x;
                        }, new { key }, _splitOn).FirstOrDefault();
        }

        public MatchView Single(Criteria criteria)
        {
            var sql = $"SELECT * FROM ({_viewerSql}) AS {_dbSchema.Name} ";

            var dapper = new DapperHelper();

            if (!string.IsNullOrEmpty(criteria?.WhereClause))
            {
                sql += " " + criteria.WhereClause;
            }

            return dapper.Query<MatchView, CasinoItem, HomeTeam, AwayTeam, Group, League, MatchView>(sql,
                        (x, c, h, a, g, l) =>
                        {
                            x.CasinoItem = c;
                            x.Home = h;
                            x.Away = a;
                            x.Group = g;
                            x.League = l;

                            return x;
                        }, criteria?.Parameters, _splitOn).FirstOrDefault();
        }

        public List<MatchView> All()
        {
            var sql = $"SELECT * FROM ({_viewerSql}) AS {_dbSchema.Name} ORDER BY {_dbSchema.Sort}";

            var dapper = new DapperHelper();

            return dapper.Query<MatchView, CasinoItem, HomeTeam, AwayTeam, Group, League, MatchView>(sql,
                        (x, c, h, a, g, l) =>
                        {
                            x.CasinoItem = c;
                            x.Home = h;
                            x.Away = a;
                            x.Group = g;
                            x.League = l;

                            return x;
                        }, null, _splitOn).ToList();
        }

        public List<MatchView> All(IPager page, string orderBy = null)
        {
            throw new NotImplementedException();
        }

        public List<MatchView> Query(Criteria criteria)
        {
            var sql = $"SELECT * FROM ({_viewerSql}) AS {_dbSchema.Name} ";

            var dapper = new DapperHelper();

            if (!string.IsNullOrEmpty(criteria?.WhereClause))
            {
                sql += " WHERE " + criteria.WhereClause;
                sql += " ORDER BY " + (!string.IsNullOrEmpty(criteria.OrderClause)
                    ? criteria.OrderClause : _dbSchema.Sort);
            }

            return dapper.Query<MatchView, CasinoItem, HomeTeam, AwayTeam, Group, League, MatchView>(sql,
                        (x, c, h, a, g, l) =>
                        {
                            x.CasinoItem = c;
                            x.Home = h;
                            x.Away = a;
                            x.Group = g;
                            x.League = l;

                            return x;
                        }, criteria?.Parameters, _splitOn).ToList();
        }
    }
}

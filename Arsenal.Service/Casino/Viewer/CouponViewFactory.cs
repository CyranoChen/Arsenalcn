using System;
using System.Collections.Generic;
using System.Linq;
using Arsenalcn.Core;

namespace Arsenal.Service.Casino
{
    public class CouponViewFactory : IViewerFactory<CouponView>
    {
        private string _viewerSql;
        private string _splitOn;
        private DbSchema _dbSchema;

        public CouponViewFactory()
        {
            _viewerSql = @"SELECT b.ID, b.UserID, b.UserName, m.MatchGuid, bdh.DetailValue AS BetResultHome, bda.DetailValue AS BetResultAway, m.PlayTime, m.Round, 
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

            _splitOn = "ID, HomeTeamGuid, AwayTeamGuid, LeagueGuid";

            _dbSchema = Repository.GetTableAttr<CouponView>();
        }

        public CouponView Single(Criteria criteria)
        {
            var sql = $"SELECT * FROM ({_viewerSql}) AS {_dbSchema.Name} ";

            var dapper = new DapperHelper();

            if (!string.IsNullOrEmpty(criteria?.WhereClause))
            {
                sql += " " + criteria.WhereClause;
            }

            return dapper.Query<CouponView, HomeTeam, AwayTeam, League, CouponView>(sql,
                (x, h, a, l) =>
                {
                    x.Home = h;
                    x.Away = a;
                    x.League = l;

                    return x;
                }, criteria?.Parameters, _splitOn).FirstOrDefault();
        }

        public List<CouponView> All()
        {
            var sql = $"SELECT * FROM ({_viewerSql}) AS {_dbSchema.Name} ORDER BY {_dbSchema.Sort}";

            var dapper = new DapperHelper();

            return dapper.Query<CouponView, HomeTeam, AwayTeam, League, CouponView>(sql,
                (x, h, a, l) =>
                {
                    x.Home = h;
                    x.Away = a;
                    x.League = l;

                    return x;
                }, null, _splitOn).ToList();
        }

        public List<CouponView> All(IPager page, string orderBy = null)
        {
            throw new NotImplementedException();
        }

        public List<CouponView> Query(Criteria criteria)
        {
            var sql = $"SELECT * FROM ({_viewerSql}) AS {_dbSchema.Name} ";

            var dapper = new DapperHelper();

            if (!string.IsNullOrEmpty(criteria?.WhereClause))
            {
                sql += " WHERE " + criteria.WhereClause;
                sql += " ORDER BY " + (!string.IsNullOrEmpty(criteria.OrderClause)
                    ? criteria.OrderClause : _dbSchema.Sort);
            }

            return dapper.Query<CouponView, HomeTeam, AwayTeam, League, CouponView>(sql,
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

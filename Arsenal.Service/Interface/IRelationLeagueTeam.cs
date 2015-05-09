using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Arsenal.Service.Interface
{
    public interface IRelationLeagueTeam
    {
        void Single();
        bool Any();

        IQueryable<RelationLeagueTeam> All();
        IQueryable<RelationLeagueTeam> Query(Expression<Func<RelationLeagueTeam, bool>> predicate);

        void Create(SqlTransaction trans = null);
        void Create(IEnumerable<RelationLeagueTeam> instances, SqlTransaction trans = null);

        //void Update(SqlTransaction trans = null);
        //void Update(IEnumerable<RelationLeagueTeam> instances, SqlTransaction trans = null);

        void Delete(SqlTransaction trans = null);
        void Delete(Expression<Func<RelationLeagueTeam, bool>> predicate, SqlTransaction trans = null);

        void Clean(SqlTransaction trans = null);
    }
}

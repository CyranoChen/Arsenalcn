using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace Arsenal.Service
{
    public interface IRelationLeagueTeam
    {
        void Single();
        bool Any();

        //IQueryable<RelationLeagueTeam> All();
        IQueryable<RelationLeagueTeam> QueryByLeagueGuid();
        IQueryable<RelationLeagueTeam> QueryByTeamGuid();

        void Create(SqlTransaction trans = null);
        void Create(IEnumerable<RelationLeagueTeam> instances, SqlTransaction trans = null);

        //void Update(SqlTransaction trans = null);
        //void Update(IEnumerable<RelationLeagueTeam> instances, SqlTransaction trans = null);

        void Delete(SqlTransaction trans = null);
        void Delete(IEnumerable<RelationLeagueTeam> instances, SqlTransaction trans = null);
        //void Delete(Expression<Func<RelationLeagueTeam, bool>> predicate, SqlTransaction trans = null);

        void Clean(SqlTransaction trans = null);
    }
}

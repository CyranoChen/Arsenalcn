using System;
using System.Data.SqlClient;
using System.Linq;

namespace Arsenalcn.Core
{
    public interface IConfig
    {
        void Single();
        bool Any();

        IQueryable<Config> All();
        IQueryable<Config> All(ConfigSystem cs);
        //IQueryable<Config> Query(Expression<Func<Config, bool>> predicate);

        //void Create(SqlTransaction trans = null);
        //void Create(IEnumerable<Config> instances, SqlTransaction trans = null);

        void Update(SqlTransaction trans = null);
        //void Update(IEnumerable<Config> instances, SqlTransaction trans = null);

        //void Delete(SqlTransaction trans = null);
        //void Delete(Expression<Func<Config, bool>> predicate, SqlTransaction trans = null);
    }
}

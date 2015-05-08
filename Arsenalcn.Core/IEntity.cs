using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Arsenalcn.Core
{
    public interface IEntity
    {
        T Single<T>(object key) where T : class;
        //T Single<T>(Expression<Func<T, bool>> predicate, params string[] includePaths) where T : class;

        List<T> All<T>();

        //IQueryable<T> All<T>(params string[] includePaths);
        //IQueryable<T> Query<T>(Expression<Func<T, bool>> predicate, params string[] includePaths) where T : class;

        void Create<T>(T instance, SqlTransaction trans = null) where T : class;
        void Create<T>(IEnumerable<T> instances, SqlTransaction trans = null) where T : class;

        void Update<T>(T instance, SqlTransaction trans = null) where T : class;

        void Delete<T>(object key, SqlTransaction trans = null) where T : class;
        void Delete<T>(T instance, SqlTransaction trans = null) where T : class;
        //void Delete<T>(Expression<Func<T, bool>> predicate) where T : class;
    }
}

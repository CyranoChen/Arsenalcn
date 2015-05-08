using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace Arsenalcn.Core
{
    public interface IEntity
    {
        T Single<T>(object key) where T : class;
        T Single<T>(Expression<Func<T, bool>> predicate) where T : class;

        IQueryable<T> All<T>() where T : class;
        IQueryable<T> Query<T>(Expression<Func<T, bool>> predicate) where T : class;

        void Create<T>(T instance, SqlTransaction trans = null) where T : class;
        void Create<T>(IEnumerable<T> instances, SqlTransaction trans = null) where T : class;

        void Update<T>(T instance, SqlTransaction trans = null) where T : class;
        void Update<T>(IEnumerable<T> instances, SqlTransaction trans = null) where T : class;

        void Delete<T>(T instance, SqlTransaction trans = null) where T : class;
        void Delete<T>(Expression<Func<T, bool>> predicate, SqlTransaction trans = null) where T : class;
    }
}

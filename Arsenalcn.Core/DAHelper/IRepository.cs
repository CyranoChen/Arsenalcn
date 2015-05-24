using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace Arsenalcn.Core
{
    public interface IRepository
    {
        T Single<T>(object key) where T : class, IEntity;

        List<T> All<T>() where T : class, IEntity;
        List<T> Query<T>(Hashtable htWhere) where T : class, IEntity;
        IQueryable<T> Query<T>(Expression<Func<T, bool>> predicate) where T : class, IEntity;

        void Insert<T>(T instance, SqlTransaction trans = null) where T : class, IEntity;
        void Insert<T>(T instance, out object key, SqlTransaction trans = null) where T : class, IEntity;
        void Insert<T>(IEnumerable<T> instances, SqlTransaction trans = null) where T : class, IEntity;

        void Update<T>(T instance, SqlTransaction trans = null) where T : class, IEntity;
        void Update<T>(IEnumerable<T> instances, SqlTransaction trans = null) where T : class, IEntity;

        void Delete<T>(object key, SqlTransaction trans = null) where T : class, IEntity;
        void Delete<T>(T instance, SqlTransaction trans = null) where T : class, IEntity;
        void Delete<T>(Expression<Func<T, bool>> predicate, out int count, SqlTransaction trans = null) where T : class, IEntity;
    }
}
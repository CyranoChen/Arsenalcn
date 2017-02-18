using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace Arsenalcn.Core
{
    public interface IRepository
    {
        T Single<T>(object key) where T : class, IEntity, new();
        T Single<T>(Expression<Func<T, bool>> whereBy) where T : class, IDao, new();

        int Count<T>(Expression<Func<T, bool>> whereBy) where T : class, IDao;

        bool Any<T>(object key) where T : class, IEntity;
        bool Any<T>(Expression<Func<T, bool>> whereBy) where T : class, IDao;

        List<T> All<T>() where T : class, IDao, new();
        List<T> All<T>(IPager pager, string orderBy = null) where T : class, IDao, new();

        List<T> Query<T>(Expression<Func<T, bool>> whereBy) where T : class, IDao, new();
        List<T> Query<T>(IPager pager, Expression<Func<T, bool>> whereBy, string orderBy = null) where T : class, IDao, new();
        List<T> Query<T>(Criteria criteria) where T : class, IDao, new();

        int Insert<T>(T instance, SqlTransaction trans = null) where T : class, IDao;
        int Insert<T>(T instance, out object key, SqlTransaction trans = null) where T : class, IEntity;

        int Update<T>(T instance, SqlTransaction trans = null) where T : class, IEntity;
        int Update<T>(T instance, Expression<Func<T, bool>> whereBy, SqlTransaction trans = null) where T : class, IDao;

        int Save<T>(T instance, SqlTransaction trans = null) where T : class, IEntity;
        int Save<T>(T instance, Expression<Func<T, bool>> whereBy, SqlTransaction trans = null) where T : class, IDao;

        int Delete<T>(object key, SqlTransaction trans = null) where T : class, IEntity;
        int Delete<T>(T instance, SqlTransaction trans = null) where T : class, IEntity;
        int Delete<T>(Expression<Func<T, bool>> whereBy, SqlTransaction trans = null) where T : class, IDao;
    }
}
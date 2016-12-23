﻿using System;
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

        void Insert<T>(T instance, SqlTransaction trans = null) where T : class, IDao;
        void Insert<T>(T instance, out object key, SqlTransaction trans = null) where T : class, IEntity;

        void Update<T>(T instance, SqlTransaction trans = null) where T : class, IEntity;
        void Update<T>(T instance, Expression<Func<T, bool>> whereBy, SqlTransaction trans = null) where T : class, IDao;

        void Save<T>(T instance, SqlTransaction trans = null) where T : class, IEntity;
        void Save<T>(T instance, Expression<Func<T, bool>> whereBy, SqlTransaction trans = null) where T : class, IDao;

        void Delete<T>(object key, SqlTransaction trans = null) where T : class, IEntity;
        void Delete<T>(T instance, SqlTransaction trans = null) where T : class, IEntity;
        void Delete<T>(Expression<Func<T, bool>> whereBy, SqlTransaction trans = null) where T : class, IDao;
    }
}
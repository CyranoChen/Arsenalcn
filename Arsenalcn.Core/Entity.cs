using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Arsenalcn.Core
{
    public class Entity : IEntity
    {
        private readonly IRepository repo;
        public Entity()
        {
            repo = new Repository();
        }

        public T Single<T>(object key) where T : class
        {
            Contract.Requires(key != null);

            DataRow dr = repo.Select<T>(key);

            if (dr != null)
            {
                ConstructorInfo ci = typeof(T).GetConstructor(new Type[] { typeof(DataRow) });
                Object[] para = new Object[] { dr };

                return (T)ci.Invoke(para);
            }
            else
            { return null; }
        }

        public T Single<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            Contract.Requires(predicate != null);

            return All<T>().SingleOrDefault(predicate);
        }

        public IQueryable<T> All<T>() where T : class
        {
            DataTable dt = repo.Select<T>();

            var list = new List<T>();

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ConstructorInfo ci = typeof(T).GetConstructor(new Type[] { typeof(DataRow) });
                    Object[] para = new Object[] { dr };

                    list.Add((T)ci.Invoke(para));
                }
            }

            return list.AsQueryable();
        }

        public IQueryable<T> Query<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            Contract.Requires(predicate != null);

            return All<T>().Where(predicate);
        }

        public void Create<T>(T instance, SqlTransaction trans = null) where T : class
        {
            Contract.Requires(instance != null);

            repo.Insert<T>(instance, trans);
        }

        public void Create<T>(IEnumerable<T> instances, SqlTransaction trans = null) where T : class
        {
            Contract.Requires(instances != null);

            foreach (var instance in instances)
            {
                Create(instance, trans);
            }
        }

        public void Update<T>(T instance, SqlTransaction trans = null) where T : class
        {
            Contract.Requires(instance != null);

            repo.Update<T>(instance, trans);
        }

        public void Update<T>(IEnumerable<T> instances, SqlTransaction trans = null) where T : class
        {
            Contract.Requires(instances != null);

            foreach (var instance in instances)
            {
                Update(instance, trans);
            }
        }

        public void Delete<T>(T instance, SqlTransaction trans = null) where T : class
        {
            Contract.Requires(instance != null);

            repo.Delete<T>(instance, trans);
        }

        public void Delete<T>(Expression<Func<T, bool>> predicate, SqlTransaction trans = null) where T : class
        {
            Contract.Requires(predicate != null);

            repo.Delete<T>(Single(predicate), trans);
        }

        public static class Cache
        {
            static Cache()
            {
                InitCache();
            }

            public static void RefreshCache()
            {
                InitCache();
            }

            private static void InitCache()
            {
                IEntity entity = new Entity();
                CacheList = entity.All<Entity>().ToList();
            }

            public static Entity Load(Expression<Func<Entity, bool>> predicate)
            {
                return CacheList.AsQueryable().SingleOrDefault(predicate);
            }

            public static List<Entity> CacheList;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Linq;

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

        //public T Single<T>(Expression<Func<T, bool>> predicate, params string[] includePaths) where T : class
        //{
        //    Contract.Requires(predicate != null);

        //    var instance = GetSetWithIncludedPaths<T>(includePaths).SingleOrDefault(predicate);
        //    return instance;
        //}

        public List<T> All<T>()
        {
            DataTable dt = repo.Select<T>();

            List<T> list = new List<T>();

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ConstructorInfo ci = typeof(T).GetConstructor(new Type[] { typeof(DataRow) });
                    Object[] para = new Object[] { dr };

                    list.Add((T)ci.Invoke(para));
                }
            }

            return list;
        }

        //public IQueryable<T> All<T>(params string[] includePaths) where T : class
        //{
        //    return Query<T>(x => true, includePaths);
        //}

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

        public void Delete<T>(object key, SqlTransaction trans = null) where T : class
        {
            Contract.Requires(key != null);

            repo.Delete<T>(key, trans);
        }

        public void Delete<T>(T instance, SqlTransaction trans = null) where T : class
        {
            Contract.Requires(instance != null);

            repo.Delete<T>(instance, trans);
        }

        //public void Delete<TModel>(Expression<Func<TModel, bool>> predicate)
        //    where TModel : class, IEntity
        //{
        //    Contract.Requires(predicate != null);

        //    TModel entity = Single(predicate);
        //    Delete(entity);
        //}

        //public IQueryable<TModel> Query<TModel>(Expression<Func<TModel, bool>> predicate, params string[] includePaths)
        //    where TModel : class, IEntity
        //{
        //    Contract.Requires(predicate != null);

        //    var items = GetSetWithIncludedPaths<TModel>(includePaths);

        //    if (predicate != null)
        //        return items.Where(predicate);

        //    return items;
        //}


        //private DbQuery<TModel> GetSetWithIncludedPaths<TModel>(IEnumerable<string> includedPaths) where TModel : class
        //{
        //    DbQuery<TModel> items = _context.Set<TModel>();

        //    foreach (var path in includedPaths ?? Enumerable.Empty<string>())
        //    {
        //        items = items.Include(path);
        //    }

        //    return items;
        //}
    }
}

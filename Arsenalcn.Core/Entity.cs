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
    public abstract class Entity : IEntity
    {
        private readonly IRepository repo;
        public Entity()
        {
            repo = new Repository();
        }

        protected Entity(DataRow dr)
        {
            Contract.Requires(dr != null);

            foreach (var pi in this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var attrCol = GetColumnAttr(pi);
                var type = Nullable.GetUnderlyingType(pi.PropertyType) ?? pi.PropertyType;

                if (attrCol != null)
                {
                    if (!Convert.IsDBNull(dr[attrCol.Name]))
                    {
                        // SetValue for EnumType
                        if (type.BaseType.Equals(typeof(Enum)))
                        {
                            object value = Enum.Parse(type, dr[attrCol.Name].ToString(), true);

                            pi.SetValue(this, Convert.ChangeType(value, type), null);
                        }
                        else
                        {
                            pi.SetValue(this, Convert.ChangeType(dr[attrCol.Name], type), null);
                        }
                    }
                }
                else
                { continue; }
            }
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

            var attr = (AttrDbTable)Attribute.GetCustomAttribute(typeof(T), typeof(AttrDbTable));

            var key = instance.GetType().GetProperty(attr.Key.ToString()).GetValue(instance, null);

            repo.Delete<T>(key, trans);
        }

        public void Delete<T>(Expression<Func<T, bool>> predicate, SqlTransaction trans = null) where T : class
        {
            Contract.Requires(predicate != null);

            var instances = Query(predicate);

            foreach (var instance in instances)
            {
                Delete<T>(instance, trans);
            }
        }

        public static AttrDbTable GetTableAttr<T>()
        {
            return (AttrDbTable)Attribute.GetCustomAttribute(typeof(T), typeof(AttrDbTable));
        }

        public static AttrDbColumn GetColumnAttr(PropertyInfo pi)
        {
            return (AttrDbColumn)Attribute.GetCustomAttribute(pi, typeof(AttrDbColumn));
        }

        public static AttrDbColumn GetColumnAttr<T>(string name)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));

            return GetColumnAttr(typeof(T).GetProperty(name));
        }

        public static IEnumerable<T> DistinctBy<T, TKey>(IEnumerable<T> instances, Func<T, TKey> keySelector)
        {
            Contract.Requires(instances != null);

            HashSet<TKey> seenKeys = new HashSet<TKey>();

            foreach (T instance in instances)
            {
                if (seenKeys.Add(keySelector(instance)))
                {
                    yield return instance;
                }
            }
        }

        public static IEnumerable<TKey> DistinctOrderBy<T, TKey>(IEnumerable<T> instances, Func<T, TKey> keySelector)
        {
            return DistinctBy(instances, keySelector).OrderBy(keySelector).Select(keySelector);
        }

        //protected static class Cache
        //{
        //    static Cache()
        //    {
        //        InitCache();
        //    }

        //    public static void RefreshCach()
        //    {
        //        InitCache();
        //    }

        //    private static void InitCache()
        //    {
        //        IEntity entity = new Entity();
        //        CacheList = entity.All<Entity>().ToList();
        //    }

        //    public static Entity Load(Expression<Func<Entity, bool>> predicate)
        //    {
        //        return CacheList.AsQueryable().SingleOrDefault(predicate);
        //    }

        //    public static List<Entity> CacheList;
        //}
    }
}

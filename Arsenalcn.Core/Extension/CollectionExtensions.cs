using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Arsenalcn.Core
{
    public static class CollectionExtensions
    {
        public static IEnumerable<TOne> Many<TOne, TMany>(this IEnumerable<TOne> source, Func<TOne, TMany, bool> func)
            where TOne : class, IViewer, new()
            where TMany : class, IViewer, new()
        {
            Contract.Requires(func != null);

            // Get the property which matches IEnumerable<TMany>
            var property = typeof(TOne).GetProperties()
                .FirstOrDefault(x => (Nullable.GetUnderlyingType(x.PropertyType) ?? x.PropertyType) == typeof(IEnumerable<TMany>));

            if (source != null && source.Any() && property != null)
            {
                //var propertyName = string.Format("List{0}", typeof(TMany).Name);
                //var property = typeof(TOne).GetProperty(propertyName, typeof(IEnumerable<TMany>));

                var attrCol = Repository.GetColumnAttr(property);

                if (attrCol != null && !string.IsNullOrEmpty(attrCol.ForeignKey))
                {
                    IRepository repo = new Repository();

                    var list = repo.All<TMany>();

                    #region Package each property Ts in TSource
                    if (list != null && list.Count > 0)
                    {
                        foreach (var instance in source)
                        {
                            var pi = instance.GetType().GetProperty(property.Name, typeof(IEnumerable<TMany>));
                            if (pi == null) { continue; }

                            var predicate = new Predicate<TMany>(t => func(instance, t));

                            var result = list.FindAll(predicate);

                            if (result.Count > 0)
                            {
                                pi.SetValue(instance, result, null);
                            }
                        }
                    }
                    #endregion
                }
            }

            return source;
        }

        public static IEnumerable<TOne> Many<TOne, TMany, TOneKey>(this IEnumerable<TOne> source, Func<TOne, TOneKey> keySelector)
            where TMany : class, IViewer, new()
            where TOne : class, IViewer, new()
            where TOneKey : struct
        {
            Contract.Requires(keySelector != null);

            // Get the property which matches IEnumerable<TMany>
            var property = typeof(TOne).GetProperties()
                .FirstOrDefault(x => (Nullable.GetUnderlyingType(x.PropertyType) ?? x.PropertyType) == typeof(IEnumerable<TMany>));

            if (source != null && source.Any() && property != null)
            {
                //var propertyName = string.Format("List{0}", typeof(TMany).Name);
                //var property = typeof(TOne).GetProperty(propertyName, typeof(IEnumerable<TMany>));

                var attr = Repository.GetTableAttr<TMany>();
                var attrCol = Repository.GetColumnAttr(property);

                if (attr != null && !string.IsNullOrEmpty(attrCol?.ForeignKey))
                {
                    var list = new List<TMany>();

                    #region Get All T instances where ForeignKey in T.PrimaryKeys
                    var keys = source.Select(keySelector).ToArray();
                    var names = new List<string>();
                    var paras = new List<SqlParameter>();

                    for (var i = 0; i < keys.Length; i++)
                    {
                        names.Add("@" + i);
                        paras.Add(new SqlParameter("@" + i, keys[i]));
                    }

                    var sql = $"SELECT * FROM {attr.Name} WHERE {attrCol.ForeignKey} IN ({string.Join(", ", names)})";

                    var ds = DataAccess.ExecuteDataset(sql, paras.ToArray());

                    var dt = ds.Tables[0];

                    if (dt.Rows.Count > 0)
                    {
                        using (var reader = dt.CreateDataReader())
                        {
                            list = reader.DataReaderMapTo<TMany>().ToList();
                        }
                    }
                    #endregion

                    #region Package each property Ts in TSource
                    if (list.Count > 0)
                    {
                        foreach (var instance in source)
                        {
                            var pi = instance.GetType().GetProperty(property.Name, typeof(IEnumerable<TMany>));
                            if (pi == null) { continue; }

                            var fKey = typeof(TMany).GetProperty(attrCol.ForeignKey);
                            if (fKey == null) { break; }

                            var keyValue = keySelector(instance);

                            var predicate = new Predicate<TMany>(many =>
                                fKey.GetValue(many, null).Equals(keyValue));

                            var result = list.FindAll(predicate);

                            if (result.Count > 0)
                            {
                                pi.SetValue(instance, result, null);
                            }
                        }
                    }
                    #endregion
                }
            }

            return source;
        }


        // Load All Records
        public static IEnumerable<T> Page<T>(this IEnumerable<T> source, int pageIndex, int pageSize)
        {
            Contract.Requires(pageIndex >= 0);
            Contract.Requires(pageSize >= 0);

            var skip = pageIndex * pageSize;

            if (skip > 0)
                source = source.Skip(skip);

            source = source.Take(pageSize);

            return source;
        }

        //// Load on Demand
        //public static IQueryable<T> Page<T>(this IQueryable<T> source, int pageIndex, int pageSize)
        //{
        //    Contract.Requires(pageIndex >= 0);
        //    Contract.Requires(pageSize >= 0);

        //    int skip = pageIndex * pageSize;

        //    if (skip > 0)
        //        source = source.Skip(skip);

        //    source = source.Take(pageSize);

        //    return source;
        //}

        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
        {
            Contract.Requires(source != null);

            var seenKeys = new HashSet<TKey>();

            foreach (var instance in source)
            {
                if (seenKeys.Add(keySelector(instance)))
                {
                    yield return instance;
                }
            }
        }

        public static IEnumerable<TKey> DistinctOrderBy<T, TKey>(this IEnumerable<T> instances, Func<T, TKey> keySelector)
        {
            return instances.DistinctBy(keySelector).OrderBy(keySelector).Select(keySelector);
        }
    }
}

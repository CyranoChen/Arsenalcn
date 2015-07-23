using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;

namespace Arsenalcn.Core
{
    public static class CollectionExtensions
    {
        public static IEnumerable<TSource> Many<TSource, T>(this IEnumerable<TSource> source, Func<TSource, T, bool> func)
            where T : class, IViewer, new()
            where TSource : class, IViewer, new()
        {
            var propertyName = string.Format("List{0}", typeof(T).Name);
            var property = typeof(TSource).GetProperty(propertyName, typeof(IEnumerable<T>));

            var attrCol = Repository.GetColumnAttr(property);

            if (attrCol != null && !string.IsNullOrEmpty(attrCol.ForeignKey))
            {
                IRepository repo = new Repository();

                // Get All T instances
                var query = repo.All<T>();

                if (query != null && query.Count > 0)
                {
                    foreach (var instance in source)
                    {
                        var pi = instance.GetType().GetProperty(propertyName, typeof(IEnumerable<T>));
                        if (pi == null) { continue; }

                        var predicate = new Predicate<T>(t => func(instance, t));

                        var list = query.FindAll(predicate);

                        if (list != null && list.Count > 0)
                        {
                            pi.SetValue(instance, list, null);
                        }
                    }
                }
            }

            return source;
        }

        // Load All Records
        public static IEnumerable<T> Page<T>(this IEnumerable<T> source, int pageIndex, int pageSize)
        {
            Contract.Requires(pageIndex >= 0);
            Contract.Requires(pageSize >= 0);

            int skip = pageIndex * pageSize;

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

            HashSet<TKey> seenKeys = new HashSet<TKey>();

            foreach (T instance in source)
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

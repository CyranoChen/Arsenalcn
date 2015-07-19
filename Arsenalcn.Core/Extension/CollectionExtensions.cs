using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Arsenalcn.Core
{
    public static class CollectionExtensions
    {
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

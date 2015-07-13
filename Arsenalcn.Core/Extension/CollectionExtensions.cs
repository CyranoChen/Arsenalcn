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
            Contract.Requires(pageIndex >= 0, "Page index cannot be negative");
            Contract.Requires(pageSize >= 0, "Page size cannot be negative");

            int skip = pageIndex * pageSize;

            if (skip > 0)
                source = source.Skip(skip);

            source = source.Take(pageSize);

            return source;
        }

        // Load on Demand
        public static IQueryable<T> Page<T>(this IQueryable<T> source, int pageIndex, int pageSize)
        {
            Contract.Requires(pageIndex >= 0, "Page index cannot be negative");
            Contract.Requires(pageSize >= 0, "Page size cannot be negative");

            int skip = pageIndex * pageSize;

            if (skip > 0)
                source = source.Skip(skip);

            source = source.Take(pageSize);

            return source;
        }
    }
}

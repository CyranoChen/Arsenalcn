using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Arsenalcn.Core
{
    public static class RepositoryExtensions
    {
        public static int Insert<T>(this IEnumerable<T> source, SqlTransaction trans = null) where T : class, IEntity
        {
            Contract.Requires(source != null);

            IRepository repo = new Repository();

            foreach (var instance in source)
            {
                repo.Insert(instance, trans);
            }

            return source.Count();
        }


        public static int Update<T>(this IEnumerable<T> source, SqlTransaction trans = null) where T : class, IEntity
        {
            Contract.Requires(source != null);

            IRepository repo = new Repository();

            foreach (var instance in source)
            {
                repo.Update(instance, trans);
            }

            return source.Count();
        }

        public static int Delete<T>(this IEnumerable<T> source, SqlTransaction trans = null) where T : class, IEntity
        {
            Contract.Requires(source != null);

            IRepository repo = new Repository();

            foreach (var instance in source)
            {
                repo.Delete(instance, trans);
            }

            return source.Count();
        }
    }
}

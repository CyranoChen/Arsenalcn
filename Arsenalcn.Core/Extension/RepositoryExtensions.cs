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

            var list = source as IList<T> ?? source.ToList();

            foreach (var instance in list)
            {
                repo.Insert((IDao)instance, trans);
            }

            return list.Count;
        }


        public static int Update<T>(this IEnumerable<T> source, SqlTransaction trans = null) where T : class, IEntity
        {
            Contract.Requires(source != null);

            IRepository repo = new Repository();

            var list = source as IList<T> ?? source.ToList();

            foreach (var instance in list)
            {
                repo.Update(instance, trans);
            }

            return list.Count;
        }

        public static int Delete<T>(this IEnumerable<T> source, SqlTransaction trans = null) where T : class, IEntity
        {
            Contract.Requires(source != null);

            IRepository repo = new Repository();

            var list = source as IList<T> ?? source.ToList();

            foreach (var instance in list)
            {
                repo.Delete(instance, trans);
            }

            return list.Count;
        }
    }
}
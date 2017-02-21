using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Script.Serialization;
using Dapper;

namespace Arsenalcn.Core
{
    public static class RepositoryExtensions
    {
        public static string ToSqlDebugInfo(this string sql, object para = null)
        {
            var jsonSerializer = new JavaScriptSerializer();

            if (para != null)
            {
                return jsonSerializer.Serialize(new { sql, para });
            }
            else
            {
                return sql;
            }
        }

        public static object ToDapperParameters(this SqlParameter[] para)
        {
            var args = new DynamicParameters(new { });

            foreach (var p in para)
            {
                args.Add(p.ParameterName, p.Value != DBNull.Value ? p.Value : null);
            }

            return args;
        }


        public static int Insert<T>(this IEnumerable<T> source, SqlTransaction trans = null) where T : class, IEntity
        {
            var list = source as IList<T> ?? source.ToList();

            if (list.Count > 0)
            {
                IRepository repo = new Repository();

                foreach (var instance in list)
                {
                    repo.Insert((IDao)instance, trans);
                }
            }

            return list.Count;
        }

        public static int Update<T>(this IEnumerable<T> source, SqlTransaction trans = null) where T : class, IEntity
        {
            var list = source as IList<T> ?? source.ToList();

            if (list.Count > 0)
            {
                IRepository repo = new Repository();

                foreach (var instance in list)
                {
                    repo.Update(instance, trans);
                }
            }

            return list.Count;
        }

        public static int Delete<T>(this IEnumerable<T> source, SqlTransaction trans = null) where T : class, IEntity
        {
            var list = source as IList<T> ?? source.ToList();

            if (list.Count > 0)
            {
                IRepository repo = new Repository();

                foreach (var instance in list)
                {
                    repo.Delete(instance, trans);
                }
            }

            return list.Count;
        }
    }
}
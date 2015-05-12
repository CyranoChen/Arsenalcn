using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using Microsoft.ApplicationBlocks.Data;

namespace Arsenalcn.Core
{
    public class Repository : IRepository
    {
        private readonly string conn;
        public Repository()
        {
            conn = DataAccess.ConnectString;
        }

        public T Single<T>(object key) where T : class, IEntity
        {
            Contract.Requires(key != null);

            var attr = GetTableAttr<T>();

            string sql = string.Format("SELECT * FROM {0} WHERE {1} = @key", attr.Name, attr.Key);

            SqlParameter[] para = { new SqlParameter("@key", key) };

            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, para);

            if (ds.Tables[0].Rows.Count == 0) { return null; }

            ConstructorInfo ci = typeof(T).GetConstructor(new Type[] { typeof(DataRow) });

            return (T)ci.Invoke(new Object[] { ds.Tables[0].Rows[0] });
        }

        public T Single<T>(Expression<Func<T, bool>> predicate) where T : class, IEntity
        {
            Contract.Requires(predicate != null);

            return All<T>().SingleOrDefault(predicate);
        }

        public IQueryable<T> All<T>() where T : class, IEntity
        {
            var attr = GetTableAttr<T>();

            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("SELECT * FROM {0}  ", attr.Name);

            if (!string.IsNullOrEmpty(attr.Sort))
            {
                sql.AppendFormat("ORDER BY {0}", attr.Sort);
            }

            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql.ToString());

            if (ds.Tables[0].Rows.Count == 0) { return null; }

            var list = new List<T>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ConstructorInfo ci = typeof(T).GetConstructor(new Type[] { typeof(DataRow) });

                list.Add((T)ci.Invoke(new Object[] { dr }));
            }

            return list.AsQueryable();
        }

        public IQueryable<T> Query<T>(Expression<Func<T, bool>> predicate) where T : class, IEntity
        {
            Contract.Requires(predicate != null);

            return All<T>().Where(predicate);
        }

        public void Insert<T>(T instance, SqlTransaction trans = null) where T : class, IEntity
        {
            Contract.Requires(instance != null);

            List<string> listCol = new List<string>();
            List<string> listColPara = new List<string>();
            List<SqlParameter> listPara = new List<SqlParameter>();

            foreach (var pi in instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var attrCol = GetColumnAttr(pi);

                if (attrCol != null)
                {
                    object _value = pi.GetValue(instance, null);

                    listCol.Add(attrCol.Name);
                    listColPara.Add("@" + attrCol.Name);

                    SqlParameter _para = new SqlParameter("@" + attrCol.Name,
                        _value != null ? _value : (object)DBNull.Value);
                    listPara.Add(_para);
                }
                else
                { continue; }
            }

            var attr = GetTableAttr<T>();

            if (listCol.Count > 0 && listColPara.Count > 0 && listPara.Count > 0)
            {
                string sql = string.Format("INSERT INTO {0} ({1}) VALUES ({2})", attr.Name,
                    string.Join(", ", listCol.ToArray()), string.Join(", ", listColPara.ToArray()));

                var primary = instance.GetType().GetProperty("ID");

                // skip the property of the self-increase main-key
                if (!primary.PropertyType.Equals(typeof(int)))
                {
                    listCol.Add(attr.Key);
                    listColPara.Add("@key");
                    listPara.Add(new SqlParameter("@key", primary.GetValue(instance, null)));
                }

                if (trans != null)
                {
                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, listPara.ToArray());
                }
                else
                {
                    SqlHelper.ExecuteNonQuery(conn, CommandType.Text, sql, listPara.ToArray());
                }
            }
            else { throw new Exception("Unable to find any valid DB columns"); }
        }

        public void Insert<T>(IEnumerable<T> instances, SqlTransaction trans = null) where T : class, IEntity
        {
            Contract.Requires(instances != null);

            foreach (var instance in instances)
            {
                Insert<T>(instance, trans);
            }
        }

        public void Update<T>(T instance, SqlTransaction trans = null) where T : class, IEntity
        {
            Contract.Requires(instance != null);

            List<string> listCol = new List<string>();
            List<SqlParameter> listPara = new List<SqlParameter>();

            foreach (var pi in instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var attrCol = GetColumnAttr(pi);

                if (attrCol != null)
                {
                    object _value = pi.GetValue(instance, null);

                    listCol.Add(string.Format("{0} = @{0}", attrCol.Name));

                    SqlParameter _para = new SqlParameter("@" + attrCol.Name,
                        _value != null ? _value : (object)DBNull.Value);
                    listPara.Add(_para);
                }
                else
                { continue; }
            }

            var attr = GetTableAttr<T>();

            if (listCol.Count > 0 && listPara.Count > 0)
            {
                string sql = string.Format("UPDATE {0} SET {1} WHERE {2} = @key",
                    attr.Name, string.Join(", ", listCol.ToArray()), attr.Key);

                var primary = instance.GetType().GetProperty("ID");

                listPara.Add(new SqlParameter("@key", primary.GetValue(instance, null)));

                if (trans != null)
                {
                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, listPara.ToArray());
                }
                else
                {
                    SqlHelper.ExecuteNonQuery(conn, CommandType.Text, sql, listPara.ToArray());
                }
            }
            else { throw new Exception("Unable to find any valid DB columns"); }
        }

        public void Update<T>(IEnumerable<T> instances, SqlTransaction trans = null) where T : class, IEntity
        {
            Contract.Requires(instances != null);

            foreach (var instance in instances)
            {
                Update<T>(instance, trans);
            }
        }

        public void Delete<T>(object key, SqlTransaction trans = null) where T : class, IEntity
        {
            Contract.Requires(key != null);

            var attr = GetTableAttr<T>();

            string sql = string.Format("DELETE {0} WHERE {1} = @key", attr.Name, attr.Key);

            SqlParameter[] para = { new SqlParameter("@key", key) };

            if (trans != null)
            {
                SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, para);
            }
            else
            {
                SqlHelper.ExecuteNonQuery(conn, CommandType.Text, sql, para);
            }
        }

        public void Delete<T>(T instance, SqlTransaction trans = null) where T : class, IEntity
        {
            Contract.Requires(instance != null);

            var attr = (AttrDbTable)Attribute.GetCustomAttribute(typeof(T), typeof(AttrDbTable));

            var key = instance.GetType().GetProperty("ID").GetValue(instance, null);

            Delete<T>(key, trans);
        }

        public void Delete<T>(Expression<Func<T, bool>> predicate, SqlTransaction trans = null) where T : class, IEntity
        {
            Contract.Requires(predicate != null);

            var instances = Query<T>(predicate);

            foreach (var instance in instances)
            {
                Delete<T>(instance, trans);
            }
        }

        public static AttrDbTable GetTableAttr<T>() where T : class
        {
            return (AttrDbTable)Attribute.GetCustomAttribute(typeof(T), typeof(AttrDbTable));
        }

        public static AttrDbColumn GetColumnAttr(PropertyInfo pi)
        {
            return (AttrDbColumn)Attribute.GetCustomAttribute(pi, typeof(AttrDbColumn));
        }

        public static AttrDbColumn GetColumnAttr<T>(string name) where T : class
        {
            Contract.Requires(!string.IsNullOrEmpty(name));

            return GetColumnAttr(typeof(T).GetProperty(name));
        }

        public static IEnumerable<T> DistinctBy<T, TKey>(IEnumerable<T> instances, Func<T, TKey> keySelector) where T : class
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

        public static IEnumerable<TKey> DistinctOrderBy<T, TKey>(IEnumerable<T> instances, Func<T, TKey> keySelector) where T : class
        {
            return DistinctBy(instances, keySelector).OrderBy(keySelector).Select(keySelector);
        }
    }
}
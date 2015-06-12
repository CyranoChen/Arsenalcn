using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using Microsoft.ApplicationBlocks.Data;
using Arsenalcn.Core.Logger;
using System.Threading;

namespace Arsenalcn.Core
{
    public class Repository : IRepository
    {
        private readonly string conn;
        private readonly ILog log;

        public Repository()
        {
            conn = DataAccess.ConnectString;
            log = new DaoLog();
        }

        public T Single<T>(object key) where T : class, IEntity
        {
            try
            {
                Contract.Requires(key != null);

                var attr = GetTableAttr<T>();

                string sql = string.Format("SELECT * FROM {0} WHERE {1} = @key", attr.Name, attr.Key);

                SqlParameter[] para = { new SqlParameter("@key", key) };

                DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, para);

                log.Debug(sql, new LogInfo()
                {
                    MethodInstance = MethodBase.GetCurrentMethod(),
                    ThreadInstance = Thread.CurrentThread
                });

                if (ds.Tables[0].Rows.Count == 0) { return null; }

                ConstructorInfo ci = typeof(T).GetConstructor(new Type[] { typeof(DataRow) });

                return (T)ci.Invoke(new Object[] { ds.Tables[0].Rows[0] });
            }
            catch (Exception ex)
            {
                log.Debug(ex, new LogInfo()
                {
                    MethodInstance = MethodBase.GetCurrentMethod(),
                    ThreadInstance = Thread.CurrentThread
                });

                throw;
            }
        }

        public List<T> All<T>() where T : class, IEntity
        {
            try
            {
                var list = new List<T>();

                var attr = GetTableAttr<T>();

                StringBuilder sql = new StringBuilder();
                sql.AppendFormat("SELECT * FROM {0}  ", attr.Name);

                if (!string.IsNullOrEmpty(attr.Sort))
                {
                    sql.AppendFormat("ORDER BY {0}", attr.Sort);
                }

                DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql.ToString());

                log.Debug(sql.ToString(), new LogInfo()
                {
                    MethodInstance = MethodBase.GetCurrentMethod(),
                    ThreadInstance = Thread.CurrentThread
                });

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ConstructorInfo ci = typeof(T).GetConstructor(new Type[] { typeof(DataRow) });

                        list.Add((T)ci.Invoke(new Object[] { dr }));
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                log.Debug(ex, new LogInfo()
                {
                    MethodInstance = MethodBase.GetCurrentMethod(),
                    ThreadInstance = Thread.CurrentThread
                });

                throw;
            }
        }

        public List<T> All<T>(Pager pager, Hashtable htOrder = null) where T : class, IEntity
        {
            try
            {
                Contract.Requires(pager != null);

                var list = new List<T>();

                var attr = GetTableAttr<T>();

                string _strOrderBy = !string.IsNullOrEmpty(attr.Sort) ? attr.Sort : attr.Key;

                if (htOrder != null)
                {
                    var listOrder = new List<string>();

                    foreach (var de in htOrder.Keys)
                    {
                        var attrCol = GetColumnAttr<T>(de.ToString());
                        string _value = htOrder[de].ToString().ToUpper();

                        if (attrCol != null)
                        {
                            listOrder.Add(string.Format("{0} {1}", attrCol.Name, _value.Equals("DESC") ? _value : string.Empty));
                        }
                        else if (de.ToString().Equals("ID"))
                        {
                            listOrder.Add(string.Format("{0} {1}", attr.Key, _value.Equals("DESC") ? _value : string.Empty));
                        }
                        else
                        { continue; }
                    }

                    _strOrderBy = string.Join(", ", listOrder.ToArray());
                }

                string innerSql = string.Format("(SELECT ROW_NUMBER() OVER(ORDER BY {1}) AS RowNo, * FROM {0})", attr.Name, _strOrderBy);

                string sql = string.Format("SELECT * FROM {0} AS t WHERE t.RowNo BETWEEN {1} AND {2}",
                    innerSql, ((pager.Index - 1) * pager.Size).ToString(), (pager.Index * pager.Size - 1).ToString());

                DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql);

                log.Debug(sql, new LogInfo()
                {
                    MethodInstance = MethodBase.GetCurrentMethod(),
                    ThreadInstance = Thread.CurrentThread
                });

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ConstructorInfo ci = typeof(T).GetConstructor(new Type[] { typeof(DataRow) });

                        list.Add((T)ci.Invoke(new Object[] { dr }));
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                log.Debug(ex, new LogInfo()
                {
                    MethodInstance = MethodBase.GetCurrentMethod(),
                    ThreadInstance = Thread.CurrentThread
                });

                throw;
            }
        }

        public List<T> Query<T>(Hashtable htWhere) where T : class, IEntity
        {
            try
            {
                Contract.Requires(htWhere != null);

                var list = new List<T>();

                List<string> listCol = new List<string>();
                List<SqlParameter> listPara = new List<SqlParameter>();

                var attr = GetTableAttr<T>();

                foreach (var de in htWhere.Keys)
                {
                    var attrCol = GetColumnAttr<T>(de.ToString());

                    if (attrCol != null)
                    {
                        object _value = htWhere[de];

                        listCol.Add(string.Format("{0} = @{0}", attrCol.Name));

                        SqlParameter _para = new SqlParameter("@" + attrCol.Name,
                            _value != null ? _value : (object)DBNull.Value);
                        listPara.Add(_para);
                    }
                    else if (de.ToString().Equals("ID"))
                    {
                        listCol.Add(string.Format("{0} = @key", attr.Key));
                        listPara.Add(new SqlParameter("@key", htWhere[de]));
                    }
                    else
                    { continue; }
                }

                if (listCol.Count > 0 && listPara.Count > 0)
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendFormat("SELECT * FROM {0} WHERE {1} ", attr.Name, string.Join(" AND ", listCol.ToArray()));

                    if (!string.IsNullOrEmpty(attr.Sort))
                    {
                        sql.AppendFormat("ORDER BY {0}", attr.Sort);
                    }

                    DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql.ToString(), listPara.ToArray());

                    log.Debug(sql.ToString(), new LogInfo()
                    {
                        MethodInstance = MethodBase.GetCurrentMethod(),
                        ThreadInstance = Thread.CurrentThread
                    });

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            ConstructorInfo ci = typeof(T).GetConstructor(new Type[] { typeof(DataRow) });

                            list.Add((T)ci.Invoke(new Object[] { dr }));
                        }
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                log.Debug(ex, new LogInfo()
                {
                    MethodInstance = MethodBase.GetCurrentMethod(),
                    ThreadInstance = Thread.CurrentThread
                });

                throw;
            }
        }

        public List<T> Query<T>(Pager pager, Hashtable htWhere, Hashtable htOrder = null) where T : class, IEntity
        {
            try
            {
                Contract.Requires(pager != null);
                Contract.Requires(htWhere != null);

                var list = new List<T>();

                var attr = GetTableAttr<T>();

                // Generate WhereBy Clause
                List<string> listCol = new List<string>();
                List<SqlParameter> listPara = new List<SqlParameter>();

                foreach (var de in htWhere.Keys)
                {
                    var attrCol = GetColumnAttr<T>(de.ToString());

                    if (attrCol != null)
                    {
                        object _value = htWhere[de];

                        listCol.Add(string.Format("{0} = @{0}", attrCol.Name));

                        SqlParameter _para = new SqlParameter("@" + attrCol.Name,
                            _value != null ? _value : (object)DBNull.Value);
                        listPara.Add(_para);
                    }
                    else if (de.ToString().Equals("ID"))
                    {
                        listCol.Add(string.Format("{0} = @key", attr.Key));
                        listPara.Add(new SqlParameter("@key", htWhere[de]));
                    }
                    else
                    { continue; }
                }

                // Generate OrderBy Clause
                string _strOrderBy = !string.IsNullOrEmpty(attr.Sort) ? attr.Sort : attr.Key;

                if (htOrder != null)
                {
                    var listOrder = new List<string>();

                    foreach (var de in htOrder.Keys)
                    {
                        var attrCol = GetColumnAttr<T>(de.ToString());
                        string _value = htOrder[de].ToString().ToUpper();

                        if (attrCol != null)
                        {
                            listOrder.Add(string.Format("{0} {1}", attrCol.Name, _value.Equals("DESC") ? _value : string.Empty));
                        }
                        else if (de.ToString().Equals("ID"))
                        {
                            listOrder.Add(string.Format("{0} {1}", attr.Key, _value.Equals("DESC") ? _value : string.Empty));
                        }
                        else
                        { continue; }
                    }

                    _strOrderBy = string.Join(", ", listOrder.ToArray());
                }

                // Build Sql and Execute
                if (listCol.Count > 0 && listPara.Count > 0)
                {
                    string innerSql = string.Format("(SELECT ROW_NUMBER() OVER(ORDER BY {1}) AS RowNo, * FROM {0} WHERE {2})",
                        attr.Name, _strOrderBy, string.Join(" AND ", listCol.ToArray()));

                    string sql = string.Format("SELECT * FROM {0} AS t WHERE t.RowNo BETWEEN {1} AND {2}",
                        innerSql, ((pager.Index - 1) * pager.Size).ToString(), (pager.Index * pager.Size - 1).ToString());

                    DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, listPara.ToArray());

                    log.Debug(sql, new LogInfo()
                    {
                        MethodInstance = MethodBase.GetCurrentMethod(),
                        ThreadInstance = Thread.CurrentThread
                    });

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            ConstructorInfo ci = typeof(T).GetConstructor(new Type[] { typeof(DataRow) });

                            list.Add((T)ci.Invoke(new Object[] { dr }));
                        }
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                log.Debug(ex, new LogInfo()
                {
                    MethodInstance = MethodBase.GetCurrentMethod(),
                    ThreadInstance = Thread.CurrentThread
                });

                throw;
            }
        }

        public IQueryable<T> Query<T>(Expression<Func<T, bool>> predicate) where T : class, IEntity
        {
            Contract.Requires(predicate != null);

            return All<T>().AsQueryable().Where(predicate);
        }

        public void Insert<T>(T instance, SqlTransaction trans = null) where T : class, IEntity
        {
            try
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
                    // skip the property of the self-increase main-key
                    var primary = instance.GetType().GetProperty("ID");

                    if (!primary.PropertyType.Equals(typeof(int)))
                    {
                        listCol.Add(attr.Key);
                        listColPara.Add("@key");
                        listPara.Add(new SqlParameter("@key", primary.GetValue(instance, null)));
                    }

                    string sql = string.Format("INSERT INTO {0} ({1}) VALUES ({2})", attr.Name,
                        string.Join(", ", listCol.ToArray()), string.Join(", ", listColPara.ToArray()));

                    if (trans != null)
                    {
                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, listPara.ToArray());
                    }
                    else
                    {
                        SqlHelper.ExecuteNonQuery(conn, CommandType.Text, sql, listPara.ToArray());
                    }

                    log.Debug(sql, new LogInfo()
                    {
                        MethodInstance = MethodBase.GetCurrentMethod(),
                        ThreadInstance = Thread.CurrentThread
                    });
                }
                else { throw new Exception("Unable to find any valid DB columns"); }
            }
            catch (Exception ex)
            {
                log.Debug(ex, new LogInfo()
                {
                    MethodInstance = MethodBase.GetCurrentMethod(),
                    ThreadInstance = Thread.CurrentThread
                });

                throw;
            }
        }

        public void Insert<T>(T instance, out object key, SqlTransaction trans = null) where T : class, IEntity
        {
            try
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
                    // skip the property of the self-increase main-key
                    var primary = instance.GetType().GetProperty("ID");

                    if (!primary.PropertyType.Equals(typeof(int)))
                    {
                        listCol.Add(attr.Key);
                        listColPara.Add("@key");
                        listPara.Add(new SqlParameter("@key", primary.GetValue(instance, null)));
                    }

                    string sql = string.Format("INSERT INTO {0} ({1}) VALUES ({2}); SELECT SCOPE_IDENTITY();",
                        attr.Name, string.Join(", ", listCol.ToArray()), string.Join(", ", listColPara.ToArray()));

                    if (trans != null)
                    {
                        key = SqlHelper.ExecuteScalar(trans, CommandType.Text, sql, listPara.ToArray());
                    }
                    else
                    {
                        key = SqlHelper.ExecuteScalar(conn, CommandType.Text, sql, listPara.ToArray());
                    }

                    log.Debug(sql, new LogInfo()
                    {
                        MethodInstance = MethodBase.GetCurrentMethod(),
                        ThreadInstance = Thread.CurrentThread
                    });
                }
                else
                {
                    key = null;
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex, new LogInfo()
                {
                    MethodInstance = MethodBase.GetCurrentMethod(),
                    ThreadInstance = Thread.CurrentThread
                });

                throw;
            }
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
            try
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

                    log.Debug(sql, new LogInfo()
                    {
                        MethodInstance = MethodBase.GetCurrentMethod(),
                        ThreadInstance = Thread.CurrentThread
                    });
                }
                else { throw new Exception("Unable to find any valid DB columns"); }
            }
            catch (Exception ex)
            {
                log.Debug(ex, new LogInfo()
                {
                    MethodInstance = MethodBase.GetCurrentMethod(),
                    ThreadInstance = Thread.CurrentThread
                });

                throw;
            }
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
            try
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

                log.Debug(sql, new LogInfo()
                {
                    MethodInstance = MethodBase.GetCurrentMethod(),
                    ThreadInstance = Thread.CurrentThread
                });
            }
            catch (Exception ex)
            {
                log.Debug(ex, new LogInfo()
                {
                    MethodInstance = MethodBase.GetCurrentMethod(),
                    ThreadInstance = Thread.CurrentThread
                });

                throw;
            }
        }

        public void Delete<T>(T instance, SqlTransaction trans = null) where T : class, IEntity
        {
            Contract.Requires(instance != null);

            var attr = (DbTable)Attribute.GetCustomAttribute(typeof(T), typeof(DbTable));

            var key = instance.GetType().GetProperty("ID").GetValue(instance, null);

            Delete<T>(key, trans);
        }

        public void Delete<T>(Expression<Func<T, bool>> predicate, out int count, SqlTransaction trans = null) where T : class, IEntity
        {
            Contract.Requires(predicate != null);

            var instances = Query<T>(predicate);

            count = instances.Count();

            if (instances != null && count > 0)
            {
                foreach (var instance in instances)
                {
                    Delete<T>(instance, trans);
                }
            }
        }

        public static DbTable GetTableAttr<T>() where T : class
        {
            var attr = (DbTable)Attribute.GetCustomAttribute(typeof(T), typeof(DbTable));

            if (attr != null)
            { return attr; }
            else
            { return new DbTable(typeof(T).Name); }
        }

        public static DbColumn GetColumnAttr(PropertyInfo pi)
        {
            return (DbColumn)Attribute.GetCustomAttribute(pi, typeof(DbColumn));
        }

        public static DbColumn GetColumnAttr<T>(string name) where T : class
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
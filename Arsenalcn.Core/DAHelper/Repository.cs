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
using System.Threading;

using Microsoft.ApplicationBlocks.Data;

using Arsenalcn.Core.Logger;

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

        public T Single<T>(object key) where T : class, IViewer, new()
        {
            try
            {
                Contract.Requires(key != null);

                var attr = GetTableAttr<T>();

                string sql = string.Format("SELECT * FROM {0} WHERE {1} = @key", attr.Name, attr.Key);

                SqlParameter[] para = { new SqlParameter("@key", key) };

                DataSet ds = DataAccess.ExecuteDataset(sql, para);

                var dt = ds.Tables[0];

                if (dt.Rows.Count == 0) { return null; }

                using (var reader = dt.CreateDataReader())
                {
                    return reader.DataReaderMapTo<T>().FirstOrDefault();
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

        public List<T> All<T>() where T : class, IViewer, new()
        {
            try
            {
                var list = new List<T>();

                var attr = GetTableAttr<T>();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT * FROM " + attr.Name);

                if (!string.IsNullOrEmpty(attr.Sort))
                {
                    sql.Append(" ORDER BY " + attr.Sort);
                }

                DataSet ds = DataAccess.ExecuteDataset(sql.ToString());

                var dt = ds.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    using (var reader = dt.CreateDataReader())
                    {
                        list = reader.DataReaderMapTo<T>().ToList();
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

        public List<T> All<T>(Pager pager, string orderBy = null) where T : class, IViewer, new()
        {
            try
            {
                Contract.Requires(pager != null);

                var list = new List<T>();

                var attr = GetTableAttr<T>();

                string _strOrderBy = !string.IsNullOrEmpty(attr.Sort) ? attr.Sort : attr.Key;

                if (!string.IsNullOrEmpty(orderBy))
                {
                    _strOrderBy = orderBy;
                }

                string innerSql = string.Format("(SELECT ROW_NUMBER() OVER(ORDER BY {1}) AS RowNo, * FROM {0})", attr.Name, _strOrderBy);

                string sql = string.Format("SELECT * FROM {0} AS t WHERE t.RowNo BETWEEN {1} AND {2}",
                    innerSql, (pager.CurrentPage * pager.PagingSize).ToString(), ((pager.CurrentPage + 1) * pager.PagingSize - 1).ToString());

                DataSet ds = DataAccess.ExecuteDataset(sql);

                var dt = ds.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    using (var reader = dt.CreateDataReader())
                    {
                        list = reader.DataReaderMapTo<T>().ToList();
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

        public List<T> Query<T>(Expression<Func<T, bool>> whereBy) where T : class, IViewer, new()
        {
            try
            {
                Contract.Requires(whereBy != null);

                var list = new List<T>();

                var attr = GetTableAttr<T>();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT * FROM " + attr.Name);

                var condition = new ConditionBuilder();
                condition.Build(whereBy.Body);

                if (!string.IsNullOrEmpty(condition.Condition))
                {
                    //var where = string.Format(condition.Condition, condition.Arguments);
                    sql.Append(" WHERE " + condition.Condition);
                }

                if (!string.IsNullOrEmpty(attr.Sort))
                {
                    sql.Append(" ORDER BY " + attr.Sort);
                }

                DataSet ds = null;

                if (condition.SqlArguments != null && condition.SqlArguments.Count > 0)
                {
                    ds = DataAccess.ExecuteDataset(sql.ToString(), condition.SqlArguments.ToArray());
                }
                else
                {
                    ds = DataAccess.ExecuteDataset(sql.ToString());
                }

                var dt = ds.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    using (var reader = dt.CreateDataReader())
                    {
                        list = reader.DataReaderMapTo<T>().ToList();
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

        public List<T> Query<T>(Pager pager, Expression<Func<T, bool>> whereBy, string orderBy = null) where T : class, IViewer, new()
        {
            try
            {
                Contract.Requires(pager != null);
                Contract.Requires(whereBy != null);

                var list = new List<T>();

                var attr = GetTableAttr<T>();

                // Generate WhereBy Clause
                var condition = new ConditionBuilder();
                condition.Build(whereBy.Body);

                // Generate OrderBy Clause
                string _strOrderBy = !string.IsNullOrEmpty(attr.Sort) ? attr.Sort : attr.Key;

                if (!string.IsNullOrEmpty(orderBy))
                {
                    _strOrderBy = orderBy;
                }

                // Build Sql and Execute
                string innerSql = string.Format("(SELECT ROW_NUMBER() OVER(ORDER BY {1}) AS RowNo, * FROM {0} WHERE {2})",
                    attr.Name, _strOrderBy, condition.Condition);

                string sql = string.Format("SELECT * FROM {0} AS t WHERE t.RowNo BETWEEN {1} AND {2}",
                    innerSql, (pager.CurrentPage * pager.PagingSize).ToString(), ((pager.CurrentPage + 1) * pager.PagingSize - 1).ToString());

                DataSet ds = DataAccess.ExecuteDataset(sql, condition.SqlArguments.ToArray());

                var dt = ds.Tables[0];

                if (ds.Tables[0].Rows.Count > 0)
                {
                    using (var reader = dt.CreateDataReader())
                    {
                        list = reader.DataReaderMapTo<T>().ToList();
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
                        DataAccess.ExecuteNonQuery(sql, listPara.ToArray(), trans);
                    }
                    else
                    {
                        DataAccess.ExecuteNonQuery(sql, listPara.ToArray());
                    }
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
                var sql = string.Empty;

                if (listCol.Count > 0 && listColPara.Count > 0 && listPara.Count > 0)
                {
                    // skip the property of the self-increase main-key
                    var primary = instance.GetType().GetProperty("ID");

                    if (!primary.PropertyType.Equals(typeof(int)))
                    {
                        listCol.Add(attr.Key);
                        listColPara.Add("@key");

                        key = primary.GetValue(instance, null);
                        listPara.Add(new SqlParameter("@key", key));

                        sql = string.Format("INSERT INTO {0} ({1}) VALUES ({2})",
                           attr.Name, string.Join(", ", listCol.ToArray()), string.Join(", ", listColPara.ToArray()));

                        if (trans != null)
                        {
                            DataAccess.ExecuteNonQuery(sql, listPara.ToArray(), trans);
                        }
                        else
                        {
                            DataAccess.ExecuteNonQuery(sql, listPara.ToArray());
                        }
                    }
                    else
                    {
                        sql = string.Format("INSERT INTO {0} ({1}) VALUES ({2}); SELECT SCOPE_IDENTITY();",
                           attr.Name, string.Join(", ", listCol.ToArray()), string.Join(", ", listColPara.ToArray()));

                        if (trans != null)
                        {
                            key = DataAccess.ExecuteScalar(sql, listPara.ToArray(), trans);
                        }
                        else
                        {
                            key = DataAccess.ExecuteScalar(sql, listPara.ToArray());
                        }
                    }
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
                        DataAccess.ExecuteNonQuery(sql, listPara.ToArray(), trans);
                    }
                    else
                    {
                        DataAccess.ExecuteNonQuery(sql, listPara.ToArray());
                    }
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
                    DataAccess.ExecuteNonQuery(sql, para, trans);
                }
                else
                {
                    DataAccess.ExecuteNonQuery(sql, para);
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

        public void Delete<T>(T instance, SqlTransaction trans = null) where T : class, IEntity
        {
            Contract.Requires(instance != null);

            var attr = (DbSchema)Attribute.GetCustomAttribute(typeof(T), typeof(DbSchema));

            var key = instance.GetType().GetProperty("ID").GetValue(instance, null);

            Delete<T>(key, trans);
        }

        public static DbSchema GetTableAttr<T>() where T : class
        {
            var attr = Attribute.GetCustomAttribute(typeof(T), typeof(DbSchema)) as DbSchema;
            return attr ?? new DbSchema(typeof(T).Name);
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
    }
}
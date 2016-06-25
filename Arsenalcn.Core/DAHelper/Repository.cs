using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using Arsenalcn.Core.Logger;

namespace Arsenalcn.Core
{
    public class Repository : IRepository
    {
        private readonly ILog _log = new DaoLog();

        public T Single<T>(object key) where T : class, IViewer, new()
        {
            try
            {
                Contract.Requires(key != null);

                var attr = GetTableAttr<T>();

                string sql = $"SELECT * FROM {attr.Name} WHERE {attr.Key} = @key";

                SqlParameter[] para = { new SqlParameter("@key", key) };

                var ds = DataAccess.ExecuteDataset(sql, para);

                var dt = ds.Tables[0];

                if (dt.Rows.Count == 0)
                {
                    return null;
                }

                using (var reader = dt.CreateDataReader())
                {
                    return reader.DataReaderMapTo<T>().FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex, new LogInfo
                {
                    MethodInstance = MethodBase.GetCurrentMethod(),
                    ThreadInstance = Thread.CurrentThread
                });

                throw;
            }
        }

        public int Count<T>(Expression<Func<T, bool>> whereBy) where T : class, IViewer, new()
        {
            try
            {
                Contract.Requires(whereBy != null);

                var attr = GetTableAttr<T>();

                var sql = new StringBuilder();
                sql.Append("SELECT COUNT(*) FROM " + attr.Name);

                var condition = new ConditionBuilder();
                condition.Build(whereBy.Body);

                if (!string.IsNullOrEmpty(condition.Condition))
                {
                    sql.Append(" WHERE " + condition.Condition);
                }

                object count;

                if (condition.SqlArguments != null && condition.SqlArguments.Count > 0)
                {
                    count = DataAccess.ExecuteScalar(sql.ToString(), condition.SqlArguments.ToArray());
                }
                else
                {
                    count = DataAccess.ExecuteScalar(sql.ToString());
                }

                return Convert.ToInt32(count);
            }
            catch (Exception ex)
            {
                _log.Error(ex, new LogInfo
                {
                    MethodInstance = MethodBase.GetCurrentMethod(),
                    ThreadInstance = Thread.CurrentThread
                });

                throw;
            }
        }

        public bool Any<T>(object key) where T : class, IEntity
        {
            try
            {
                Contract.Requires(key != null);

                var attr = GetTableAttr<T>();

                string sql = $"SELECT COUNT(*) FROM {attr.Name} WHERE {attr.Key} = @key";

                SqlParameter[] para = { new SqlParameter("@key", key) };

                var count = DataAccess.ExecuteScalar(sql, para);

                return Convert.ToInt32(count) > 0;
            }
            catch (Exception ex)
            {
                _log.Error(ex, new LogInfo
                {
                    MethodInstance = MethodBase.GetCurrentMethod(),
                    ThreadInstance = Thread.CurrentThread
                });

                throw;
            }
        }

        public bool Any<T>(Expression<Func<T, bool>> whereBy) where T : class, IViewer, new()
        {
            Contract.Requires(whereBy != null);

            return Count(whereBy) > 0;
        }


        public List<T> All<T>() where T : class, IViewer, new()
        {
            try
            {
                var list = new List<T>();

                var attr = GetTableAttr<T>();

                var sql = new StringBuilder();
                sql.Append("SELECT * FROM " + attr.Name);

                if (!string.IsNullOrEmpty(attr.Sort))
                {
                    sql.Append(" ORDER BY " + attr.Sort);
                }

                var ds = DataAccess.ExecuteDataset(sql.ToString());

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
                _log.Error(ex, new LogInfo
                {
                    MethodInstance = MethodBase.GetCurrentMethod(),
                    ThreadInstance = Thread.CurrentThread
                });

                throw;
            }
        }

        public List<T> All<T>(IPager pager, string orderBy = null) where T : class, IViewer, new()
        {
            try
            {
                Contract.Requires(pager != null);

                var list = new List<T>();

                var attr = GetTableAttr<T>();

                var strOrderBy = !string.IsNullOrEmpty(attr.Sort) ? attr.Sort : attr.Key;

                if (!string.IsNullOrEmpty(orderBy))
                {
                    strOrderBy = orderBy;
                }

                // Get TotalCount First
                var countSql = string.Format("SELECT COUNT({1}) AS TotalCount FROM {0}", attr.Name, attr.Key);

                var ds = DataAccess.ExecuteDataset(countSql);

                pager.SetTotalCount((int)ds.Tables[0].Rows[0]["TotalCount"]);

                // Get Query Result
                var innerSql = string.Format("(SELECT ROW_NUMBER() OVER(ORDER BY {1}) AS RowNo, * FROM {0})", attr.Name,
                    strOrderBy);

                string sql =
                    $"SELECT * FROM {innerSql} AS t WHERE t.RowNo BETWEEN {pager.CurrentPage * pager.PagingSize + 1} AND {(pager.CurrentPage + 1) * pager.PagingSize};";

                //sql += string.Format("SELECT COUNT({1}) AS TotalCount FROM {0}", attr.Name, attr.Key);

                ds = DataAccess.ExecuteDataset(sql);

                var dt = ds.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    using (var reader = dt.CreateDataReader())
                    {
                        list = reader.DataReaderMapTo<T>().ToList();
                    }
                }

                //pager.SetTotalCount((int)ds.Tables[1].Rows[0]["TotalCount"]);

                return list;
            }
            catch (Exception ex)
            {
                _log.Error(ex, new LogInfo
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

                var sql = new StringBuilder();
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

                DataSet ds;

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
                _log.Error(ex, new LogInfo
                {
                    MethodInstance = MethodBase.GetCurrentMethod(),
                    ThreadInstance = Thread.CurrentThread
                });

                throw;
            }
        }

        public List<T> Query<T>(IPager pager, Expression<Func<T, bool>> whereBy, string orderBy = null)
            where T : class, IViewer, new()
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
                var strOrderBy = !string.IsNullOrEmpty(attr.Sort) ? attr.Sort : attr.Key;

                if (!string.IsNullOrEmpty(orderBy))
                {
                    strOrderBy = orderBy;
                }

                // Get TotalCount First
                var countSql = string.Format("SELECT COUNT({1}) AS TotalCount FROM {0} WHERE {2}", attr.Name, attr.Key,
                    condition.Condition);

                DataSet ds;

                if (condition.SqlArguments != null && condition.SqlArguments.Count > 0)
                {
                    ds = DataAccess.ExecuteDataset(countSql, condition.SqlArguments.ToArray());
                }
                else
                {
                    ds = DataAccess.ExecuteDataset(countSql);
                }

                pager.SetTotalCount((int)ds.Tables[0].Rows[0]["TotalCount"]);

                // Build Sql and Execute
                var innerSql = string.Format("(SELECT ROW_NUMBER() OVER(ORDER BY {1}) AS RowNo, * FROM {0} WHERE {2})",
                    attr.Name, strOrderBy, condition.Condition);

                string sql =
                    $"SELECT * FROM {innerSql} AS t WHERE t.RowNo BETWEEN {pager.CurrentPage * pager.PagingSize + 1} AND {(pager.CurrentPage + 1) * pager.PagingSize}";

                //sql += string.Format("SELECT COUNT({1}) AS TotalCount FROM {0} WHERE {2}", attr.Name, attr.Key, condition.Condition);

                if (condition.SqlArguments != null && condition.SqlArguments.Count > 0)
                {
                    ds = DataAccess.ExecuteDataset(sql, condition.SqlArguments.ToArray());
                }
                else
                {
                    ds = DataAccess.ExecuteDataset(sql);
                }

                var dt = ds.Tables[0];

                if (ds.Tables[0].Rows.Count > 0)
                {
                    using (var reader = dt.CreateDataReader())
                    {
                        list = reader.DataReaderMapTo<T>().ToList();
                    }
                }

                //pager.SetTotalCount((int)ds.Tables[1].Rows[0]["TotalCount"]);

                return list;
            }
            catch (Exception ex)
            {
                _log.Error(ex, new LogInfo
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

                var listCol = new List<string>();
                var listColPara = new List<string>();
                var listPara = new List<SqlParameter>();

                foreach (var pi in instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    var attrCol = GetColumnAttr(pi);

                    if (attrCol != null)
                    {
                        var value = pi.GetValue(instance, null);

                        listCol.Add(attrCol.Name);
                        listColPara.Add("@" + attrCol.Name);

                        var para = new SqlParameter("@" + attrCol.Name, value ?? DBNull.Value);
                        listPara.Add(para);
                    }
                }

                var attr = GetTableAttr<T>();

                if (listCol.Count > 0 && listColPara.Count > 0 && listPara.Count > 0)
                {
                    // skip the property of the self-increase main-key
                    var primary = instance.GetType().GetProperty("ID");

                    if (primary.PropertyType != typeof(int))
                    {
                        listCol.Add(attr.Key);
                        listColPara.Add("@key");
                        listPara.Add(new SqlParameter("@key", primary.GetValue(instance, null)));
                    }

                    string sql =
                        $"INSERT INTO {attr.Name} ({string.Join(", ", listCol.ToArray())}) VALUES ({string.Join(", ", listColPara.ToArray())})";

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
                    throw new Exception("Unable to find any valid DB columns");
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex, new LogInfo
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

                var listCol = new List<string>();
                var listColPara = new List<string>();
                var listPara = new List<SqlParameter>();

                foreach (var pi in instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    var attrCol = GetColumnAttr(pi);

                    if (attrCol != null)
                    {
                        var value = pi.GetValue(instance, null);

                        listCol.Add(attrCol.Name);
                        listColPara.Add("@" + attrCol.Name);

                        var para = new SqlParameter("@" + attrCol.Name, value ?? DBNull.Value);
                        listPara.Add(para);
                    }
                }

                var attr = GetTableAttr<T>();

                if (listCol.Count > 0 && listColPara.Count > 0 && listPara.Count > 0)
                {
                    // skip the property of the self-increase main-key
                    var primary = instance.GetType().GetProperty("ID");

                    string sql;

                    if (primary.PropertyType != typeof(int))
                    {
                        listCol.Add(attr.Key);
                        listColPara.Add("@key");

                        key = primary.GetValue(instance, null);
                        listPara.Add(new SqlParameter("@key", key));

                        sql =
                            $"INSERT INTO {attr.Name} ({string.Join(", ", listCol.ToArray())}) VALUES ({string.Join(", ", listColPara.ToArray())})";

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
                        sql =
                            $"INSERT INTO {attr.Name} ({string.Join(", ", listCol.ToArray())}) VALUES ({string.Join(", ", listColPara.ToArray())}); SELECT SCOPE_IDENTITY();";

                        key = trans != null
                            ? DataAccess.ExecuteScalar(sql, listPara.ToArray(), trans)
                            : DataAccess.ExecuteScalar(sql, listPara.ToArray());
                    }
                }
                else
                {
                    key = null;
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex, new LogInfo
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

                var listCol = new List<string>();
                var listPara = new List<SqlParameter>();

                foreach (var pi in instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    var attrCol = GetColumnAttr(pi);

                    if (attrCol != null)
                    {
                        var value = pi.GetValue(instance, null);

                        listCol.Add(string.Format("{0} = @{0}", attrCol.Name));

                        var para = new SqlParameter("@" + attrCol.Name, value ?? DBNull.Value);
                        listPara.Add(para);
                    }
                }

                var attr = GetTableAttr<T>();

                if (listCol.Count > 0 && listPara.Count > 0)
                {
                    string sql =
                        $"UPDATE {attr.Name} SET {string.Join(", ", listCol.ToArray())} WHERE {attr.Key} = @key";

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
                else
                {
                    throw new Exception("Unable to find any valid DB columns");
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex, new LogInfo
                {
                    MethodInstance = MethodBase.GetCurrentMethod(),
                    ThreadInstance = Thread.CurrentThread
                });

                throw;
            }
        }

        public void Save<T>(T instance, SqlTransaction trans = null) where T : class, IEntity
        {
            try
            {
                Contract.Requires(instance != null);

                var key = instance.GetType().GetProperty("ID").GetValue(instance, null);

                if (Any<T>(key))
                {
                    Update(instance, trans);
                }
                else
                {
                    Insert(instance, trans);
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex, new LogInfo
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

                string sql = $"DELETE {attr.Name} WHERE {attr.Key} = @key";

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
                _log.Error(ex, new LogInfo
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

        public static DbColumn GetColumnAttr<T>(Expression<Func<T, object>> expr) where T : class
        {
            var name = string.Empty;

            var body = expr.Body as UnaryExpression;

            if (body != null)
            {
                name = ((MemberExpression)body.Operand).Member.Name;
            }
            else if (expr.Body is MemberExpression)
            {
                name = ((MemberExpression)expr.Body).Member.Name;
            }
            else if (expr.Body is ParameterExpression)
            {
                name = ((ParameterExpression)expr.Body).Type.Name;
            }

            return GetColumnAttr<T>(name);
        }
    }
}
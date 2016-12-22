using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Arsenalcn.Core.Logger;
using Dapper;
using DataReaderMapper.Internal;

namespace Arsenalcn.Core
{
    public class Repository : IRepository
    {
        private readonly ILog _log = new DaoLog();
        private readonly IDbConnection _conn = DapperHelper.GetOpenConnection();

        public T Single<T>(object key) where T : class, IViewer, new()
        {
            var attr = GetTableAttr<T>();

            string sql = $"SELECT {attr.Key} AS ID, * FROM {attr.Name} WHERE {attr.Key} = @key";

            var instance = _conn.QueryFirstOrDefault<T>(sql, new { key });

            instance?.Inital();

            return instance;
        }

        public int Count<T>(Expression<Func<T, bool>> whereBy) where T : class, IViewer, new()
        {
            var attr = GetTableAttr<T>();

            var sql = new StringBuilder();
            sql.Append("SELECT COUNT(*) FROM " + attr.Name);

            var condition = new ConditionBuilder();
            condition.Build(whereBy.Body);

            if (!string.IsNullOrEmpty(condition.Condition))
            {
                sql.Append(" WHERE " + condition.Condition);
            }

            return _conn.ExecuteScalar<int>(sql.ToString(), condition.DapperArguments);
        }

        public bool Any<T>(object key) where T : class, IEntity
        {
            var attr = GetTableAttr<T>();

            string sql = $"SELECT COUNT(*) FROM {attr.Name} WHERE {attr.Key} = @key";

            return _conn.ExecuteScalar<int>(sql, new { key }) > 0;
        }

        public bool Any<T>(Expression<Func<T, bool>> whereBy) where T : class, IViewer, new()
        {
            return Count(whereBy) > 0;
        }

        public List<T> All<T>() where T : class, IViewer, new()
        {
            var attr = GetTableAttr<T>();

            var sql = new StringBuilder();
            sql.Append($"SELECT {attr.Key} AS ID, * FROM {attr.Name}");

            if (!string.IsNullOrEmpty(attr.Sort))
            {
                sql.Append(" ORDER BY " + attr.Sort);
            }

            var list = _conn.Query<T>(sql.ToString()).ToList();

            if (list.Count > 0) { list.Each(x => x.Inital()); }

            return list;
        }

        public List<T> All<T>(IPager pager, string orderBy = null) where T : class, IViewer, new()
        {
            var attr = GetTableAttr<T>();

            var strOrderBy = !string.IsNullOrEmpty(attr.Sort) ? attr.Sort : attr.Key;

            if (!string.IsNullOrEmpty(orderBy))
            {
                strOrderBy = orderBy;
            }

            // Get TotalCount First
            var countSql = $"SELECT COUNT({attr.Key}) AS TotalCount FROM {attr.Name}";

            pager.SetTotalCount(_conn.ExecuteScalar<int>(countSql));

            // Get Query Result
            var innerSql = $"SELECT ROW_NUMBER() OVER(ORDER BY {strOrderBy}) AS RowNo, * FROM {attr.Name}";

            string sql =
                $"SELECT {attr.Key} AS ID, * FROM ({innerSql}) AS t WHERE t.RowNo BETWEEN {pager.CurrentPage * pager.PagingSize + 1} AND {(pager.CurrentPage + 1) * pager.PagingSize};";

            //sql += string.Format("SELECT COUNT({1}) AS TotalCount FROM {0}", attr.Name, attr.Key);

            var list = _conn.Query<T>(sql).ToList();

            if (list.Count > 0) { list.Each(x => x.Inital()); }

            return list;
        }

        public List<T> Query<T>(Expression<Func<T, bool>> whereBy) where T : class, IViewer, new()
        {
            var attr = GetTableAttr<T>();

            var sql = new StringBuilder();
            sql.Append($"SELECT {attr.Key} AS ID, * FROM {attr.Name}");

            var condition = new ConditionBuilder();
            condition.Build(whereBy.Body);

            if (!string.IsNullOrEmpty(condition.Condition))
            {
                sql.Append(" WHERE " + condition.Condition);
            }

            if (!string.IsNullOrEmpty(attr.Sort))
            {
                sql.Append(" ORDER BY " + attr.Sort);
            }

            var list = _conn.Query<T>(sql.ToString(), condition.DapperArguments).ToList();

            if (list.Count > 0) { list.Each(x => x.Inital()); }

            return list;
        }

        public List<T> Query<T>(IPager pager, Expression<Func<T, bool>> whereBy, string orderBy = null)
            where T : class, IViewer, new()
        {
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
            var countSql = $"SELECT COUNT({attr.Key}) AS TotalCount FROM {attr.Name} WHERE {condition.Condition}";

            pager.SetTotalCount(_conn.ExecuteScalar<int>(countSql, condition.DapperArguments));

            // Build Sql and Execute
            var innerSql = $"SELECT ROW_NUMBER() OVER(ORDER BY {strOrderBy}) AS RowNo, * FROM {attr.Name} WHERE {condition.Condition}";

            string sql =
                $"SELECT {attr.Key} AS ID, * FROM ({innerSql}) AS t WHERE t.RowNo BETWEEN {pager.CurrentPage * pager.PagingSize + 1} AND {(pager.CurrentPage + 1) * pager.PagingSize}";

            var list = _conn.Query<T>(sql, condition.DapperArguments).ToList();

            if (list.Count > 0) { list.Each(x => x.Inital()); }

            return list;
        }

        public void Insert<T>(T instance, SqlTransaction trans = null) where T : class, IEntity
        {
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

                _conn.Execute(sql, DapperHelper.BuildDapperParameters(listPara.ToArray()), trans);
            }
            else
            {
                throw new Exception("Unable to find any valid DB columns");
            }

        }

        public void Insert<T>(T instance, out object key, SqlTransaction trans = null) where T : class, IEntity
        {
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

                    _conn.Execute(sql, DapperHelper.BuildDapperParameters(listPara.ToArray()), trans);
                }
                else
                {
                    sql =
                        $"INSERT INTO {attr.Name} ({string.Join(", ", listCol.ToArray())}) VALUES ({string.Join(", ", listColPara.ToArray())}); SELECT SCOPE_IDENTITY();";

                    key = _conn.ExecuteScalar(sql, DapperHelper.BuildDapperParameters(listPara.ToArray()), trans);
                }
            }
            else
            {
                key = null;
            }
        }

        public void Update<T>(T instance, SqlTransaction trans = null) where T : class, IEntity
        {

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

                _conn.Execute(sql, DapperHelper.BuildDapperParameters(listPara.ToArray()), trans);
            }
            else
            {
                throw new Exception("Unable to find any valid DB columns");
            }
        }

        public void Save<T>(T instance, SqlTransaction trans = null) where T : class, IEntity
        {
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

        public void Delete<T>(object key, SqlTransaction trans = null) where T : class, IEntity
        {
            var attr = GetTableAttr<T>();

            string sql = $"DELETE {attr.Name} WHERE {attr.Key} = @key";

            _conn.Execute(sql, new { key }, trans);
        }

        public void Delete<T>(T instance, SqlTransaction trans = null) where T : class, IEntity
        {
            var key = instance.GetType().GetProperty("ID").GetValue(instance, null);

            Delete<T>(key, trans);
        }

        public static DbSchema GetTableAttr<T>() where T : class
        {
            var attr = Attribute.GetCustomAttribute(typeof(T), typeof(DbSchema)) as DbSchema;
            return attr ?? new DbSchema(nameof(T));
        }

        public static DbColumn GetColumnAttr(PropertyInfo pi)
        {
            return (DbColumn)Attribute.GetCustomAttribute(pi, typeof(DbColumn));
        }

        private static DbColumn GetColumnAttr<T>(string name) where T : class
        {
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
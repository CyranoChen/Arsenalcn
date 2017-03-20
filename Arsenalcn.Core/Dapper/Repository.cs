using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Dapper;

namespace Arsenalcn.Core
{
    public class Repository : IRepository
    {
        private IDapperHelper _dapper;
        private IDapperHelper Dapper => _dapper ?? new DapperHelper();

        public Repository() { }

        public Repository(string connection)
        {
            _dapper = new DapperHelper(connection);
        }

        public T Single<T>(object key, IDbTransaction trans = null) where T : class, IEntity, new()
        {
            var attr = GetTableAttr<T>();

            string sql = $"SELECT {BuildSelectSqlColumn(attr)} FROM {attr.Name} WHERE {attr.Key} = @key";

            var instance = Dapper.QueryFirstOrDefault<T>(sql, new { key }, trans);

            instance?.Inital();

            return instance;
        }

        public T Single<T>(Expression<Func<T, bool>> whereBy, IDbTransaction trans = null) where T : class, IDao, new()
        {
            var attr = GetTableAttr<T>();

            var sql = new StringBuilder();
            sql.Append($"SELECT {BuildSelectSqlColumn(attr)} FROM {attr.Name}");

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

            var instance = Dapper.QueryFirstOrDefault<T>(sql.ToString(), condition.DapperArguments, trans);

            instance?.Inital();

            return instance;
        }

        public int Count<T>(Expression<Func<T, bool>> whereBy, IDbTransaction trans = null) where T : class, IDao
        {
            var attr = GetTableAttr<T>();

            var sql = new StringBuilder();
            sql.Append($"SELECT COUNT(*) FROM {attr.Name}");

            var condition = new ConditionBuilder();
            condition.Build(whereBy.Body);

            if (!string.IsNullOrEmpty(condition.Condition))
            {
                sql.Append(" WHERE " + condition.Condition);
            }

            return Dapper.ExecuteScalar<int>(sql.ToString(), condition.DapperArguments, trans);
        }

        public bool Any<T>(object key, IDbTransaction trans = null) where T : class, IEntity
        {
            var attr = GetTableAttr<T>();

            string sql = $"SELECT COUNT(*) FROM {attr.Name} WHERE {attr.Key} = @key";

            return Dapper.ExecuteScalar<int>(sql, new { key }, trans) > 0;
        }

        public bool Any<T>(Expression<Func<T, bool>> whereBy, IDbTransaction trans = null) where T : class, IDao
        {
            return Count(whereBy, trans) > 0;
        }

        public List<T> All<T>(IDbTransaction trans = null) where T : class, IDao, new()
        {
            var attr = GetTableAttr<T>();

            var sql = new StringBuilder();
            sql.Append($"SELECT {BuildSelectSqlColumn(attr)} FROM {attr.Name}");

            if (!string.IsNullOrEmpty(attr.Sort))
            {
                sql.Append(" ORDER BY " + attr.Sort);
            }

            var list = Dapper.Query<T>(sql.ToString(), trans).ToList();

            if (list.Count > 0) { list.ForEach(x => x.Inital()); }

            return list;
        }

        public List<T> All<T>(IPager pager, string orderBy = null, IDbTransaction trans = null) where T : class, IDao, new()
        {
            var attr = GetTableAttr<T>();

            var strOrderBy = !string.IsNullOrEmpty(attr.Sort) ? attr.Sort : attr.Key;

            if (!string.IsNullOrEmpty(orderBy))
            {
                strOrderBy = orderBy;
            }

            // Get TotalCount First
            var countSql = $"SELECT COUNT({attr.Key}) AS TotalCount FROM {attr.Name}";

            pager.SetTotalCount(Dapper.ExecuteScalar<int>(countSql, trans));

            // Get Query Result
            var innerSql = $"SELECT ROW_NUMBER() OVER(ORDER BY {strOrderBy}) AS RowNo, * FROM {attr.Name}";

            string sql =
                $"SELECT {BuildSelectSqlColumn(attr)} FROM ({innerSql}) AS t WHERE t.RowNo BETWEEN {pager.CurrentPage * pager.PagingSize + 1} AND {(pager.CurrentPage + 1) * pager.PagingSize};";

            //sql += string.Format("SELECT COUNT({1}) AS TotalCount FROM {0}", attr.Name, attr.Key);

            var list = Dapper.Query<T>(sql, trans).ToList();

            if (list.Count > 0) { list.ForEach(x => x.Inital()); }

            return list;
        }

        public List<T> Query<T>(Expression<Func<T, bool>> whereBy, IDbTransaction trans = null) where T : class, IDao, new()
        {
            var attr = GetTableAttr<T>();

            var sql = new StringBuilder();
            sql.Append($"SELECT {BuildSelectSqlColumn(attr)} FROM {attr.Name}");

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

            var list = Dapper.Query<T>(sql.ToString(), condition.DapperArguments, trans).ToList();

            if (list.Count > 0) { list.ForEach(x => x.Inital()); }

            return list;
        }

        public List<T> Query<T>(IPager pager, Expression<Func<T, bool>> whereBy, string orderBy = null, IDbTransaction trans = null)
            where T : class, IDao, new()
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

            pager.SetTotalCount(Dapper.ExecuteScalar<int>(countSql, condition.DapperArguments, trans));

            // Build Sql and Execute
            var innerSql = $"SELECT ROW_NUMBER() OVER(ORDER BY {strOrderBy}) AS RowNo, * FROM {attr.Name} WHERE {condition.Condition}";

            string sql =
                $"SELECT {BuildSelectSqlColumn(attr)} FROM ({innerSql}) AS t WHERE t.RowNo BETWEEN {pager.CurrentPage * pager.PagingSize + 1} AND {(pager.CurrentPage + 1) * pager.PagingSize}";

            var list = Dapper.Query<T>(sql, condition.DapperArguments, trans).ToList();

            if (list.Count > 0) { list.ForEach(x => x.Inital()); }

            return list;
        }

        public List<T> Query<T>(Criteria criteria, IDbTransaction trans = null) where T : class, IDao, new()
        {
            var attr = GetTableAttr<T>();

            var strOrderBy = " ORDER BY " + (!string.IsNullOrEmpty(criteria?.OrderClause) ? criteria.OrderClause : attr.Sort);

            var strWhere = criteria?.GetWhereClause();

            if (!string.IsNullOrEmpty(strWhere))
            {
                strWhere = " WHERE " + strWhere;
            }

            string sql;

            if (criteria?.PagingSize > 0)
            {
                // Get TotalCount First
                var countSql = $"SELECT COUNT(*) AS TotalCount FROM {attr.Name} {strWhere}";

                criteria.SetTotalCount(new DapperHelper().ExecuteScalar<int>(countSql, criteria.Parameters, trans));

                // Get Query Result
                var innerSql =
                    $"SELECT ROW_NUMBER() OVER({strOrderBy}) AS RowNo, * FROM {attr.Name} {strWhere}";

                sql =
                    $"SELECT {BuildSelectSqlColumn(attr)} FROM ({innerSql}) AS t WHERE t.RowNo BETWEEN {criteria.CurrentPage * criteria.PagingSize + 1} AND {(criteria.CurrentPage + 1) * criteria.PagingSize}";
            }
            else
            {
                sql = $"SELECT {BuildSelectSqlColumn(attr)} FROM {attr.Name} {strWhere} {strOrderBy}";
            }

            var list = Dapper.Query<T>(sql, criteria?.Parameters, trans).ToList();

            if (list.Count > 0) { list.ForEach(x => x.Inital()); }

            return list;
        }

        public int Insert<T>(T instance, IDbTransaction trans = null) where T : class, IDao
        {
            var listCol = new List<string>();
            var listColPara = new List<string>();
            var sqlPara = new DynamicParameters(new { });

            foreach (var pi in instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var attrCol = GetColumnAttr(pi);

                if (attrCol != null)
                {
                    var value = pi.GetValue(instance, null);

                    listCol.Add(attrCol.Name);
                    listColPara.Add("@" + attrCol.Name);

                    sqlPara.Add(attrCol.Name, value == null && pi.PropertyType == typeof(string) ? string.Empty : value);
                }
            }

            var attr = GetTableAttr<T>();

            if (listCol.Count > 0 && listColPara.Count > 0 && sqlPara.ParameterNames.Any())
            {
                if (instance is IEntity)
                {
                    // skip the property of the self-increase main-key
                    var primary = instance.GetType().GetProperty("ID");

                    if (primary.PropertyType != typeof(int))
                    {
                        listCol.Add(attr.Key);
                        listColPara.Add("@key");
                        sqlPara.Add("key", primary.GetValue(instance, null));
                    }
                }

                string sql = $@"INSERT INTO {attr.Name} ({string.Join(", ", listCol.ToArray())}) 
                                     VALUES ({string.Join(", ", listColPara.ToArray())})";

                return Dapper.Execute(sql, sqlPara, trans);
            }

            return -1;
        }

        public int Insert<T>(T instance, out object key, IDbTransaction trans = null) where T : class, IEntity
        {
            var listCol = new List<string>();
            var listColPara = new List<string>();
            var sqlPara = new DynamicParameters(new { });
            key = null;

            foreach (var pi in instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var attrCol = GetColumnAttr(pi);

                if (attrCol != null)
                {
                    var value = pi.GetValue(instance, null);

                    listCol.Add(attrCol.Name);
                    listColPara.Add("@" + attrCol.Name);

                    sqlPara.Add(attrCol.Name, value == null && pi.PropertyType == typeof(string) ? string.Empty : value);
                }
            }

            var attr = GetTableAttr<T>();

            if (listCol.Count > 0 && listColPara.Count > 0 && sqlPara.ParameterNames.Any())
            {
                // skip the property of the self-increase main-key
                var primary = instance.GetType().GetProperty("ID");

                string sql;

                if (primary.PropertyType == typeof(int))
                {
                    sql = $"INSERT INTO {attr.Name} ({string.Join(", ", listCol.ToArray())}) VALUES ({string.Join(", ", listColPara.ToArray())}); SELECT SCOPE_IDENTITY();";

                    key = Dapper.ExecuteScalar(sql, sqlPara, trans);

                    return 1;
                }
                else
                {
                    listCol.Add(attr.Key);
                    listColPara.Add("@key");

                    key = primary.GetValue(instance, null);
                    sqlPara.Add("key", key);

                    sql = $"INSERT INTO {attr.Name} ({string.Join(", ", listCol.ToArray())}) VALUES ({string.Join(", ", listColPara.ToArray())})";

                    return Dapper.Execute(sql, sqlPara, trans);
                }
            }

            return -1;
        }

        public int Update<T>(T instance, IDbTransaction trans = null) where T : class, IEntity
        {
            var listCol = new List<string>();
            var sqlPara = new DynamicParameters(new { });

            foreach (var pi in instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var attrCol = GetColumnAttr(pi);

                if (attrCol != null)
                {
                    var value = pi.GetValue(instance, null);

                    listCol.Add($"{attrCol.Name} = @{attrCol.Name}");
                    sqlPara.Add(attrCol.Name, value == null && pi.PropertyType == typeof(string) ? string.Empty : value);
                }
            }

            var attr = GetTableAttr<T>();

            if (listCol.Count > 0 && sqlPara.ParameterNames.Any())
            {
                string sql = $"UPDATE {attr.Name} SET {string.Join(", ", listCol.ToArray())} WHERE {attr.Key} = @key";

                sqlPara.Add("key", instance.GetType().GetProperty("ID").GetValue(instance, null));

                return Dapper.Execute(sql, sqlPara, trans);
            }

            return -1;
        }

        public int Update<T>(T instance, Expression<Func<T, bool>> whereBy, IDbTransaction trans = null) where T : class, IDao
        {
            var listCol = new List<string>();

            var sqlPara = new DynamicParameters(new { });

            foreach (var pi in instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var attrCol = GetColumnAttr(pi);

                if (attrCol != null)
                {
                    var value = pi.GetValue(instance, null);

                    listCol.Add($"{attrCol.Name} = @{attrCol.Name}");
                    sqlPara.Add(attrCol.Name, value == null && pi.PropertyType == typeof(string) ? string.Empty : value);
                }
            }

            var attr = GetTableAttr<T>();

            if (listCol.Count > 0 && sqlPara.ParameterNames.Any())
            {
                var sql = new StringBuilder();

                sql.Append($"UPDATE {attr.Name} SET {string.Join(", ", listCol.ToArray())}");

                var condition = new ConditionBuilder();
                condition.Build(whereBy.Body);

                if (!string.IsNullOrEmpty(condition.Condition))
                {
                    sql.Append(" WHERE " + condition.Condition);
                }

                foreach (var p in condition.DapperArguments)
                {
                    sqlPara.Add(p.Key, p.Value);
                }

                return Dapper.Execute(sql.ToString(), sqlPara, trans);
            }

            return -1;
        }

        public int Save<T>(T instance, out object key, IDbTransaction trans = null) where T : class, IEntity
        {
            key = instance.GetType().GetProperty("ID").GetValue(instance, null);

            if (Any<T>(key, trans))
            {
                return Update(instance, trans);
            }
            else
            {
                return Insert(instance, out key, trans);
            }
        }

        public int Save<T>(T instance, Expression<Func<T, bool>> whereBy, IDbTransaction trans = null) where T : class, IDao
        {
            if (Any(whereBy, trans))
            {
                return Update(instance, whereBy, trans);
            }
            else
            {
                return Insert(instance, trans);
            }
        }

        public int Delete<T>(object key, IDbTransaction trans = null) where T : class, IEntity
        {
            var attr = GetTableAttr<T>();

            string sql = $"DELETE {attr.Name} WHERE {attr.Key} = @key";

            return Dapper.Execute(sql, new { key }, trans);
        }

        public int Delete<T>(T instance, IDbTransaction trans = null) where T : class, IEntity
        {
            var key = instance.GetType().GetProperty("ID").GetValue(instance, null);

            return Delete<T>(key, trans);
        }

        public int Delete<T>(Expression<Func<T, bool>> whereBy, IDbTransaction trans = null) where T : class, IDao
        {
            var attr = GetTableAttr<T>();

            var sql = new StringBuilder();
            sql.Append($"DELETE {attr.Name}");

            var condition = new ConditionBuilder();
            condition.Build(whereBy.Body);

            if (!string.IsNullOrEmpty(condition.Condition))
            {
                sql.Append(" WHERE " + condition.Condition);
            }

            return Dapper.Execute(sql.ToString(), condition.DapperArguments, trans);
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

        private string BuildSelectSqlColumn(DbSchema attr)
        {
            return attr.Key != "ID" ? $" {attr.Key} AS ID, * " : " * ";
        }
    }
}
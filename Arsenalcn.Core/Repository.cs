using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
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

        public DataTable Select<T>()
        {
            var attr = Entity.GetTableAttr<T>();

            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("SELECT * FROM {0}  ", attr.Name);

            if (!string.IsNullOrEmpty(attr.Sort))
            {
                sql.AppendFormat("ORDER BY {0}", attr.Sort);
            }

            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql.ToString());

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }

        public DataRow Select<T>(object key)
        {
            Contract.Requires(key != null);

            var attr = Entity.GetTableAttr<T>();

            string sql = string.Format("SELECT * FROM {0} WHERE {1} = @key", attr.Name, attr.Key);

            SqlParameter[] para = { new SqlParameter("@key", key) };

            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, para);

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0].Rows[0];
        }

        public void Insert<T>(T instance, SqlTransaction trans = null)
        {
            Contract.Requires(instance != null);

            List<string> listCol = new List<string>();
            List<string> listColPara = new List<string>();
            List<SqlParameter> listPara = new List<SqlParameter>();

            foreach (var pi in instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var attrCol = Entity.GetColumnAttr(pi);

                if (attrCol != null)
                {
                    object _value = pi.GetValue(instance, null);

                    // skip the property of the self-increase main-key
                    if (attrCol.IsKey)
                    {
                        var type = Nullable.GetUnderlyingType(pi.PropertyType) ?? pi.PropertyType;
                        if (type.Equals(typeof(int))) { continue; }
                    }

                    listCol.Add(attrCol.Name);
                    listColPara.Add("@" + attrCol.Name);

                    SqlParameter _para = new SqlParameter("@" + attrCol.Name,
                        _value != null ? _value : (object)DBNull.Value);
                    listPara.Add(_para);
                }
                else
                { continue; }
            }

            if (listCol.Count > 0 && listColPara.Count > 0 && listPara.Count > 0)
            {
                string sql = string.Format("INSERT INTO {0} ({1}) VALUES ({2})", Entity.GetTableAttr<T>().Name,
                    string.Join(", ", listCol.ToArray()), string.Join(", ", listColPara.ToArray()));

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

        public void Update<T>(T instance, SqlTransaction trans = null)
        {
            Contract.Requires(instance != null);

            List<string> listCol = new List<string>();
            List<SqlParameter> listPara = new List<SqlParameter>();

            foreach (var propertyInfo in instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var attrCol = Entity.GetColumnAttr(propertyInfo);

                if (attrCol != null)
                {
                    object _value = propertyInfo.GetValue(instance, null);

                    listCol.Add(string.Format("{0} = @{0}", attrCol.Name));

                    SqlParameter _para = new SqlParameter("@" + attrCol.Name,
                        _value != null ? _value : (object)DBNull.Value);
                    listPara.Add(_para);
                }
                else
                { continue; }
            }

            if (listCol.Count > 0 && listPara.Count > 0)
            {
                string sql = string.Format("UPDATE {0} SET {1} WHERE {2} = @{2}",
                    Entity.GetTableAttr<T>().Name, string.Join(", ", listCol.ToArray()), Entity.GetTableAttr<T>().Key);

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

        public void Delete<T>(object key, SqlTransaction trans = null)
        {
            Contract.Requires(key != null);

            string sql = string.Format("DELETE {0} WHERE {1} = @key",
                Entity.GetTableAttr<T>().Name, Entity.GetTableAttr<T>().Key);

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
    }
}
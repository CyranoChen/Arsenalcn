﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

using Microsoft.ApplicationBlocks.Data;

namespace Arsenalcn.Core
{
    public class Repository : IRepository
    {
        private readonly string conn;

        public Repository()
        {
            conn = ConfigurationManager.ConnectionStrings["Arsenalcn.ConnectionString"].ConnectionString; ;
        }

        public DataTable Select<T>()
        {
            var attr = (AttrDbTable)Attribute.GetCustomAttribute(typeof(T), typeof(AttrDbTable));

            string sql = string.Format("SELECT * FROM {0}", attr.Name);

            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }

        public DataRow Select<T>(object key)
        {
            var attr = (AttrDbTable)Attribute.GetCustomAttribute(typeof(T), typeof(AttrDbTable));

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
            var attr = (AttrDbTable)Attribute.GetCustomAttribute(typeof(T), typeof(AttrDbTable));

            List<string> listCol = new List<string>();
            List<string> listColPara = new List<string>();
            List<SqlParameter> listPara = new List<SqlParameter>();

            foreach (var properInfo in instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var attrCol = (AttrDbColumn)Attribute.GetCustomAttribute(properInfo, typeof(AttrDbColumn));

                if (attrCol != null)
                {
                    listCol.Add(attrCol.Name);
                    listColPara.Add("@" + attrCol.Name);

                    object _value = properInfo.GetValue(instance, null);
                    SqlParameter _para = new SqlParameter("@" + attrCol.Name,
                        _value != null ? _value : (object)DBNull.Value);
                    listPara.Add(_para);
                }
                else
                { continue; }
            }

            if (listCol.Count > 0 && listColPara.Count > 0 && listPara.Count > 0)
            {
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
            }
            else { throw new Exception("Unable to find any valid DB columns"); }
        }

        public void Update<T>(T instance, SqlTransaction trans = null)
        {
            var attr = (AttrDbTable)Attribute.GetCustomAttribute(typeof(T), typeof(AttrDbTable));

            List<string> listCol = new List<string>();
            List<SqlParameter> listPara = new List<SqlParameter>();

            foreach (var properInfo in instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var attrCol = (AttrDbColumn)Attribute.GetCustomAttribute(properInfo, typeof(AttrDbColumn));

                if (attrCol != null)
                {
                    listCol.Add(string.Format("{0} = @{0}", attrCol.Name));

                    object _value = properInfo.GetValue(instance, null);
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
                    attr.Name, string.Join(", ", listCol.ToArray()), attr.Key);

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
            var attr = (AttrDbTable)Attribute.GetCustomAttribute(typeof(T), typeof(AttrDbTable));

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

        public void Delete<T>(T instance, SqlTransaction trans = null)
        {
            var attr = (AttrDbTable)Attribute.GetCustomAttribute(typeof(T), typeof(AttrDbTable));

            var key = instance.GetType().GetProperty(attr.Key.ToString()).GetValue(instance, null);

            Delete<T>(key, trans);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace Arsenalcn.Core
{
    public class DapperHelper : IDapperHelper, IDisposable
    {
        private static string _connectionString;
        public static string ConnectionString => _connectionString ?? (_connectionString =
            ConfigurationManager.ConnectionStrings["Arsenalcn.ConnectionString"].ConnectionString);

        public static readonly IDbConnection Connection = GetOpenConnection();
        public static readonly IDbConnection MarsConnection = GetOpenConnection(true);

        private bool DebugMode => ConfigurationManager.AppSettings["DebugMode"] != null &&
                Convert.ToBoolean(ConfigurationManager.AppSettings["DebugMode"]);
        private int CommandTimeout => 90;

        public DapperHelper() { }

        public DapperHelper(string conn)
        {
            _connectionString = conn;
        }

        private static SqlConnection GetOpenConnection(bool mars = false)
        {
            var cs = ConnectionString;
            if (mars)
            {
                var scsb = new SqlConnectionStringBuilder(cs)
                {
                    MultipleActiveResultSets = true
                };

                cs = scsb.ConnectionString;
            }
            var connection = new SqlConnection(cs);
            connection.Open();
            return connection;
        }

        private static SqlConnection GetClosedConnection()
        {
            var conn = new SqlConnection(ConnectionString);
            if (conn.State != ConnectionState.Closed) throw new InvalidOperationException("should be closed!");
            return conn;
        }

        public static object BuildDapperParameters(SqlParameter[] para)
        {
            var args = new DynamicParameters(new { });

            foreach (var p in para)
            {
                args.Add(p.ParameterName, p.Value != DBNull.Value ? p.Value : null);
            }

            return args;
        }

        public int Execute(string sql, object para = null, IDbTransaction trans = null, CommandType? commandType = null)
        {
            return MarsConnection.Execute(sql, para, trans, CommandTimeout, commandType);
        }

        public IDataReader ExecuteReader(string sql, object para = null, IDbTransaction trans = null, CommandType? commandType = null)
        {
            return MarsConnection.ExecuteReader(sql, para, trans, CommandTimeout, commandType);
        }

        public DataTable ExecuteDataTable(string sql, object para = null, IDbTransaction trans = null, CommandType? commandType = null)
        {
            using (var reader = MarsConnection.ExecuteReader(sql, para, trans, CommandTimeout, commandType))
            {
                var dt = new DataTable();

                var intFieldCount = reader.FieldCount;

                for (var intCounter = 0; intCounter < intFieldCount; ++intCounter)
                {
                    dt.Columns.Add(reader.GetName(intCounter).ToUpper(), reader.GetFieldType(intCounter));
                }

                dt.BeginLoadData();

                var values = new object[intFieldCount];
                while (reader.Read())
                {
                    reader.GetValues(values);
                    dt.LoadDataRow(values, true);
                }

                dt.EndLoadData();

                return dt;
            }
        }

        public object ExecuteScalar(string sql, object para = null, IDbTransaction trans = null, CommandType? commandType = null)
        {
            return MarsConnection.ExecuteScalar(sql, para, trans, CommandTimeout, commandType);
        }

        public T ExecuteScalar<T>(string sql, object para = null, IDbTransaction trans = null, CommandType? commandType = null)
        {
            return MarsConnection.ExecuteScalar<T>(sql, para, trans, CommandTimeout, commandType);
        }

        public T QueryFirstOrDefault<T>(string sql, object para = null, IDbTransaction trans = null, CommandType? commandType = null)
        {
            return MarsConnection.QueryFirstOrDefault<T>(sql, para, trans, CommandTimeout, commandType);
        }

        public IEnumerable<dynamic> Query(string sql, object para = null, IDbTransaction trans = null,
            CommandType? commandType = null)
        {
            return MarsConnection.Query(sql, para, trans, true, CommandTimeout, commandType);
        }

        public IEnumerable<T> Query<T>(string sql, object para = null, IDbTransaction trans = null,
            CommandType? commandType = null)
        {
            return MarsConnection.Query<T>(sql, para, trans, true, CommandTimeout, commandType);
        }

        public IEnumerable<T> Query<T1, T2, T>(string sql, Func<T1, T2, T> map,
            object para = null, string splitOn = "Id", CommandType? commandType = null)
        {
            return MarsConnection.Query(sql, map, para, null, true, splitOn, CommandTimeout, commandType);
        }

        public IEnumerable<T> Query<T1, T2, T3, T>(string sql, Func<T1, T2, T3, T> map,
            object para = null, string splitOn = "Id", CommandType? commandType = null)
        {
            return MarsConnection.Query(sql, map, para, null, true, splitOn, CommandTimeout, commandType);
        }

        public IEnumerable<T> Query<T1, T2, T3, T4, T>(string sql, Func<T1, T2, T3, T4, T> map,
            object para = null, string splitOn = "Id", CommandType? commandType = null)
        {
            return MarsConnection.Query(sql, map, para, null, true, splitOn, CommandTimeout, commandType);
        }

        public IEnumerable<T> Query<T1, T2, T3, T4, T5, T>(string sql, Func<T1, T2, T3, T4, T5, T> map,
            object para = null, string splitOn = "Id", CommandType? commandType = null)
        {
            return MarsConnection.Query(sql, map, para, null, true, splitOn, CommandTimeout, commandType);
        }

        public IEnumerable<T> Query<T1, T2, T3, T4, T5, T6, T>(string sql, Func<T1, T2, T3, T4, T5, T6, T> map,
            object para = null, string splitOn = "Id", CommandType? commandType = null)
        {
            return MarsConnection.Query(sql, map, para, null, true, splitOn, CommandTimeout, commandType);
        }

        public void Dispose()
        {
            Connection?.Dispose();
            MarsConnection?.Dispose();
        }
    }
}
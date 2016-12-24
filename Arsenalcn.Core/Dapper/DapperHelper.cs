using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Arsenalcn.Core.Logger;
using Dapper;

namespace Arsenalcn.Core
{
    public class DapperHelper : IDisposable
    {
        public static readonly string ConnectionString;

        private IDbConnection _connection, _marsConnection;

        private IDbConnection Connection => _connection ?? (_connection = GetOpenConnection());
        private IDbConnection MarsConnection => _marsConnection ?? (_marsConnection = GetOpenConnection(true));

        private static bool _debugMode;
        private static int _commandTimeout;
        private static ILog _log = new DaoLog();

        static DapperHelper()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["Arsenalcn.ConnectionString"].ConnectionString;

            _commandTimeout = 90;

            _debugMode = ConfigurationManager.AppSettings["DebugMode"] != null &&
                Convert.ToBoolean(ConfigurationManager.AppSettings["DebugMode"]);
        }

        public static SqlConnection GetOpenConnection(bool mars = false)
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

        public static SqlConnection GetClosedConnection()
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

        public int Execute(string sql, object para = null, IDbTransaction tran = null, CommandType? commandType = null)
        {
            return Connection.Execute(sql, para, tran, _commandTimeout, commandType);
        }

        public IDataReader ExecuteReader(string sql, object para = null, IDbTransaction tran = null, CommandType? commandType = null)
        {
            return Connection.ExecuteReader(sql, para, tran, _commandTimeout, commandType);
        }

        public DataTable ExecuteDataTable(string sql, object para = null, IDbTransaction tran = null, CommandType? commandType = null)
        {
            using (var reader = Connection.ExecuteReader(sql, para, tran, _commandTimeout, commandType))
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

        public T ExecuteScalar<T>(string sql, object para = null, IDbTransaction tran = null, CommandType? commandType = null)
        {
            return Connection.ExecuteScalar<T>(sql, para, tran, _commandTimeout, commandType);
        }

        public void Dispose()
        {
            Connection?.Dispose();
        }
    }
}
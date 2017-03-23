using System;
using System.Collections.Generic;
using System.Data;

namespace Arsenalcn.Core
{
    public interface IDapperHelper
    {
        int Execute(string sql, object para = null, IDbTransaction trans = null, CommandType? commandType = null);

        IDataReader ExecuteReader(string sql, object para = null, IDbTransaction trans = null, CommandType? commandType = null);
        DataTable ExecuteDataTable(string sql, object para = null, IDbTransaction trans = null,
            CommandType? commandType = null);

        object ExecuteScalar(string sql, object para = null, IDbTransaction trans = null, CommandType? commandType = null);
        T ExecuteScalar<T>(string sql, object para = null, IDbTransaction trans = null, CommandType? commandType = null);

        T QueryFirstOrDefault<T>(string sql, object para = null, IDbTransaction trans = null,
            CommandType? commandType = null);

        IEnumerable<dynamic> Query(string sql, object para = null, IDbTransaction trans = null,
            CommandType? commandType = null);

        IEnumerable<T> Query<T>(string sql, object para = null, IDbTransaction trans = null,
            CommandType? commandType = null);

        IEnumerable<T> Query<T1, T2, T>(string sql, Func<T1, T2, T> map,
            object para = null, IDbTransaction trans = null, string splitOn = "Id", CommandType? commandType = null);

        IEnumerable<T> Query<T1, T2, T3, T>(string sql, Func<T1, T2, T3, T> map,
            object para = null, IDbTransaction trans = null, string splitOn = "Id", CommandType? commandType = null);

        IEnumerable<T> Query<T1, T2, T3, T4, T>(string sql, Func<T1, T2, T3, T4, T> map,
            object para = null, IDbTransaction trans = null, string splitOn = "Id", CommandType? commandType = null);

        IEnumerable<T> Query<T1, T2, T3, T4, T5, T>(string sql, Func<T1, T2, T3, T4, T5, T> map,
            object para = null, IDbTransaction trans = null, string splitOn = "Id", CommandType? commandType = null);

        IEnumerable<T> Query<T1, T2, T3, T4, T5, T6, T>(string sql, Func<T1, T2, T3, T4, T5, T6, T> map,
            object para = null, IDbTransaction trans = null, string splitOn = "Id", CommandType? commandType = null);
    }
}

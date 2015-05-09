using System;
using System.Data;
using System.Data.SqlClient;

namespace Arsenalcn.Core
{
    public interface IRepository
    {
        DataTable Select<T>();

        DataRow Select<T>(object key);

        void Insert<T>(T instance, SqlTransaction trans = null);

        void Update<T>(T instance, SqlTransaction trans = null);

        void Delete<T>(object key, SqlTransaction trans = null);
    }
}
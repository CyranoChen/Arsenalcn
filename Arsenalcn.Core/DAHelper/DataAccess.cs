using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Threading;

using Arsenalcn.Core.Logger;
using Microsoft.ApplicationBlocks.Data;

namespace Arsenalcn.Core
{
    public static class DataAccess
    {
        public static string ConnectString;

        static DataAccess()
        {
            ConnectString = ConfigurationManager.ConnectionStrings["Arsenalcn.ConnectionString"].ConnectionString;
        }

        public static DataSet ExecuteDataset(string sql, SqlParameter[] para = null)
        {
            ILog log = new DaoLog();

            try
            {
                Contract.Requires(!string.IsNullOrEmpty(sql));

                DataSet ds = SqlHelper.ExecuteDataset(ConnectString, CommandType.Text, sql, para);

                log.Debug(sql, new LogInfo()
                {
                    MethodInstance = MethodBase.GetCurrentMethod(),
                    ThreadInstance = Thread.CurrentThread
                });

                return ds;
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

        public static void ExecuteNonQuery(string sql, SqlParameter[] para = null, SqlTransaction trans = null)
        {
            ILog log = new DaoLog();

            try
            {
                Contract.Requires(!string.IsNullOrEmpty(sql));

                if (trans != null)
                {
                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, para);
                }
                else
                {
                    SqlHelper.ExecuteNonQuery(ConnectString, CommandType.Text, sql, para);
                }

                log.Debug(sql, new LogInfo()
                {
                    MethodInstance = MethodBase.GetCurrentMethod(),
                    ThreadInstance = Thread.CurrentThread
                });
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

        public static object ExecuteScalar(string sql, SqlParameter[] para = null, SqlTransaction trans = null)
        {
            ILog log = new DaoLog();
            object key;

            try
            {
                Contract.Requires(!string.IsNullOrEmpty(sql));

                if (trans != null)
                {
                    key = SqlHelper.ExecuteScalar(trans, CommandType.Text, sql, para);
                }
                else
                {
                    key = SqlHelper.ExecuteScalar(ConnectString, CommandType.Text, sql, para);
                }

                log.Debug(sql, new LogInfo()
                {
                    MethodInstance = MethodBase.GetCurrentMethod(),
                    ThreadInstance = Thread.CurrentThread
                });

                return key;
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
    }
}

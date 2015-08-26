using System.Data;
using System.Data.SqlClient;

using Microsoft.ApplicationBlocks.Data;

namespace Arsenalcn.Common.DataAccess
{
    public class LogEvent
    {
        public static DataRow GetLogEventByID(int logID)
        {
            var sql = "SELECT * FROM dbo.Arsenalcn_LogEvent WHERE ID = @logID";

            var ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@logID", logID));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0].Rows[0];
        }

        public static void UpdateLogEvent(int logID, string eventType, string message, string errorStackTrace, string errorParam)
        {
            var sql = "UPDATE dbo.Arsenalcn_LogEvent SET EventType = @eventType, Message = @message, ErrorStackTrace = @errorStackTrace, ErrorParam = @errorParam WHERE ID = @logID";

            SqlParameter[] para = { new SqlParameter("@eventType", eventType), new SqlParameter("@message", message), new SqlParameter("@errorStackTrace", errorStackTrace), new SqlParameter("@errorParam", errorParam), new SqlParameter("@logID", logID) };

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static void InsertLogEvent(string eventType, string message, string errorStackTrace, string errorParam)
        {
            var sql = "INSERT INTO dbo.Arsenalcn_LogEvent (EventType, Message, ErrorStackTrace, EventDate, ErrorParam) VALUES (@eventType, @message, @errorStackTrace, GETDATE(), @errorParam)";

            SqlParameter[] para = { new SqlParameter("@eventType", eventType), new SqlParameter("@message", message), new SqlParameter("@errorStackTrace", errorStackTrace), new SqlParameter("@errorParam", errorParam) };

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static void DeleteLogEvent(int logID)
        {
            var sql = "DELETE dbo.Arsenalcn_LogEvent WHERE ID = @logID";

            SqlParameter[] para = { new SqlParameter("@logID", logID) };

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static DataTable GetLogEvents()
        {
            var sql = @"SELECT ID, EventType, Message, ErrorStackTrace, EventDate, ErrorParam FROM dbo.Arsenalcn_LogEvent ORDER BY EventDate DESC";

            var ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }
    }
}

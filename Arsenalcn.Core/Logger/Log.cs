using System;
using System.Data.SqlClient;
using System.Reflection;
using System.Threading;

namespace Arsenalcn.Core.Logger
{
    [AttrDbTable("Arsenalcn_Log", Sort = "ID DESC")]
    public abstract class Log
    {
        #region Members and Properties

        [AttrDbColumn("ID", Key = true)]
        public int ID
        { get; set; }

        [AttrDbColumn("Logger")]
        public string Logger
        { get; set; }

        [AttrDbColumn("CreateTime")]
        public DateTime CreateTime
        { get; set; }

        [AttrDbColumn("LogLevel")]
        public LogLevel Level
        { get; set; }

        [AttrDbColumn("Message")]
        public string Message
        { get; set; }

        [AttrDbColumn("StackTrace")]
        public string StackTrace
        { get; set; }

        [AttrDbColumn("Thread")]
        public string Thread
        { get; set; }

        [AttrDbColumn("Method")]
        public string Method
        { get; set; }

        [AttrDbColumn("UserID")]
        public int UserID
        { get; set; }

        [AttrDbColumn("UserIP")]
        public string UserIP
        { get; set; }

        [AttrDbColumn("UserBrowser")]
        public string UserBrowser
        { get; set; }

        #endregion

        protected static void Logging(string logger, DateTime createTime, LogLevel level, string message, string stackTrace, UserClientInfo userClient = null)
        {
            string sql = @"INSERT INTO {0} (Logger, CreateTime, LogLevel, Message, StackTrace, Thread, Method, UserID, UserIP, UserBrowser) 
                               VALUES (@logger, @createTime, @logLevel, @message, @stackTrace, @thread, @method, @userID, @userIP, @userBrowser)";

            sql = string.Format(sql, Repository.GetTableAttr<Log>().Name);

            SqlParameter[] para = { 
                                      new SqlParameter("@logger", logger), 
                                      new SqlParameter("@createTime", createTime), 
                                      new SqlParameter("@logLevel", level.ToString()),
                                      new SqlParameter("@message", message),
                                      new SqlParameter("@stackTrace", stackTrace),
                                      new SqlParameter("@thread", string.Empty),
                                      new SqlParameter("@method", string.Empty),
                                      new SqlParameter("@userID", userClient != null ? userClient.UserID : -1),
                                      new SqlParameter("@userIP", userClient != null ? userClient.UserIP : "127.0.0.1"),
                                      new SqlParameter("@userBrowser", userClient != null ? userClient.UserBrowser : string.Empty)
                                  };

            DataAccess.ExecuteNonQuery(sql, para);
        }

        protected static void Logging(string logger, DateTime createTime, LogLevel level, string message, string stackTrace, Thread thread, MethodBase method, UserClientInfo userClient = null)
        {
            string sql = @"INSERT INTO {0} (Logger, CreateTime, LogLevel, Message, StackTrace, Thread, Method, UserID, UserIP, UserBrowser) 
                               VALUES (@logger, @createTime, @logLevel, @message, @stackTrace, @thread, @method, @userID, @userIP, @userBrowser)";

            sql = string.Format(sql, Repository.GetTableAttr<Log>().Name);

            SqlParameter[] para = { 
                                      new SqlParameter("@logger", logger), 
                                      new SqlParameter("@createTime", createTime), 
                                      new SqlParameter("@logLevel", level.ToString()),
                                      new SqlParameter("@message", message),
                                      new SqlParameter("@stackTrace", stackTrace),
                                      new SqlParameter("@thread", thread.Name ?? thread.ManagedThreadId.ToString()),
                                      new SqlParameter("@method", method.Name ?? method.ToString()),
                                      new SqlParameter("@userID", userClient != null ? userClient.UserID : -1),
                                      new SqlParameter("@userIP", userClient != null ? userClient.UserIP : "127.0.0.1"),
                                      new SqlParameter("@userBrowser", userClient != null ? userClient.UserBrowser : string.Empty)
                                  };

            DataAccess.ExecuteNonQuery(sql, para);
        }
    }

    public class LogInfo
    {
        public LogInfo() { }

        #region Members and Properties

        public Thread ThreadInstance
        { get; set; }

        public MethodBase MethodInstance
        { get; set; }

        public UserClientInfo UserClient
        { get; set; }

        #endregion
    }

    public class UserClientInfo
    {
        public UserClientInfo() { }

        #region Members and Properties

        public int UserID
        { get; set; }

        public string UserName
        { get; set; }

        public string UserIP
        { get; set; }

        public string UserBrowser
        { get; set; }

        #endregion
    }

    /// <summary>
    //FATAL（致命错误）：记录系统中出现的能使用系统完全失去功能，服务停止，系统崩溃等使系统无法继续运行下去的错误。例如，数据库无法连接，系统出现死循环。
    //ERROR（一般错误）：记录系统中出现的导致系统不稳定，部分功能出现混乱或部分功能失效一类的错误。例如，数据字段为空，数据操作不可完成，操作出现异常等。
    //WARN（警告）：记录系统中不影响系统继续运行，但不符合系统运行正常条件，有可能引起系统错误的信息。例如，记录内容为空，数据内容不正确等。
    //INFO（一般信息）：记录系统运行中应该让用户知道的基本信息。例如，服务开始运行，功能已经开户等。
    //DEBUG （调试信息）：记录系统用于调试的一切信息，内容或者是一些关键数据内容的输出。
    /// </summary>
    public enum LogLevel
    {
        Debug,
        Info,
        Warn,
        Error,
        Fatal
    }
}

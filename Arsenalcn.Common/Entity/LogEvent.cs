using System;
using System.Collections.Generic;
using System.Data;

namespace Arsenalcn.Common.Entity
{
    public class LogEvent
    {
        public LogEvent() { }

        public LogEvent(DataRow dr)
        {
            InitLogEvent(dr);
        }

        private void InitLogEvent(DataRow dr)
        {
            if (dr != null)
            {
                LogID = Convert.ToInt32(dr["ID"]);
                EventType = (LogEventType)Enum.Parse(typeof(LogEventType), dr["EventType"].ToString());
                Message = dr["Message"].ToString();
                ErrorStackTrace = dr["ErrorStackTrace"].ToString();
                EventDate = (DateTime)(dr["EventDate"]);
                ErrorParam = dr["ErrorParam"].ToString();
            }
            else
                throw new Exception("Unable to init LogEvent.");
        }

        public void Select()
        {
            DataRow dr = DataAccess.LogEvent.GetLogEventByID(LogID);

            if (dr != null)
                InitLogEvent(dr);
        }

        public void Update()
        {
            DataAccess.LogEvent.UpdateLogEvent(LogID, EventType.ToString(), Message, ErrorStackTrace, ErrorParam);
        }

        public void Insert()
        {
            DataAccess.LogEvent.InsertLogEvent(EventType.ToString(), Message, ErrorStackTrace, ErrorParam);
        }

        public void Delete()
        {
            DataAccess.LogEvent.DeleteLogEvent(LogID);
        }

        public static List<LogEvent> GetLogEvents()
        {
            DataTable dt = DataAccess.LogEvent.GetLogEvents();
            List<LogEvent> list = new List<LogEvent>();

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new LogEvent(dr));
                }
            }

            return list;
        }

        public static void Logging(LogEventType let, string message, string errorStackTrace, string errorParam)
        {
            LogEvent l = new LogEvent();
            l.EventType = let;
            l.Message = message;
            l.ErrorStackTrace = errorStackTrace;
            l.ErrorParam = errorParam;
            l.Insert();
        }

        #region Members and Properties

        public int LogID
        { get; set; }

        public LogEventType EventType
        { get; set; }

        public string Message
        { get; set; }

        public string ErrorStackTrace
        { get; set; }

        public DateTime EventDate
        { get; set; }

        public string ErrorParam
        { get; set; }

        #endregion
    }

    public enum LogEventType
    {
        Success,
        Error
    }
}

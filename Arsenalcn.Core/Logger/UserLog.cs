using System;
using System.Threading;

namespace Arsenalcn.Core.Logger
{
    public class UserLog : Log, ILog
    {
        public UserLog() { }

        public void Debug(Thread thread, string message)
        {
            Logging(this.GetType().Name, DateTime.Now, thread, LogLevel.Debug, message);
        }

        public void Debug(Thread thread, Exception ex)
        {
            Logging(this.GetType().Name, DateTime.Now, thread, LogLevel.Debug, ex.Message, ex.StackTrace);
        }

        public void Info(Thread thread, string message)
        {
            Logging(this.GetType().Name, DateTime.Now, thread, LogLevel.Info, message);
        }

        public void Info(Thread thread, Exception ex)
        {
            Logging(this.GetType().Name, DateTime.Now, thread, LogLevel.Info, ex.Message, ex.StackTrace);
        }

        public void Warn(Thread thread, string message)
        {
            Logging(this.GetType().Name, DateTime.Now, thread, LogLevel.Warn, message);
        }

        public void Warn(Thread thread, Exception ex)
        {
            Logging(this.GetType().Name, DateTime.Now, thread, LogLevel.Warn, ex.Message, ex.StackTrace);
        }

        public void Error(Thread thread, string message)
        {
            Logging(this.GetType().Name, DateTime.Now, thread, LogLevel.Error, message);
        }

        public void Error(Thread thread, Exception ex)
        {
            Logging(this.GetType().Name, DateTime.Now, thread, LogLevel.Error, ex.Message, ex.StackTrace);
        }

        public void Fatal(Thread thread, string message)
        {
            Logging(this.GetType().Name, DateTime.Now, thread, LogLevel.Fatal, message);
        }

        public void Fatal(Thread thread, Exception ex)
        {
            Logging(this.GetType().Name, DateTime.Now, thread, LogLevel.Fatal, ex.Message, ex.StackTrace);
        }
    }
}

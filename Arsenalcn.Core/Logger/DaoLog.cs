using System;
using System.Diagnostics.Contracts;
using System.Threading;

namespace Arsenalcn.Core.Logger
{
    public class DaoLog : Log, ILog
    {
        public DaoLog() { }

        public void Debug(string message, LogInfo para = null)
        {
            if (para != null)
            {
                Logging(this.GetType().Name, DateTime.Now, LogLevel.Debug, message, string.Empty,
                    para.ThreadInstance, para.MethodInstance);
            }
            else
            {
                Logging(this.GetType().Name, DateTime.Now, LogLevel.Debug, message, string.Empty);
            }
        }

        public void Debug(Exception ex, LogInfo para = null)
        {
            Contract.Requires(ex != null);

            if (para != null)
            {
                Logging(this.GetType().Name, DateTime.Now, LogLevel.Debug, ex.Message, ex.StackTrace,
                    para.ThreadInstance, para.MethodInstance);
            }
            else
            {
                Logging(this.GetType().Name, DateTime.Now, LogLevel.Debug, ex.Message, ex.StackTrace);
            }
        }

        public void Info(Thread thread, string message)
        {
            //Logging(this.GetType().Name, DateTime.Now, thread, LogLevel.Info, message);
        }

        public void Info(Thread thread, Exception ex)
        {
            //Logging(this.GetType().Name, DateTime.Now, thread, LogLevel.Info, ex.Message, ex.StackTrace);
        }

        public void Warn(Thread thread, string message)
        {
            //Logging(this.GetType().Name, DateTime.Now, thread, LogLevel.Warn, message);
        }

        public void Warn(Thread thread, Exception ex)
        {
            //Logging(this.GetType().Name, DateTime.Now, thread, LogLevel.Warn, ex.Message, ex.StackTrace);
        }

        public void Error(Thread thread, string message)
        {
            //Logging(this.GetType().Name, DateTime.Now, thread, LogLevel.Error, message);
        }

        public void Error(Thread thread, Exception ex)
        {
            //Logging(this.GetType().Name, DateTime.Now, thread, LogLevel.Error, ex.Message, ex.StackTrace);
        }

        public void Fatal(Thread thread, string message)
        {
            //Logging(this.GetType().Name, DateTime.Now, thread, LogLevel.Fatal, message);
        }

        public void Fatal(Thread thread, Exception ex)
        {
            //Logging(this.GetType().Name, DateTime.Now, thread, LogLevel.Fatal, ex.Message, ex.StackTrace);
        }
    }
}

using System;
using System.Diagnostics.Contracts;

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

        public void Info(string message, LogInfo para = null)
        {
            if (para != null)
            {
                Logging(this.GetType().Name, DateTime.Now, LogLevel.Info, message, string.Empty,
                    para.ThreadInstance, para.MethodInstance);
            }
            else
            {
                Logging(this.GetType().Name, DateTime.Now, LogLevel.Info, message, string.Empty);
            }
        }

        public void Info(Exception ex, LogInfo para = null)
        {
            Contract.Requires(ex != null);

            if (para != null)
            {
                Logging(this.GetType().Name, DateTime.Now, LogLevel.Info, ex.Message, ex.StackTrace,
                    para.ThreadInstance, para.MethodInstance);
            }
            else
            {
                Logging(this.GetType().Name, DateTime.Now, LogLevel.Info, ex.Message, ex.StackTrace);
            }
        }

        public void Warn(string message, LogInfo para = null)
        {
            if (para != null)
            {
                Logging(this.GetType().Name, DateTime.Now, LogLevel.Warn, message, string.Empty,
                    para.ThreadInstance, para.MethodInstance);
            }
            else
            {
                Logging(this.GetType().Name, DateTime.Now, LogLevel.Warn, message, string.Empty);
            }
        }

        public void Warn(Exception ex, LogInfo para = null)
        {
            Contract.Requires(ex != null);

            if (para != null)
            {
                Logging(this.GetType().Name, DateTime.Now, LogLevel.Warn, ex.Message, ex.StackTrace,
                    para.ThreadInstance, para.MethodInstance);
            }
            else
            {
                Logging(this.GetType().Name, DateTime.Now, LogLevel.Warn, ex.Message, ex.StackTrace);
            }
        }

        public void Error(string message, LogInfo para = null)
        {
            if (para != null)
            {
                Logging(this.GetType().Name, DateTime.Now, LogLevel.Error, message, string.Empty,
                    para.ThreadInstance, para.MethodInstance);
            }
            else
            {
                Logging(this.GetType().Name, DateTime.Now, LogLevel.Error, message, string.Empty);
            }
        }

        public void Error(Exception ex, LogInfo para = null)
        {
            Contract.Requires(ex != null);

            if (para != null)
            {
                Logging(this.GetType().Name, DateTime.Now, LogLevel.Error, ex.Message, ex.StackTrace,
                    para.ThreadInstance, para.MethodInstance);
            }
            else
            {
                Logging(this.GetType().Name, DateTime.Now, LogLevel.Error, ex.Message, ex.StackTrace);
            }
        }

        public void Fatal(string message, LogInfo para = null)
        {
            if (para != null)
            {
                Logging(this.GetType().Name, DateTime.Now, LogLevel.Fatal, message, string.Empty,
                    para.ThreadInstance, para.MethodInstance);
            }
            else
            {
                Logging(this.GetType().Name, DateTime.Now, LogLevel.Fatal, message, string.Empty);
            }
        }

        public void Fatal(Exception ex, LogInfo para = null)
        {
            Contract.Requires(ex != null);

            if (para != null)
            {
                Logging(this.GetType().Name, DateTime.Now, LogLevel.Fatal, ex.Message, ex.StackTrace,
                    para.ThreadInstance, para.MethodInstance);
            }
            else
            {
                Logging(this.GetType().Name, DateTime.Now, LogLevel.Fatal, ex.Message, ex.StackTrace);
            }
        }
    }
}

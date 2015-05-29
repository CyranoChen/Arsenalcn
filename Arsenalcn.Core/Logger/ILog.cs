using System;
using System.Threading;

namespace Arsenalcn.Core.Logger
{
    public interface ILog
    {
        void Debug(string message, LogInfo para = null);
        void Debug(Exception ex, LogInfo para = null);

        void Info(Thread thread, string message);
        void Info(Thread thread, Exception ex);

        void Warn(Thread thread, string message);
        void Warn(Thread thread, Exception ex);

        void Error(Thread thread, string message);
        void Error(Thread thread, Exception ex);

        void Fatal(Thread thread, string message);
        void Fatal(Thread thread, Exception ex);
    }
}

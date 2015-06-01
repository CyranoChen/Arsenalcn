using System;
using System.Threading;

namespace Arsenalcn.Core.Logger
{
    public interface ILog
    {
        void Debug(string message, LogInfo para = null);
        void Debug(Exception ex, LogInfo para = null);

        void Info(string message, LogInfo para = null);
        void Info(Exception ex, LogInfo para = null);

        void Warn(string message, LogInfo para = null);
        void Warn(Exception ex, LogInfo para = null);

        void Error(string message, LogInfo para = null);
        void Error(Exception ex, LogInfo para = null);

        void Fatal(string message, LogInfo para = null);
        void Fatal(Exception ex, LogInfo para = null);
    }
}

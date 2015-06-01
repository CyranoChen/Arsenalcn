using System;

namespace Arsenalcn.Core.Utility
{
    public class OSInfo
    {
        public static string GetOS()
        {
            OperatingSystem osInfo = Environment.OSVersion;

            return osInfo.VersionString;
        }
    }
}

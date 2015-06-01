using System;

namespace Arsenalcn.Core.Utility
{
    public class OSInfo
    {
        public static string GetOS()
        {
            return Environment.OSVersion.VersionString;
        }
    }
}

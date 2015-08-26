using System;
using System.Collections.Generic;
using Arsenalcn.Core;

namespace Arsenal.Service
{
    public static class ConfigGlobal
    {
        public static Dictionary<string, string> ConfigDictionary
        {
            get
            {
                var currSystem = ConfigSystem.Arsenal;
                return Config.Cache.GetDictionaryByConfigSystem(currSystem);
            }
        }

        public static bool IsPluginAdmin(int userid)
        {
            var admins = ConfigGlobal.PluginAdmin;
            if (userid > 0 && admins.Length > 0)
            {
                foreach (var a in admins)
                {
                    if (a == userid.ToString())
                        return true;
                }
            }

            return false;
        }

        #region Members and Properties

        public static bool AcnSync
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(ConfigDictionary["AcnSync"]);
                }
                catch
                {
                    return true;
                }
            }
        }

        public static string APIAppKey
        {
            get
            {
                return ConfigDictionary["APIAppKey"];
            }
        }

        public static string APICryptographicKey
        {
            get
            {
                return ConfigDictionary["APICryptographicKey"];
            }
        }

        public static string APILoginURL
        {
            get
            {
                return ConfigDictionary["APILoginURL"];
            }
        }

        public static string APILogoutURL
        {
            get
            {
                return ConfigDictionary["APILogoutURL"];
            }
        }

        public static string APIServiceURL
        {
            get
            {
                return ConfigDictionary["APIServiceURL"];
            }
        }

        public static string[] PluginAdmin
        {
            get
            {
                var admins = ConfigDictionary["PluginAdmin"];
                return admins.Split('|');
            }
        }

        public static string PluginName
        {
            get
            {
                return ConfigDictionary["PluginName"];
            }
        }

        public static string PluginVersion
        {
            get
            {
                return ConfigDictionary["PluginVersion"];
            }
        }

        public static string PluginDisplayName
        {
            get
            {
                return ConfigDictionary["PluginDisplayName"];
            }
        }

        public static bool PluginActive
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(ConfigDictionary["PluginActive"]);
                }
                catch
                {
                    return false;
                }
            }
        }

        //public static bool PluginContainerActive
        //{
        //    get
        //    {
        //        try
        //        {
        //            return Convert.ToBoolean(ConfigDictionary["PluginContainerActive"]);
        //        }
        //        catch
        //        {
        //            return false;
        //        }
        //    }
        //}

        public static bool SchedulerActive
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(ConfigDictionary["SchedulerActive"]);
                }
                catch
                {
                    return false;
                }
            }
        }

        public static Guid ArsenalTeamGuid
        {
            get
            {
                try
                {
                    var tmpID = ConfigDictionary["ArsenalTeamGuid"];

                    if (!string.IsNullOrEmpty(tmpID))
                    { return new Guid(tmpID); }
                    else
                    { return Guid.Empty; }
                }
                catch
                {
                    return new Guid("036478f0-9062-4533-be33-43197a0b1568");
                }
            }
        }

        public static string ArsenalVideoUrl
        {
            get
            {
                var tmpUrl = ConfigDictionary["ArsenalVideoUrl"];

                if (!string.IsNullOrEmpty(tmpUrl))
                    return tmpUrl;
                else
                    return "http://ftp.arsenalcn.com/playervideo/";
            }
        }

        public static bool WeChatActive
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(ConfigDictionary["WeChatActive"]);
                }
                catch
                {
                    return true;
                }
            }
        }

        public static string WeChatAppKey
        {
            get
            {
                return ConfigDictionary["WeChatAppKey"];
            }
        }

        public static string WeChatAppSecret
        {
            get
            {
                return ConfigDictionary["WeChatAppSecret"];
            }
        }

        public static string WeChatServiceURL
        {
            get
            {
                return ConfigDictionary["WeChatServiceURL"];
            }
        }

        #endregion
    }
}


using System;
using System.Collections.Generic;
using System.Data.SqlClient;

using Arsenalcn.Core;

namespace Arsenal.Service
{
    public static class ConfigGlobal
    {
        static ConfigGlobal()
        {
            Init();
        }

        public static void RefreshCache()
        {
            Init();
        }

        private static void Init()
        {
            const ConfigSystem currSystem = ConfigSystem.Arsenal;
            ConfigDictionary = Config.Cache.GetDictionaryByConfigSystem(currSystem);
        }

        public static bool IsPluginAdmin(int userid)
        {
            string[] admins = ConfigGlobal.PluginAdmin;
            if (userid > 0 && admins.Length > 0)
            {
                foreach (string a in admins)
                {
                    if (a == userid.ToString())
                        return true;
                }
            }

            return false;
        }

        public static Dictionary<string, string> ConfigDictionary;

        #region Members and Properties

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
                string admins = ConfigDictionary["PluginAdmin"];
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

        public static bool PluginContainerActive
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(ConfigDictionary["PluginContainerActive"]);
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
                    string tmpID = ConfigDictionary["ArsenalTeamGuid"];

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
                string tmpUrl = ConfigDictionary["ArsenalVideoUrl"];

                if (!string.IsNullOrEmpty(tmpUrl))
                    return tmpUrl;
                else
                    return "http://ftp.arsenalcn.com/playervideo/";
            }
        }

        #endregion
    }
}


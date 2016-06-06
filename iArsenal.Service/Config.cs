using System;
using System.Collections.Generic;
using Arsenalcn.Core;

namespace iArsenal.Service
{
    public static class ConfigGlobal
    {
        static ConfigGlobal()
        {
            Init();
        }

        private static Dictionary<string, string> ConfigDictionary { get; set; }
        public static AssemblyInfo Assembly { get; set; }

        public static void Refresh()
        {
            Init();
        }

        private static void Init()
        {
            Config.Cache.RefreshCache();
            ConfigDictionary = Config.Cache.GetDictionaryByConfigSystem(ConfigSystem.iArsenal);

            Assembly = new AssemblyInfo
            {
                Title = ConfigDictionary["AssemblyTitle"],
                Description = ConfigDictionary["AssemblyDescription"],
                Configuration = ConfigDictionary["AssemblyConfiguration"],
                Company = ConfigDictionary["AssemblyCompany"],
                Product = ConfigDictionary["AssemblyProduct"],
                Copyright = ConfigDictionary["AssemblyCopyright"],
                Trademark = ConfigDictionary["AssemblyTrademark"],
                Culture = ConfigDictionary["AssemblyCulture"],
                Version = ConfigDictionary["AssemblyVersion"],
                FileVersion = ConfigDictionary["AssemblyFileVersion"]
            };
        }

        public static bool IsPluginAdmin(int userid)
        {
            var admins = PluginAdmin;
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

        public static string APIAppKey => ConfigDictionary["APIAppKey"];

        public static string APICryptographicKey => ConfigDictionary["APICryptographicKey"];

        public static string APILoginURL => ConfigDictionary["APILoginURL"];

        public static string APILogoutURL => ConfigDictionary["APILogoutURL"];

        public static string APIServiceURL => ConfigDictionary["APIServiceURL"];

        public static string AcnCasinoURL => ConfigDictionary["AcnCasinoURL"];

        public static string[] PluginAdmin
        {
            get
            {
                var admins = ConfigDictionary["PluginAdmin"];
                return admins.Split('|');
            }
        }

        public static string PluginName => ConfigDictionary["PluginName"];

        public static string PluginVersion => ConfigDictionary["PluginVersion"];

        public static string PluginDisplayName => ConfigDictionary["PluginDisplayName"];

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
                var tmpID = ConfigDictionary["ArsenalTeamGuid"];

                if (!string.IsNullOrEmpty(tmpID))
                    return new Guid(tmpID);
                return new Guid("036478f0-9062-4533-be33-43197a0b1568");
            }
        }

        public static string SysNotice => ConfigDictionary["SysNotice"];

        public static string[] BulkOrderInfo
        {
            get
            {
                var configValue = ConfigDictionary["BulkOrderInfo"];

                return configValue.Split('|');
            }
        }

        public static DateTime? DefaultMatchDate
        {
            get
            {
                try
                {
                    return Convert.ToDateTime(ConfigDictionary["DefaultMatchDate"]);
                }
                catch
                {
                    return null;
                }
            }
        }

        public static float ExchangeRateGBP
        {
            get
            {
                try
                {
                    return Convert.ToSingle(ConfigDictionary["ExchangeRateGBP"]);
                }
                catch
                {
                    return 11f;
                }
            }
        }

        public static float ExchangeRateUSD
        {
            get
            {
                try
                {
                    return Convert.ToSingle(ConfigDictionary["ExchangeRateUSD"]);
                }
                catch
                {
                    return 6.3f;
                }
            }
        }

        #endregion
    }
}
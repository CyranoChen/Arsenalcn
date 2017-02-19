using System;
using System.Collections.Generic;
using System.Linq;
using Arsenalcn.Core;

namespace Arsenal.Service
{
    public static class ConfigGlobal_Arsenal
    {
        static ConfigGlobal_Arsenal()
        {
            Init();
        }

        private static Dictionary<string, string> ConfigDictionary { get; set; }
        public static AssemblyInfo Assembly { get; private set; }

        public static void Refresh()
        {
            Init();
        }

        private static void Init()
        {
            Config.Cache.RefreshCache();
            ConfigDictionary = Config.Cache.GetDictionaryByConfigSystem(ConfigSystem.Arsenal);

            Assembly = new AssemblyInfo
            {
                Title = ConfigDictionary["AssemblyTitle"] ?? string.Empty,
                Description = ConfigDictionary["AssemblyDescription"] ?? string.Empty,
                Configuration = ConfigDictionary["AssemblyConfiguration"] ?? string.Empty,
                Company = ConfigDictionary["AssemblyCompany"] ?? string.Empty,
                Product = ConfigDictionary["AssemblyProduct"] ?? string.Empty,
                Copyright = ConfigDictionary["AssemblyCopyright"] ?? string.Empty,
                Trademark = ConfigDictionary["AssemblyTrademark"] ?? string.Empty,
                Culture = ConfigDictionary["AssemblyCulture"] ?? string.Empty,
                Version = ConfigDictionary["AssemblyVersion"] ?? string.Empty,
                FileVersion = ConfigDictionary["AssemblyFileVersion"] ?? string.Empty
            };
        }

        public static bool IsPluginAdmin(int userid)
        {
            var admins = PluginAdmin;
            if (userid > 0 && admins.Length > 0)
            {
                return admins.Any(a => a == userid.ToString());
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

        public static string APIAppKey => ConfigDictionary["APIAppKey"];

        public static string APICryptographicKey => ConfigDictionary["APICryptographicKey"];

        public static string APILoginURL => ConfigDictionary["APILoginURL"];

        public static string APILogoutURL => ConfigDictionary["APILogoutURL"];

        public static string APIServiceURL => ConfigDictionary["APIServiceURL"];

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

        public static string PluginAcnCasinoPath => ConfigDictionary["PluginAcnCasinoPath"];

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
                    var tmpId = ConfigDictionary["ArsenalTeamGuid"];

                    if (!string.IsNullOrEmpty(tmpId))
                    {
                        return new Guid(tmpId);
                    }
                    return Guid.Empty;
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

        public static string WeChatAppKey => ConfigDictionary["WeChatAppKey"];

        public static string WeChatAppSecret => ConfigDictionary["WeChatAppSecret"];

        public static string WeChatServiceURL => ConfigDictionary["WeChatServiceURL"];

        #endregion
    }

    public static class ConfigGlobal_AcnCasino
    {
        static ConfigGlobal_AcnCasino()
        {
            Init();
        }

        private static Dictionary<string, string> ConfigDictionary { get; set; }

        public static void Refresh()
        {
            Init();
        }

        private static void Init()
        {
            Config.Cache.RefreshCache();
            ConfigDictionary = Config.Cache.GetDictionaryByConfigSystem(ConfigSystem.AcnCasino);
        }

        #region Members and Properties

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

        public static Guid DefaultBankerID
        {
            get
            {
                try
                {
                    var tmpId = ConfigDictionary["DefaultBankerID"];
                    if (!string.IsNullOrEmpty(tmpId))
                        return new Guid(tmpId);
                    return new Guid("f2e3dfe0-2ef6-49df-8518-15e66cafe594");
                }
                catch
                {
                    return new Guid("f2e3dfe0-2ef6-49df-8518-15e66cafe594");
                }
            }
        }

        public static Guid DefaultLeagueID
        {
            get
            {
                try
                {
                    var tmpId = ConfigDictionary["DefaultLeagueID"];

                    if (!string.IsNullOrEmpty(tmpId))
                        return new Guid(tmpId);
                    return Guid.Empty;
                }
                catch
                {
                    return Guid.Empty;
                }
            }
        }

        public static float SingleBetLimit
        {
            get
            {
                try
                {
                    return Convert.ToSingle(ConfigDictionary["SingleBetLimit"]);
                }
                catch
                {
                    return 50000f;
                }
            }
        }

        public static float TotalBetStandard
        {
            get
            {
                try
                {
                    return Convert.ToSingle(ConfigDictionary["TotalBetStandard"]);
                }
                catch
                {
                    return 1000000f;
                }
            }
        }

        public static bool ContestLimitIgnore
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(ConfigDictionary["ContestLimitIgnore"]);
                }
                catch
                {
                    return true;
                }
            }
        }

        public static int[] ContestCondition
        {
            get
            {
                try
                {
                    var configValue = ConfigDictionary["ContestCondition"];
                    var tmpStrings = configValue.Split('|');
                    var retInts = new int[tmpStrings.Length];

                    for (var i = 0; i < tmpStrings.Length; i++)
                    {
                        int.TryParse(tmpStrings[i], out retInts[i]);
                    }

                    return retInts;
                }
                catch
                { return new[] { 5, 5000, 3 }; }
            }
        }

        public static int ExchangeRate
        {
            get
            {
                try
                {
                    return Convert.ToInt16(ConfigDictionary["ExchangeRate"]);
                }
                catch
                {
                    return 10;
                }
            }
        }

        public static float ExchangeFee
        {
            get
            {
                try
                {
                    return Convert.ToSingle(ConfigDictionary["ExchangeFee"]);
                }
                catch
                {
                    return 0.02f;
                }
            }
        }


        public static int CasinoValidDays
        {
            get
            {
                try
                {
                    return Convert.ToInt16(ConfigDictionary["CasinoValidDays"]);
                }
                catch
                {
                    return 5;
                }
            }
        }

        #endregion
    }

    public static class ConfigGlobal_AcnClub
    {
        static ConfigGlobal_AcnClub()
        {
            Init();
        }

        private static Dictionary<string, string> ConfigDictionary { get; set; }

        public static void Refresh()
        {
            Init();
        }

        private static void Init()
        {
            Config.Cache.RefreshCache();
            ConfigDictionary = Config.Cache.GetDictionaryByConfigSystem(ConfigSystem.AcnClub);
        }

        #region Members and Properties

        public static bool SignInActive
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(ConfigDictionary["SignInActive"]);
                }
                catch
                {
                    return false;
                }
            }
        }

        public static float SignInBonus
        {
            get
            {
                try
                {
                    return Convert.ToSingle(ConfigDictionary["SignInBonus"]);
                }
                catch
                {
                    // QSB
                    return 100f;
                }
            }
        }

        public static float SignInFund
        {
            get
            {
                try
                {
                    return Convert.ToSingle(ConfigDictionary["SignInFund"]);
                }
                catch
                {
                    // QSB
                    return 3000f;
                }
            }
        }

        public static int SignInMaxRate
        {
            get
            {
                try
                {
                    return Convert.ToInt16(ConfigDictionary["SignInMaxRate"]);
                }
                catch
                {
                    return 5;
                }
            }
        }

        public static string SignInKeyword => ConfigDictionary["SignInKeyword"];
        public static string SignInKeywordBonus => ConfigDictionary["SignInKeywordBonus"];


        #endregion
    }
}
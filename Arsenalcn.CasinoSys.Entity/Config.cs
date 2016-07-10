using System;
using System.Collections.Generic;
using Arsenalcn.Common.Entity;

namespace Arsenalcn.CasinoSys.Entity
{
    public class ConfigGlobal : Config
    {
        private const ConfigSystem CurrSystem = ConfigSystem.AcnCasino;

        private ConfigGlobal() { }

        #region Members and Properties

        private static Dictionary<string, string> ConfigDictionary => GetDictionaryByConfigSystem(CurrSystem);

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

        public static string PluginAcnClubPath => ConfigDictionary["PluginAcnClubPath"];

        // ReSharper disable once InconsistentNaming
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

        // ReSharper disable once InconsistentNaming
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

        public static string SysNotice => ConfigDictionary["SysNotice"];

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

    public static class ConfigAdmin
    {
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
    }
}
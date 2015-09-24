using System;
using System.Collections.Generic;

using Arsenalcn.Common.Entity;

namespace Arsenalcn.CasinoSys.Entity
{
    public class ConfigGlobal : Config
    {
        ConfigGlobal() { }

        const ConfigSystem currSystem = ConfigSystem.AcnCasino;

        #region Members and Properties

        public static Dictionary<string, string> ConfigDictionary
        {
            get
            {
                return GetDictionaryByConfigSystem(currSystem);
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

        public static string PluginAcnClubPath
        {
            get
            {
                return ConfigDictionary["PluginAcnClubPath"];
            }
        }

        public static Guid DefaultBankerID
        {
            get
            {
                try
                {
                    var tmpID = ConfigDictionary["DefaultBankerID"];
                    if (!string.IsNullOrEmpty(tmpID))
                        return new Guid(tmpID);
                    else
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
                    var tmpID = ConfigDictionary["DefaultLeagueID"];

                    if (!string.IsNullOrEmpty(tmpID))
                        return new Guid(tmpID);
                    else
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

        public static string SysNotice
        {
            get
            {
                return ConfigDictionary["SysNotice"];
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

    public class ConfigAdmin
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


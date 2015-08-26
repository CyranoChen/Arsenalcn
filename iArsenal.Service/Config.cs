using System;
using System.Collections.Generic;
using Arsenalcn.Core;

namespace iArsenal.Service
{
    public static class ConfigGlobal
    {
        public static Dictionary<string, string> ConfigDictionary
        {
            get
            {
                var currSystem = ConfigSystem.iArsenal;
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

        public static string AcnCasinoURL
        {
            get
            {
                return ConfigDictionary["AcnCasinoURL"];
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
                else
                    return new Guid("036478f0-9062-4533-be33-43197a0b1568");
            }
        }

        public static string SysNotice
        {
            get
            {
                return ConfigDictionary["SysNotice"];
            }
        }

        //public static string CodePlayerNumber
        //{
        //    get
        //    {
        //        return ConfigDictionary["CodePlayerNumber"];
        //    }
        //}

        //public static string CodePlayerName
        //{
        //    get
        //    {
        //        return ConfigDictionary["CodePlayerName"];
        //    }
        //}

        //public static string CodeChampionshipPatch
        //{
        //    get
        //    {
        //        return ConfigDictionary["CodeChampionshipPatch"];
        //    }
        //}

        //public static string CodePremiershipPatch
        //{
        //    get
        //    {
        //        return ConfigDictionary["CodePremiershipPatch"];
        //    }
        //}

        //public static string CodeArsenalFont
        //{
        //    get
        //    {
        //        return ConfigDictionary["CodeArsenalFont"];
        //    }
        //}

        //public static string[] CodeReplicaKitHome
        //{
        //    get
        //    {
        //        string configValue = ConfigDictionary["CodeReplicaKitHome"];

        //        return configValue.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        //    }
        //}

        //public static string[] CodeReplicaKitAway
        //{
        //    get
        //    {
        //        string configValue = ConfigDictionary["CodeReplicaKitAway"];

        //        return configValue.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        //    }
        //}

        //public static string[] CodeTicketBeijing
        //{
        //    get
        //    {
        //        string configValue = ConfigDictionary["CodeTicketBeijing"];

        //        return configValue.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        //    }
        //}

        //public static string[] CodeEmirateTravel
        //{
        //    get
        //    {
        //        string configValue = ConfigDictionary["CodeEmirateTravel"];

        //        return configValue.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        //    }
        //}

        //public static string[] CodeMatchTicket
        //{
        //    get
        //    {
        //        string configValue = ConfigDictionary["CodeMatchTicket"];

        //        return configValue.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        //    }
        //}

        public static string[] BulkOrderInfo
        {
            get
            {
                var configValue = ConfigDictionary["BulkOrderInfo"];

                return configValue.Split(new char[] { '|' });
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

using System;
using System.Collections.Generic;

using Arsenalcn.Common.Entity;

namespace Arsenalcn.ClubSys.Entity
{
    public class ConfigGlobal: Config
    {
        ConfigGlobal() { }

        const ConfigSystem currSystem = ConfigSystem.AcnClub;

        #region Members and Properties

        public static Dictionary<string, string> ConfigDictionary
        {
            get
            {
                return Config.GetDictionaryByConfigSystem(currSystem);
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

        public static string SysNotice
        {
            get
            {
                return ConfigDictionary["SysNotice"];
            }
        }

        public static string AuthPrivateKey
        {
            get
            {
                return ConfigDictionary["AuthPrivateKey"];
            }
        }

        public static int BingoPlayCountPerHour
        {
            get
            {
                try
                {
                    return Convert.ToInt16(ConfigDictionary["BingoPlayCountPerHour"]);
                }
                catch
                {
                    return 5;
                }
            }
        }

        public static int BingoCost
        {
            get
            {
                try
                {
                    return Convert.ToInt16(ConfigDictionary["BingoCost"]);
                }
                catch
                {
                    return 10;
                }
            }
        }

        public static int BingoGetCost
        {
            get
            {
                try
                {
                    return Convert.ToInt16(ConfigDictionary["BingoGetCost"]);
                }
                catch
                {
                    return 0;
                }
            }
        }

        public static float BingoBonusRate
        {
            get
            {
                try
                {
                    return Convert.ToSingle(ConfigDictionary["BingoBonusRate"]);
                }
                catch
                {
                    return 10f;
                }
            }
        }

        public static int ChampionsClubID
        {
            get
            {
                try
                {
                    return Convert.ToInt16(ConfigDictionary["Champions.ClubID"]);
                }
                catch
                {
                    return int.MinValue;
                }
            }
        }

        public static string ChampionsTitle
        {
            get
            {
                return ConfigDictionary["Champions.Title"];
            }
        }

        public static string ClubLogoPath
        {
            get
            {
                return ConfigDictionary["ClubLogoPath"];
            }
        }

        public static int ClubDefaultRankLevel
        {
            get
            {
                return 0;
            }
        }

        public static float ClubFortuneIncrementVariable
        {
            get
            {
                try
                {
                    return Convert.ToSingle(ConfigDictionary["ClubFortuneIncrementVariable"]);
                }
                catch
                {
                    return 0.1f;
                }
            }
        }

        public static string DefaultClubLogoName
        {
            get
            {
                return ConfigDictionary["DefaultClubLogoName"];
            }
        }

        public static int DailyClubEquipmentCount
        {
            get
            {
                try
                {
                    return Convert.ToInt16(ConfigDictionary["DailyClubEquipmentCount"]);
                }
                catch
                {
                    return 10;
                }
            }
        }

        public static int DailyUserEquipmentCount
        {
            get
            {
                try
                {
                    return Convert.ToInt16(ConfigDictionary["DailyUserEquipmentCount"]);
                }
                catch
                {
                    return 10;
                }
            }
        }

        public static bool DailyVideoActive
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(ConfigDictionary["DailyVideoActive"]);
                }
                catch
                {
                    return true;
                }
            }
        }

        public static string DailyVideoGuid
        {
            get
            {
                return ConfigDictionary["DailyVideoGuid"];
            }
        }

        public static bool GoogleAdvActive
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(ConfigDictionary["GoogleAdvActive"]);
                }
                catch
                {
                    return false;
                }
            }
        }

        public static bool LuckyPlayerBonusGot
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(ConfigDictionary["LuckyPlayerBonusGot"]);
                }
                catch
                {
                    return false;
                }
            }
        }

        public static int LuckyPlayerID
        {
            get
            {
                try
                {
                    return Convert.ToInt16(ConfigDictionary["LuckyPlayerID"]);
                }
                catch
                {
                    return -1;
                }
            }
        }

        public static float LuckyPlayerBonusPercentage
        {
            get
            {
                try
                {
                    return Convert.ToSingle(ConfigDictionary["LuckyPlayerBonusPercentage"]);
                }
                catch
                {
                    return 0.5f;
                }
            }
        }

        public static bool LuckyPlayerActive
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(ConfigDictionary["LuckyPlayerActive"]);
                }
                catch
                {
                    return false;
                }
            }
        }

        public static int LuckyPlayerDeadline
        {
            get
            {
                try
                {
                    return Convert.ToInt16(ConfigDictionary["LuckyPlayerDeadline"]);
                }
                catch
                {
                    return 25;
                }
            }
        }

        public static int SingleUserMaxClubCount
        {
            get
            {
                try
                {
                    return Convert.ToInt16(ConfigDictionary["SingleUserMaxClubCount"]);
                }
                catch
                {
                    return 1;
                }
            }
        }

        public static int MinPostsToCreateClub
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ConfigDictionary["MinPostsToCreateClub"]);
                }
                catch
                {
                    return 10000;
                }
            }
        }

        public static int PlayerMaxLv
        {
            get
            {
                try
                {
                    return Convert.ToInt16(ConfigDictionary["PlayerMaxLv"]);
                }
                catch
                {
                    return 5;
                }
            }
        }

        public static int SummaryRankPoint_MemberCountWeight
        {
            get
            {
                try
                {
                    return Convert.ToInt16(ConfigDictionary["SummaryRankPoint.MemberCountWeight"]);
                }
                catch
                {
                    return 1;
                }
            }
        }

        public static int SummaryRankPoint_ClubFortuneWeight
        {
            get
            {
                try
                {
                    return Convert.ToInt16(ConfigDictionary["SummaryRankPoint.ClubFortuneWeight"]);
                }
                catch
                {
                    return 1;
                }
            }
        }

        public static int SummaryRankPoint_MemberCreditWeight
        {
            get
            {
                try
                {
                    return Convert.ToInt16(ConfigDictionary["SummaryRankPoint.MemberCreditWeight"]);
                }
                catch
                {
                    return 1;
                }
            }
        }

        public static int SummaryRankPoint_MemberRPWeight
        {
            get
            {
                try
                {
                    return Convert.ToInt16(ConfigDictionary["SummaryRankPoint.MemberRPWeight"]);
                }
                catch
                {
                    return 1;
                }
            }
        }

        public static int SummaryRankPoint_MemberEquipmentWeight
        {
            get
            {
                try
                {
                    return Convert.ToInt16(ConfigDictionary["SummaryRankPoint.MemberEquipmentWeight"]);
                }
                catch
                {
                    return 1;
                }
            }
        }

        #endregion
    }

    public class ConfigAdmin
    {
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
    }
}

using System;
using System.Collections.Generic;

using Arsenalcn.Common.Entity;

namespace Arsenal.Entity
{
    public class ConfigGlobal : Config
    {
        ConfigGlobal() { }

        const ConfigSystem currSystem = ConfigSystem.Arsenal;

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

        public static Guid ArsenalTeamGuid
        {
            get
            {
                try
                {
                    string tmpID = ConfigDictionary["ArsenalTeamGuid"];
                    if (!string.IsNullOrEmpty(tmpID))
                        return new Guid(tmpID);
                    else
                        throw new Exception();
                }
                catch
                {
                    return new Guid("036478f0-9062-4533-be33-43197a0b1568");
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


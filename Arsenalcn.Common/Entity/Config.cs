using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Arsenalcn.Common.Entity
{
    public class Config
    {
        public Config() { }

        public Config(DataRow dr)
        {
            InitConfig(dr);
        }

        private void InitConfig(DataRow dr)
        {
            if (dr != null)
            {
                ConfigSystem = (ConfigSystem)Enum.Parse(typeof(ConfigSystem), dr["ConfigSystem"].ToString());
                ConfigKey = dr["ConfigKey"].ToString();
                ConfigValue = dr["ConfigValue"].ToString();
            }
            else
                throw new Exception("Unable to init Config.");
        }

        public void Select()
        {
            DataRow dr = DataAccess.Config.GetConfigByID(ConfigSystem.ToString(), ConfigKey);

            if (dr != null)
                InitConfig(dr);
        }

        public void Update()
        {
            using (SqlConnection conn = SQLConn.GetConnection())
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    DataAccess.Config.UpdateConfig(ConfigSystem.ToString(), ConfigKey, ConfigValue, trans);

                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();
                }

                //conn.Close();
            }
        }

        private void Insert()
        {
            using (SqlConnection conn = SQLConn.GetConnection())
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    DataAccess.Config.InsertConfig(ConfigSystem.ToString(), ConfigKey, ConfigValue, trans);

                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();
                }

                //conn.Close();
            }
        }

        private void Delete()
        {
            DataAccess.Config.DeleteConfig(ConfigSystem.ToString(), ConfigKey);
        }

        public static List<Config> GetConfigs()
        {
            DataTable dt = DataAccess.Config.GetConfigs();
            List<Config> list = new List<Config>();

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new Config(dr));
                }
            }

            return list;
        }

        protected static Dictionary<string, string> GetDictionaryByConfigSystem(ConfigSystem cs)
        {
            List<Config> list = Config.Cache.ConfigList.FindAll(delegate(Config c) { return c.ConfigSystem.Equals(cs); });

            if (list != null && list.Count > 0)
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();

                foreach (Config c in list)
                {
                    try { dict.Add(c.ConfigKey, c.ConfigValue); }
                    catch { continue; }
                }

                return dict;
            }
            else
            {
                return null;
            }
        }

        public static class Cache
        {
            static Cache()
            {
                InitCache();
            }

            public static void RefreshCache()
            {
                InitCache();
            }

            private static void InitCache()
            {
                ConfigList = Config.GetConfigs();
            }

            public static Config Load(ConfigSystem cs, string key)
            {
                return ConfigList.Find(delegate(Config c) { return c.ConfigSystem.Equals(cs) && c.ConfigKey.Equals(key); });
            }

            public static string LoadDict(ConfigSystem cs, string key)
            {
                return GetDictionaryByConfigSystem(cs)[key];
            }

            public static List<Config> ConfigList;
        }

        #region Members and Properties

        public ConfigSystem ConfigSystem
        { get; set; }

        public string ConfigKey
        { get; set; }

        public string ConfigValue
        { get; set; }

        #endregion
    }

    public enum ConfigSystem
    {
        AcnClub,
        AcnCasino,
        iArsenal,
        Arsenal
    }
}

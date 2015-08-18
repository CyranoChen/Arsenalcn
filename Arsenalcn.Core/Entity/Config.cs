using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Arsenalcn.Core
{
    [DbSchema("Arsenalcn_Config", Key = "", Sort = "ConfigSystem, ConfigKey")]
    public class Config
    {
        public Config() { }

        private static void CreateMap()
        {
            var map = AutoMapper.Mapper.CreateMap<IDataReader, Config>();

            map.ForMember(d => d.ConfigSystem, opt => opt.MapFrom(s =>
               (ConfigSystem)Enum.Parse(typeof(ConfigSystem), s.GetValue("ConfigSystem").ToString())));
        }

        //public Config(DataRow dr)
        //{
        //    Contract.Requires(dr != null);

        //    Init(dr);
        //}

        //private void Init(DataRow dr)
        //{
        //    if (dr != null)
        //    {
        //        ConfigSystem = (ConfigSystem)Enum.Parse(typeof(ConfigSystem), dr["ConfigSystem"].ToString());
        //        ConfigKey = dr["ConfigKey"].ToString();
        //        ConfigValue = dr["ConfigValue"].ToString();
        //    }
        //    else
        //        throw new Exception("Unable to init Config.");
        //}

        //public void Single()
        //{
        //    string sql = string.Format("SELECT * FROM {0} WHERE ConfigSystem = @configSystem AND ConfigKey = @configKey",
        //        Repository.GetTableAttr<Config>().Name);

        //    SqlParameter[] para = {
        //                              new SqlParameter("@configSystem", ConfigSystem.ToString()), 
        //                              new SqlParameter("@configKey", ConfigKey)
        //                          };

        //    DataSet ds = DataAccess.ExecuteDataset(sql, para);

        //    if (ds.Tables[0].Rows.Count > 0) { Init(ds.Tables[0].Rows[0]); }
        //}

        public bool Any()
        {
            string sql = string.Format("SELECT * FROM {0} WHERE ConfigSystem = @configSystem AND ConfigKey = @configKey",
                Repository.GetTableAttr<Config>().Name);

            SqlParameter[] para = {
                                      new SqlParameter("@configSystem", ConfigSystem.ToString()),
                                      new SqlParameter("@configKey", ConfigKey)
                                  };

            DataSet ds = DataAccess.ExecuteDataset(sql, para);

            return ds.Tables[0].Rows.Count > 0;
        }

        private static List<Config> All()
        {
            var attr = Repository.GetTableAttr<Config>();

            var list = new List<Config>();

            string sql = string.Format("SELECT * FROM {0} ORDER BY {1}", attr.Name, attr.Sort);

            DataSet ds = DataAccess.ExecuteDataset(sql);

            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    {
            //        list.Add(new Config(dr));
            //    }
            //}

            var dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                using (var reader = dt.CreateDataReader())
                {
                    Config.CreateMap();

                    list = AutoMapper.Mapper.Map<IDataReader, List<Config>>(reader);
                }
            }

            return list;
        }

        public static IEnumerable<Config> All(ConfigSystem cs)
        {
            return All().Where(x => x.ConfigSystem.Equals(cs));
        }

        public void Update(SqlTransaction trans = null)
        {
            Contract.Requires(this.Any());

            string sql = string.Format("UPDATE {0} SET ConfigValue = @configValue WHERE ConfigSystem = @configSystem AND ConfigKey = @configKey",
                 Repository.GetTableAttr<Config>().Name);

            SqlParameter[] para = {
                                      new SqlParameter("@configSystem", ConfigSystem.ToString()),
                                      new SqlParameter("@configKey", ConfigKey),
                                      new SqlParameter("@configValue", ConfigValue) };

            DataAccess.ExecuteNonQuery(sql, para, trans);
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
                ConfigList = All();
            }

            public static Config Load(ConfigSystem cs, string key)
            {
                return ConfigList.Find(x => x.ConfigSystem.Equals(cs) && x.ConfigKey.Equals(key));
            }

            public static string LoadDict(ConfigSystem cs, string key)
            {
                return GetDictionaryByConfigSystem(cs)[key];
            }

            public static Dictionary<string, string> GetDictionaryByConfigSystem(ConfigSystem cs)
            {
                List<Config> list = ConfigList.FindAll(x => x.ConfigSystem.Equals(cs));

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

            public static List<Config> ConfigList;
        }

        #region Members and Properties

        [DbColumn("ConfigSystem", IsKey = true)]
        public ConfigSystem ConfigSystem
        { get; set; }

        [DbColumn("ConfigKey", IsKey = true)]
        public string ConfigKey
        { get; set; }

        [DbColumn("ConfigValue")]
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



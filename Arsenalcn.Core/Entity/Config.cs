using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using Dapper;
using DataReaderMapper;
using DataReaderMapper.Mappers;

namespace Arsenalcn.Core
{
    [DbSchema("Arsenalcn_Config", Key = "", Sort = "ConfigSystem, ConfigKey")]
    public class Config : Dao
    {
        private static void CreateMap()
        {
            var map = Mapper.CreateMap<IDataReader, Config>();

            map.ForMember(d => d.ConfigSystem, opt => opt.MapFrom(s =>
                (ConfigSystem)Enum.Parse(typeof(ConfigSystem), s.GetValue("ConfigSystem").ToString())));
        }

        private static readonly IDbConnection _conn = DapperHelper.GetOpenConnection();

        public bool Any()
        {
            var sql =
                $"SELECT COUNT(*) FROM {Repository.GetTableAttr<Config>().Name} WHERE ConfigSystem = @configSystem AND ConfigKey = @configKey";

            SqlParameter[] para =
            {
                new SqlParameter("@configSystem", ConfigSystem.ToString()),
                new SqlParameter("@configKey", ConfigKey)
            };

            var result = _conn.Query<int>(sql, DapperHelper.BuildDapperParameters(para)).ToList();

            return Convert.ToInt32(result[0]) > 0;
        }

        private static List<Config> All()
        {
            var attr = Repository.GetTableAttr<Config>();

            var sql = $"SELECT * FROM {attr.Name} ORDER BY {attr.Sort}";

            var res = _conn.ExecuteReader(sql);

            Mapper.Initialize(cfg =>
            {
                MapperRegistry.Mappers.Insert(0,
                    new DataReaderMapper.DataReaderMapper { YieldReturnEnabled = false });

                CreateMap();
            });

            return Mapper.Map<IDataReader, List<Config>>(res);
        }

        public static IEnumerable<Config> All(ConfigSystem cs)
        {
            return All().Where(x => x.ConfigSystem.Equals(cs));
        }

        public void Update(SqlTransaction trans = null)
        {
            Contract.Requires(Any());

            var sql =
                $"UPDATE {Repository.GetTableAttr<Config>().Name} SET ConfigValue = @configValue WHERE ConfigSystem = @configSystem AND ConfigKey = @configKey";

            SqlParameter[] para =
            {
                new SqlParameter("@configSystem", ConfigSystem.ToString()),
                new SqlParameter("@configKey", ConfigKey),
                new SqlParameter("@configValue", ConfigValue)
            };

            _conn.Execute(sql, para, trans);
        }

        public void Save(SqlTransaction trans = null)
        {
            if (Any())
            {
                Update();
            }
            else
            {
                var sql =
                    $"INSERT INTO {Repository.GetTableAttr<Config>().Name} (ConfigValue, ConfigSystem, ConfigKey) VALUES (@configValue, @configSystem, @configKey)";

                SqlParameter[] para =
                {
                    new SqlParameter("@configSystem", ConfigSystem.ToString()),
                    new SqlParameter("@configKey", ConfigKey),
                    new SqlParameter("@configValue", ConfigValue)
                };

                _conn.Execute(sql, para, trans);
            }
        }

        public static void UpdateAssemblyInfo(Assembly assembly, ConfigSystem configSystem)
        {
            if (assembly != null)
            {
                //[assembly: AssemblyTitle("Arsenalcn.Core")]
                //[assembly: AssemblyDescription("沪ICP备12045527号")]
                //[assembly: AssemblyConfiguration("webmaster@arsenalcn.com")]
                //[assembly: AssemblyCompany("Arsenal China Official Supporters Club")]
                //[assembly: AssemblyProduct("Arsenalcn.com")]
                //[assembly: AssemblyCopyright("© 2015")]
                //[assembly: AssemblyTrademark("ArsenalCN")]
                //[assembly: AssemblyCulture("")]
                //[assembly: AssemblyVersion("1.8.*")]
                //[assembly: AssemblyFileVersion("1.8.2")]

                var c = new Config();
                c.ConfigSystem = configSystem;

                //AssemblyTitle
                c.ConfigKey = "AssemblyTitle";
                c.ConfigValue =
                    ((AssemblyTitleAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyTitleAttribute)))?
                        .Title;

                c.Save();

                //AssemblyDescription
                c.ConfigKey = "AssemblyDescription";
                c.ConfigValue =
                    ((AssemblyDescriptionAttribute)
                        Attribute.GetCustomAttribute(assembly, typeof(AssemblyDescriptionAttribute)))?.Description;

                c.Save();

                //AssemblyConfiguration
                c.ConfigKey = "AssemblyConfiguration";
                c.ConfigValue =
                    ((AssemblyConfigurationAttribute)
                        Attribute.GetCustomAttribute(assembly, typeof(AssemblyConfigurationAttribute)))?.Configuration;

                c.Save();

                //AssemblyCompany
                c.ConfigKey = "AssemblyCompany";
                c.ConfigValue =
                    ((AssemblyCompanyAttribute)
                        Attribute.GetCustomAttribute(assembly, typeof(AssemblyCompanyAttribute)))?.Company;

                c.Save();

                //AssemblyProduct
                c.ConfigKey = "AssemblyProduct";
                c.ConfigValue =
                    ((AssemblyProductAttribute)
                        Attribute.GetCustomAttribute(assembly, typeof(AssemblyProductAttribute)))?.Product;

                c.Save();

                //AssemblyCopyright
                c.ConfigKey = "AssemblyCopyright";
                c.ConfigValue =
                    ((AssemblyCopyrightAttribute)
                        Attribute.GetCustomAttribute(assembly, typeof(AssemblyCopyrightAttribute)))?.Copyright;

                c.Save();

                //AssemblyTrademark
                c.ConfigKey = "AssemblyTrademark";
                c.ConfigValue =
                    ((AssemblyTrademarkAttribute)
                        Attribute.GetCustomAttribute(assembly, typeof(AssemblyTrademarkAttribute)))?.Trademark;

                c.Save();

                //AssemblyCulture
                c.ConfigKey = "AssemblyCulture";
                c.ConfigValue =
                    ((AssemblyCultureAttribute)
                        Attribute.GetCustomAttribute(assembly, typeof(AssemblyCultureAttribute)))?.Culture;

                c.Save();

                //AssemblyVersion
                var assemblyName = assembly.GetName();
                var version = assemblyName.Version;

                c.ConfigKey = "AssemblyVersion";
                c.ConfigValue = version?.ToString();

                c.Save();

                //AssemblyFileVersion
                c.ConfigKey = "AssemblyFileVersion";
                c.ConfigValue =
                    ((AssemblyFileVersionAttribute)
                        Attribute.GetCustomAttribute(assembly, typeof(AssemblyFileVersionAttribute)))?.Version;

                c.Save();
            }
        }

        public static class Cache
        {
            public static List<Config> ConfigList;

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
                var list = ConfigList.FindAll(x => x.ConfigSystem.Equals(cs));

                if (list.Count > 0)
                {
                    var dict = new Dictionary<string, string>();

                    foreach (var c in list)
                    {
                        try
                        {
                            dict.Add(c.ConfigKey, c.ConfigValue);
                        }
                        catch
                        {
                            // ignored
                        }
                    }

                    return dict;
                }
                return null;
            }
        }

        #region Members and Properties

        [DbColumn("ConfigSystem", IsKey = true)]
        public ConfigSystem ConfigSystem { get; set; }

        [DbColumn("ConfigKey", IsKey = true)]
        public string ConfigKey { get; set; }

        [DbColumn("ConfigValue")]
        public string ConfigValue { get; set; }

        #endregion
    }

    public enum ConfigSystem
    {
        AcnClub,
        AcnCasino,
        // ReSharper disable once InconsistentNaming
        iArsenal,
        Arsenal
    }
}
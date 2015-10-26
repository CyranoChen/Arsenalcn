using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Arsenal.Service;
using Arsenal.Service.Casino;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Arsenalcn.Core.Tests
{
    [TestClass]
    public class AutoMapperTest
    {
        [TestMethod]
        public void DataColumn_DefautValue_Test()
        {
            //创建一个新的DataTable
            var dt = new DataTable();

            dt.Columns.Add("Data");

            dt.Rows.Add("test1");

            //创建一个新的DataColumn
            var col = new DataColumn("@include", typeof(decimal));

            //将DataColumn添加dt中
            dt.Columns.Add(col);

            //设置DataColumn的默认值
            col.DefaultValue = 0;

            var dr = dt.Rows.Add("test2");
            Assert.AreEqual(dr["@include"].ToString(), "0");
        }

        [TestMethod]
        public void AutoMapper_Single_Test()
        {
            //DataTable dt = new DataTable();
            //dt.Columns.Add("LeagueGuid", typeof(Guid));
            //dt.Columns.Add("LeagueName", typeof(string));
            //dt.Columns.Add("LeagueSeason", typeof(string));

            //DataRow dr = dt.NewRow();
            //dr["LeagueGuid"] = Guid.Empty;
            //dr["LeagueName"] = "cyrano";
            //dr["LeagueSeason"] = "good";
            //dt.Rows.Add(dr);

            var list = new List<Person>();
            list.Add(new Person { LeagueGuid = Guid.Empty, LeagueName = "cyrano", LeagueSeason = "good" });

            var map = Mapper.CreateMap<Person, League>()
                .AfterMap((s, d) => d.LeagueNameInfo = "test");

            map.ForMember(d => d.ID, opt => opt.MapFrom(s => s.LeagueGuid));

            var instance = Mapper.Map<List<League>>(list).First();

            Assert.AreEqual(instance.LeagueNameInfo, "test");
        }

        public class Person
        {
            public Guid LeagueGuid { get; set; }
            public string LeagueName { get; set; }
            public string LeagueSeason { get; set; }
        }

        //[TestMethod]
        //public void AutoMapper_Collections_Test()
        //{
        //    IRepository repo = new Repository();

        //    var query = repo.All<MatchView>().FindAll(x => x.ResultHome.HasValue && x.ResultAway.HasValue)
        //        .Many<MatchView, ChoiceOption>((tSource, t) => tSource.CasinoItem.ID.Equals(t.CasinoItemGuid));

        //    MatchDto.CreateMap();

        //    var result = Mapper.Map<IEnumerable<MatchDto>>(source: query.AsEnumerable());

        //    Assert.IsNotNull(result);
        //}

        [TestMethod]
        public void AutoMapper_DataReader_Test()
        {
            var sql = @"SELECT * FROM [AcnCasino_CasinoItem]";

            var ds = DataAccess.ExecuteDataset(sql);

            var dt = ds.Tables[0];

            var list = new List<CasinoItem>();

            if (dt.Rows.Count > 0)
            {
                // db float? -> c# double?
                // Error on Enum?
                var mapper = typeof(CasinoItem).GetMethod("CreateMap",
                    BindingFlags.Static | BindingFlags.Public);

                if (mapper != null)
                {
                    mapper.Invoke(null, null);
                }
                else
                {
                    Mapper.CreateMap<IDataReader, CasinoItem>();
                }

                list = Mapper.Map<IDataReader, IEnumerable<CasinoItem>>(dt.CreateDataReader()).ToList();
            }

            Assert.IsTrue(list.Count > 0);
        }
    }
}

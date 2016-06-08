using System;
using System.Collections.Generic;
using System.Linq;
using Arsenal.Service;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Arsenalcn.Core.Tests
{
    [TestClass]
    public class AutoMapperTest
    {
        //[TestMethod]
        //public void DataColumn_DefautValue_Test()
        //{
        //    //创建一个新的DataTable
        //    var dt = new DataTable();

        //    dt.Columns.Add("Data");

        //    dt.Rows.Add("test1");

        //    //创建一个新的DataColumn
        //    var col = new DataColumn("@include", typeof(decimal));

        //    //将DataColumn添加dt中
        //    dt.Columns.Add(col);

        //    //设置DataColumn的默认值
        //    col.DefaultValue = 0;

        //    var dr = dt.Rows.Add("test2");
        //    Assert.AreEqual(dr["@include"].ToString(), "0");
        //}

        [TestMethod]
        public void Test_AutoMapper_421()
        {
            var list = new List<Person>
            {
                new Person {LeagueGuid = Guid.Empty, LeagueName = "cyrano", LeagueSeason = "good"}
            };


            var config = new MapperConfiguration(cfg => cfg.CreateMap<Person, League>()
                .ForMember(d => d.ID, opt => opt.MapFrom(s => s.LeagueGuid))
                .AfterMap((s, d) => d.LeagueNameInfo = "test"));

            var mapper = config.CreateMapper();

            var instance = mapper.Map<List<League>>(list).First();

            Assert.AreEqual(instance.LeagueNameInfo, "test");
        }

        private class Person
        {
            public Guid LeagueGuid { get; set; }
            public string LeagueName { get; set; }
            public string LeagueSeason { get; set; }
        }
    }
}
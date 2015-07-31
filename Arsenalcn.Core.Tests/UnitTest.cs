using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Arsenal.MvcWeb.Models.Casino;
using Arsenal.Service.Casino;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Arsenalcn.Core.Tests
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void DataColumn_DefautValue_Test()
        {
            //创建一个新的DataTable
            DataTable dt = new DataTable();

            dt.Columns.Add("Data");

            dt.Rows.Add("test1");

            //创建一个新的DataColumn
            DataColumn col = new DataColumn("@include", typeof(decimal));

            //将DataColumn添加dt中
            dt.Columns.Add(col);

            //设置DataColumn的默认值
            col.DefaultValue = 0;

            DataRow dr = dt.Rows.Add("test2");
            Assert.AreEqual(dr["@include"].ToString(), "0");
        }

        [TestMethod]
        public void AutoMapper_Test()
        {
            var match = new MatchDto();

            match.ID = new Guid("12236a72-f35b-4a0f-90e6-67b11c3364bc");

            IRepository repo = new Repository();

            var instance = repo.Single<MatchView>(match.ID);

            instance.Many<ChoiceOption>(instance.CasinoItem.ID);

            MatchDto.CreateMap();

            match = Mapper.Map<MatchDto>(instance);

            Assert.IsNotNull(match);
        }

        [TestMethod]
        public void AutoMapper_Collections_Test()
        {
            IRepository repo = new Repository();

            var query = repo.All<MatchView>().FindAll(x => x.ResultHome.HasValue && x.ResultAway.HasValue)
                .Many<MatchView, ChoiceOption>((tSource, t) => tSource.CasinoItem.ID.Equals(t.CasinoItemGuid)).AsEnumerable();

            MatchDto.CreateMap();

            var result = Mapper.Map<IEnumerable<MatchDto>>(source: query.AsEnumerable());

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AutoMapper_DataReader_Test()
        {
            var sql = @"SELECT * FROM [AcnCasino_CasinoItem]";

            DataSet ds = DataAccess.ExecuteDataset(sql);

            DataTable dt = ds.Tables[0];

            var list = new List<CasinoItem>();

            if (dt.Rows.Count > 0)
            {
                // db float? -> c# double?
                // Error on Enum?
                var mapper = typeof(CasinoItem).GetMethod("CreateMap",
                    System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

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

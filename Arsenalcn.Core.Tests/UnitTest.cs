using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Arsenal.Service.Casino;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Arsenal.Service;

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

            match.MatchGuid = new Guid("12236a72-f35b-4a0f-90e6-67b11c3364bc");

            IRepository repo = new Repository();

            var instance = repo.Single<MatchView>(match.MatchGuid);

            instance.Many<ChoiceOption>(instance.CasinoItem.ID);

            var map = Mapper.CreateMap<MatchView, MatchDto>();

            map.ConstructUsing(s => new MatchDto
            {
                MatchGuid = s.ID,
                TeamHomeName = s.Home.TeamDisplayName,
                TeamHomeLogo = s.Home.TeamLogo,
                TeamAwayName = s.Away.TeamDisplayName,
                TeamAwayLogo = s.Away.TeamLogo,
                HomeRate = s.ListChoiceOption.Single(x => x.OptionName.Equals("home", StringComparison.OrdinalIgnoreCase)).OptionRate,
                DrawRate = s.ListChoiceOption.Single(x => x.OptionName.Equals("draw", StringComparison.OrdinalIgnoreCase)).OptionRate,
                AwayRate = s.ListChoiceOption.Single(x => x.OptionName.Equals("away", StringComparison.OrdinalIgnoreCase)).OptionRate,
            });


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
            var sql = @"SELECT * FROM [AcnCasino_Bet]";

            DataSet ds = DataAccess.ExecuteDataset(sql);

            DataTable dt = ds.Tables[0];

            var list = new List<Bet>();

            if (dt.Rows.Count > 0)
            {
                // db float? -> c# double?
                // Error on Enum?
                list = Mapper.DynamicMap<IDataReader, List<Bet>>(dt.CreateDataReader());
            }

            Assert.IsTrue(list.Count > 0);
        }
    }
}

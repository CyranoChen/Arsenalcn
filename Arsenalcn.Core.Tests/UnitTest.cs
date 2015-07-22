using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;

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
    }
}

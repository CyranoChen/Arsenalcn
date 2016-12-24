using System;
using System.Data.SqlClient;
using Arsenalcn.Core;

namespace Arsenal.Service.Casino
{
    [DbSchema("AcnCasino_CasinoItem", Key = "CasinoItemGuid", Sort = "CloseTime DESC, CreateTime DESC")]
    public class CasinoItem : Entity<Guid>
    {
        public void Statistics()
        {
            var sql =
                $@"SELECT ISNULL(SUM(BetAmount), 0) - ISNULL(SUM(Earning), 0) AS TotalEarning 
                   FROM {Repository.GetTableAttr<Bet>().Name} WHERE CasinoItemGuid = @key";

            var dapper = new DapperHelper();

            Earning = dapper.ExecuteScalar<double>(sql, new { key = ID });

            IRepository repo = new Repository();

            repo.Update(this);
        }

        public static void Clean(SqlTransaction trans = null)
        {
            //DELETE FROM AcnCasino_CasinoItem WHERE (MatchGuid NOT IN(SELECT MatchGuid FROM AcnCasino_Match))
            var sql = string.Format(@"DELETE FROM {0} WHERE (MatchGuid NOT IN (SELECT MatchGuid FROM {1}));
                   DELETE FROM AcnCasino_MatchResult WHERE (CasinoItemGuid NOT IN (SELECT CasinoItemGuid FROM {0}));
                   DELETE FROM AcnCasino_SingleChoice WHERE (CasinoItemGuid NOT IN (SELECT CasinoItemGuid FROM {0}))",
                Repository.GetTableAttr<CasinoItem>().Name,
                Repository.GetTableAttr<Match>().Name);

            var dapper = new DapperHelper();

            dapper.Execute(sql, trans);
        }

        #region Members and Properties

        [DbColumn("ItemType")]
        public CasinoType ItemType { get; set; }

        [DbColumn("MatchGuid")]
        public Guid? MatchGuid { get; set; }

        [DbColumn("ItemTitle")]
        public string ItemTitle { get; set; }

        [DbColumn("ItemBody")]
        public string ItemBody { get; set; }

        [DbColumn("CreateTime")]
        public DateTime CreateTime { get; set; }

        [DbColumn("PublishTime")]
        public DateTime PublishTime { get; set; }

        [DbColumn("CloseTime")]
        public DateTime CloseTime { get; set; }

        [DbColumn("BankerID")]
        public Guid BankerID { get; set; }

        [DbColumn("BankerName")]
        public string BankerName { get; set; }

        [DbColumn("Earning")]
        public double? Earning { get; set; }

        [DbColumn("OwnerID")]
        public int OwnerID { get; set; }

        [DbColumn("OwnerUserName")]
        public string OwnerUserName { get; set; }

        #endregion
    }

    public enum CasinoType
    {
        SingleChoice = 2,
        MatchResult = 1
    }
}
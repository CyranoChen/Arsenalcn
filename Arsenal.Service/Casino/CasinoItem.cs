using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;

using Arsenalcn.Core;

namespace Arsenal.Service.Casino
{
    [DbSchema("AcnCasino_CasinoItem", Key = "CasinoItemGuid", Sort = "CloseTime DESC, CreateTime DESC")]
    public class CasinoItem : Entity<Guid>
    {
        public CasinoItem() : base() { }

        public static void CreateMap()
        {
            var map = AutoMapper.Mapper.CreateMap<IDataReader, CasinoItem>();

            map.ForMember(d => d.ID, opt => opt.MapFrom(s => (Guid)s.GetValue("CasinoItemGuid")));
        }

        public void Statistics()
        {
            Contract.Requires(this.ID != null && !this.ID.Equals(Guid.Empty));

            string sql = string.Format(@"SELECT ISNULL(SUM(Bet), 0) - ISNULL(SUM(Earning), 0) AS TotalEarning 
                   FROM {0} WHERE CasinoItemGuid = @key",
                   Repository.GetTableAttr<Bet>().Name);

            SqlParameter[] para = { new SqlParameter("@key", this.ID) };

            DataSet ds = DataAccess.ExecuteDataset(sql, para);

            if (ds.Tables[0].Rows.Count > 0)
            {
                this.Earning = (double)ds.Tables[0].Rows[0]["TotalEarning"];

                IRepository repo = new Repository();
                repo.Update<CasinoItem>(this);
            }
        }

        public static void Clean(SqlTransaction trans = null)
        {
            //DELETE FROM AcnCasino_CasinoItem WHERE (MatchGuid NOT IN(SELECT MatchGuid FROM AcnCasino_Match))
            string sql = string.Format(@"DELETE FROM {0} WHERE (MatchGuid NOT IN (SELECT MatchGuid FROM {1}));
                   DELETE FROM AcnCasino_MatchResult WHERE (CasinoItemGuid NOT IN (SELECT CasinoItemGuid FROM {0}));
                   DELETE FROM AcnCasino_SingleChoice WHERE (CasinoItemGuid NOT IN (SELECT CasinoItemGuid FROM {0}))",
                   Repository.GetTableAttr<CasinoItem>().Name,
                   Repository.GetTableAttr<Match>().Name);

            DataAccess.ExecuteNonQuery(sql, null, trans);
        }

        #region Members and Properties

        [DbColumn("ItemType")]
        public CasinoType ItemType
        { get; set; }

        [DbColumn("MatchGuid")]
        public Guid? MatchGuid
        { get; set; }

        [DbColumn("ItemTitle")]
        public string ItemTitle
        { get; set; }

        [DbColumn("ItemBody")]
        public string ItemBody
        { get; set; }

        [DbColumn("CreateTime")]
        public DateTime CreateTime
        { get; set; }

        [DbColumn("PublishTime")]
        public DateTime PublishTime
        { get; set; }

        [DbColumn("CloseTime")]
        public DateTime CloseTime
        { get; set; }

        [DbColumn("BankerID")]
        public Guid BankerID
        { get; set; }

        [DbColumn("BankerName")]
        public string BankerName
        { get; set; }

        [DbColumn("Earning")]
        public double? Earning
        { get; set; }

        [DbColumn("OwnerID")]
        public int OwnerID
        { get; set; }

        [DbColumn("OwnerUserName")]
        public string OwnerUserName
        { get; set; }

        #endregion
    }

    public enum CasinoType
    {
        SingleChoice = 2,
        MatchResult = 1
    }
}

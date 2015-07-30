using System;
using System.Data;

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
        SingleChoice,
        MatchResult
    }
}

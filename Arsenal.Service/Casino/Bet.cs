using System;
using System.Data;

using Arsenalcn.Core;

namespace Arsenal.Service.Casino
{
    [DbSchema("AcnCasino_Bet", Sort = "BetTime DESC")]
    public class Bet : Entity<int>
    {
        public Bet() : base() { }

        public static void CreateMap()
        {
            var map = AutoMapper.Mapper.CreateMap<IDataReader, Bet>();

            map.ForMember(d => d.BetAmount, opt => opt.MapFrom(s => s.GetValue("Bet")));
        }

        #region Members and Properties

        [DbColumn("UserID")]
        public int UserID
        { get; set; }

        [DbColumn("UserName")]
        public string UserName
        { get; set; }

        [DbColumn("CasinoItemGuid")]
        public Guid CasinoItemGuid
        { get; set; }

        [DbColumn("Bet")]
        public double? BetAmount
        { get; set; }

        [DbColumn("BetTime")]
        public DateTime BetTime
        { get; set; }

        [DbColumn("BetRate")]
        public double? BetRate
        { get; set; }

        [DbColumn("IsWin")]
        public bool? IsWin
        { get; set; }

        [DbColumn("Earning")]
        public double? Earning
        { get; set; }

        [DbColumn("EarningDesc")]
        public string EarningDesc
        { get; set; }

        #endregion
    }
}

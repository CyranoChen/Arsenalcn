using System;
using System.Data;

using Arsenalcn.Core;

namespace Arsenal.Service.Casino
{
    [DbSchema("AcnCasino_Bet", Sort = "BetTime DESC")]
    public class Bet : Entity<int>
    {
        public Bet() : base() { }

        public Bet(DataRow dr) : base(dr) { }

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
        public float? BetAmount
        { get; set; }

        [DbColumn("BetTime")]
        public DateTime BetTime
        { get; private set; }

        [DbColumn("BetRate")]
        public float? BetRate
        { get; set; }

        [DbColumn("IsWin")]
        public bool? IsWin
        { get; set; }

        [DbColumn("Earning")]
        public float? Earning
        { get; set; }

        [DbColumn("EarningDesc")]
        public string EarningDesc
        { get; set; }

        #endregion
    }
}

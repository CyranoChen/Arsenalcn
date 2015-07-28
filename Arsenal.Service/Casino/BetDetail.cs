using System;
using System.Data;

using Arsenalcn.Core;

namespace Arsenal.Service.Casino
{
    [DbSchema("AcnCasino_BetDetail", Sort = "ID DESC")]
    public class BetDetail : Entity<int>
    {
        public BetDetail() : base() { }

        public BetDetail(DataRow dr) : base(dr) { }

        #region Members and Properties

        [DbColumn("BetID")]
        public int BetID
        { get; set; }

        [DbColumn("DetailName")]
        public string DetailName
        { get; set; }

        [DbColumn("DetailValue")]
        public string DetailValue
        { get; set; }

        #endregion
    }
}

using System;
using System.Data;

using Arsenalcn.Core;

namespace Arsenal.Service.Casino
{
    [DbSchema("AcnCasino_ChoiceOption", Sort = "CasinoItemGuid, OrderID")]
    public class ChoiceOption : Entity<int>
    {
        public ChoiceOption() : base() { }

        public ChoiceOption(DataRow dr) : base(dr) { }

        #region Members and Properties

        [DbColumn("CasinoItemGuid")]
        public Guid CasinoItemGuid
        { get; set; }

        [DbColumn("OptionValue")]
        public string OptionName
        { get; set; }

        [DbColumn("OptionDisplay")]
        public string OptionDisplay
        { get; set; }

        [DbColumn("OptionRate")]
        public float OptionRate
        { get; set; }

        [DbColumn("OrderID")]
        public int OptionOrder
        { get; set; }

        #endregion
    }
}

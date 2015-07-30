using System;
using System.Data;

using Arsenalcn.Core;

namespace Arsenal.Service.Casino
{
    [DbSchema("AcnCasino_ChoiceOption", Sort = "CasinoItemGuid, OrderID")]
    public class ChoiceOption : Entity<int>
    {
        public ChoiceOption() : base() { }

        public static void CreateMap()
        {
            var map = AutoMapper.Mapper.CreateMap<IDataReader, ChoiceOption>();

            map.ForMember(d => d.OptionName,
                opt => opt.MapFrom(s => s.GetValue("OptionValue")));

            map.ForMember(d => d.OptionOrder,
                opt => opt.MapFrom(s => s.GetValue("OrderID")));
        }

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

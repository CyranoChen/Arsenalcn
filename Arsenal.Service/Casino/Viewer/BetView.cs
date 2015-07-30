using System;
using System.Collections.Generic;
using System.Data;

using Arsenalcn.Core;

namespace Arsenal.Service.Casino
{
    [DbSchema("AcnCasino_BetView", Sort = "b_BetTime DESC")]
    public class BetView : Viewer
    {
        public BetView() : base() { }

        public static void CreateMap()
        {
            var map = AutoMapper.Mapper.CreateMap<IDataReader, BetView>();

            map.ConstructUsing(s => new BetView()
            {
                ID = (int)s.GetValue("ID"),
                UserID = (int)s.GetValue("b_UserID"),
                CasinoItem = new CasinoItem()
                {
                    ItemType = (CasinoType)s.GetValue("c_ItemType")

                }
            });
        }


        public BetView(DataRow dr) : base(dr) { }

        #region Members and Properties

        [DbColumn("ID", IsKey = true)]
        public int ID
        { get; set; }

        [DbColumn("b_UserID")]
        public int UserID
        { get; set; }

        [DbColumn("b_UserName")]
        public string UserName
        { get; set; }

        [DbColumn("b_Bet")]
        public double? BetAmount
        { get; set; }

        [DbColumn("b_BetTime")]
        public DateTime BetTime
        { get; private set; }

        [DbColumn("b_BetRate")]
        public double? BetRate
        { get; set; }

        [DbColumn("b_IsWin")]
        public bool? IsWin
        { get; set; }

        [DbColumn("b_Earning")]
        public double? Earning
        { get; set; }

        [DbColumn("b_EarningDesc")]
        public string EarningDesc
        { get; set; }

        // Complex Object
        [DbColumn("c", Key = "c_CasinoItemGuid")]
        public CasinoItem CasinoItem
        { get; set; }

        [DbColumn("m", Key = "m_MatchGuid")]
        public Match Match
        { get; set; }

        [DbColumn("h", Key = "h_TeamGuid")]
        public Team Home
        { get; set; }

        [DbColumn("a", Key = "a_TeamGuid")]
        public Team Away
        { get; set; }

        [DbColumn("l", Key = "l_LeagueGuid")]
        public League League
        { get; set; }

        [DbColumn("@BetDetail", ForeignKey = "BetID")]
        public IEnumerable<BetDetail> ListBetDetail
        { get; set; }

        #endregion
    }
}

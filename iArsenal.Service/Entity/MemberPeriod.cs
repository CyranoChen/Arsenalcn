using System;
using System.Data;

using Arsenalcn.Core;

namespace iArsenal.Service
{
    [AttrDbTable("iArsenal_MemberPeriod", Sort = "ID DESC")]
    public class MemberPeriod : Entity<int>
    {
        public MemberPeriod() : base() { }

        public MemberPeriod(DataRow dr) : base(dr) { }

        #region Members and Properties

        [AttrDbColumn("MemberID")]
        public int MemberID
        { get; set; }

        [AttrDbColumn("MemberName")]
        public string MemberName
        { get; set; }

        [AttrDbColumn("MemberCardNo")]
        public string MemberCardNo
        { get; set; }

        [AttrDbColumn("MemberClass")]
        public MemberClassType MemberClass
        { get; set; }

        [AttrDbColumn("OrderID")]
        public int? OrderID
        { get; set; }

        [AttrDbColumn("StartDate")]
        public DateTime StartDate
        { get; set; }

        [AttrDbColumn("EndDate")]
        public DateTime EndDate
        { get; set; }

        [AttrDbColumn("IsActive")]
        public bool IsActive
        { get; set; }

        [AttrDbColumn("Description")]
        public string Description
        { get; set; }

        [AttrDbColumn("Remark")]
        public string Remark
        { get; set; }

        #endregion

    }

    public enum MemberClassType
    {
        Core = 1,
        Premier = 2
    }
}

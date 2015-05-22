using System;
using System.Data;
using System.Linq;

using Arsenalcn.Core;

namespace iArsenal.Service
{
    [AttrDbTable("iArsenal_MemberPeriod", Sort = "ID DESC")]
    public class MemberPeriod : Entity<int>
    {
        public MemberPeriod() : base() { }

        public MemberPeriod(DataRow dr) : base(dr) { }

        public static MemberPeriod GetCurrentMemberPeriodByMemberID(int id)
        {
            var pcMemberPeriod = new PropertyCollection();

            pcMemberPeriod.Add("MemberID", id);
            pcMemberPeriod.Add("IsActive", true);

            IRepository repo = new Repository();

            return repo.Query<MemberPeriod>(pcMemberPeriod).FirstOrDefault(x =>
                x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now);
        }

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

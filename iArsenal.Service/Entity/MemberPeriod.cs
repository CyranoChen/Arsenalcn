using System;
using Arsenalcn.Core;

namespace iArsenal.Service
{
    [DbSchema("iArsenal_MemberPeriod", Sort = "ID DESC")]
    public class MemberPeriod : Entity<int>
    {
        public bool IsCurrentSeason(int year = 0)
        {
            var date = DateTime.Now.AddYears(year);
            return EndDate.AddYears(-1) <= date && EndDate >= date;
        }

        public static MemberPeriod GetCurrentMemberPeriodByMemberID(int id, int year = 0)
        {
            var date = DateTime.Now.AddYears(year);

            IRepository repo = new Repository();

            // StartDate change to the EndDate.Addyears(-1) for the correct date range
            return repo.Query<MemberPeriod>(x => x.MemberID == id)
                .Find(x => x.IsActive && x.EndDate.AddYears(-1) <= date && x.EndDate >= date);
        }

        #region Members and Properties

        [DbColumn("MemberID")]
        public int MemberID { get; set; }

        [DbColumn("MemberName")]
        public string MemberName { get; set; }

        [DbColumn("MemberCardNo")]
        public string MemberCardNo { get; set; }

        [DbColumn("MemberClass")]
        public MemberClassType MemberClass { get; set; }

        [DbColumn("OrderID")]
        public int? OrderID { get; set; }

        [DbColumn("StartDate")]
        public DateTime StartDate { get; set; }

        [DbColumn("EndDate")]
        public DateTime EndDate { get; set; }

        [DbColumn("IsActive")]
        public bool IsActive { get; set; }

        [DbColumn("Description")]
        public string Description { get; set; }

        [DbColumn("Remark")]
        public string Remark { get; set; }

        #endregion
    }

    public enum MemberClassType
    {
        Core = 1,
        Premier = 2
    }
}
using System;
using System.Collections;
using System.Linq;
using Arsenalcn.Core;

namespace iArsenal.Service
{
    [DbSchema("iArsenal_MemberPeriod", Sort = "ID DESC")]
    public class MemberPeriod : Entity<int>
    {
        public bool IsCurrentSeason(int year = 0)
        {
            var _date = DateTime.Now.AddYears(year);
            return StartDate <= _date && EndDate >= _date;
        }

        public static MemberPeriod GetCurrentMemberPeriodByMemberID(int id, int year = 0)
        {
            var htWhere = new Hashtable();
            var _date = DateTime.Now.AddYears(year);

            htWhere.Add("MemberID", id);
            htWhere.Add("IsActive", true);

            IRepository repo = new Repository();

            return repo.Query<MemberPeriod>(x =>
                x.MemberID == id && x.StartDate <= _date && x.EndDate >= _date).Find(x => x.IsActive);
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
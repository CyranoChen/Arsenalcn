using System;
using System.Collections.Generic;
using System.Data;

using Arsenalcn.Common.Entity;

namespace iArsenal.Entity
{
    public class MemberPeriod
    {
        public MemberPeriod() { }

        private MemberPeriod(DataRow dr)
        {
            InitMemberPeriod(dr);
        }

        private void InitMemberPeriod(DataRow dr)
        {
            if (dr != null)
            {
                MemberPeriodID = Convert.ToInt32(dr["ID"]);
                MemberID = Convert.ToInt32(dr["MemberID"]);
                MemberName = dr["MemberName"].ToString();
                MemberCardNo = dr["MemberCardNo"].ToString();
                MemberClass = (MemberClassType)Enum.Parse(typeof(MemberClassType), dr["MemberClass"].ToString());

                if (!Convert.IsDBNull(dr["OrderID"]))
                    OrderID = Convert.ToInt32(dr["OrderID"]);
                else
                    OrderID = null;

                StartDate = (DateTime)dr["StartDate"];
                EndDate = (DateTime)dr["EndDate"];
                IsActive = Convert.ToBoolean(dr["IsActive"]);
                Description = dr["Description"].ToString();
                Remark = dr["Remark"].ToString();
            }
            else
                throw new Exception("Unable to init MemberPeriod.");
        }

        public void Select()
        {
            DataRow dr = DataAccess.MemberPeriod.GetMemberPeriodByID(MemberPeriodID);

            if (dr != null)
                InitMemberPeriod(dr);
        }

        public void Update()
        {
            DataAccess.MemberPeriod.UpdateMemberPeriod(MemberPeriodID, MemberID, MemberName, MemberCardNo, (int)MemberClass, OrderID, StartDate, EndDate, IsActive, Description, Remark);
        }

        public void Insert()
        {
            DataAccess.MemberPeriod.InsertMemberPeriod(MemberPeriodID, MemberID, MemberName, MemberCardNo, (int)MemberClass, OrderID, StartDate, EndDate, IsActive, Description, Remark);
        }

        public void Delete()
        {
            DataAccess.MemberPeriod.DeleteMemberPeriod(MemberPeriodID);
        }

        public static List<MemberPeriod> GetMemberPeriods()
        {
            DataTable dt = DataAccess.MemberPeriod.GetMemberPeriods();
            List<MemberPeriod> list = new List<MemberPeriod>();

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new MemberPeriod(dr));
                }
            }

            return list;
        }

        public static List<MemberPeriod> GetMemberPeriods(int memberID)
        {
            DataTable dt = DataAccess.MemberPeriod.GetMemberPeriods(memberID);
            List<MemberPeriod> list = new List<MemberPeriod>();

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new MemberPeriod(dr));
                }
            }

            return list;
        }


        #region Members and Properties

        public int MemberPeriodID
        { get; set; }

        public int MemberID
        { get; set; }

        public string MemberName
        { get; set; }

        public string MemberCardNo
        { get; set; }

        public MemberClassType MemberClass
        { get; set; }

        public int? OrderID
        { get; set; }

        public DateTime StartDate
        { get; set; }

        public DateTime EndDate
        { get; set; }

        public bool IsActive
        { get; set; }

        public string Description
        { get; set; }

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

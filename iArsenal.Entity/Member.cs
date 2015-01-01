using System;
using System.Collections.Generic;
using System.Data;

using Arsenalcn.Common.Entity;

namespace iArsenal.Entity
{
    public class Member
    {
        public Member() { }

        private Member(DataRow dr)
        {
            InitMember(dr);
        }

        private void InitMember(DataRow dr)
        {
            if (dr != null)
            {
                MemberID = Convert.ToInt32(dr["ID"]);
                Name = dr["Name"].ToString();
                Gender = Convert.ToBoolean(dr["Gender"]);

                if (!Convert.IsDBNull(dr["Birthday"]))
                    Birthday = (DateTime)dr["Birthday"];
                else
                    Birthday = null;

                Career = dr["Career"].ToString();
                Nation = dr["Nation"].ToString();
                Region = dr["Region"].ToString();
                Mobile = dr["Mobile"].ToString();
                Telephone = dr["Telephone"].ToString();
                Address = dr["Address"].ToString();
                Email = dr["Email"].ToString();
                Zipcode = dr["Zipcode"].ToString();
                MSN = dr["MSN"].ToString();
                QQ = dr["QQ"].ToString();
                IDCardNo = dr["IDCardNo"].ToString();
                PassportNo = dr["PassportNo"].ToString();
                PassportName = dr["PassportName"].ToString();
                AcnID = Convert.ToInt32(dr["AcnID"]);
                AcnName = dr["AcnName"].ToString();
                IP = dr["IP"].ToString();
                TaobaoName = dr["TaobaoName"].ToString();
                Evalution = (MemberEvalution)Enum.Parse(typeof(MemberEvalution), dr["Evalution"].ToString());
                MemberType = (MemberType)Enum.Parse(typeof(MemberType), dr["MemberType"].ToString());
                MemberCardNo = dr["MemberCardNo"].ToString();
                JoinDate = (DateTime)dr["JoinDate"];
                LastLoginTime = (DateTime)dr["LastLoginTime"];
                IsActive = Convert.ToBoolean(dr["IsActive"]);
                Description = dr["Description"].ToString();
                Remark = dr["Remark"].ToString();

                #region Generate Member RegionInfo
                if (Nation.Equals("中国"))
                {
                    string[] region = Region.Split('|');
                    int itemID = int.MinValue;
                    if (region.Length > 1)
                    {
                        foreach (string s in region)
                        {
                            if (int.TryParse(s, out itemID))
                                RegionInfo += DictionaryItem.Cache.Load(itemID).Name;
                        }
                    }
                    else if (region.Length == 1 && int.TryParse(region[0], out itemID))
                        RegionInfo = DictionaryItem.Cache.Load(itemID).Name;
                    else
                        RegionInfo = region[0];
                }
                else
                {
                    RegionInfo = Nation;
                }
                #endregion

                #region Generate Member MemberTypeInfo
                switch (MemberType)
                {
                    case MemberType.Match:
                        MemberTypeInfo = "观赛";
                        break;
                    case MemberType.VIP:
                        MemberTypeInfo = "VIP";
                        break;
                    case MemberType.Secretary:
                        MemberTypeInfo = "干事";
                        break;
                    case MemberType.Buyer:
                        MemberTypeInfo = "团购";
                        break;
                    default:
                        MemberTypeInfo = string.Empty;
                        break;
                }
                #endregion
            }
            else
                throw new Exception("Unable to init Member.");
        }

        public void Select()
        {
            DataRow dr = DataAccess.Member.GetMemberByID(MemberID);

            if (dr != null)
                InitMember(dr);
        }

        public void Select(int acnID)
        {
            DataRow dr = DataAccess.Member.GetMemberByAcnID(acnID);

            if (dr != null)
                InitMember(dr);
        }

        public void Update()
        {
            DataAccess.Member.UpdateMember(MemberID, Name, Gender, Birthday, Career, Nation, Region, Mobile, Telephone, Address, Email, Zipcode, MSN, QQ, IDCardNo, PassportNo, PassportName, AcnID, AcnName, IP, TaobaoName, (int)Evalution, (int)MemberType, MemberCardNo, JoinDate, LastLoginTime, IsActive, Description, Remark);
        }

        public void Insert()
        {
            DataAccess.Member.InsertMember(MemberID, Name, Gender, Birthday, Career, Nation, Region, Mobile, Telephone, Address, Email, Zipcode, MSN, QQ, IDCardNo, PassportNo, PassportName, AcnID, AcnName, IP, TaobaoName, (int)Evalution, (int)MemberType, MemberCardNo, JoinDate, LastLoginTime, IsActive, Description, Remark);
        }

        public void Delete()
        {
            DataAccess.Member.DeleteMember(MemberID);
        }

        public static List<Member> GetMembers()
        {
            DataTable dt = DataAccess.Member.GetMembers();
            List<Member> list = new List<Member>();

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new Member(dr));
                }
            }

            return list;
        }

        public static class Cache
        {
            static Cache()
            {
                InitCache();
            }

            public static void RefreshCache()
            {
                InitCache();
            }

            private static void InitCache()
            {
                MemberList = GetMembers();
            }

            public static Member Load(int id)
            {
                return MemberList.Find(m => m.MemberID.Equals(id));
            }

            public static List<Member> MemberList;
        }

        #region Members and Properties

        public int MemberID
        { get; set; }

        public string Name
        { get; set; }

        public bool Gender
        { get; set; }

        public DateTime? Birthday
        { get; set; }

        public string Career
        { get; set; }

        public string Nation
        { get; set; }

        public string Region
        { get; set; }

        public string Mobile
        { get; set; }

        public string Telephone
        { get; set; }

        public string Address
        { get; set; }

        public string Email
        { get; set; }

        public string Zipcode
        { get; set; }

        public string MSN
        { get; set; }

        public string QQ
        { get; set; }

        public string IDCardNo
        { get; set; }

        public string PassportNo
        { get; set; }

        public string PassportName
        { get; set; }

        public int AcnID
        { get; set; }

        public string AcnName
        { get; set; }

        public string IP
        { get; set; }

        public string TaobaoName
        { get; set; }

        public MemberEvalution Evalution
        { get; set; }

        public MemberType MemberType
        { get; set; }

        public string MemberCardNo
        { get; set; }

        public DateTime JoinDate
        { get; set; }

        public DateTime LastLoginTime
        { get; set; }

        public bool IsActive
        { get; set; }

        public string Description
        { get; set; }

        public string Remark
        { get; set; }

        public string RegionInfo
        { get; set; }

        public string MemberTypeInfo
        { get; set; }

        #endregion
    }

    public enum MemberType
    {
        Null = 0,
        Match = 1,
        VIP = 2,
        Secretary = 3,
        Buyer = 4
    }

    public enum MemberEvalution
    {
        Null = 0,
        BlackList = 1,
        WhiteList = 2
    }
}

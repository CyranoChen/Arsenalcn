using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Arsenalcn.Core;

namespace iArsenal.Service
{
    [AttrDbTable("iArsenal_Member", Sort = "ID DESC")]
    public class Member : Entity<int>
    {
        public Member() : base() { }

        public Member(DataRow dr)
            : base(dr)
        {
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
                IRepository repo = new Repository();

                MemberList = repo.All<Member>().ToList();
            }

            public static Member Load(int id)
            {
                return MemberList.Find(x => x.ID.Equals(id));
            }

            public static Member LoadByAcnID(int id)
            {
                return MemberList.Find(x => x.AcnID.Equals(id));
            }

            public static List<Member> MemberList;
        }

        #region Members and Properties

        [AttrDbColumn("Name")]
        public string Name
        { get; set; }

        [AttrDbColumn("Gender")]
        public bool Gender
        { get; set; }

        [AttrDbColumn("Birthday")]
        public DateTime? Birthday
        { get; set; }

        [AttrDbColumn("Career")]
        public string Career
        { get; set; }

        [AttrDbColumn("Nation")]
        public string Nation
        { get; set; }

        [AttrDbColumn("Region")]
        public string Region
        { get; set; }

        [AttrDbColumn("Mobile")]
        public string Mobile
        { get; set; }

        [AttrDbColumn("Telephone")]
        public string Telephone
        { get; set; }

        [AttrDbColumn("Address")]
        public string Address
        { get; set; }

        [AttrDbColumn("Email")]
        public string Email
        { get; set; }

        [AttrDbColumn("Zipcode")]
        public string Zipcode
        { get; set; }

        [AttrDbColumn("MSN")]
        public string MSN
        { get; set; }

        [AttrDbColumn("QQ")]
        public string QQ
        { get; set; }

        [AttrDbColumn("IDCardNo")]
        public string IDCardNo
        { get; set; }

        [AttrDbColumn("PassportNo")]
        public string PassportNo
        { get; set; }

        [AttrDbColumn("PassportName")]
        public string PassportName
        { get; set; }

        [AttrDbColumn("AcnID")]
        public int AcnID
        { get; set; }

        [AttrDbColumn("AcnName")]
        public string AcnName
        { get; set; }

        [AttrDbColumn("IP")]
        public string IP
        { get; set; }

        [AttrDbColumn("TaobaoName")]
        public string TaobaoName
        { get; set; }

        [AttrDbColumn("Evalution")]
        public MemberEvalution Evalution
        { get; set; }

        [AttrDbColumn("MemberType")]
        public MemberType MemberType
        { get; set; }

        [AttrDbColumn("MemberCardNo")]
        public string MemberCardNo
        { get; set; }

        [AttrDbColumn("JoinDate")]
        public DateTime JoinDate
        { get; set; }
        
        [AttrDbColumn("LastLoginTime")]
        public DateTime LastLoginTime
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

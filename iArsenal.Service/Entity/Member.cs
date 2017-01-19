using System;
using System.Collections.Generic;
using System.Linq;
using Arsenalcn.Core;

namespace iArsenal.Service
{
    [DbSchema("iArsenal_Member", Sort = "ID DESC")]
    public class Member : Entity<int>
    {
        public override void Inital()
        {
            switch (MemberType)
            {
                case MemberType.Buyer:
                    MemberTypeInfo = "团购";
                    break;
                case MemberType.Match:
                    MemberTypeInfo = "观赛";
                    break;
                case MemberType.Activity:
                    MemberTypeInfo = "活动";
                    break;
                case MemberType.Secretary:
                    MemberTypeInfo = "干事";
                    break;
                default:
                    MemberTypeInfo = string.Empty;
                    break;
            }

            if (Nation.Equals("中国"))
            {
                var regions = Region.Split('|');
                int itemId;
                if (regions.Length > 1)
                {
                    foreach (var r in regions)
                    {
                        if (int.TryParse(r, out itemId))
                        { RegionInfo += DictionaryItem.Cache.Load(itemId).Name; }
                    }
                }
                else if (regions.Length == 1 && int.TryParse(regions[0], out itemId))
                {
                    RegionInfo = DictionaryItem.Cache.Load(itemId).Name;
                }
                else
                {
                    RegionInfo = regions[0];
                }
            }
            else
            {
                RegionInfo = Nation;
            }
        }

        public static class Cache
        {
            public static List<Member> MemberList;

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
        }

        #region Members and Properties

        [DbColumn("Name")]
        public string Name { get; set; }

        [DbColumn("Gender")]
        public bool Gender { get; set; }

        [DbColumn("Birthday")]
        public DateTime? Birthday { get; set; }

        [DbColumn("Career")]
        public string Career { get; set; }

        [DbColumn("Nation")]
        public string Nation { get; set; }

        [DbColumn("Region")]
        public string Region { get; set; }

        [DbColumn("Mobile")]
        public string Mobile { get; set; }

        [DbColumn("Telephone")]
        public string Telephone { get; set; }

        [DbColumn("Address")]
        public string Address { get; set; }

        [DbColumn("Email")]
        public string Email { get; set; }

        [DbColumn("Zipcode")]
        public string Zipcode { get; set; }

        [DbColumn("WeChat")]
        public string WeChat { get; set; }

        [DbColumn("QQ")]
        public string QQ { get; set; }

        [DbColumn("IDCardNo")]
        public string IDCardNo { get; set; }

        [DbColumn("PassportNo")]
        public string PassportNo { get; set; }

        [DbColumn("PassportName")]
        public string PassportName { get; set; }

        [DbColumn("AcnID")]
        public int AcnID { get; set; }

        [DbColumn("AcnName")]
        public string AcnName { get; set; }

        [DbColumn("IP")]
        public string IP { get; set; }

        [DbColumn("TaobaoName")]
        public string TaobaoName { get; set; }

        [DbColumn("Evalution")]
        public MemberEvalution Evalution { get; set; }

        [DbColumn("MemberType")]
        public MemberType MemberType { get; set; }

        [DbColumn("MemberCardNo")]
        public string MemberCardNo { get; set; }

        [DbColumn("OfficialSync")]
        public string OfficialSync { get; set; }

        [DbColumn("JoinDate")]
        public DateTime JoinDate { get; set; }

        [DbColumn("LastLoginTime")]
        public DateTime LastLoginTime { get; set; }

        [DbColumn("IsActive")]
        public bool IsActive { get; set; }

        [DbColumn("Description")]
        public string Description { get; set; }

        [DbColumn("Remark")]
        public string Remark { get; set; }

        public string RegionInfo { get; set; }

        public string MemberTypeInfo { get; set; }

        #endregion
    }

    public enum MemberType
    {
        None = 0,
        Buyer = 1,
        Match = 2,
        Activity = 3,
        Secretary = 4
    }

    public enum MemberEvalution
    {
        None = 0,
        BlackList = 1,
        WhiteList = 2
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Arsenalcn.Core;
using DataReaderMapper;

namespace iArsenal.Service
{
    [DbSchema("iArsenal_Member", Sort = "ID DESC")]
    public class Member : Entity<int>
    {
        public static void CreateMap()
        {
            var map = Mapper.CreateMap<IDataReader, Member>();

            map.ForMember(d => d.MemberTypeInfo, opt => opt.ResolveUsing(s =>
            {
                #region Generate Member MemberTypeInfo

                var retValue = string.Empty;

                switch ((MemberType) ((int) s.GetValue("MemberType")))
                {
                    case MemberType.Match:
                        retValue = "观赛";
                        break;
                    case MemberType.VIP:
                        retValue = "VIP";
                        break;
                    case MemberType.Secretary:
                        retValue = "干事";
                        break;
                    case MemberType.Buyer:
                        retValue = "团购";
                        break;
                    default:
                        retValue = string.Empty;
                        break;
                }

                return retValue;

                #endregion
            }));

            map.ForMember(d => d.RegionInfo, opt => opt.ResolveUsing(s =>
            {
                #region Generate Member RegionInfo

                var nation = s.GetValue("Nation").ToString();
                var region = s.GetValue("Region").ToString();
                var retValue = string.Empty;

                if (nation.Equals("中国"))
                {
                    var regions = region.Split('|');
                    var itemID = int.MinValue;
                    if (regions.Length > 1)
                    {
                        foreach (var r in regions)
                        {
                            if (int.TryParse(r, out itemID))
                                retValue += DictionaryItem.Cache.Load(itemID).Name;
                        }
                    }
                    else if (regions.Length == 1 && int.TryParse(regions[0], out itemID))
                    {
                        retValue = DictionaryItem.Cache.Load(itemID).Name;
                    }
                    else
                    {
                        retValue = regions[0];
                    }
                }
                else
                {
                    retValue = nation;
                }

                return retValue;

                #endregion
            }));
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

        [DbColumn("MSN")]
        public string MSN { get; set; }

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
        Match = 1,
        VIP = 2,
        Secretary = 3,
        Buyer = 4
    }

    public enum MemberEvalution
    {
        None = 0,
        BlackList = 1,
        WhiteList = 2
    }
}
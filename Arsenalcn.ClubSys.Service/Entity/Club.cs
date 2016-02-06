using System;
using System.Collections.Generic;
using System.Data;

namespace Arsenalcn.ClubSys.Entity
{
    public class Club : ClubSysObject
    {
        public Club()
        {
        }

        public Club(DataRow dr)
            : base(dr)
        {
        }

        #region Members and Properties

        /// <summary>
        ///     Club UID
        /// </summary>
        [ClubSysDbColumn("ClubUid")]
        public int? ID { get; set; } = null;

        /// <summary>
        ///     Club Full Name
        /// </summary>
        [ClubSysDbColumn("FullName")]
        public string FullName { get; set; }

        /// <summary>
        ///     Club Short Name
        /// </summary>
        [ClubSysDbColumn("ShortName")]
        public string ShortName { get; set; }

        /// <summary>
        ///     Club Rank Level
        /// </summary>
        [ClubSysDbColumn("RankLevel")]
        public int? RankLevel { get; set; }

        /// <summary>
        ///     Club Rank Score
        /// </summary>
        [ClubSysDbColumn("RankScore")]
        public int? RankScore { get; set; }

        /// <summary>
        ///     Club Logo Storage Path
        /// </summary>
        [ClubSysDbColumn("Logo")]
        public string LogoName { get; set; }

        /// <summary>
        ///     Club Slogan
        /// </summary>
        [ClubSysDbColumn("Slogan")]
        public string Slogan { get; set; }

        /// <summary>
        ///     Club Description
        /// </summary>
        [ClubSysDbColumn("Description")]
        public string Description { get; set; }

        /// <summary>
        ///     Club Creator User ID
        /// </summary>
        [ClubSysDbColumn("CreatorUid")]
        public int? CreatorUid { get; set; }

        /// <summary>
        ///     Club Creator UserName
        /// </summary>
        [ClubSysDbColumn("CreatorUserName")]
        public string CreatorUserName { get; set; }

        /// <summary>
        ///     Club Manager User ID
        /// </summary>
        [ClubSysDbColumn("ManagerUid")]
        public int? ManagerUid { get; set; }

        /// <summary>
        ///     Club Manager UserName
        /// </summary>
        [ClubSysDbColumn("ManagerUserName")]
        public string ManagerUserName { get; set; }

        /// <summary>
        ///     Club Create Date
        /// </summary>
        [ClubSysDbColumn("CreateDate")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        ///     Club Update Date
        /// </summary>
        [ClubSysDbColumn("UpdateDate")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        ///     Club Active Flag
        /// </summary>
        [ClubSysDbColumn("IsActive")]
        public bool? IsActive { get; set; }

        /// <summary>
        ///     Club Appliable Flag
        /// </summary>
        [ClubSysDbColumn("IsAppliable")]
        public bool? IsAppliable { get; set; }

        /// <summary>
        ///     Club Tax
        /// </summary>
        [ClubSysDbColumn("Fortune")]
        public int? Fortune { get; set; }

        /// <summary>
        ///     Club Member Total Credit
        /// </summary>
        [ClubSysDbColumn("MemberCredit")]
        public int? MemberCredit { get; set; }

        /// <summary>
        ///     Club Member Total Fortune
        /// </summary>
        [ClubSysDbColumn("MemberFortune")]
        public int? MemberFortune { get; set; }

        /// <summary>
        ///     Club Member Total RP
        /// </summary>
        [ClubSysDbColumn("MemberRP")]
        public int? MemberRP { get; set; }

        /// <summary>
        ///     Club Member Total Loyalty
        /// </summary>
        [ClubSysDbColumn("MemberLoyalty")]
        public int? MemberLoyalty { get; set; }

        #endregion
    }

    public class ClubComparer : IComparer<Club>
    {
        #region IComparer<Club> Members

        public int Compare(Club x, Club y)
        {
            var xFortune = 0;
            var yFortune = 0;

            if (x.Fortune.HasValue)
                xFortune = x.Fortune.Value;

            if (y.Fortune.HasValue)
                yFortune = y.Fortune.Value;

            return yFortune - xFortune;
        }

        #endregion
    }
}
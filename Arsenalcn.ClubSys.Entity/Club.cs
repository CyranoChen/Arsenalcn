using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Collections.Generic;

namespace Arsenalcn.ClubSys.Entity
{
    public class Club : ClubSysObject
    {
        #region Members and Properties

        private int? _id = null;
        /// <summary>
        /// Club UID
        /// </summary>
        [ClubSysDbColumn("ClubUid")]
        public int? ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        private string _fullName;
        /// <summary>
        /// Club Full Name
        /// </summary>
        [ClubSysDbColumn("FullName")]
        public string FullName
        {
            get
            {
                return _fullName;
            }
            set
            {
                _fullName = value;
            }
        }

        private string _shortName;
        /// <summary>
        /// Club Short Name
        /// </summary>
        [ClubSysDbColumn("ShortName")]
        public string ShortName
        {
            get
            {
                return _shortName;
            }
            set
            {
                _shortName = value;
            }
        }

        private int? _rankLevel;
        /// <summary>
        /// Club Rank Level
        /// </summary>
        [ClubSysDbColumn("RankLevel")]
        public int? RankLevel
        {
            get
            {
                return _rankLevel;
            }
            set
            {
                _rankLevel = value;
            }
        }

        private int? _rankScore;
        /// <summary>
        /// Club Rank Score
        /// </summary>
        [ClubSysDbColumn("RankScore")]
        public int? RankScore
        {
            get
            {
                return _rankScore;
            }
            set
            {
                _rankScore = value;
            }
        }

        private string _logo;
        /// <summary>
        /// Club Logo Storage Path
        /// </summary>
        [ClubSysDbColumn("Logo")]
        public string LogoName
        {
            get
            {
                return _logo;
            }
            set
            {
                _logo = value;
            }
        }

        private string _slogan;
        /// <summary>
        /// Club Slogan
        /// </summary>
        [ClubSysDbColumn("Slogan")]
        public string Slogan
        {
            get
            {
                return _slogan;
            }
            set
            {
                _slogan = value;
            }
        }

        private string _desc;
        /// <summary>
        /// Club Description
        /// </summary>
        [ClubSysDbColumn("Description")]
        public string Description
        {
            get
            {
                return _desc;
            }
            set
            {
                _desc = value;
            }
        }

        private int? _creatorUid;
        /// <summary>
        /// Club Creator User ID
        /// </summary>
        [ClubSysDbColumn("CreatorUid")]
        public int? CreatorUid
        {
            get
            {
                return _creatorUid;
            }
            set
            {
                _creatorUid = value;
            }
        }

        private string _creatorUsername;
        /// <summary>
        /// Club Creator UserName
        /// </summary>
        [ClubSysDbColumn("CreatorUserName")]
        public string CreatorUserName
        {
            get
            {
                return _creatorUsername;
            }
            set
            {
                _creatorUsername = value;
            }
        }

        private int? _managerUid;
        /// <summary>
        /// Club Manager User ID
        /// </summary>
        [ClubSysDbColumn("ManagerUid")]
        public int? ManagerUid
        {
            get
            {
                return _managerUid;
            }
            set
            {
                _managerUid = value;
            }
        }

        private string _managerUsername;
        /// <summary>
        /// Club Manager UserName
        /// </summary>
        [ClubSysDbColumn("ManagerUserName")]
        public string ManagerUserName
        {
            get
            {
                return _managerUsername;
            }
            set
            {
                _managerUsername = value;
            }
        }

        private DateTime _createDate;
        /// <summary>
        /// Club Create Date
        /// </summary>
        [ClubSysDbColumn("CreateDate")]
        public DateTime CreateDate
        {
            get
            {
                return _createDate;
            }
            set
            {
                _createDate = value;
            }
        }

        private DateTime _updateDate;
        /// <summary>
        /// Club Update Date
        /// </summary>
        [ClubSysDbColumn("UpdateDate")]
        public DateTime UpdateDate
        {
            get
            {
                return _updateDate;
            }
            set
            {
                _updateDate = value;
            }
        }

        private bool? _isActive;
        /// <summary>
        /// Club Active Flag
        /// </summary>
        [ClubSysDbColumn("IsActive")]
        public bool? IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                _isActive = value;
            }
        }

        private bool? _isAppliable;
        /// <summary>
        /// Club Appliable Flag
        /// </summary>
        [ClubSysDbColumn("IsAppliable")]
        public bool? IsAppliable
        {
            get
            {
                return _isAppliable;
            }
            set
            {
                _isAppliable = value;
            }
        }

        private int? _fortune;
        /// <summary>
        /// Club Tax
        /// </summary>
        [ClubSysDbColumn("Fortune")]
        public int? Fortune
        {
            get
            {
                return _fortune;
            }
            set
            {
                _fortune = value;
            }
        }

        private int? _memberCredit;
        /// <summary>
        /// Club Member Total Credit
        /// </summary>
        [ClubSysDbColumn("MemberCredit")]
        public int? MemberCredit
        {
            get
            {
                return _memberCredit;
            }
            set
            {
                _memberCredit = value;
            }
        }

        private int? _memberFortune;
        /// <summary>
        /// Club Member Total Fortune
        /// </summary>
        [ClubSysDbColumn("MemberFortune")]
        public int? MemberFortune
        {
            get
            {
                return _memberFortune;
            }
            set
            {
                _memberFortune = value;
            }
        }

        private int? _memberRP;
        /// <summary>
        /// Club Member Total RP
        /// </summary>
        [ClubSysDbColumn("MemberRP")]
        public int? MemberRP
        {
            get
            {
                return _memberRP;
            }
            set
            {
                _memberRP = value;
            }
        }

        private int? _memberLoyalty;
        /// <summary>
        /// Club Member Total Loyalty
        /// </summary>
        [ClubSysDbColumn("MemberLoyalty")]
        public int? MemberLoyalty
        {
            get
            {
                return _memberLoyalty;
            }
            set
            {
                _memberLoyalty = value;
            }
        }

        #endregion

        public Club()
        {
        }

        public Club(DataRow dr)
            : base(dr)
        {
        }
    }

    public class ClubComparer : IComparer<Club>
    {
        #region IComparer<Club> Members

        public int Compare(Club x, Club y)
        {
            int xFortune = 0;
            int yFortune = 0;

            if( x.Fortune.HasValue )
                xFortune = x.Fortune.Value;

            if( y.Fortune.HasValue )
                yFortune = y.Fortune.Value;

            return yFortune - xFortune;
        }

        #endregion
    }
}

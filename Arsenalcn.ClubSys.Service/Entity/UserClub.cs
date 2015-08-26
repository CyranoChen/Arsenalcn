using System;
using System.Data;

namespace Arsenalcn.ClubSys.Entity
{
    public class UserClub : ClubSysObject
    {
        #region Members and Properties

        private int? _id = null;

        /// <summary>
        /// ID
        /// </summary>
        [ClubSysDbColumn("ID")]
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

        private int? _userId;
        /// <summary>
        /// User ID
        /// </summary>
        [ClubSysDbColumn("UserID")]
        public int? Userid
        {
            get
            {
                return _userId;
            }
            set
            {
                _userId = value;
            }
        }

        private string _username;
        /// <summary>
        /// User Name
        /// </summary>
        [ClubSysDbColumn("UserName")]
        public string UserName
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
            }
        }

        private int? _clubUid;
        /// <summary>
        /// Club ID
        /// </summary>
        [ClubSysDbColumn("ClubUid")]
        public int? ClubUid
        {
            get
            {
                return _clubUid;
            }
            set
            {
                _clubUid = value;
            }
        }

        private int? _responsibility;
        /// <summary>
        /// Responsibility
        /// </summary>
        [ClubSysDbColumn("Responsibility")]
        public int? Responsibility
        {
            get
            {
                return _responsibility;
            }
            set
            {
                _responsibility = value;
            }
        }

        private DateTime _fromDate;
        /// <summary>
        /// From Date
        /// </summary>
        [ClubSysDbColumn("FromDate")]
        public DateTime FromDate
        {
            get
            {
                return _fromDate;
            }
            set
            {
                _fromDate = value;
            }
        }

        private DateTime? _toDate = null;
        /// <summary>
        /// To Date
        /// </summary>
        [ClubSysDbColumn("ToDate")]
        public DateTime? ToDate
        {
            get
            {
                return _toDate;
            }
            set
            {
                _toDate = value;
            }
        }

        private DateTime? _joinClubDate = null;
        /// <summary>
        /// Join Club Date
        /// </summary>
        [ClubSysDbColumn("JoinClubDate")]
        public DateTime? JoinClubDate
        {
            get
            {
                return _joinClubDate;
            }
            set
            {
                _joinClubDate = value;
            }
        }

        private bool? _isActive;
        /// <summary>
        /// Active Flag
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

        #endregion

        public UserClub()
        {
        }

        public UserClub(DataRow dr)
            : base(dr)
        {
        }
    }
}

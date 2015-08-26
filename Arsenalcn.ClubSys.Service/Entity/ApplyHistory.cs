using System;
using System.Data;

namespace Arsenalcn.ClubSys.Entity
{
    public class ApplyHistory : ClubSysObject
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

        private int _userId;
        /// <summary>
        /// User ID
        /// </summary>
        [ClubSysDbColumn("UserID")]
        public int Userid
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

        private int _clubUid;
        /// <summary>
        /// Club ID
        /// </summary>
        [ClubSysDbColumn("ClubUid")]
        public int ClubUid
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

        private DateTime _applyDate;
        /// <summary>
        /// Apply Date
        /// </summary>
        [ClubSysDbColumn("ApplyDate")]
        public DateTime ApplyDate
        {
            get
            {
                return _applyDate;
            }
            set
            {
                _applyDate = value;
            }
        }

        private bool? _isAccepted;
        /// <summary>
        /// Is Accepted
        /// </summary>
        [ClubSysDbColumn("IsAccepted")]
        public bool? IsAccepted
        {
            get
            {
                return _isAccepted;
            }
            set
            {
                _isAccepted = value;
            }
        }

        #endregion

        public ApplyHistory()
        {

        }

        public ApplyHistory(DataRow dr)
            : base(dr)
        {
        }
    }
}

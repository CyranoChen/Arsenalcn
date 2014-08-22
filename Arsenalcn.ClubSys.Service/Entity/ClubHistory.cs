using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Collections.Generic;

namespace Arsenalcn.ClubSys.Entity
{
    public class ClubHistory : ClubSysObject
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

        private int _clubId;
        /// <summary>
        /// Club ID
        /// </summary>
        [ClubSysDbColumn("ClubID")]
        public int ClubID
        {
            get
            {
                return _clubId;
            }
            set
            {
                _clubId = value;
            }
        }

        private string _actionType;
        /// <summary>
        /// ActionType
        /// </summary>
        [ClubSysDbColumn("ActionType")]
        public string ActionType
        {
            get
            {
                return _actionType;
            }
            set
            {
                _actionType = value;
            }
        }

        private DateTime _actionyDate;
        /// <summary>
        /// ActionDate
        /// </summary>
        [ClubSysDbColumn("ActionDate")]
        public DateTime ActionDate
        {
            get
            {
                return _actionyDate;
            }
            set
            {
                _actionyDate = value;
            }
        }

        private string _actionUserName;
        /// <summary>
        /// ActionUserName
        /// </summary>
        [ClubSysDbColumn("ActionUserName")]
        public string ActionUserName
        {
            get
            {
                return _actionUserName;
            }
            set
            {
                _actionUserName = value;
            }
        }

        private string _operatorUserName;
        /// <summary>
        /// OperatorUserName
        /// </summary>
        [ClubSysDbColumn("OperatorUserName")]
        public string OperatorUserName
        {
            get
            {
                return _operatorUserName;
            }
            set
            {
                _operatorUserName = value;
            }
        }

        private string _actionDescription;
        /// <summary>
        /// Action Description
        /// </summary>
        [ClubSysDbColumn("ActionDescription")]
        public string ActionDescription
        {
            get
            {
                return _actionDescription;
            }
            set
            {
                _actionDescription = value;
            }
        }

        #endregion

        public ClubHistory()
        {

        }

        public ClubHistory(DataRow dr)
            : base(dr)
        {
        }
    }

    public enum ClubHistoryActionType
    {
        JoinClub,
        RejectJoinClub,
        LeaveClub,
        MandatoryLeaveClub,
        Nominated,
        Dismiss,
        LuckyPlayer,
        TransferExtcredit
    }

    public class ClubHistoryComparer : IComparer<ClubHistory>
    {

        #region IComparer<Club> Members

        public int Compare(ClubHistory x, ClubHistory y)
        {
            DateTime xDate = x.ActionDate;
            DateTime yDate = y.ActionDate;

            if ((yDate - xDate).TotalSeconds > 0)
                return 1;
            else if ((yDate - xDate).TotalSeconds < 0)
                return -1;
            else
                return 0;
        }

        #endregion
    }
}

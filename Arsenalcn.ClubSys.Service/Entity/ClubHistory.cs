using System;
using System.Collections.Generic;
using System.Data;

namespace Arsenalcn.ClubSys.Entity
{
    public class ClubHistory : ClubSysObject
    {
        public ClubHistory()
        {
        }

        public ClubHistory(DataRow dr)
            : base(dr)
        {
        }

        #region Members and Properties

        /// <summary>
        ///     ID
        /// </summary>
        [ClubSysDbColumn("ID")]
        public int? ID { get; set; } = null;

        /// <summary>
        ///     Club ID
        /// </summary>
        [ClubSysDbColumn("ClubID")]
        public int ClubID { get; set; }

        /// <summary>
        ///     ActionType
        /// </summary>
        [ClubSysDbColumn("ActionType")]
        public string ActionType { get; set; }

        /// <summary>
        ///     ActionDate
        /// </summary>
        [ClubSysDbColumn("ActionDate")]
        public DateTime ActionDate { get; set; }

        /// <summary>
        ///     ActionUserName
        /// </summary>
        [ClubSysDbColumn("ActionUserName")]
        public string ActionUserName { get; set; }

        /// <summary>
        ///     OperatorUserName
        /// </summary>
        [ClubSysDbColumn("OperatorUserName")]
        public string OperatorUserName { get; set; }

        /// <summary>
        ///     Action Description
        /// </summary>
        [ClubSysDbColumn("ActionDescription")]
        public string ActionDescription { get; set; }

        #endregion
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
            var xDate = x.ActionDate;
            var yDate = y.ActionDate;

            if ((yDate - xDate).TotalSeconds > 0)
                return 1;
            if ((yDate - xDate).TotalSeconds < 0)
                return -1;
            return 0;
        }

        #endregion
    }
}
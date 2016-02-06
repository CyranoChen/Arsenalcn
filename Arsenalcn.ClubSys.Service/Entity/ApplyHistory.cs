using System;
using System.Data;

namespace Arsenalcn.ClubSys.Entity
{
    public class ApplyHistory : ClubSysObject
    {
        public ApplyHistory()
        {
        }

        public ApplyHistory(DataRow dr)
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
        ///     User ID
        /// </summary>
        [ClubSysDbColumn("UserID")]
        public int Userid { get; set; }

        /// <summary>
        ///     User Name
        /// </summary>
        [ClubSysDbColumn("UserName")]
        public string UserName { get; set; }

        /// <summary>
        ///     Club ID
        /// </summary>
        [ClubSysDbColumn("ClubUid")]
        public int ClubUid { get; set; }

        /// <summary>
        ///     Apply Date
        /// </summary>
        [ClubSysDbColumn("ApplyDate")]
        public DateTime ApplyDate { get; set; }

        /// <summary>
        ///     Is Accepted
        /// </summary>
        [ClubSysDbColumn("IsAccepted")]
        public bool? IsAccepted { get; set; }

        #endregion
    }
}
using System;
using System.Data;

namespace Arsenalcn.ClubSys.Entity
{
    public class UserClub : ClubSysObject
    {
        public UserClub()
        {
        }

        public UserClub(DataRow dr)
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
        public int? Userid { get; set; }

        /// <summary>
        ///     User Name
        /// </summary>
        [ClubSysDbColumn("UserName")]
        public string UserName { get; set; }

        /// <summary>
        ///     Club ID
        /// </summary>
        [ClubSysDbColumn("ClubUid")]
        public int? ClubUid { get; set; }

        /// <summary>
        ///     Responsibility
        /// </summary>
        [ClubSysDbColumn("Responsibility")]
        public int? Responsibility { get; set; }

        /// <summary>
        ///     From Date
        /// </summary>
        [ClubSysDbColumn("FromDate")]
        public DateTime FromDate { get; set; }

        /// <summary>
        ///     To Date
        /// </summary>
        [ClubSysDbColumn("ToDate")]
        public DateTime? ToDate { get; set; } = null;

        /// <summary>
        ///     Join Club Date
        /// </summary>
        [ClubSysDbColumn("JoinClubDate")]
        public DateTime? JoinClubDate { get; set; } = null;

        /// <summary>
        ///     Active Flag
        /// </summary>
        [ClubSysDbColumn("IsActive")]
        public bool? IsActive { get; set; }

        #endregion
    }
}
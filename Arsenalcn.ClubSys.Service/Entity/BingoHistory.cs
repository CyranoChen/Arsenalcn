using System;
using System.Data;

namespace Arsenalcn.ClubSys.Entity
{
    public class BingoHistory : ClubSysObject
    {
        public BingoHistory()
        {
        }

        public BingoHistory(DataRow dr)
            : base(dr)
        {
        }

        #region Members and Properties

        /// <summary>
        ///     History ID
        /// </summary>
        [ClubSysDbColumn("ID")]
        public int ID { get; set; }

        /// <summary>
        ///     User ID
        /// </summary>
        [ClubSysDbColumn("UserID")]
        public int UserID { get; set; }

        /// <summary>
        ///     User Name
        /// </summary>
        [ClubSysDbColumn("UserName")]
        public string UserName { get; set; }

        /// <summary>
        ///     Club ID
        /// </summary>
        [ClubSysDbColumn("ClubID")]
        public int ClubID { get; set; }

        /// <summary>
        ///     Action Date
        /// </summary>
        [ClubSysDbColumn("ActionDate")]
        public DateTime ActionDate { get; set; }

        /// <summary>
        ///     Result
        /// </summary>
        [ClubSysDbColumn("Result")]
        public int? Result { get; set; }

        /// <summary>
        ///     Result Detail
        /// </summary>
        [ClubSysDbColumn("ResultDetail")]
        public string ResultDetail { get; set; }

        #endregion
    }
}
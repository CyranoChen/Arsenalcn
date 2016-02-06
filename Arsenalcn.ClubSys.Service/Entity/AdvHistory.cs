using System;
using System.Data;

namespace Arsenalcn.ClubSys.Entity
{
    public class AdvHistory : ClubSysObject
    {
        public AdvHistory()
        {
        }

        public AdvHistory(DataRow dr)
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
        ///     Type Code
        /// </summary>
        [ClubSysDbColumn("TypeCode")]
        public int TypeCode { get; set; }

        /// <summary>
        ///     Adv URL
        /// </summary>
        [ClubSysDbColumn("AdvURL")]
        public string AdvURL { get; set; }

        /// <summary>
        ///     Client IP
        /// </summary>
        [ClubSysDbColumn("ClientIP")]
        public string ClientIP { get; set; }

        /// <summary>
        ///     Action Date
        /// </summary>
        [ClubSysDbColumn("ActionDate")]
        public DateTime ActionDate { get; set; }

        #endregion
    }
}
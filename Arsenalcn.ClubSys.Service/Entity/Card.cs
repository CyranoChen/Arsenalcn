using System;
using System.Data;

namespace Arsenalcn.ClubSys.Entity
{
    public class Card : ClubSysObject
    {
        public Card()
        {
        }

        public Card(DataRow dr)
            : base(dr)
        {
        }

        #region Members and Properties

        /// <summary>
        ///     ID
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

        //private int _num;
        ///// <summary>
        ///// Number
        ///// </summary>
        //[ClubSysDbColumn("Number")]
        //public int Number
        //{
        //    get
        //    {
        //        return _num;
        //    }
        //    set
        //    {
        //        _num = value;
        //    }
        //}

        /// <summary>
        ///     card player guid
        /// </summary>
        [ClubSysDbColumn("ArsenalPlayerGuid")]
        public Guid? ArsenalPlayerGuid { get; set; }

        /// <summary>
        ///     Is in use
        /// </summary>
        [ClubSysDbColumn("IsInUse")]
        public bool IsInUse { get; set; }

        /// <summary>
        ///     IsActive
        /// </summary>
        [ClubSysDbColumn("IsActive")]
        public bool IsActive { get; set; }

        /// <summary>
        ///     Gain Date
        /// </summary>
        [ClubSysDbColumn("GainDate")]
        public DateTime GainDate { get; set; }

        /// <summary>
        ///     Active Date
        /// </summary>
        [ClubSysDbColumn("ActiveDate")]
        public DateTime? ActiveDate { get; set; } = null;

        #endregion
    }
}
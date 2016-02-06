using System;
using System.Data;

namespace Arsenalcn.ClubSys.Entity
{
    public class Gamer : ClubSysObject
    {
        public Gamer()
        {
        }

        public Gamer(DataRow dr)
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
        ///     Shirt Lv
        /// </summary>
        [ClubSysDbColumn("Shirt")]
        public int Shirt { get; set; }

        /// <summary>
        ///     Shorts Lv
        /// </summary>
        [ClubSysDbColumn("Shorts")]
        public int Shorts { get; set; }

        /// <summary>
        ///     Sock Lv
        /// </summary>
        [ClubSysDbColumn("Sock")]
        public int Sock { get; set; }

        /// <summary>
        ///     Current Player Guid
        /// </summary>
        [ClubSysDbColumn("CurrentGuid")]
        public Guid? CurrentGuid { get; set; }

        /// <summary>
        ///     IsActive
        /// </summary>
        [ClubSysDbColumn("IsActive")]
        public bool IsActive { get; set; }

        #endregion
    }
}
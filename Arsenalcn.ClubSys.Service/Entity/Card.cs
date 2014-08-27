using System;
using System.Collections.Generic;
using System.Text;

using System.Data;

namespace Arsenalcn.ClubSys.Entity
{
    public class Card : ClubSysObject
    {
        #region Members and Properties

        private int _id;
        /// <summary>
        /// ID
        /// </summary>
        [ClubSysDbColumn("ID")]
        public int ID
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
        public int UserID
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

        private string _userName;
        /// <summary>
        /// User Name
        /// </summary>
        [ClubSysDbColumn("UserName")]
        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
            }
        }

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

        private Guid? _guid;
        /// <summary>
        /// card player guid
        /// </summary>
        [ClubSysDbColumn("ArsenalPlayerGuid")]
        public Guid? ArsenalPlayerGuid
        {
            get
            {
                return _guid;
            }
            set
            {
                _guid = value;
            }
        }

        private bool _isInUse;
        /// <summary>
        /// Is in use
        /// </summary>
        [ClubSysDbColumn("IsInUse")]
        public bool IsInUse
        {
            get
            {
                return _isInUse;
            }
            set
            {
                _isInUse = value;
            }
        }

        private bool _isActive;
        /// <summary>
        /// IsActive
        /// </summary>
        [ClubSysDbColumn("IsActive")]
        public bool IsActive
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

        private DateTime _gainDate;
        /// <summary>
        /// Gain Date
        /// </summary>
        [ClubSysDbColumn("GainDate")]
        public DateTime GainDate
        {
            get
            {
                return _gainDate;
            }
            set
            {
                _gainDate = value;
            }
        }

        private DateTime? _activeDate = null;
        /// <summary>
        /// Active Date
        /// </summary>
        [ClubSysDbColumn("ActiveDate")]
        public DateTime? ActiveDate
        {
            get
            {
                return _activeDate;
            }
            set
            {
                _activeDate = value;
            }
        }

        #endregion

        public Card()
        {
        }

        public Card(DataRow dr)
            : base(dr)
        {
        }
    }
}

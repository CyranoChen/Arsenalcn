using System;
using System.Collections.Generic;
using System.Text;

using System.Data;

namespace Arsenalcn.ClubSys.Entity
{
    public class PlayerHistory : ClubSysObject
    {
        #region Members and Properties

        private int _id;
        /// <summary>
        /// History ID
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

        private int _typeCode;
        /// <summary>
        /// Type Code
        /// </summary>
        [ClubSysDbColumn("TypeCode")]
        public int TypeCode
        {
            get
            {
                return _typeCode;
            }
            set
            {
                _typeCode = value;
            }
        }

        private string _typeDesc;
        /// <summary>
        /// Type Desc
        /// </summary>
        [ClubSysDbColumn("TypeDesc")]
        public string TypeDesc
        {
            get
            {
                return _typeDesc;
            }
            set
            {
                _typeDesc = value;
            }
        }

        private DateTime _actionDate;
        /// <summary>
        /// Action Date
        /// </summary>
        [ClubSysDbColumn("ActionDate")]
        public DateTime ActionDate
        {
            get
            {
                return _actionDate;
            }
            set
            {
                _actionDate = value;
            }
        }

        #endregion

        public PlayerHistory()
        {
        }

        public PlayerHistory(DataRow dr)
            : base(dr)
        {
        }
    }
}

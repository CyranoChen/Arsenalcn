using System;
using System.Data;

namespace Arsenalcn.ClubSys.Entity
{
    public class AdvHistory : ClubSysObject
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

        private string _advURL;
        /// <summary>
        /// Adv URL
        /// </summary>
        [ClubSysDbColumn("AdvURL")]
        public string AdvURL
        {
            get
            {
                return _advURL;
            }
            set
            {
                _advURL = value;
            }
        }

        private string _clientIP;
        /// <summary>
        /// Client IP
        /// </summary>
        [ClubSysDbColumn("ClientIP")]
        public string ClientIP
        {
            get
            {
                return _clientIP;
            }
            set
            {
                _clientIP = value;
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

        public AdvHistory()
        {
        }

        public AdvHistory(DataRow dr)
            : base(dr)
        {
        }
    }
}

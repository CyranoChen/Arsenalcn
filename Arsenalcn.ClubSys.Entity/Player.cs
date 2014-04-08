using System;
using System.Collections.Generic;
using System.Text;

using System.Data;

namespace Arsenalcn.ClubSys.Entity
{
    public class Player : ClubSysObject
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

        private int _shirt;
        /// <summary>
        /// Shirt Lv
        /// </summary>
        [ClubSysDbColumn("Shirt")]
        public int Shirt
        {
            get
            {
                return _shirt;
            }
            set
            {
                _shirt = value;
            }
        }

        private int _shorts;
        /// <summary>
        /// Shorts Lv
        /// </summary>
        [ClubSysDbColumn("Shorts")]
        public int Shorts
        {
            get
            {
                return _shorts;
            }
            set
            {
                _shorts = value;
            }
        }
        
        private int _sock;
        /// <summary>
        /// Sock Lv
        /// </summary>
        [ClubSysDbColumn("Sock")]
        public int Sock
        {
            get
            {
                return _sock;
            }
            set
            {
                _sock = value;
            }
        }

        private Guid? _currentGuid;
        /// <summary>
        /// Current Player Guid
        /// </summary>
        [ClubSysDbColumn("CurrentGuid")]
        public Guid? CurrentGuid
        {
            get
            {
                return _currentGuid;
            }
            set
            {
                _currentGuid = value;
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

        #endregion

        public Player()
        {
        }

        public Player(DataRow dr)
            : base(dr)
        {
        }
    }
}

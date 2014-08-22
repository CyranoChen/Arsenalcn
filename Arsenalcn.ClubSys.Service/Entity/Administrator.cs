using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Arsenalcn.ClubSys.Entity
{
    public class Administrator : ClubSysObject
    {
        #region Members and Properties

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

        private string _userName = string.Empty;
        /// <summary>
        /// UserName
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

        #endregion

        public Administrator()
        {
        }

        public Administrator(DataRow dr)
            : base(dr)
        {
        }
    }
}

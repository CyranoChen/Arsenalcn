using System;
using System.Collections.Generic;
using System.Text;

using System.Data;

namespace Arsenalcn.ClubSys.Entity
{
    public class BingoHistory : ClubSysObject
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

        private int _clubID;
        /// <summary>
        /// Club ID
        /// </summary>
        [ClubSysDbColumn("ClubID")]
        public int ClubID
        {
            get
            {
                return _clubID;
            }
            set
            {
                _clubID = value;
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

        private int? _result;
        /// <summary>
        /// Result
        /// </summary>
        [ClubSysDbColumn("Result")]
        public int? Result
        {
            get
            {
                return _result;
            }
            set
            {
                _result = value;
            }
        }

        private string _resultDetail;
        /// <summary>
        /// Result Detail
        /// </summary>
        [ClubSysDbColumn("ResultDetail")]
        public string ResultDetail
        {
            get
            {
                return _resultDetail;
            }
            set
            {
                _resultDetail = value;
            }
        }

        #endregion

        public BingoHistory()
        {
        }

        public BingoHistory(DataRow dr)
            : base(dr)
        {
        }
    }
}

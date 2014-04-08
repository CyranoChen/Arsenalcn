using System;

namespace Arsenalcn.CasinoSys.Web
{
    public partial class DefaultMaster : System.Web.UI.MasterPage
    {
        private string _userName = string.Empty;
        /// <summary>
        /// Current User Name
        /// </summary>
        public string UserName
        {
            set
            {
                _userName = value;
            }
        }

        private int _userId = -1;
        /// <summary>
        /// Current User ID
        /// </summary>
        public int UserID
        {
            set
            {
                _userId = value;
            }
        }

        private string _userKey = string.Empty;
        /// <summary>
        /// Current User Key
        /// </summary>
        public string UserKey
        {
            set
            {
                _userKey = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlHeader.UserID = this._userId;
            ctrlHeader.UserName = this._userName;
            ctrlHeader.UserKey = this._userKey;
        }
    }
}

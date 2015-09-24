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
        public int UserId
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
            ctrlHeader.UserId = _userId;
            ctrlHeader.UserName = _userName;
            ctrlHeader.UserKey = _userKey;
        }
    }
}

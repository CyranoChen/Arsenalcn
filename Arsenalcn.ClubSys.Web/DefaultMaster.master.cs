using System;
using System.Web.UI;

namespace Arsenalcn.ClubSys.Web
{
    public partial class DefaultMaster : MasterPage
    {
        private int _userId = -1;

        private string _userKey = string.Empty;
        private string _userName = string.Empty;

        /// <summary>
        ///     Current User Name
        /// </summary>
        public string UserName
        {
            set { _userName = value; }
        }

        /// <summary>
        ///     Current User ID
        /// </summary>
        public int UserID
        {
            set { _userId = value; }
        }

        /// <summary>
        ///     Current User Key
        /// </summary>
        public string UserKey
        {
            set { _userKey = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlHeader.UserID = _userId;
            ctrlHeader.UserName = _userName;
            ctrlHeader.UserKey = _userKey;
        }
    }
}
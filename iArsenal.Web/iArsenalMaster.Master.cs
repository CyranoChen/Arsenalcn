using System;
using System.Web.UI;

namespace iArsenal.Web
{
    public partial class iArsenalMaster : MasterPage
    {
        private int _memberId = -1;

        private string _memberName = string.Empty;

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

        /// <summary>
        ///     Current Member Name
        /// </summary>
        public string MemberName
        {
            set { _memberName = value; }
        }

        /// <summary>
        ///     Current Member ID
        /// </summary>
        public int MemberID
        {
            set { _memberId = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlHeader.UserID = _userId;
            ctrlHeader.UserName = _userName;
            ctrlHeader.UserKey = _userKey;
            ctrlHeader.MemberID = _memberId;
            ctrlHeader.MemberName = _memberName;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!string.IsNullOrEmpty(masterHead.Title))
            {
                masterHead.Title = $"Arsenal China 阿森纳官方球迷会 服务中心 iArsenal.cn | {Page.Title}";
            }
        }
    }
}
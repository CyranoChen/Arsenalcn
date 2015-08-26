using System;
using iArsenal.Service;
using Arsenalcn.Core;

namespace iArsenal.Web.Control
{
    public partial class iArsenalHeader : System.Web.UI.UserControl
    {
        private readonly IRepository repo = new Repository();

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

        private string _memberName = string.Empty;
        /// <summary>
        /// Current Member Name
        /// </summary>
        public string MemberName
        {
            set
            {
                _memberName = value;
            }
        }

        private int _memberId = -1;
        /// <summary>
        /// Current Member ID
        /// </summary>
        public int MemberID
        {
            set
            {
                _memberId = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (_userId > 0)
            {
                pnlAnonymousUser.Visible = false;
                pnlLoginUser.Visible = true;

                var m = Member.Cache.LoadByAcnID(_userId);

                if (m != null && m.ID > 0)
                {
                    lblUserInfo.Text = string.Format("欢迎访问，<b>{0}</b> (<em>NO.{1}</em>)", m.Name, m.ID.ToString());
                }
                else
                {
                    lblUserInfo.Text = string.Format("欢迎访问，<b>{0}</b> (<em>ID.{1}</em>)", _userName, _userId.ToString());
                }

                if (ConfigGlobal.IsPluginAdmin(_userId))
                {
                    ltrlAdminConfig.Text = string.Format("<a href=\"AdminConfig.aspx\" target=\"_blank\">后台管理</a> - ");
                }
                else
                {
                    ltrlAdminConfig.Visible = false;
                }
            }
            else
            {
                pnlAnonymousUser.Visible = true;
                pnlLoginUser.Visible = false;

                hlLogin.NavigateUrl = string.Format("{0}?api_key={1}&next={2}", ConfigGlobal.APILoginURL, ConfigGlobal.APIAppKey, Request.Url.PathAndQuery);
            }
        }
    }
}
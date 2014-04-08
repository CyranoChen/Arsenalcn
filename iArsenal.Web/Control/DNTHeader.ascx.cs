using System;

using iArsenal.Entity;

namespace iArsenal.Web.Control
{
    public partial class DNTHeader : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (_userId > 0)
            {
                phAnonymous.Visible = false;
                phAthenticated.Visible = true;

                ltrlUserName.Text = this._userName;
                linkLogout.NavigateUrl = string.Format("{0}{1}", linkLogout.NavigateUrl, this._userKey);
            }
            else
            {
                phAnonymous.Visible = true;
                phAthenticated.Visible = false;
            }

            ltrlTitle.Text = string.Format("<a href=\"http://www.arsenalcn.com\">{0}</a> &raquo; <a href=\"default.aspx\">{1}</a> &raquo; <strong>{2}</strong>", "阿森纳中国官方球迷会", ConfigGlobal.PluginDisplayName, this.Page.Title);
        }

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
    }
}
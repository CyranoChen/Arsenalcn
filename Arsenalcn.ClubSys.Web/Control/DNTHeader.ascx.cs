using System;
using System.Web.UI;
using Arsenalcn.ClubSys.Entity;
using Arsenalcn.ClubSys.Service;

namespace Arsenalcn.ClubSys.Web.Control
{
    public partial class DNTHeader : UserControl
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
            if (_userId != -1)
            {
                phAnonymous.Visible = false;
                phAthenticated.Visible = true;

                ltrlUserName.Text = _userName;
                linkLogout.NavigateUrl = $"{linkLogout.NavigateUrl}{_userKey}";
            }
            else
            {
                phAnonymous.Visible = true;
                phAthenticated.Visible = false;
            }

            ltrlTitle.Text =
                $"<a href=\"/index.aspx\">{"阿森纳中国官方球迷会"}</a> &raquo; <a href=\"default.aspx\">{ConfigGlobal.PluginDisplayName}</a> &raquo; <strong>{Page.Title}</strong>";

            ltrlClubCount.Text = ClubLogic.GetActiveClubCount().ToString();
            ltrlUserCount.Text = ClubLogic.GetActiveUserCount().ToString();
            ltrlPlayerCount.Text = PlayerStrip.GetAllPlayerCount().ToString();
        }
    }
}
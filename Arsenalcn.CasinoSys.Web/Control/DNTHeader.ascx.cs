using System;

using Arsenalcn.CasinoSys.Entity;

namespace Arsenalcn.CasinoSys.Web.Control
{
    public partial class DNTHeader : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (_userId != -1)
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

            ltrlTitle.Text = string.Format("<a href=\"/index.aspx\">{0}</a> &raquo; <a href=\"default.aspx\">{1}</a> &raquo; <strong>{2}</strong>", "阿森纳中国官方球迷会", ConfigGlobal.PluginDisplayName, this.Page.Title);

            ltrlGamblerCount.Text = Gambler.Cache.GamblerList.Count.ToString();
            ltrlGameCount.Text = (CasinoItem.GetMatchCasinoItemCount() + CasinoItem.GetOtherCasinoItemCount()).ToString();

            Banker defaultBanker = new Banker(Banker.DefaultBankerID);
            ltrlDefaultBanker.Text = defaultBanker.Cash.ToString("N2");
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
using System;
using System.Data;
using System.Web.UI.WebControls;
using Arsenalcn.ClubSys.Service;

namespace Arsenalcn.ClubSys.Web
{
    public partial class ClubLuckyPlayerLog : Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region SetControlProperty
            ctrlLeftPanel.UserID = this.userid;
            ctrlLeftPanel.UserName = this.username;
            ctrlLeftPanel.UserKey = this.userkey;

            ctrlFieldToolBar.UserID = this.userid;
            ctrlFieldToolBar.UserName = this.username;
            #endregion

            BindLuckyPlayerLog();
        }

        private DataTable dt = null;
        private void BindLuckyPlayerLog()
        {
            if (dt == null)
            {
                dt = LuckyPlayer.GetLuckPlayerHistory();
            }

            gvLuckyPlayerLog.DataSource = dt;
            gvLuckyPlayerLog.DataBind();
        }

        protected void gvLuckyPlayerLog_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvLuckyPlayerLog.PageIndex = e.NewPageIndex;

            BindLuckyPlayerLog();
        }
    }
}

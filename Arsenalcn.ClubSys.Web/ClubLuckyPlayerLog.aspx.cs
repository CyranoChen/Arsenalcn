using System;
using System.Data;
using System.Web.UI.WebControls;
using Arsenalcn.ClubSys.Service;
using Arsenalcn.ClubSys.Web.Common;

namespace Arsenalcn.ClubSys.Web
{
    public partial class ClubLuckyPlayerLog : BasePage
    {
        private DataTable dt;

        protected void Page_Load(object sender, EventArgs e)
        {
            #region SetControlProperty

            ctrlLeftPanel.UserID = userid;
            ctrlLeftPanel.UserName = username;
            ctrlLeftPanel.UserKey = userkey;

            ctrlFieldToolBar.UserID = userid;
            ctrlFieldToolBar.UserName = username;

            #endregion

            BindLuckyPlayerLog();
        }

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
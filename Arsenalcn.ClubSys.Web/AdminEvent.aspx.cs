using System;
using System.Web.UI.WebControls;
using Arsenalcn.ClubSys.Web.Common;
using Arsenalcn.Common.Entity;

namespace Arsenalcn.ClubSys.Web
{
    public partial class AdminEvent : AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = username;
            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            var logList = LogEvent.GetLogEvents();

            gvEvent.DataSource = logList;
            gvEvent.DataBind();
        }

        protected void gvEvent_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvEvent.PageIndex = e.NewPageIndex;

            BindData();
        }
    }
}
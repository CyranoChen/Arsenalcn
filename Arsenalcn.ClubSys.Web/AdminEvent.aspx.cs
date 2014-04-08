using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Arsenalcn.Common.Entity;

namespace Arsenalcn.ClubSys.Web
{
    public partial class AdminEvent : Common.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = this.username;
            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            List<LogEvent> logList = LogEvent.GetLogEvents();

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
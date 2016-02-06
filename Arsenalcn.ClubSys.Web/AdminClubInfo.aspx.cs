using System;
using System.Web.UI.WebControls;
using Arsenalcn.ClubSys.Web.Common;
using Arsenalcn.Common;

namespace Arsenalcn.ClubSys.Web
{
    public partial class AdminClubInfo : AdminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            ClubInfoDataSouce.ConnectionString = SQLConn.GetConnection().ConnectionString;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = username;
        }

        protected void gvClubInfo_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvClubInfo.EditIndex = e.NewEditIndex;
        }

        protected void gvClubInfo_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvClubInfo.EditIndex = -1;
        }

        protected void gvClubInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvClubInfo.PageIndex = e.NewPageIndex;
        }
    }
}
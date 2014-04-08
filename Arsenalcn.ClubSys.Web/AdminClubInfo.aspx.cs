using System;
using System.Web.UI.WebControls;

namespace Arsenalcn.ClubSys.Web
{
    public partial class AdminClubInfo : Common.AdminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            ClubInfoDataSouce.ConnectionString = DataAccess.SQLConn.ConnectionString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = this.username;

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

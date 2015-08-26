using System;
using System.Web.UI.WebControls;
using Arsenalcn.Common;

namespace Arsenalcn.ClubSys.Web
{
    public partial class AdminRank : Common.AdminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            RanksConfigSqlDataSource.ConnectionString = SQLConn.GetConnection().ConnectionString;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = this.username;
        }

        protected void gvRanks_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvRanks.EditIndex = e.NewEditIndex;
        }

        protected void gvRanks_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvRanks.EditIndex = -1;
        }

    }
}

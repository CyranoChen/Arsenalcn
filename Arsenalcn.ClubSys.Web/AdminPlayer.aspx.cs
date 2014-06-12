using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Arsenalcn.ClubSys.DataAccess;
using Arsenalcn.ClubSys.Entity;
using Arsenalcn.Common;

namespace Arsenalcn.ClubSys.Web
{
    public partial class AdminPlayer : Common.AdminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            PlayerDataSouce.ConnectionString = SQLConn.GetConnection().ConnectionString;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = this.username;
        }

        protected void gvPlayer_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvPlayer.EditIndex = e.NewEditIndex;
        }

        protected void gvPlayer_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvPlayer.EditIndex = -1;
        }

        protected void gvPlayer_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPlayer.PageIndex = e.NewPageIndex;
        }
    }
}

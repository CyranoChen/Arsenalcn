using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Arsenalcn.Common;
using Arsenalcn.ClubSys.Entity;


namespace Arsenalcn.ClubSys.Web
{
    public partial class AdminPlayer : Common.AdminBasePage
    {
        //protected override void OnInit(EventArgs e)
        //{
        //    base.OnInit(e);

        //    PlayerDataSouce.ConnectionString = SQLConn.GetConnection().ConnectionString;
        //}

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
            List<Player> list = Service.PlayerStrip.GetPlayers(); ;

            gvPlayer.DataSource = list;
            gvPlayer.DataBind();
        }

        //protected void gvPlayer_RowEditing(object sender, GridViewEditEventArgs e)
        //{
        //    gvPlayer.EditIndex = e.NewEditIndex;
        //}

        //protected void gvPlayer_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        //{
        //    gvPlayer.EditIndex = -1;
        //}

        protected void gvPlayer_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPlayer.PageIndex = e.NewPageIndex;

            BindData();
        }
    }
}

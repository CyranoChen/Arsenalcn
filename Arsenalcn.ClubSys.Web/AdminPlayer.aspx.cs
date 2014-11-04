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

        protected void gvPlayer_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Player p = e.Row.DataItem as Player;

                TextBox tbShirt = e.Row.FindControl("tbShirt") as TextBox;
                TextBox tbShorts = e.Row.FindControl("tbShorts") as TextBox;
                TextBox tbSock = e.Row.FindControl("tbSock") as TextBox;
                DropDownList ddlIsActive = e.Row.FindControl("ddlIsActive") as DropDownList;

                if (tbShirt != null)
                { 
                    tbShirt.Text = p.Shirt.ToString(); 
                }

                if (tbShorts != null)
                {
                    tbShorts.Text = p.Shorts.ToString();
                }

                if (tbSock != null)
                {
                    tbSock.Text = p.Sock.ToString();
                }

                if (ddlIsActive != null)
                {
                    ddlIsActive.SelectedValue = p.IsActive.ToString().ToLower();
                }
            }
        }

        protected void gvPlayer_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            TextBox tbShirt = gvPlayer.Rows[gvPlayer.EditIndex].FindControl("tbShirt") as TextBox;
            TextBox tbShorts = gvPlayer.Rows[gvPlayer.EditIndex].FindControl("tbShorts") as TextBox;
            TextBox tbSock = gvPlayer.Rows[gvPlayer.EditIndex].FindControl("tbSock") as TextBox;
            DropDownList ddlIsActive = gvPlayer.Rows[gvPlayer.EditIndex].FindControl("ddlIsActive") as DropDownList;

            if (tbShirt != null && tbShorts != null && tbSock != null && ddlIsActive != null)
            {
                try
                {
                    int pid = (int)gvPlayer.DataKeys[gvPlayer.EditIndex].Value;

                    int _shirt;
                    int _shorts;
                    int _sock;

                    if (int.TryParse(tbShirt.Text.Trim(), out _shirt)
                        && int.TryParse(tbShorts.Text.Trim(), out _shorts)
                        && int.TryParse(tbSock.Text.Trim(), out _sock)
                        && !string.IsNullOrEmpty(ddlIsActive.SelectedValue))
                    {
                        Service.PlayerStrip.UpdatePlayerInfo(pid, _shirt, _shorts, _sock, Convert.ToBoolean(ddlIsActive.SelectedValue));
                    }
                    else
                    {
                        throw new Exception("请正确填写会员信息");
                    }
                }
                catch (Exception ex)
                {
                    this.ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}');", ex.Message.ToString()), true);
                }
            }

            gvPlayer.EditIndex = -1;

            BindData();
        }

        protected void gvPlayer_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvPlayer.EditIndex = e.NewEditIndex;

            BindData();
        }

        protected void gvPlayer_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvPlayer.EditIndex = -1;

            BindData();
        }

        protected void gvPlayer_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPlayer.PageIndex = e.NewPageIndex;

            BindData();
        }
    }
}

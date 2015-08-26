using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Arsenalcn.CasinoSys.Entity;

using Discuz.Forum;

namespace Arsenalcn.CasinoSys.Web
{
    public partial class AdminGambler : Common.AdminBasePage
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
            var queryUsername = string.Empty;
            if (ViewState["username"] != null)
                queryUsername = ViewState["username"].ToString();

            var list = Entity.Gambler.GetGamblers().FindAll(delegate(Gambler g)
            {
                var returnValue = true;
                var tmpString = string.Empty;

                if (ViewState["username"] != null)
                {
                    tmpString = ViewState["username"].ToString();
                    if (!string.IsNullOrEmpty(tmpString) && tmpString != "-请输入用户名-")
                        returnValue = returnValue && g.UserName.Contains(tmpString);
                }

                return returnValue;
            });

            gvGambler.DataSource = list;
            gvGambler.DataBind();
        }

        protected void gvGambler_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var g = e.Row.DataItem as Gambler;

                var lblQSB = e.Row.FindControl("lblQSB") as Label;
                var lblRP = e.Row.FindControl("lblRP") as Label;
                var lblCash = e.Row.FindControl("lblCash") as Label;
                var lblWin = e.Row.FindControl("lblWin") as Label;
                var lblLose = e.Row.FindControl("lblLose") as Label;

                var tbCash = e.Row.FindControl("tbCash") as TextBox;
                var tbWin = e.Row.FindControl("tbWin") as TextBox;
                var tbLose = e.Row.FindControl("tbLose") as TextBox;

                var btnResetGambler = e.Row.FindControl("btnResetGambler") as LinkButton;

                if (AdminUsers.GetUserInfo(g.UserID) != null && lblQSB != null && lblRP != null)
                {
                    lblQSB.Text = AdminUsers.GetUserExtCredits(g.UserID, 2).ToString("N2");
                    lblRP.Text = AdminUsers.GetUserExtCredits(g.UserID, 4).ToString();
                }

                if (lblCash != null)
                {
                    lblCash.Text = string.Format("<em>{0}<em>", g.Cash.ToString("N2"));
                }
                else if (tbCash != null)
                {
                    tbCash.Text = g.Cash.ToString("N2");
                }

                if (lblWin != null)
                {
                    lblWin.Text = string.Format("<em>{0}</em>", g.Win.ToString());
                }
                else if (tbWin != null)
                {
                    tbWin.Text = g.Win.ToString();
                }

                if (lblLose != null)
                {
                    lblLose.Text = string.Format("<em>{0}</em>", g.Lose.ToString());
                }
                else if (tbLose != null)
                {
                    tbLose.Text = g.Lose.ToString();
                }

                if (btnResetGambler != null)
                    btnResetGambler.CommandArgument = g.UserID.ToString();
            }
        }

        protected void gvGambler_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var tbCash = gvGambler.Rows[gvGambler.EditIndex].FindControl("tbCash") as TextBox;
            var tbWin = gvGambler.Rows[gvGambler.EditIndex].FindControl("tbWin") as TextBox;
            var tbLose = gvGambler.Rows[gvGambler.EditIndex].FindControl("tbLose") as TextBox;

            var gambler = new Entity.Gambler((int)gvGambler.DataKeys[gvGambler.EditIndex].Value);

            if (gambler != null && tbCash != null && tbWin != null && tbLose != null)
            {
                try
                {
                    gambler.Cash = Convert.ToSingle(tbCash.Text);
                    gambler.Win = Convert.ToInt16(tbWin.Text);
                    gambler.Lose = Convert.ToInt16(tbLose.Text);

                    gambler.Update(null);

                    this.ClientScript.RegisterClientScriptBlock(typeof(string), "success", "alert('修改玩家信息成功');", true);
                }
                catch
                {
                    this.ClientScript.RegisterClientScriptBlock(typeof(string), "failed", "alert('修改玩家信息失败');", true);
                }
            }

            gvGambler.EditIndex = -1;

            BindData();
        }

        protected void gvGambler_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvGambler.EditIndex = e.NewEditIndex;

            BindData();
        }

        protected void gvGambler_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvGambler.EditIndex = -1;

            BindData();
        }

        protected void gvGambler_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ResetGambler")
            {
                try
                {
                    Entity.Gambler.GamblerStatistics(Convert.ToInt32(e.CommandArgument));

                    this.ClientScript.RegisterClientScriptBlock(typeof(string), "success", "alert('统计玩家信息成功');", true);
                }
                catch (Exception ex)
                {
                    this.ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}');", ex.Message), true);
                }
            }
        }

        protected void gvGambler_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvGambler.PageIndex = e.NewPageIndex;

            BindData();
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbUserName.Text.Trim()))
                ViewState["username"] = tbUserName.Text.Trim();
            else
                ViewState["username"] = string.Empty;

            BindData();
        }
    }
}

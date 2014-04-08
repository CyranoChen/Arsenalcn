using System;
using System.Data;
using System.Web.UI.WebControls;

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
            string queryUsername = string.Empty;
            if (ViewState["username"] != null)
                queryUsername = ViewState["username"].ToString();

            DataTable dt = Entity.Gambler.GetGambler(queryUsername);

            if (dt != null)
            {
                int uid = int.MinValue;
                dt.Columns.Add("QSB", typeof(string));
                dt.Columns.Add("RP", typeof(string));

                foreach (DataRow dr in dt.Rows)
                {
                    uid = Convert.ToInt32(dr["UserID"]);

                    if (AdminUsers.GetUserInfo(uid) != null)
                    {
                        dr["QSB"] = AdminUsers.GetUserExtCredits(uid, 2).ToString("N2");
                        dr["RP"] = AdminUsers.GetUserExtCredits(uid, 4).ToString();
                    }
                }
            }

            gvGambler.DataSource = dt;
            gvGambler.DataBind();
        }

        protected void gvGambler_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = e.Row.DataItem as DataRowView;

                TextBox tbCash = e.Row.FindControl("tbCash") as TextBox;
                TextBox tbWin = e.Row.FindControl("tbWin") as TextBox;
                TextBox tbLose = e.Row.FindControl("tbLose") as TextBox;
                LinkButton btnResetGambler = e.Row.FindControl("btnResetGambler") as LinkButton;

                if (tbCash != null)
                    tbCash.Text = ((double)drv["Cash"]).ToString("N2");

                if (tbWin != null)
                    tbWin.Text = drv["Win"].ToString();

                if (tbLose != null)
                    tbLose.Text = drv["Lose"].ToString();

                if (btnResetGambler != null)
                    btnResetGambler.CommandArgument = drv["UserID"].ToString();
            }
        }

        protected void gvGambler_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            TextBox tbCash = gvGambler.Rows[gvGambler.EditIndex].FindControl("tbCash") as TextBox;
            TextBox tbWin = gvGambler.Rows[gvGambler.EditIndex].FindControl("tbWin") as TextBox;
            TextBox tbLose = gvGambler.Rows[gvGambler.EditIndex].FindControl("tbLose") as TextBox;

            Entity.Gambler gambler = new Entity.Gambler((int)gvGambler.DataKeys[gvGambler.EditIndex].Value, null);

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
                catch
                {
                    this.ClientScript.RegisterClientScriptBlock(typeof(string), "failed", "alert('统计玩家信息失败');", true);
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

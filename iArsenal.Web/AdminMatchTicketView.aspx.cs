using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Linq;

using iArsenal.Entity;

namespace iArsenal.Web
{
    public partial class AdminMatchTicketView : PageBase.AdminPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = this.Username;

            if (!IsPostBack)
            {
                #region Bind ddlProductCode

                List<Product> pList = Product.Cache.Load(ProductType.MatchTicket);

                ddlProductCode.DataSource = pList;
                ddlProductCode.DataTextField = "DisplayName";
                ddlProductCode.DataValueField = "Code";
                ddlProductCode.DataBind();

                ddlProductCode.Items.Insert(0, new ListItem("--请选择比赛等级--", string.Empty));

                #endregion

                InitForm();
            }
        }

        private Guid MatchGuid
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["MatchGuid"]))
                {
                    try { return new Guid(Request.QueryString["MatchGuid"]); }
                    catch { return Guid.Empty; }
                }
                else
                    return Guid.Empty;
            }
        }

        private void InitForm()
        {
            if (MatchGuid != Guid.Empty)
            {
                MatchTicket mt = new MatchTicket();
                mt.MatchGuid = MatchGuid;
                mt.Select();

                // Get Match Info

                lblMatchGuid.Text = mt.MatchGuid.ToString();

                if (mt.LeagueGuid.HasValue && !string.IsNullOrEmpty(mt.LeagueName))
                    lblLeagueName.Text = string.Format("<em>{0}</em>", mt.LeagueName);
                else
                    lblLeagueName.Text = "无";

                if (mt.TeamGuid != null && !string.IsNullOrEmpty(mt.TeamName))
                    lblTeamName.Text = string.Format("<em>{0}</em>", mt.TeamName);
                else
                    lblTeamName.Text = "无";

                lblIsHome.Text = mt.IsHome ? "主场" : "客场";

                if (mt.Round.HasValue)
                    lblRound.Text = mt.Round.Value.ToString();
                else
                    lblRound.Text = "/";

                lblPlayTime.Text = string.Format("<em>{0}</em>", mt.PlayTimeLocal.ToString("yyyy-MM-dd HH:mm"));
                lblResultInfo.Text = string.Format("<em>{0}</em>", mt.ResultInfo);

                // Get Ticket Info

                ddlProductCode.SelectedValue = mt.ProductCode;
                tbDeadline.Text = mt.Deadline.ToString("yyyy-MM-dd");
                cbIsActive.Checked = mt.IsActive;
                tbRemark.Text = mt.Remark;

                // Bind MatchOrder data of this MatchTicket
                BindItemData();
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('选定的比赛不存在');window.location.href = 'AdminMatchTicket.aspx'", true);
            }
        }

        private void BindItemData()
        {
            List<OrderItemBase> list = OrderItemBase.GetOrderItems().FindAll(oi => oi.Remark.Equals(MatchGuid.ToString()));
            List<OrderBase> oList = OrderBase.GetOrders().FindAll(o => list.Any(oi => oi.OrderID.Equals(o.OrderID)));

            gvMatchOrder.DataSource = oList;
            gvMatchOrder.DataBind();
        }

        protected void gvMatchOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvMatchOrder.SelectedIndex != -1)
            {
                Response.Redirect(string.Format("AdminOrderView.aspx?OrderID={0}", gvMatchOrder.DataKeys[gvMatchOrder.SelectedIndex].Value.ToString()));
            }
        }

        protected void gvMatchOrder_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string _strStatus = string.Empty;
                OrderBase o = e.Row.DataItem as OrderBase;

                Label lblOrderStatus = e.Row.FindControl("lblOrderStatus") as Label;

                if (lblOrderStatus != null)
                {
                    if (o.Status.Equals(OrderStatusType.Confirmed))
                        _strStatus = string.Format("<em>{0}</em>", o.StatusInfo);
                    else
                        _strStatus = o.StatusInfo;

                    lblOrderStatus.Text = _strStatus;
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                MatchTicket mt = new MatchTicket();
                mt.MatchGuid = MatchGuid;
                mt.Select();

                DateTime _deadline;
                if (!string.IsNullOrEmpty(tbDeadline.Text.Trim()) && DateTime.TryParse(tbDeadline.Text.Trim(), out _deadline))
                    mt.Deadline = _deadline;
                else
                    mt.Deadline = mt.PlayTime.AddMonths(-2);

                mt.IsActive = cbIsActive.Checked;
                mt.Remark = tbRemark.Text.Trim();

                if (!string.IsNullOrEmpty(mt.ProductCode) && !string.IsNullOrEmpty(mt.ProductInfo))
                {
                    mt.ProductCode = ddlProductCode.SelectedValue;

                    mt.Update();

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('更新成功');window.location.href=window.location.href", true);
                }
                else
                {
                    mt.ProductCode = ddlProductCode.SelectedValue;

                    mt.Insert();

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('添加成功');window.location.href=window.location.href", true);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}')", ex.Message.ToString()), true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (MatchGuid != Guid.Empty)
            {
                Response.Redirect("AdminMatchTicket.aspx?MatchGuid=" + MatchGuid.ToString());
            }
            else
            {
                Response.Redirect("AdminMatchTicket.aspx");
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (MatchGuid != Guid.Empty)
                {
                    MatchTicket mt = new MatchTicket();
                    mt.MatchGuid = MatchGuid;
                    mt.Delete();

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('删除成功');window.location.href='AdminMatchTicket.aspx'", true);
                }
                else
                {
                    throw new Exception("选定的比赛不存在");
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}')", ex.Message.ToString()), true);
            }
        }
    }
}
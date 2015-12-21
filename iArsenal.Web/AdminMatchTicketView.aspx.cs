using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Linq;

using iArsenal.Service;
using Arsenalcn.Core;

namespace iArsenal.Web
{
    public partial class AdminMatchTicketView : AdminPageBase
    {
        private readonly IRepository repo = new Repository();
        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = this.Username;

            if (!IsPostBack)
            {
                #region Bind ddlProductCode

                var pList = Product.Cache.Load(ProductType.MatchTicket);

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
                var mt = new MatchTicket();
                mt.ID = MatchGuid;
                mt.Single();

                // Get Match Info

                lblMatchGuid.Text = mt.ID.ToString();

                if (mt.LeagueGuid.HasValue && !string.IsNullOrEmpty(mt.LeagueName))
                    lblLeagueName.Text = $"<em>{mt.LeagueName}</em>";
                else
                    lblLeagueName.Text = "无";

                if (mt.TeamGuid != null && !string.IsNullOrEmpty(mt.TeamName))
                    lblTeamName.Text = $"<em>{mt.TeamName}</em>";
                else
                    lblTeamName.Text = "无";

                lblIsHome.Text = mt.IsHome ? "主场" : "客场";

                if (mt.Round.HasValue)
                    lblRound.Text = mt.Round.Value.ToString();
                else
                    lblRound.Text = "/";

                lblPlayTime.Text = $"<em>{mt.PlayTimeLocal.ToString("yyyy-MM-dd HH:mm")}</em>";
                lblResultInfo.Text = $"<em>{mt.ResultInfo}</em>";

                // Get Ticket Info

                ddlProductCode.SelectedValue = mt.ProductCode;
                tbDeadline.Text = mt.Deadline.ToString("yyyy-MM-dd");

                if (mt.AllowMemberClass.HasValue)
                {
                    ddlAllowMemberClass.SelectedValue = mt.AllowMemberClass.Value.ToString();
                }
                else
                {
                    ddlAllowMemberClass.SelectedValue = string.Empty;
                }

                cbIsActive.Checked = mt.IsActive;
                tbRemark.Text = mt.Remark;

                //Bind MatchOrder data of this MatchTicket
                BindItemData();

                //gvMatchOrder.DataSource = mt.OrderTicketList;
                //gvMatchOrder.DataBind();
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('选定的比赛不存在');window.location.href = 'AdminMatchTicket.aspx'", true);
            }
        }

        private void BindItemData()
        {
            var list = repo.Query<OrderItem>(x => x.Remark == MatchGuid.ToString());
            var query = repo.All<Order>().FindAll(o => list.Exists(x => x.OrderID.Equals(o.ID)));

            gvMatchOrder.DataSource = query.ToList();
            gvMatchOrder.DataBind();
        }

        protected void gvMatchOrder_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMatchOrder.PageIndex = e.NewPageIndex;
            //MatchGuid = Guid.Empty;

            BindItemData();
        }

        protected void gvMatchOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvMatchOrder.SelectedIndex != -1)
            {
                Response.Redirect(
                    $"AdminOrderView.aspx?OrderID={gvMatchOrder.DataKeys[gvMatchOrder.SelectedIndex].Value.ToString()}");
            }
        }

        protected void gvMatchOrder_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var o = e.Row.DataItem as Order;

                var hlName = e.Row.FindControl("hlName") as HyperLink;
                var lblOrderStatus = e.Row.FindControl("lblOrderStatus") as Label;

                if (hlName != null)
                {
                    var m = Member.Cache.Load(o.MemberID);

                    switch (m.Evalution)
                    {
                        case MemberEvalution.BlackList:
                            hlName.Text = string.Format("<em class=\"{1}\">{0}</em>",
                                o.MemberName, "asc_memberName_blackList");
                            break;
                        case MemberEvalution.WhiteList:
                            hlName.Text = string.Format("<em class=\"{1}\">{0}</em>",
                                o.MemberName, "asc_memberName_whiteList");
                            break;
                        default:
                            hlName.Text = $"<em>{o.MemberName}</em>";
                            break;
                    }

                    hlName.NavigateUrl = $"AdminOrder.aspx?MemberID={o.MemberID}";
                }

                if (lblOrderStatus != null)
                {
                    var _strStatus = string.Empty;

                    if (o.Status.Equals(OrderStatusType.Confirmed))
                        _strStatus = $"<em>{o.StatusInfo}</em>";
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
                var mt = new MatchTicket();
                mt.ID = MatchGuid;
                mt.Single();

                DateTime _deadline;
                if (!string.IsNullOrEmpty(tbDeadline.Text.Trim()) && DateTime.TryParse(tbDeadline.Text.Trim(), out _deadline))
                    mt.Deadline = _deadline;
                else
                    mt.Deadline = mt.PlayTime.AddMonths(-2).AddDays(-7);

                if (!string.IsNullOrEmpty(ddlAllowMemberClass.SelectedValue))
                {
                    mt.AllowMemberClass = Convert.ToInt16(ddlAllowMemberClass.SelectedValue);
                }
                else
                {
                    mt.AllowMemberClass = null;
                }

                mt.IsActive = cbIsActive.Checked;
                mt.Remark = tbRemark.Text.Trim();

                mt.ProductCode = ddlProductCode.SelectedValue;

                // Check whether MatchTicket Instance in DB
                if (mt.Any())
                {
                    mt.Update();

                    MatchTicket.Cache.RefreshCache();

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('更新成功');window.location.href=window.location.href", true);
                }
                else
                {
                    mt.Create();

                    MatchTicket.Cache.RefreshCache();

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('添加成功');window.location.href = 'AdminMatchTicket.aspx'", true);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", $"alert('{ex.Message.ToString()}')", true);
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
                    var mt = new MatchTicket();
                    mt.ID = MatchGuid;
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
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", $"alert('{ex.Message.ToString()}')", true);
            }
        }
    }
}
using System;
using System.Linq;
using System.Web.UI.WebControls;
using Arsenalcn.Core;
using iArsenal.Service;

namespace iArsenal.Web
{
    public partial class AdminMatchTicketView : AdminPageBase
    {
        private readonly IRepository _repo = new Repository();

        private Guid MatchGuid
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["MatchGuid"]))
                {
                    try
                    {
                        return new Guid(Request.QueryString["MatchGuid"]);
                    }
                    catch
                    {
                        return Guid.Empty;
                    }
                }
                return Guid.Empty;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = Username;

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

        private void InitForm()
        {
            if (MatchGuid != Guid.Empty)
            {
                var mt = new MatchTicket { ID = MatchGuid };
                mt.Single();

                // Get Match Info

                lblMatchGuid.Text = mt.ID.ToString();

                if (mt.LeagueGuid.HasValue && !string.IsNullOrEmpty(mt.LeagueName))
                    lblLeagueName.Text = $"<em>{mt.LeagueName}</em>";
                else
                    lblLeagueName.Text = "无";

                lblTeamName.Text = !string.IsNullOrEmpty(mt.TeamName) ? $"<em>{mt.TeamName}</em>" : "无";

                lblIsHome.Text = mt.IsHome ? "主场" : "客场";

                lblRound.Text = mt.Round?.ToString() ?? "/";

                lblPlayTime.Text = $"<em>{mt.PlayTimeLocal.ToString("yyyy-MM-dd HH:mm")}</em>";
                lblResultInfo.Text = $"<em>{mt.ResultInfo}</em>";

                // Get Ticket Info

                ddlProductCode.SelectedValue = mt.ProductCode;
                tbDeadline.Text = mt.Deadline.ToString("yyyy-MM-dd");
                tbWaitingDeadline.Text = mt.WaitingDeadline.ToString("yyyy-MM-dd");

                ddlAllowMemberClass.SelectedValue = mt.AllowMemberClass?.ToString() ?? string.Empty;

                cbIsActive.Checked = mt.IsActive;
                tbRemark.Text = mt.Remark;

                //Bind MatchOrder data of this MatchTicket
                BindItemData();

                //gvMatchOrder.DataSource = mt.OrderTicketList;
                //gvMatchOrder.DataBind();
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "succeed",
                    "alert('选定的比赛不存在');window.location.href = 'AdminMatchTicket.aspx'", true);
            }
        }

        private void BindItemData()
        {
            var list = _repo.Query<OrderItem>(x => x.Remark == MatchGuid.ToString());
            var query = _repo.All<Order>().FindAll(o => list.Exists(x => x.OrderID.Equals(o.ID)));

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
                var key = gvMatchOrder.DataKeys[gvMatchOrder.SelectedIndex];
                if (key != null)
                {
                    Response.Redirect($"AdminOrderView.aspx?OrderID={key.Value}");
                }
            }
        }

        protected void gvMatchOrder_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var o = e.Row.DataItem as Order;

                var hlName = e.Row.FindControl("hlName") as HyperLink;
                var lblOrderStatus = e.Row.FindControl("lblOrderStatus") as Label;

                if (o != null && hlName != null)
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

                if (o != null && lblOrderStatus != null)
                {
                    lblOrderStatus.Text = o.Status.Equals(OrderStatusType.Confirmed) ? $"<em>{o.StatusInfo}</em>" : o.StatusInfo;
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                var mt = new MatchTicket { ID = MatchGuid };
                mt.Single();

                DateTime deadline;
                if (!string.IsNullOrEmpty(tbDeadline.Text.Trim()) && DateTime.TryParse(tbDeadline.Text.Trim(), out deadline))
                {
                    mt.Deadline = deadline;
                }
                else
                {
                    mt.Deadline = mt.PlayTime.AddMonths(-2).AddDays(-7);
                }

                if (!string.IsNullOrEmpty(tbWaitingDeadline.Text.Trim()) &&
                    DateTime.TryParse(tbWaitingDeadline.Text.Trim(), out deadline))
                {
                    mt.WaitingDeadline = deadline;
                }
                else
                {
                    mt.WaitingDeadline = mt.PlayTime.AddMonths(-1).AddDays(+7);
                }

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

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed",
                        "alert('更新成功');window.location.href=window.location.href", true);
                }
                else
                {
                    mt.Create();

                    MatchTicket.Cache.RefreshCache();

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed",
                        "alert('添加成功');window.location.href = 'AdminMatchTicket.aspx'", true);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", $"alert('{ex.Message}')", true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (MatchGuid != Guid.Empty)
            {
                Response.Redirect("AdminMatchTicket.aspx?MatchGuid=" + MatchGuid);
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
                    var mt = new MatchTicket { ID = MatchGuid };
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
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", $"alert('{ex.Message}')", true);
            }
        }
    }
}
using System;

using iArsenal.Service;
using Arsenalcn.Core;

namespace iArsenal.Web
{
    public partial class iArsenalOrderView_TicketBeijing : MemberPageBase
    {
        private readonly IRepository repo = new Repository();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitForm();
            }
        }

        private int OrderID
        {
            get
            {
                int _orderID;
                if (!string.IsNullOrEmpty(Request.QueryString["OrderID"]) && int.TryParse(Request.QueryString["OrderID"], out _orderID))
                {
                    return _orderID;
                }
                else
                    return int.MinValue;
            }
        }

        private void InitForm()
        {
            try
            {
                lblMemberName.Text = string.Format("<b>{0}</b> (<em>NO.{1}</em>)", this.MemberName, this.MID.ToString());

                if (OrderID > 0)
                {
                    OrdrTicket o = repo.Single<OrdrTicket>(OrderID);

                    if (ConfigGlobal.IsPluginAdmin(UID) && o != null)
                    {
                        lblMemberName.Text = string.Format("<b>{0}</b> (<em>NO.{1}</em>)", o.MemberName, o.MemberID.ToString());
                    }
                    else
                    {
                        if (o == null || !o.MemberID.Equals(MID) || !o.IsActive)
                            throw new Exception("此订单无效或非当前用户订单");
                    }

                    #region Bind OrderView Status Workflow

                    if (ucPortalWorkflowInfo != null)
                    {
                        ucPortalWorkflowInfo.JSONOrderStatusList = string.Format("[ {0} ]", string.Join(",", o.StatusWorkflowInfo));
                        ucPortalWorkflowInfo.CurrOrderStatus = o.Status;
                    }

                    #endregion

                    Member m = repo.Single<Member>(o.MemberID);

                    if (m == null || !m.IsActive)
                        throw new Exception("无此会员信息");

                    lblMemberIDCardNo.Text = string.Format("<em>{0}</em>", m.IDCardNo);
                    lblMemberEmail.Text = string.Format("<em>{0}</em>", m.Email);
                    lblMemberRegion.Text = m.RegionInfo;
                    lblOrderMobile.Text = string.Format("<em>{0}</em>", o.Mobile);
                    lblOrderPayment.Text = o.PaymentInfo;
                    lblOrderDescription.Text = o.Description;
                    lblOrderID.Text = string.Format("<em>{0}</em>", o.ID.ToString());
                    lblOrderCreateTime.Text = o.CreateTime.ToString("yyyy-MM-dd HH:mm");

                    if (!string.IsNullOrEmpty(o.Remark))
                    {
                        lblOrderRemark.Text = o.Remark.Replace("\r\n", "<br />");
                        phOrderRemark.Visible = true;
                    }
                    else
                    {
                        phOrderRemark.Visible = false;
                    }

                    // Should be Calculator in this Page
                    float price = 0f;
                    string priceInfo = string.Empty;

                    OrdrItm2012TicketBeijing oiTicket = o.OITicketBeijing;
                    if (oiTicket != null && oiTicket.IsActive)
                    {
                        lblOrderItem_TicketBeijing.Text = string.Format("<em>{0}</em>", oiTicket.ProductName);
                        tbOrderItem_TicketBeijing.Text = oiTicket.ProductGuid.ToString();
                        lblOrderItemQuantity.Text = oiTicket.Quantity.ToString();

                        if (oiTicket.Size.Equals("1"))
                            lblOrderItemSize.Text = "一层看台";
                        else if (oiTicket.Size.Equals("2"))
                            lblOrderItemSize.Text = "二层看台";
                        else
                            lblOrderItemSize.Text = "不介意";

                        lblOrderItemRemak.Text = oiTicket.SeatLevel;
                    }
                    else
                    {
                        throw new Exception("此订单未购买球票商品");
                    }

                    // Set Order Price

                    price = oiTicket.TotalPrice;
                    priceInfo = string.Format("<合计> {0} × {1}", oiTicket.UnitPrice.ToString("f2"), oiTicket.Quantity.ToString());

                    if (!o.Sale.HasValue)
                        lblOrderPrice.Text = string.Format("{0} = <em>{1}</em>元 (CNY)", priceInfo, price.ToString("f2"));
                    else
                        lblOrderPrice.Text = string.Format("{0} = <em>{1}</em>元<br /><结算价>：<em>{2}</em>元 (CNY)", priceInfo, price.ToString("f2"), o.Sale.Value.ToString("f2"));

                    tbOrderPrice.Text = price.ToString();

                    if (o.Status.Equals(OrderStatusType.Draft))
                    {
                        btnSubmit.Visible = true;
                        btnModify.Visible = true;
                        btnCancel.Visible = true;
                    }
                    else if (o.Status.Equals(OrderStatusType.Submitted))
                    {
                        btnSubmit.Visible = false;
                        btnModify.Visible = false;
                        btnCancel.Visible = true;
                    }
                    else
                    {
                        btnSubmit.Visible = false;
                        btnModify.Visible = false;
                        btnCancel.Visible = false;
                    }
                }
                else
                {
                    throw new Exception("此订单不存在");
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}');window.location.href = 'iArsenalOrder.aspx'", ex.Message.ToString()), true);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (OrderID > 0)
                {
                    Order o = repo.Single<Order>(OrderID);

                    if (o == null || !o.MemberID.Equals(MID) || !o.IsActive)
                        throw new Exception("此订单无效或非当前用户订单");

                    o.Status = OrderStatusType.Submitted;
                    o.UpdateTime = DateTime.Now;
                    o.Price = Convert.ToSingle(tbOrderPrice.Text.Trim());

                    repo.Update(o);

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", string.Format("alert('谢谢您的订购，您的订单已经提交成功。\\r\\n请尽快付款以完成订单确认，订单号为：{0}'); window.location.href = window.location.href", o.ID.ToString()), true);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}');", ex.Message.ToString()), true);
            }
        }

        protected void btnModify_Click(object sender, EventArgs e)
        {
            try
            {
                if (OrderID > 0)
                {
                    Order o = repo.Single<Order>(OrderID);

                    if (o == null || !o.MemberID.Equals(MID) || !o.IsActive)
                        throw new Exception("此订单无效或非当前用户订单");

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", string.Format("window.location.href = 'iArsenalOrder_TicketBeijing.aspx?OrderID={0}'", o.ID.ToString()), true);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}');", ex.Message.ToString()), true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (OrderID > 0)
                {
                    Order o = repo.Single<Order>(OrderID);

                    if (o == null || !o.MemberID.Equals(MID) || !o.IsActive)
                        throw new Exception("此订单无效或非当前用户订单");

                    o.IsActive = false;
                    o.UpdateTime = DateTime.Now;
                    o.Price = Convert.ToSingle(tbOrderPrice.Text.Trim());

                    repo.Update(o);

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", string.Format("alert('此订单({0})已经取消');window.location.href = 'iArsenalOrder.aspx'", o.ID.ToString()), true);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}');", ex.Message.ToString()), true);
            }
        }
    }
}
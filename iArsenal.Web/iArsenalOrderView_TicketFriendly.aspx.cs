using System;
using System.Globalization;
using Arsenalcn.Core;
using iArsenal.Service;

namespace iArsenal.Web
{
    public partial class iArsenalOrderView_TicketFriendly : MemberPageBase
    {
        private readonly IRepository _repo = new Repository();

        private int OrderID
        {
            get
            {
                int orderID;
                if (!string.IsNullOrEmpty(Request.QueryString["OrderID"]) &&
                    int.TryParse(Request.QueryString["OrderID"], out orderID))
                {
                    return orderID;
                }
                return int.MinValue;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitForm();
            }
        }

        private void InitForm()
        {
            try
            {
                lblMemberName.Text = $"<b>{MemberName}</b> (<em>NO.{Mid}</em>)";

                if (OrderID > 0)
                {
                    var o = (OrdrTicket)Order.Select(OrderID);

                    if (ConfigGlobal.IsPluginAdmin(Uid) && o != null)
                    {
                        lblMemberName.Text = $"<b>{o.MemberName}</b> (<em>NO.{o.MemberID}</em>)";
                    }
                    else
                    {
                        if (o == null || !o.MemberID.Equals(Mid) || !o.IsActive)
                            throw new Exception("此订单无效或非当前用户订单");
                    }

                    #region Bind OrderView Status Workflow

                    if (ucPortalWorkflowInfo != null)
                    {
                        ucPortalWorkflowInfo.JSONOrderStatusList = $"[ {string.Join(",", o.StatusWorkflowInfo)} ]";
                        ucPortalWorkflowInfo.CurrOrderStatus = o.Status;
                    }

                    #endregion

                    var m = _repo.Single<Member>(o.MemberID);

                    if (m == null || !m.IsActive)
                        throw new Exception("无此会员信息");

                    lblOrderMobile.Text = $"<em>{o.Mobile}</em>";
                    lblMemberIDCardNo.Text = m.IDCardNo;
                    lblMemberRegion.Text = m.RegionInfo;
                    lblMemberEmail.Text = m.Email;
                    lblMemberWeChat.Text = m.WeChat;
                    lblOrderDescription.Text = o.Description;
                    lblOrderID.Text = $"<em>{o.ID}</em>";
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
                    double price;
                    string priceInfo;

                    OrderItem oiTicket;

                    if (o.OI2012TicketBeijing != null && o.OI2012TicketBeijing.IsActive)
                    {
                        var oi = o.OI2012TicketBeijing;
                        oiTicket = o.OI2012TicketBeijing;

                        lblOrderItem_TicketBeijing.Text = $"<em>{oi.ProductName}</em>";
                        tbOrderItem_TicketBeijing.Text = oi.ProductGuid.ToString();
                        lblOrderItemQuantity.Text = oi.Quantity.ToString();

                        lblOrderItemRemark.Text = oi.SeatLevel;
                        phOrderItemRemark.Visible = true;
                    }
                    else if (o.OI2017TicketBeijing != null && o.OI2017TicketBeijing.IsActive)
                    {
                        var oi = o.OI2017TicketBeijing;
                        oiTicket = o.OI2017TicketBeijing;

                        lblOrderItem_TicketBeijing.Text = $"<em>{oi.ProductName}</em>";
                        tbOrderItem_TicketBeijing.Text = oi.ProductGuid.ToString();
                        lblOrderItemQuantity.Text = oi.Quantity.ToString();
                    }
                    else
                    {
                        throw new Exception("此订单未购买球票商品");
                    }

                    // Set Order Price
                    price = oiTicket.TotalPrice;
                    priceInfo = $"<合计> {oiTicket.UnitPrice.ToString("f2")} × {oiTicket.Quantity}";

                    if (!o.Sale.HasValue)
                        lblOrderPrice.Text = $"{priceInfo} = <em>{price.ToString("f2")}</em>元 (CNY)";
                    else
                        lblOrderPrice.Text =
                            $"{priceInfo} = <em>{price.ToString("f2")}</em>元<br /><结算价>：<em>{o.Sale.Value.ToString("f2")}</em>元 (CNY)";

                    tbOrderPrice.Text = price.ToString(CultureInfo.CurrentCulture);

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

                        if (string.IsNullOrEmpty(o.Remark))
                        {
                            lblOrderRemark.Text = "<em>请尽快按右侧提示框的付款方式进行球票支付。--><br />我们会在收到您的款项后，为您确认球票预订成功。</em>";
                            phOrderRemark.Visible = true;
                        }

                        ucPortalProductQrCode.IsLocalUrl = true;
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
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed",
                    $"alert('{ex.Message}');window.location.href = 'iArsenalOrder.aspx'", true);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (OrderID > 0)
                {
                    var o = _repo.Single<Order>(OrderID);

                    if (o == null || !o.MemberID.Equals(Mid) || !o.IsActive)
                        throw new Exception("此订单无效或非当前用户订单");

                    o.Status = OrderStatusType.Submitted;
                    o.UpdateTime = DateTime.Now;
                    o.Price = Convert.ToSingle(tbOrderPrice.Text.Trim());

                    _repo.Update(o);

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed",
                        $"alert('谢谢您的订购，您的订单已经提交成功。\\r\\n请尽快付款以完成订单确认，订单号为：{o.ID}'); window.location.href = window.location.href",
                        true);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", $"alert('{ex.Message}');", true);
            }
        }

        protected void btnModify_Click(object sender, EventArgs e)
        {
            try
            {
                if (OrderID > 0)
                {
                    var o = _repo.Single<Order>(OrderID);

                    if (o == null || !o.MemberID.Equals(Mid) || !o.IsActive)
                        throw new Exception("此订单无效或非当前用户订单");

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed",
                        $"window.location.href = 'iArsenalOrder_2017TicketBeijing.aspx?OrderID={o.ID}'", true);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", $"alert('{ex.Message}');", true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (OrderID > 0)
                {
                    var o = _repo.Single<Order>(OrderID);

                    if (o == null || !o.MemberID.Equals(Mid) || !o.IsActive)
                        throw new Exception("此订单无效或非当前用户订单");

                    o.IsActive = false;
                    o.UpdateTime = DateTime.Now;
                    o.Price = Convert.ToSingle(tbOrderPrice.Text.Trim());

                    _repo.Update(o);

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed",
                        $"alert('此订单({o.ID})已经取消');window.location.href = 'iArsenalOrder.aspx'", true);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", $"alert('{ex.Message}');", true);
            }
        }
    }
}
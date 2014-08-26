using System;
using System.Collections.Generic;
using System.Linq;

using iArsenal.Entity;

namespace iArsenal.Web
{
    public partial class iArsenalOrderView_ReplicaKit : MemberPageBase
    {
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
                    Order_ReplicaKit o = new Order_ReplicaKit(OrderID);

                    // Whether Home or Away ReplicaKit
                    OrderItemBase oiReplicaKit = null;

                    if (o.OIReplicaKitHome != null && o.OIReplicaKitHome.IsActive)
                    {
                        oiReplicaKit = (OrderItem_ReplicaKitHome)o.OIReplicaKitHome;
                    }
                    else if (o.OIReplicaKitCup != null && o.OIReplicaKitCup.IsActive)
                    {
                        oiReplicaKit = (OrderItem_ReplicaKitCup)o.OIReplicaKitCup;
                    }
                    else if (o.OIReplicaKitAway != null && o.OIReplicaKitAway.IsActive)
                    {
                        oiReplicaKit = (OrderItem_ReplicaKitAway)o.OIReplicaKitAway;
                    }
                    else
                    {
                        throw new Exception("此订单未购买球衣商品");
                    }

                    if (ConfigAdmin.IsPluginAdmin(UID) && o != null)
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

                    lblOrderMobile.Text = string.Format("<em>{0}</em>", o.Mobile);
                    lblOrderPayment.Text = o.PaymentInfo;
                    lblOrderAddress.Text = o.Address;
                    lblOrderDescription.Text = o.Description;
                    lblOrderID.Text = string.Format("<em>{0}</em>", o.OrderID.ToString());
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

                    OrderItem_PlayerNumber oiNumber = o.OIPlayerNumber;
                    OrderItem_PlayerName oiName = o.OIPlayerName;
                    OrderItem_ArsenalFont oiFont = o.OIArsenalFont; ;
                    OrderItem_PremiershipPatch oiPremierPatch = o.OIPremiershipPatch;
                    OrderItem_ChampionshipPatch oiChampionPatch = o.OIChampionshipPatch;

                    lblOrderItem_ReplicaKit.Text = string.Format("<em>{0}</em>", oiReplicaKit.ProductName);
                    tbOrderItem_ReplicaKit.Text = oiReplicaKit.ProductGuid.ToString();
                    lblOrderItem_ReplicaKitSize.Text = oiReplicaKit.Size;

                    price = oiReplicaKit.TotalPrice;
                    priceInfo = string.Format("<合计> 球衣：{0}", oiReplicaKit.TotalPrice.ToString("f2"));

                    if (oiNumber != null && oiNumber.IsActive && oiName != null && oiName.IsActive)
                    {
                        if (oiFont != null && oiFont.IsActive)
                        {
                            lblOrderItem_PlayerDetail.Text = string.Format("{0} ({1}) <em>【阿森纳字体】</em>", oiName.Size, oiNumber.Size);

                            price += oiFont.TotalPrice;
                            priceInfo += string.Format(" + 印字号(特殊)：{0}", oiFont.TotalPrice.ToString("f2"));
                        }
                        else
                        {
                            lblOrderItem_PlayerDetail.Text = string.Format("{0} ({1})", oiName.Size, oiNumber.Size);

                            price += oiNumber.TotalPrice + oiName.TotalPrice;
                            priceInfo += string.Format(" + 印字号：{0}", (oiNumber.TotalPrice + oiName.TotalPrice).ToString("f2"));
                        }
                    }
                    else
                    {
                        lblOrderItem_PlayerDetail.Text = "无";
                    }

                    if (oiPremierPatch != null && oiPremierPatch.IsActive && oiChampionPatch != null && oiChampionPatch.IsActive)
                    {
                        lblOrderItem_Patch.Text = string.Format("{0} | {1}", oiPremierPatch.ProductName, oiChampionPatch.ProductName);
                        price += (oiPremierPatch.TotalPrice + oiChampionPatch.TotalPrice);
                        priceInfo += string.Format(" + 袖标：{0}", (oiPremierPatch.TotalPrice + oiChampionPatch.TotalPrice).ToString("f2"));
                    }
                    else if (oiPremierPatch != null && oiPremierPatch.IsActive && oiChampionPatch == null)
                    {
                        lblOrderItem_Patch.Text = string.Format("{0} × {1}", oiPremierPatch.ProductName, oiPremierPatch.Quantity.ToString());
                        price += oiPremierPatch.TotalPrice;
                        priceInfo += string.Format(" + 袖标：{0}×{1}", oiPremierPatch.UnitPrice.ToString("f2"), oiPremierPatch.Quantity.ToString());
                    }
                    else if (oiPremierPatch == null && oiChampionPatch != null && oiChampionPatch.IsActive)
                    {
                        lblOrderItem_Patch.Text = string.Format("{0} × {1}", oiChampionPatch.ProductName, oiChampionPatch.Quantity.ToString());
                        price += oiChampionPatch.TotalPrice;
                        priceInfo += string.Format(" + 袖标：{0}×{1}", oiChampionPatch.UnitPrice.ToString("f2"), oiChampionPatch.Quantity.ToString());
                    }
                    else
                    {
                        lblOrderItem_Patch.Text = "无";
                    }

                    if (o.Postage > 0)
                    {
                        price += o.Postage;
                        priceInfo += string.Format(" + 快递费：{0}", o.Postage.ToString("f2"));
                    }

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

                        if (string.IsNullOrEmpty(o.Remark))
                        {
                            lblOrderRemark.Text = "<em>请尽快按右侧提示框的付款方式进行球衣全额支付。<br />我们会在收到您的款项后，为您安排确认并下单。</em>";
                            phOrderRemark.Visible = true;
                        }
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
                    OrderBase o = new OrderBase();
                    o.OrderID = OrderID;
                    o.Select();

                    if (o == null || !o.MemberID.Equals(MID) || !o.IsActive)
                        throw new Exception("此订单无效或非当前用户订单");

                    o.Status = OrderStatusType.Submitted;
                    o.UpdateTime = DateTime.Now;
                    o.Price = Convert.ToSingle(tbOrderPrice.Text.Trim());
                    o.Update();

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", string.Format("alert('谢谢您的订购，您的订单已经提交成功。\\r\\n请尽快通过支付宝或银行转帐付款，以完成订单确认。\\r\\n订单号为：{0}'); window.location.href = window.location.href", o.OrderID.ToString()), true);
                }
                else
                {
                    throw new Exception("此订单无效或非当前用户订单");
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
                    OrderBase o = new OrderBase();
                    o.OrderID = OrderID;
                    o.Select();

                    if (o == null || !o.MemberID.Equals(MID) || !o.IsActive)
                        throw new Exception("此订单无效或非当前用户订单");

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", string.Format("window.location.href = 'iArsenalOrder_ReplicaKit.aspx?OrderID={0}'", o.OrderID.ToString()), true);
                }
                else
                {
                    throw new Exception("此订单无效或非当前用户订单");
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
                    OrderBase o = new OrderBase();
                    o.OrderID = OrderID;
                    o.Select();

                    if (o == null || !o.MemberID.Equals(MID) || !o.IsActive)
                        throw new Exception("此订单无效或非当前用户订单");

                    o.IsActive = false;
                    o.UpdateTime = DateTime.Now;
                    o.Price = Convert.ToSingle(tbOrderPrice.Text.Trim());
                    o.Update();

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", string.Format("alert('此订单({0})已经取消');window.location.href = 'iArsenalOrder.aspx'", o.OrderID.ToString()), true);
                }
                else
                {
                    throw new Exception("此订单无效或非当前用户订单");
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}');", ex.Message.ToString()), true);
            }
        }
    }
}
using System;
using System.Globalization;
using Arsenalcn.Core;
using iArsenal.Service;

namespace iArsenal.Web
{
    public partial class iArsenalOrderView_ReplicaKit : MemberPageBase
    {
        private readonly IRepository _repo = new Repository();

        private int OrderID
        {
            get
            {
                int id;
                if (!string.IsNullOrEmpty(Request.QueryString["OrderID"]) &&
                    int.TryParse(Request.QueryString["OrderID"], out id))
                {
                    return id;
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
                lblMemberName.Text = $"<b>{MemberName}</b> (<em>NO.{MID}</em>)";

                if (OrderID > 0)
                {
                    var o = (OrdrReplicaKit) Order.Select(OrderID);

                    // Whether Home or Away ReplicaKit
                    OrderItem oiReplicaKit;

                    if (o.OIReplicaKitHome != null && o.OIReplicaKitHome.IsActive)
                    {
                        oiReplicaKit = o.OIReplicaKitHome;
                    }
                    else if (o.OIReplicaKitCup != null && o.OIReplicaKitCup.IsActive)
                    {
                        oiReplicaKit = o.OIReplicaKitCup;
                    }
                    else if (o.OIReplicaKitAway != null && o.OIReplicaKitAway.IsActive)
                    {
                        oiReplicaKit = o.OIReplicaKitAway;
                    }
                    else
                    {
                        throw new Exception("此订单未购买球衣商品");
                    }

                    if (ConfigGlobal.IsPluginAdmin(UID))
                    {
                        lblMemberName.Text = $"<b>{o.MemberName}</b> (<em>NO.{o.MemberID}</em>)";
                    }
                    else
                    {
                        if (!o.MemberID.Equals(MID) || !o.IsActive)
                            throw new Exception("此订单无效或非当前用户订单");
                    }

                    #region Bind OrderView Status Workflow

                    if (ucPortalWorkflowInfo != null)
                    {
                        ucPortalWorkflowInfo.JSONOrderStatusList = $"[ {string.Join(",", o.StatusWorkflowInfo)} ]";
                        ucPortalWorkflowInfo.CurrOrderStatus = o.Status;
                    }

                    #endregion

                    lblOrderMobile.Text = $"<em>{o.Mobile}</em>";
                    lblOrderPayment.Text = o.PaymentInfo;
                    lblOrderAddress.Text = o.Address;
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

                    var oiNumber = o.OIPlayerNumber;
                    var oiName = o.OIPlayerName;
                    var oiFont = o.OIArsenalFont;
                    
                    var oiPremierPatch = o.OIPremiershipPatch;
                    var oiChampionPatch = o.OIChampionshipPatch;

                    lblOrderItem_ReplicaKit.Text = $"<em>{oiReplicaKit.ProductName}</em>";
                    //tbOrderItem_ReplicaKit.Text = oiReplicaKit.ProductGuid.ToString();
                    lblOrderItem_ReplicaKitSize.Text = oiReplicaKit.Size;

                    price = oiReplicaKit.TotalPrice;
                    priceInfo = $"<合计> 球衣：{oiReplicaKit.TotalPrice.ToString("f2")}";

                    if (oiNumber != null && oiNumber.IsActive && oiName != null && oiName.IsActive)
                    {
                        if (oiFont != null && oiFont.IsActive)
                        {
                            lblOrderItem_PlayerDetail.Text =
                                $"{oiName.Size} ({oiNumber.Size}) <em>【{Product.Cache.Load(oiFont.ProductGuid).DisplayName}】</em>";

                            price += oiFont.TotalPrice;
                            priceInfo += $" + 印字号(特殊)：{oiFont.TotalPrice.ToString("f2")}";
                        }
                        else
                        {
                            lblOrderItem_PlayerDetail.Text = $"{oiName.Size} ({oiNumber.Size})";

                            price += oiNumber.TotalPrice + oiName.TotalPrice;
                            priceInfo += $" + 印字号：{(oiNumber.TotalPrice + oiName.TotalPrice).ToString("f2")}";
                        }
                    }
                    else
                    {
                        lblOrderItem_PlayerDetail.Text = "无";
                    }

                    if (oiPremierPatch != null && oiPremierPatch.IsActive && oiChampionPatch != null &&
                        oiChampionPatch.IsActive)
                    {
                        lblOrderItem_Patch.Text = $"{oiPremierPatch.ProductName} | {oiChampionPatch.ProductName}";
                        price += (oiPremierPatch.TotalPrice + oiChampionPatch.TotalPrice);
                        priceInfo += $" + 袖标：{(oiPremierPatch.TotalPrice + oiChampionPatch.TotalPrice).ToString("f2")}";
                    }
                    else if (oiPremierPatch != null && oiPremierPatch.IsActive && oiChampionPatch == null)
                    {
                        lblOrderItem_Patch.Text = $"{oiPremierPatch.ProductName} × {oiPremierPatch.Quantity}";
                        price += oiPremierPatch.TotalPrice;
                        priceInfo +=
                            $" + 袖标：{oiPremierPatch.UnitPrice.ToString("f2")}×{oiPremierPatch.Quantity}";
                    }
                    else if (oiPremierPatch == null && oiChampionPatch != null && oiChampionPatch.IsActive)
                    {
                        lblOrderItem_Patch.Text =
                            $"{oiChampionPatch.ProductName} × {oiChampionPatch.Quantity}";
                        price += oiChampionPatch.TotalPrice;
                        priceInfo +=
                            $" + 袖标：{oiChampionPatch.UnitPrice.ToString("f2")}×{oiChampionPatch.Quantity}";
                    }
                    else
                    {
                        lblOrderItem_Patch.Text = "无";
                    }

                    if (o.Postage > 0)
                    {
                        price += o.Postage;
                        priceInfo += $" + 快递费：{o.Postage.ToString("f2")}";
                    }

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
                            lblOrderRemark.Text = "<em>请尽快按右侧提示框的付款方式进行球衣全额支付。<br />我们会在收到您的款项后，为您安排确认并下单。</em>";
                            phOrderRemark.Visible = true;
                        }

                        ucPortalProductQrCode.QrCodeUrl = "~/UploadFiles/qrcode-alipay-vicky.png";
                        ucPortalProductQrCode.QrCodeProvider = "支付宝";
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
                ClientScript.RegisterClientScriptBlock(typeof (string), "failed",
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

                    if (o == null || !o.MemberID.Equals(MID) || !o.IsActive)
                        throw new Exception("此订单无效或非当前用户订单");

                    o.Status = OrderStatusType.Submitted;
                    o.UpdateTime = DateTime.Now;
                    o.Price = Convert.ToSingle(tbOrderPrice.Text.Trim());

                    _repo.Update(o);

                    ClientScript.RegisterClientScriptBlock(typeof (string), "succeed",
                        $"alert('谢谢您的订购，您的订单已经提交成功。\\r\\n请尽快通过支付宝或银行转帐付款，以完成订单确认。\\r\\n订单号为：{o.ID}'); window.location.href = window.location.href",
                        true);
                }
                else
                {
                    throw new Exception("此订单无效或非当前用户订单");
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof (string), "failed", $"alert('{ex.Message}');", true);
            }
        }

        protected void btnModify_Click(object sender, EventArgs e)
        {
            try
            {
                if (OrderID > 0)
                {
                    var o = _repo.Single<Order>(OrderID);

                    if (o == null || !o.MemberID.Equals(MID) || !o.IsActive)
                        throw new Exception("此订单无效或非当前用户订单");

                    ClientScript.RegisterClientScriptBlock(typeof (string), "succeed",
                        $"window.location.href = 'iArsenalOrder_ReplicaKit.aspx?OrderID={o.ID}'", true);
                }
                else
                {
                    throw new Exception("此订单无效或非当前用户订单");
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof (string), "failed", $"alert('{ex.Message}');", true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (OrderID > 0)
                {
                    var o = _repo.Single<Order>(OrderID);

                    if (o == null || !o.MemberID.Equals(MID) || !o.IsActive)
                        throw new Exception("此订单无效或非当前用户订单");

                    o.IsActive = false;
                    o.UpdateTime = DateTime.Now;
                    o.Price = Convert.ToSingle(tbOrderPrice.Text.Trim());

                    _repo.Update(o);

                    ClientScript.RegisterClientScriptBlock(typeof (string), "succeed",
                        $"alert('此订单({o.ID})已经取消');window.location.href = 'iArsenalOrder.aspx'", true);
                }
                else
                {
                    throw new Exception("此订单无效或非当前用户订单");
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof (string), "failed", $"alert('{ex.Message}');", true);
            }
        }
    }
}
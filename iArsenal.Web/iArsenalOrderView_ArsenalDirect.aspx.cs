using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using Arsenalcn.Core;
using iArsenal.Service;

namespace iArsenal.Web
{
    public partial class iArsenalOrderView_ArsenalDirect : MemberPageBase
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
                    var o = (OrdrWish)Order.Select(OrderID);

                    if (ConfigGlobal.IsPluginAdmin(UID) && o != null)
                    {
                        lblMemberName.Text = $"<b>{o.MemberName}</b> (<em>NO.{o.MemberID}</em>)";
                    }
                    else
                    {
                        if (o == null || !o.MemberID.Equals(MID) || !o.IsActive)
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

                    var m = _repo.Single<Member>(o.MemberID);

                    if (m != null)
                    {
                        lblMemberEmail.Text = m.Email;
                        lblMemberQQ.Text = m.QQ;
                        phMemberInfo.Visible = true;
                    }
                    else
                    {
                        lblMemberEmail.Text = string.Empty;
                        lblMemberQQ.Text = string.Empty;
                        phMemberInfo.Visible = false;
                    }

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

                    BindItemData();

                    if (o.Status.Equals(OrderStatusType.Draft))
                    {
                        btnSubmit.Visible = true;
                        btnModify.Visible = true;
                        btnConfirm.Visible = false;
                        btnCancel.Visible = true;
                    }
                    else if (o.Status.Equals(OrderStatusType.Submitted))
                    {
                        btnSubmit.Visible = false;
                        btnModify.Visible = false;
                        btnConfirm.Visible = false;
                        btnCancel.Visible = true;

                        ucPortalProductQrCode.QrCodeUrl = "~/UploadFiles/qrcode-alipay-vicky.png";
                        ucPortalProductQrCode.QrCodeProvider = "支付宝";
                        ucPortalProductQrCode.IsLocalUrl = true;
                    }
                    else if (o.Status.Equals(OrderStatusType.Approved))
                    {
                        btnSubmit.Visible = false;
                        btnModify.Visible = false;
                        btnConfirm.Visible = true;
                        btnCancel.Visible = true;
                    }
                    else
                    {
                        btnSubmit.Visible = false;
                        btnModify.Visible = false;
                        btnConfirm.Visible = false;
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

        private void BindItemData()
        {
            var o = (OrdrWish)Order.Select(OrderID);

            // Should be Calculator in this Page
            var price = default(double);
            string priceInfo;
            var lstPriceInfo = new List<string>();

            var list = _repo.Query<OrderItem>(x => x.OrderID == o.ID).FindAll(x => x.IsActive).OrderBy(x => x.ID).ToList();

            if (list.Count > 0)
            {
                foreach (var oi in list)
                {
                    if (!oi.ProductGuid.Equals(Guid.Empty) && oi.TotalPrice > 0)
                    {
                        price += oi.Sale ?? oi.TotalPrice;
                        lstPriceInfo.Add(
                            $"{oi.Code}: {(oi.Sale.HasValue ? "「折」" + oi.Sale.Value.ToString("f2") : oi.TotalPrice.ToString("f2"))}");
                    }
                    else
                    {
                        lstPriceInfo.Add($"{oi.Code}: /");
                    }
                }

                gvWishItem.DataSource = list;
                gvWishItem.DataBind();
            }
            else
            {
                throw new Exception("此订单未购买纪念品");
            }

            priceInfo = string.Join(" + ", lstPriceInfo.ToArray());

            tbOrderPrice.Text = price.ToString(CultureInfo.CurrentCulture);

            if (!o.Sale.HasValue)
            {
                lblOrderPrice.Text = $"<合计> {priceInfo} = <em>{price.ToString("f2")}</em>元 (CNY)";
            }
            else
            {
                lblOrderPrice.Text =
                    $"<合计> {priceInfo} = <em>{price.ToString("f2")}</em>元 (CNY)<br /><结算价>：<em>{o.Sale.Value.ToString("f2")}</em>元 (CNY)";
            }
        }

        protected void gvWishItem_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvWishItem.PageIndex = e.NewPageIndex;

            BindItemData();
        }

        protected void gvWishItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var oi = e.Row.DataItem as OrderItem;

                var lblWishPriceInfo = e.Row.FindControl("lblWishPriceInfo") as Label;
                var lblWishTotalPriceInfo = e.Row.FindControl("lblWishTotalPriceInfo") as Label;

                if (oi != null && lblWishTotalPriceInfo != null && lblWishPriceInfo != null)
                {
                    if (!oi.ProductGuid.Equals(Guid.Empty) && Product.Cache.Load(oi.ProductGuid) != null)
                    {
                        var p = Product.Cache.Load(oi.ProductGuid);

                        if (p.Sale.HasValue)
                        {
                            lblWishPriceInfo.Text = p.SaleInfo;
                            lblWishPriceInfo.CssClass = "Sale";

                            lblWishTotalPriceInfo.Text =
                                $"<em>{p.CurrencyInfo}{(p.Sale.Value*oi.Quantity).ToString("f2")}</em>";
                        }
                        else
                        {
                            lblWishPriceInfo.Text = p.PriceInfo;
                            lblWishTotalPriceInfo.Text =
                                $"<em>{p.CurrencyInfo}{(p.Price*oi.Quantity).ToString("f2")}</em>";
                        }
                    }
                    else
                    {
                        lblWishPriceInfo.Text = "/";
                        lblWishTotalPriceInfo.Text = "/";
                    }
                }
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

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed",
                        $"alert('谢谢您的预订，您的订单已经提交成功。\\r\\n请在审核后完成订单确认，订单号为：{o.ID}'); window.location.href = window.location.href",
                        true);
                }
                else
                {
                    throw new Exception("此订单无效或非当前用户订单");
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed",
                    $"alert('{ex.Message}');window.location.href = 'iArsenalOrder_ArsenalDirect.aspx'", true);
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

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed",
                        $"window.location.href = 'iArsenalOrder_ArsenalDirect.aspx?OrderID={o.ID}'", true);
                }
                else
                {
                    throw new Exception("此订单无效或非当前用户订单");
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", $"alert('{ex.Message}');", true);
            }
        }


        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                if (OrderID > 0)
                {
                    var o = _repo.Single<Order>(OrderID);

                    if (o == null || !o.MemberID.Equals(MID) || !o.IsActive)
                        throw new Exception("此订单无效或非当前用户订单");

                    o.Status = OrderStatusType.Confirmed;
                    o.UpdateTime = DateTime.Now;
                    o.Price = Convert.ToSingle(tbOrderPrice.Text.Trim());

                    _repo.Update(o);

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed",
                        $"alert('谢谢您的预订，您的订单已经确认成功。\\r\\n我们将在到货后与您联系，订单号为：{o.ID}'); window.location.href = window.location.href",
                        true);
                }
                else
                {
                    throw new Exception("此订单无效或非当前用户订单");
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

                    if (o == null || !o.MemberID.Equals(MID) || !o.IsActive)
                        throw new Exception("此订单无效或非当前用户订单");

                    o.IsActive = false;
                    o.UpdateTime = DateTime.Now;
                    o.Price = Convert.ToSingle(tbOrderPrice.Text.Trim());

                    _repo.Update(o);

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed",
                        $"alert('此订单({o.ID})已经取消');window.location.href = 'iArsenalOrder.aspx'", true);
                }
                else
                {
                    throw new Exception("此订单无效或非当前用户订单");
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", $"alert('{ex.Message}');", true);
            }
        }
    }
}
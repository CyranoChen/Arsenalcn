using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

using iArsenal.Service;
using Arsenalcn.Core;

namespace iArsenal.Web
{
    public partial class iArsenalOrderView_ArsenalDirect : MemberPageBase
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
                    var o = (OrdrWish)Order.Select(OrderID);

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

                    lblOrderMobile.Text = string.Format("<em>{0}</em>", o.Mobile);

                    var m = repo.Single<Member>(o.MemberID);

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
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}');window.location.href = 'iArsenalOrder.aspx'", ex.Message.ToString()), true);
            }
        }

        private void BindItemData()
        {
            var o = (OrdrWish)Order.Select(OrderID);

            // Should be Calculator in this Page
            var price = default(double);
            var priceInfo = string.Empty;
            var _lstPriceInfo = new List<string>();

            var list = repo.Query<OrderItem>(x =>
                x.OrderID == o.ID && x.IsActive == true).OrderBy(x => x.ID).ToList();

            if (list != null && list.Count > 0)
            {
                foreach (var oi in list)
                {
                    if (!oi.ProductGuid.Equals(Guid.Empty) && oi.TotalPrice > 0)
                    {
                        price += (oi.Sale.HasValue ? oi.Sale.Value : oi.TotalPrice);
                        _lstPriceInfo.Add(string.Format("{0}: {1}", oi.Code, oi.Sale.HasValue ? "「折」" + oi.Sale.Value.ToString("f2") : oi.TotalPrice.ToString("f2")));
                    }
                    else
                    {
                        _lstPriceInfo.Add(string.Format("{0}: /", oi.Code));
                    }
                }

                gvWishItem.DataSource = list;
                gvWishItem.DataBind();
            }
            else
            {
                throw new Exception("此订单未购买纪念品");
            }

            priceInfo = string.Join(" + ", _lstPriceInfo.ToArray());

            tbOrderPrice.Text = price.ToString();

            if (!o.Sale.HasValue)
            {
                lblOrderPrice.Text = string.Format("<合计> {0} = <em>{1}</em>元 (CNY)", priceInfo, price.ToString("f2"));
            }
            else
            {
                lblOrderPrice.Text = string.Format("<合计> {0} = <em>{1}</em>元 (CNY)<br /><结算价>：<em>{2}</em>元 (CNY)", priceInfo, price.ToString("f2"), o.Sale.Value.ToString("f2"));
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

                if (oi != null && !oi.ProductGuid.Equals(Guid.Empty) && Product.Cache.Load(oi.ProductGuid) != null)
                {
                    var p = Product.Cache.Load(oi.ProductGuid);

                    if (p.Sale.HasValue)
                    {
                        lblWishPriceInfo.Text = p.SaleInfo;
                        lblWishPriceInfo.CssClass = "Sale";

                        lblWishTotalPriceInfo.Text = string.Format("<em>{0}{1}</em>", p.CurrencyInfo, (p.Sale.Value * oi.Quantity).ToString("f2"));
                    }
                    else
                    {
                        lblWishPriceInfo.Text = p.PriceInfo;
                        lblWishTotalPriceInfo.Text = string.Format("<em>{0}{1}</em>", p.CurrencyInfo, (p.Price * oi.Quantity).ToString("f2"));
                    }
                }
                else
                {
                    lblWishPriceInfo.Text = "/";
                    lblWishTotalPriceInfo.Text = "/";
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (OrderID > 0)
                {
                    var o = repo.Single<Order>(OrderID);

                    if (o == null || !o.MemberID.Equals(MID) || !o.IsActive)
                        throw new Exception("此订单无效或非当前用户订单");

                    o.Status = OrderStatusType.Submitted;
                    o.UpdateTime = DateTime.Now;
                    o.Price = Convert.ToSingle(tbOrderPrice.Text.Trim());

                    repo.Update(o);

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", string.Format("alert('谢谢您的预订，您的订单已经提交成功。\\r\\n请在审核后完成订单确认，订单号为：{0}'); window.location.href = window.location.href", o.ID.ToString()), true);
                }
                else
                {
                    throw new Exception("此订单无效或非当前用户订单");
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}');window.location.href = 'iArsenalOrder_ArsenalDirect.aspx'", ex.Message.ToString()), true);
            }
        }

        protected void btnModify_Click(object sender, EventArgs e)
        {
            try
            {
                if (OrderID > 0)
                {
                    var o = repo.Single<Order>(OrderID);

                    if (o == null || !o.MemberID.Equals(MID) || !o.IsActive)
                        throw new Exception("此订单无效或非当前用户订单");

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", string.Format("window.location.href = 'iArsenalOrder_ArsenalDirect.aspx?OrderID={0}'", o.ID.ToString()), true);
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


        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                if (OrderID > 0)
                {
                    var o = repo.Single<Order>(OrderID);

                    if (o == null || !o.MemberID.Equals(MID) || !o.IsActive)
                        throw new Exception("此订单无效或非当前用户订单");

                    o.Status = OrderStatusType.Confirmed;
                    o.UpdateTime = DateTime.Now;
                    o.Price = Convert.ToSingle(tbOrderPrice.Text.Trim());

                    repo.Update(o);

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", string.Format("alert('谢谢您的预订，您的订单已经确认成功。\\r\\n我们将在到货后与您联系，订单号为：{0}'); window.location.href = window.location.href", o.ID.ToString()), true);
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
                    var o = repo.Single<Order>(OrderID);

                    if (o == null || !o.MemberID.Equals(MID) || !o.IsActive)
                        throw new Exception("此订单无效或非当前用户订单");

                    o.IsActive = false;
                    o.UpdateTime = DateTime.Now;
                    o.Price = Convert.ToSingle(tbOrderPrice.Text.Trim());

                    repo.Update(o);

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", string.Format("alert('此订单({0})已经取消');window.location.href = 'iArsenalOrder.aspx'", o.ID.ToString()), true);
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
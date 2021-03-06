﻿using System;
using System.Web.UI.WebControls;
using Arsenalcn.Core;
using Arsenalcn.Core.Dapper;
using iArsenal.Service;
using iArsenal.Web.Control;

namespace iArsenal.Web
{
    public partial class iArsenalOrder : MemberPageBase
    {
        private readonly IRepository repo = new Repository();

        private int _orderID = int.MinValue;

        private int OrderID
        {
            get
            {
                int _res;
                if (_orderID == 0)
                    return _orderID;
                if (!string.IsNullOrEmpty(Request.QueryString["OrderID"]) &&
                    int.TryParse(Request.QueryString["OrderID"], out _res))
                    return _res;
                return int.MinValue;
            }
            set { _orderID = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlCustomPagerInfo.PageChanged += ctrlCustomPagerInfo_PageChanged;
            tbOrderID.Attributes["placeholder"] = "--订单编号--";

            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            try
            {
                var list = repo.Query<Order>(x => x.MemberID == Mid).FindAll(x =>
                {
                    var returnValue = true;
                    var tmpString = string.Empty;

                    if (ViewState["OrderID"] != null)
                    {
                        tmpString = ViewState["OrderID"].ToString();
                        if (!string.IsNullOrEmpty(tmpString) && !tmpString.Equals("--订单编号--"))
                            returnValue = returnValue && x.ID.Equals(Convert.ToInt32(tmpString));
                    }

                    if (ViewState["ProductType"] != null)
                    {
                        tmpString = ViewState["ProductType"].ToString();
                        if (!string.IsNullOrEmpty(tmpString))
                            returnValue = returnValue && x.OrderType.ToString().Equals(tmpString);
                    }

                    // Find all Orders which belong to Current Member & Active
                    returnValue = returnValue && x.IsActive;

                    return returnValue;
                });

                #region set GridView Selected PageIndex

                if (OrderID > 0)
                {
                    var i = list.FindIndex(x => x.ID.Equals(OrderID));
                    if (i >= 0)
                    {
                        gvOrder.PageIndex = i/gvOrder.PageSize;
                        gvOrder.SelectedIndex = i%gvOrder.PageSize;
                    }
                    else
                    {
                        gvOrder.PageIndex = 0;
                        gvOrder.SelectedIndex = -1;
                    }
                }
                else
                {
                    gvOrder.SelectedIndex = -1;
                }

                #endregion

                gvOrder.DataSource = list;
                gvOrder.DataBind();

                #region set Control Custom Pager

                if (gvOrder.BottomPagerRow != null)
                {
                    gvOrder.BottomPagerRow.Visible = true;
                    ctrlCustomPagerInfo.Visible = true;

                    ctrlCustomPagerInfo.PageIndex = gvOrder.PageIndex;
                    ctrlCustomPagerInfo.PageCount = gvOrder.PageCount;
                    ctrlCustomPagerInfo.RowCount = list.Count;
                    ctrlCustomPagerInfo.InitComponent();
                }
                else
                {
                    ctrlCustomPagerInfo.Visible = false;
                }

                #endregion
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof (string), "failed", $"alert('{ex.Message}')", true);
            }
        }

        protected void gvOrder_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvOrder.PageIndex = e.NewPageIndex;
            OrderID = 0;

            BindData();
        }

        protected void ctrlCustomPagerInfo_PageChanged(object sender, CustomPagerInfo.DataNavigatorEventArgs e)
        {
            if (e.PageIndex > 0)
            {
                gvOrder.PageIndex = e.PageIndex;
                OrderID = 0;
            }

            BindData();
        }

        protected void gvOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvOrder.SelectedIndex != -1)
            {
                var o = repo.Single<Order>((int) gvOrder.DataKeys[gvOrder.SelectedIndex].Value);

                if (o != null)
                {
                    Response.Redirect($"ServerOrderView.ashx?OrderID={o.ID}");
                }
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbOrderID.Text.Trim()))
                ViewState["OrderID"] = tbOrderID.Text.Trim();
            else
                ViewState["OrderID"] = string.Empty;

            if (!string.IsNullOrEmpty(ddlOrderType.SelectedValue))
                ViewState["ProductType"] = ddlOrderType.SelectedValue;
            else
                ViewState["ProductType"] = string.Empty;

            OrderID = 0;
            gvOrder.PageIndex = 0;

            BindData();
        }

        protected void gvOrder_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var _strStatus = string.Empty;
                var o = e.Row.DataItem as Order;

                var lblOrderType = e.Row.FindControl("lblOrderType") as Label;
                var lblOrderStatus = e.Row.FindControl("lblOrderStatus") as Label;
                var lblPriceInfo = e.Row.FindControl("lblPriceInfo") as Label;

                if (lblOrderType != null && !o.OrderType.Equals(OrderBaseType.None))
                {
                    lblOrderType.Text = $"<em>{ddlOrderType.Items.FindByValue(o.OrderType.ToString()).Text}</em>";
                }
                else
                {
                    lblOrderType.Visible = false;
                }

                if (lblOrderStatus != null)
                {
                    if (o.Status.Equals(OrderStatusType.Confirmed))
                        _strStatus = $"<em>{o.StatusInfo}</em>";
                    else
                        _strStatus = o.StatusInfo;

                    lblOrderStatus.Text = _strStatus;
                }

                if (lblPriceInfo != null)
                {
                    if (o.Sale.HasValue)
                    {
                        lblPriceInfo.CssClass = "Sale";
                    }
                    else
                    {
                        lblPriceInfo.CssClass = string.Empty;
                    }

                    lblPriceInfo.Text = $"<em>￥{o.PriceInfo}</em>";
                }
            }
        }
    }
}
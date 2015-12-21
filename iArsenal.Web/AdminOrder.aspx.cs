using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

using Arsenalcn.Core;
using Arsenalcn.Core.Utility;
using iArsenal.Service;

namespace iArsenal.Web
{
    public partial class AdminOrder : AdminPageBase
    {
        private readonly IRepository repo = new Repository();
        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = this.Username;
            ctrlCustomPagerInfo.PageChanged += new Control.CustomPagerInfo.PageChangedEventHandler(ctrlCustomPagerInfo_PageChanged);

            if (!IsPostBack)
            {
                BindData();
            }
        }

        private int _orderID = int.MinValue;
        private int OrderID
        {
            get
            {
                int _res;
                if (_orderID == 0)
                    return _orderID;
                else if (!string.IsNullOrEmpty(Request.QueryString["OrderID"]) && int.TryParse(Request.QueryString["OrderID"], out _res))
                    return _res;
                else
                    return int.MinValue;
            }
            set { _orderID = value; }
        }

        private int _memberID = int.MinValue;
        private int MemberID
        {
            get
            {
                int _res;
                if (_memberID == 0)
                    return _memberID;
                else if (!string.IsNullOrEmpty(Request.QueryString["MemberID"]) && int.TryParse(Request.QueryString["MemberID"], out _res))
                    return _res;
                else
                    return int.MinValue;
            }
            set { _memberID = value; }
        }

        private void BindData()
        {
            if (MemberID > 0)
            {
                var m = repo.Single<Member>(MemberID);

                if (m != null)
                {
                    ViewState["MemberName"] = m.Name;
                    tbMemberName.Text = m.Name;
                    tbMemberName.ReadOnly = true;
                }
            }

            var list = repo.All<Order>().FindAll(x =>
            {
                var returnValue = true;
                var tmpString = string.Empty;

                if (ViewState["OrderID"] != null)
                {
                    tmpString = ViewState["OrderID"].ToString();
                    if (!string.IsNullOrEmpty(tmpString) && !tmpString.Equals("--订单编号--"))
                        returnValue = returnValue && x.ID.Equals(Convert.ToInt32(tmpString));
                }

                if (ViewState["MemberName"] != null)
                {
                    tmpString = ViewState["MemberName"].ToString();
                    if (!string.IsNullOrEmpty(tmpString) && !tmpString.Equals("--会员姓名--"))
                        returnValue = returnValue && x.MemberName.Contains(tmpString);
                }

                if (ViewState["Mobile"] != null)
                {
                    tmpString = ViewState["Mobile"].ToString();
                    if (!string.IsNullOrEmpty(tmpString) && !tmpString.Equals("--手机--"))
                        returnValue = returnValue && x.Mobile.Equals(tmpString);
                }

                if (ViewState["ProductType"] != null)
                {
                    tmpString = ViewState["ProductType"].ToString();
                    if (!string.IsNullOrEmpty(tmpString))
                        returnValue = returnValue && x.OrderType.ToString().Equals(tmpString, StringComparison.OrdinalIgnoreCase);
                }

                if (ViewState["Status"] != null)
                {
                    tmpString = ViewState["Status"].ToString();
                    if (!string.IsNullOrEmpty(tmpString))
                        returnValue = returnValue && ((int)x.Status).Equals(Convert.ToInt32(tmpString));
                }

                if (ViewState["IsActive"] != null)
                {
                    tmpString = ViewState["IsActive"].ToString();
                    if (!string.IsNullOrEmpty(tmpString))
                        returnValue = returnValue && x.IsActive.Equals(Convert.ToBoolean(tmpString));
                }

                return returnValue;
            });

            #region set GridView Selected PageIndex
            if (OrderID > 0)
            {
                var i = list.FindIndex(x => x.ID.Equals(OrderID));
                if (i >= 0)
                {
                    gvOrder.PageIndex = i / gvOrder.PageSize;
                    gvOrder.SelectedIndex = i % gvOrder.PageSize;
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

        protected void gvOrder_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvOrder.PageIndex = e.NewPageIndex;
            OrderID = 0;

            BindData();
        }

        protected void ctrlCustomPagerInfo_PageChanged(object sender, Control.CustomPagerInfo.DataNavigatorEventArgs e)
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
                Response.Redirect(
                    $"AdminOrderView.aspx?OrderID={gvOrder.DataKeys[gvOrder.SelectedIndex].Value.ToString()}");
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbOrderID.Text.Trim()))
                ViewState["OrderID"] = tbOrderID.Text.Trim();
            else
                ViewState["OrderID"] = string.Empty;

            if (!string.IsNullOrEmpty(tbMemberName.Text.Trim()))
                ViewState["MemberName"] = tbMemberName.Text.Trim();
            else
                ViewState["MemberName"] = string.Empty;

            if (!string.IsNullOrEmpty(tbMobile.Text.Trim()))
                ViewState["Mobile"] = tbMobile.Text.Trim();
            else
                ViewState["Mobile"] = string.Empty;

            if (!string.IsNullOrEmpty(ddlOrderType.SelectedValue))
                ViewState["ProductType"] = ddlOrderType.SelectedValue;
            else
                ViewState["ProductType"] = string.Empty;

            if (!string.IsNullOrEmpty(ddlStatus.SelectedValue))
                ViewState["Status"] = ddlStatus.SelectedValue;
            else
                ViewState["Status"] = string.Empty;

            if (!string.IsNullOrEmpty(ddlIsActive.SelectedValue))
                ViewState["IsActive"] = ddlIsActive.SelectedValue;
            else
                ViewState["IsActive"] = string.Empty;

            OrderID = 0;
            gvOrder.PageIndex = 0;

            BindData();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                #region Get the Order List
                var list = repo.All<Order>().FindAll(x =>
                {
                    var returnValue = x.IsActive; // Export the active order
                    var tmpString = string.Empty;

                    if (ViewState["OrderID"] != null)
                    {
                        tmpString = ViewState["OrderID"].ToString();
                        if (!string.IsNullOrEmpty(tmpString) && !tmpString.Equals("--订单编号--"))
                            returnValue = returnValue && x.ID.Equals(Convert.ToInt32(tmpString));
                    }

                    if (ViewState["MemberName"] != null)
                    {
                        tmpString = ViewState["MemberName"].ToString();
                        if (!string.IsNullOrEmpty(tmpString) && !tmpString.Equals("--会员姓名--"))
                            returnValue = returnValue && x.MemberName.Contains(tmpString);
                    }

                    if (ViewState["Mobile"] != null)
                    {
                        tmpString = ViewState["Mobile"].ToString();
                        if (!string.IsNullOrEmpty(tmpString) && !tmpString.Equals("--手机--"))
                            returnValue = returnValue && x.Mobile.Equals(tmpString);
                    }

                    if (ViewState["ProductType"] != null)
                    {
                        tmpString = ViewState["ProductType"].ToString();
                        if (!string.IsNullOrEmpty(tmpString))
                            returnValue = returnValue && x.OrderType.ToString().Equals(tmpString);
                    }

                    if (ViewState["Status"] != null)
                    {
                        tmpString = ViewState["Status"].ToString();
                        if (!string.IsNullOrEmpty(tmpString))
                            returnValue = returnValue && ((int)x.Status).Equals(Convert.ToInt32(tmpString));
                    }

                    return returnValue;
                });
                #endregion

                #region Convert List to DataTable
                var dt = new DataTable();
                var t = Type.GetType("System.String");

                #region Create DataTable and DataColumn
                var dcMemberAcnName = new DataColumn("User", t);
                var dcOrderMemberName = new DataColumn("Member", t);
                var dcOrderMobile = new DataColumn("Contact", t);
                var dcOrderID = new DataColumn("OID", Type.GetType("System.Int16"));
                var dcOrderItemCode = new DataColumn("Code", t);
                var dcProductName = new DataColumn("Souvenir", t);
                var dcProductDisplayName = new DataColumn("As", t);
                var dcProductColour = new DataColumn("Color", t);
                var dcOrderItemSize = new DataColumn("Size", t);
                var dcProductPrice = new DataColumn("Unit", Type.GetType("System.Single"));
                var dcOrderItemQuantity = new DataColumn("Qty", Type.GetType("System.Int16"));
                var dcProductTotalPrice = new DataColumn("GBP Price", Type.GetType("System.Single"));
                var dcOrderItemTotalPrice = new DataColumn("CNY Price", Type.GetType("System.Single"));
                var dcOrderPriceInfo = new DataColumn("Order Price", t);
                var dcOrderRemark = new DataColumn("Remark", t);

                dt.Columns.Add(dcMemberAcnName);
                dt.Columns.Add(dcOrderMemberName);
                dt.Columns.Add(dcOrderMobile);
                dt.Columns.Add(dcOrderID);
                dt.Columns.Add(dcOrderItemCode);
                dt.Columns.Add(dcProductName);
                dt.Columns.Add(dcProductDisplayName);
                dt.Columns.Add(dcProductColour);
                dt.Columns.Add(dcOrderItemSize);
                dt.Columns.Add(dcProductPrice);
                dt.Columns.Add(dcOrderItemQuantity);
                dt.Columns.Add(dcProductTotalPrice);
                dt.Columns.Add(dcOrderItemTotalPrice);
                dt.Columns.Add(dcOrderPriceInfo);
                dt.Columns.Add(dcOrderRemark);
                #endregion

                if (list != null && list.Count > 0)
                {
                    // Tackle with Every Order and OrderItem
                    foreach (var o in list)
                    {
                        var m = new Member();
                        var p = new Product();
                        var pFont = new Product();

                        switch (ddlOrderType.SelectedValue)
                        {
                            case "ReplicaKit":

                                #region Convert ReplicaKit Order to DataRow

                                var oReplicaKit = (OrdrReplicaKit)Order.Select(o.ID);

                                // Whether Home or Away or Cup ReplicaKit
                                OrderItem oiReplicaKit = null;

                                if (oReplicaKit.OIReplicaKitHome != null && oReplicaKit.OIReplicaKitHome.IsActive)
                                {
                                    oiReplicaKit = (OrdrItmReplicaKitHome)oReplicaKit.OIReplicaKitHome;
                                }
                                else if (oReplicaKit.OIReplicaKitAway != null && oReplicaKit.OIReplicaKitAway.IsActive)
                                {
                                    oiReplicaKit = (OrdrItmReplicaKitAway)oReplicaKit.OIReplicaKitAway;
                                }
                                else if (oReplicaKit.OIReplicaKitCup != null && oReplicaKit.OIReplicaKitCup.IsActive)
                                {
                                    oiReplicaKit = (OrdrItmReplicaKitCup)oReplicaKit.OIReplicaKitCup;
                                }
                                else
                                {
                                    throw new Exception($"此订单未购买球衣商品(OrderID:{oReplicaKit.ID.ToString()})");
                                }

                                var oiNumber = oReplicaKit.OIPlayerNumber;
                                var oiName = oReplicaKit.OIPlayerName;
                                var oiFont = oReplicaKit.OIArsenalFont; ;
                                var oiPremierPatch = oReplicaKit.OIPremiershipPatch;
                                var oiChampionPatch = oReplicaKit.OIChampionshipPatch;

                                // get Member Info By Order
                                m = repo.Single<Member>(o.MemberID);

                                // get Product Info By OrderItem
                                p = Product.Cache.Load(oiReplicaKit.ProductGuid);

                                var dr = dt.NewRow();
                                dr["User"] = $"{m.AcnName}({m.AcnID.ToString()})";
                                dr["Member"] = $"{o.MemberName}({o.MemberID.ToString()})";
                                dr["Contact"] = o.Mobile;

                                dr["OID"] = o.ID;
                                dr["Code"] = oiReplicaKit.Code;
                                dr["Souvenir"] = p.Name;
                                dr["As"] = p.DisplayName;
                                dr["Color"] = p.Colour;
                                dr["Size"] = oiReplicaKit.Size;

                                if (p.Sale.HasValue)
                                {
                                    dr["Unit"] = p.Sale;
                                }
                                else
                                {
                                    dr["Unit"] = p.Price;
                                }

                                dr["Qty"] = oiReplicaKit.Quantity;
                                dr["GBP Price"] = (float)dr["Unit"] * oiReplicaKit.Quantity;
                                dr["CNY Price"] = oiReplicaKit.TotalPrice;

                                dr["Order Price"] = o.PriceInfo;
                                dr["Remark"] = o.Remark;

                                dt.Rows.Add(dr);

                                if (oiNumber != null && oiNumber.IsActive && oiName != null && oiName.IsActive)
                                {
                                    // get Arsenal Font Product if oiFont is not null
                                    if (oiFont != null && oiFont.IsActive)
                                    {
                                        pFont = Product.Cache.Load(oiFont.ProductGuid);
                                    }
                                    else
                                    {
                                        pFont = null;
                                    }

                                    // Shirt Details Name
                                    p = Product.Cache.Load(oiName.ProductGuid);

                                    dr = dt.NewRow();

                                    dr["Souvenir"] = p.Name;
                                    dr["As"] = p.DisplayName;
                                    dr["Color"] = (oiFont != null) ? "Arsenal Font" : string.Empty;
                                    dr["Size"] = oiName.Size;

                                    if (pFont != null)
                                    {
                                        if (pFont.Sale.HasValue)
                                        {
                                            dr["Unit"] = pFont.Sale / 2;
                                        }
                                        else
                                        {
                                            dr["Unit"] = pFont.Price / 2;
                                        }
                                    }
                                    else
                                    {
                                        if (p.Sale.HasValue)
                                        {
                                            dr["Unit"] = p.Sale;
                                        }
                                        else
                                        {
                                            dr["Unit"] = p.Price;
                                        }
                                    }

                                    dr["Qty"] = oiName.Quantity;
                                    dr["GBP Price"] = (float)dr["Unit"] * oiName.Quantity;
                                    dr["CNY Price"] = oiName.TotalPrice;

                                    dt.Rows.Add(dr);

                                    // Shirt Details Number
                                    p = Product.Cache.Load(oiNumber.ProductGuid);

                                    dr = dt.NewRow();

                                    dr["Souvenir"] = p.Name;
                                    dr["As"] = p.DisplayName;
                                    dr["Color"] = (oiFont != null) ? "Arsenal Font" : string.Empty;
                                    dr["Size"] = oiNumber.Size;

                                    if (pFont != null)
                                    {
                                        if (pFont.Sale.HasValue)
                                        {
                                            dr["Unit"] = pFont.Sale / 2;
                                        }
                                        else
                                        {
                                            dr["Unit"] = pFont.Price / 2;
                                        }
                                    }
                                    else
                                    {
                                        if (p.Sale.HasValue)
                                        {
                                            dr["Unit"] = p.Sale;
                                        }
                                        else
                                        {
                                            dr["Unit"] = p.Price;
                                        }
                                    }

                                    dr["Qty"] = oiNumber.Quantity;
                                    dr["GBP Price"] = (float)dr["Unit"] * oiNumber.Quantity;
                                    dr["CNY Price"] = oiNumber.TotalPrice;

                                    dt.Rows.Add(dr);
                                }

                                if (oiPremierPatch != null && oiPremierPatch.IsActive)
                                {
                                    // Premier League Badge
                                    p = Product.Cache.Load(oiPremierPatch.ProductGuid);

                                    dr = dt.NewRow();

                                    dr["Souvenir"] = p.Name;
                                    dr["As"] = p.DisplayName;
                                    dr["Size"] = "PLB";

                                    if (p.Sale.HasValue)
                                    {
                                        dr["Unit"] = p.Sale;
                                    }
                                    else
                                    {
                                        dr["Unit"] = p.Price;
                                    }

                                    dr["Qty"] = oiPremierPatch.Quantity;
                                    dr["GBP Price"] = (float)dr["Unit"] * oiPremierPatch.Quantity;
                                    dr["CNY Price"] = oiPremierPatch.TotalPrice;

                                    dt.Rows.Add(dr);
                                }

                                if (oiChampionPatch != null && oiChampionPatch.IsActive)
                                {
                                    // Champions League Badge
                                    p = Product.Cache.Load(oiChampionPatch.ProductGuid);

                                    dr = dt.NewRow();

                                    dr["Souvenir"] = p.Name;
                                    dr["As"] = p.DisplayName;
                                    dr["Size"] = "CLB";

                                    if (p.Sale.HasValue)
                                    {
                                        dr["Unit"] = p.Sale;
                                    }
                                    else
                                    {
                                        dr["Unit"] = p.Price;
                                    }

                                    dr["Qty"] = oiChampionPatch.Quantity;
                                    dr["GBP Price"] = (float)dr["Unit"] * oiChampionPatch.Quantity;
                                    dr["CNY Price"] = oiChampionPatch.TotalPrice;

                                    dt.Rows.Add(dr);
                                }

                                #endregion

                                break;

                            case "Wish":

                                #region Convert Wish Order to DataRow

                                var oWish = (OrdrWish)Order.Select(o.ID);

                                // get Member Info By Order
                                m = repo.Single<Member>(o.MemberID);

                                var query = repo.Query<OrderItem>(x =>
                                    x.OrderID == o.ID && x.IsActive == true).OrderBy(x => x.ID);

                                if (query != null && query.Count() > 0)
                                {
                                    var _listCount = 0;

                                    foreach (var oi in query)
                                    {
                                        dr = dt.NewRow();

                                        if (_listCount.Equals(0))
                                        {
                                            dr["User"] = $"{m.AcnName}({m.AcnID.ToString()})";
                                            dr["Member"] = $"{o.MemberName}({o.MemberID.ToString()})";
                                            dr["Contact"] = o.Mobile;

                                            dr["OID"] = o.ID;
                                            dr["Order Price"] = o.PriceInfo;
                                            dr["Remark"] = o.Remark;
                                        }

                                        if (!oi.ProductGuid.Equals(Guid.Empty) && oi.TotalPrice > 0)
                                        {
                                            // get Product Info By OrderItem
                                            p = Product.Cache.Load(oi.ProductGuid);

                                            dr["Code"] = oi.Code;
                                            dr["Souvenir"] = p.Name;
                                            dr["As"] = p.DisplayName;
                                            dr["Color"] = p.Colour;
                                            dr["Size"] = oi.Size;

                                            if (p.Sale.HasValue)
                                            {
                                                dr["Unit"] = p.Sale;
                                            }
                                            else
                                            {
                                                dr["Unit"] = p.Price;
                                            }

                                            dr["Qty"] = oi.Quantity;
                                            dr["GBP Price"] = (float)dr["Unit"] * oi.Quantity;
                                            dr["CNY Price"] = oi.TotalPrice;
                                        }
                                        else
                                        {
                                            dr["Code"] = oi.Code;
                                            dr["Souvenir"] = oi.ProductName;
                                            dr["As"] = "/";
                                            dr["Color"] = string.Empty;
                                            dr["Size"] = oi.Size;
                                            dr["Unit"] = default(float);
                                            dr["Qty"] = oi.Quantity;
                                            dr["GBP Price"] = default(float);
                                            dr["CNY Price"] = oi.TotalPrice;
                                        }

                                        dt.Rows.Add(dr);

                                        _listCount++;
                                    }
                                }
                                else
                                {
                                    throw new Exception("此订单未购买纪念品");
                                }

                                #endregion

                                break;

                            default:
                                throw new Exception("不支持导出该类订单");
                        }
                    }
                }
                #endregion

                if (dt.Rows.Count > 0)
                {
                    // Export DataTable
                    var gvw = new GridView();
                    var fileName = $"AdminOrderExport-{DateTime.Now.ToString("yyyyMMdd-HHmmss")}.xls";

                    gvw.AutoGenerateColumns = true;

                    gvw.DataSource = dt;
                    gvw.DataBind();

                    ExportUtl.ExportToExcel(gvw, fileName);
                }
                else
                {
                    throw new Exception("无有效订单");
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", $"alert('{ex.Message.ToString()}')", true);
            }
        }

        protected void gvOrder_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var o = e.Row.DataItem as Order;

                var cbOrderID = e.Row.FindControl("cbOrderID") as CheckBox;
                var hlOrderID = e.Row.FindControl("hlOrderID") as HyperLink;
                var hlName = e.Row.FindControl("hlName") as HyperLink;
                var lblOrderType = e.Row.FindControl("lblOrderType") as Label;
                var lblOrderStatus = e.Row.FindControl("lblOrderStatus") as Label;

                if (cbOrderID != null)
                {
                    cbOrderID.Text = o.ID.ToString();

                    hlOrderID.Text = o.ID.ToString();
                    hlOrderID.NavigateUrl = $"ServerOrderView.ashx?OrderID={o.ID.ToString()}";
                }

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
                    var _strStatus = string.Empty;

                    if (o.Status.Equals(OrderStatusType.Confirmed))
                        _strStatus = $"<em>{o.StatusInfo}</em>";
                    else
                        _strStatus = o.StatusInfo;

                    lblOrderStatus.Text = _strStatus;
                }
            }
        }
    }
}

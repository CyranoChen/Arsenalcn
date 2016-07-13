using System;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;
using Arsenalcn.Core;
using iArsenal.Service;
using ArsenalPlayer = iArsenal.Service.Arsenal.Player;

namespace iArsenal.Web
{
    public partial class iArsenalOrder_ReplicaKit : MemberPageBase
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

        private ProductType CurrProductType
        {
            get
            {
                var pt = ProductType.ReplicaKitHome;

                # region Check whether home or away replicakit

                if (OrderID > 0)
                {
                    var o = (OrdrReplicaKit)Order.Select(OrderID);

                    OrderItem oiReplicaKit;

                    if (o.OIReplicaKitAway != null && o.OIReplicaKitAway.IsActive)
                    {
                        oiReplicaKit = o.OIReplicaKitAway;
                    }
                    else if (o.OIReplicaKitCup != null && o.OIReplicaKitCup.IsActive)
                    {
                        oiReplicaKit = o.OIReplicaKitCup;
                    }
                    else if (o.OIReplicaKitHome != null && o.OIReplicaKitHome.IsActive)
                    {
                        oiReplicaKit = o.OIReplicaKitHome;
                    }
                    else
                    {
                        throw new Exception("此订单未购买球衣商品");
                    }

                    pt = Product.Cache.Load(oiReplicaKit.ProductGuid).ProductType;
                }
                else
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["Type"]) &&
                        Request.QueryString["Type"].Equals("Away", StringComparison.OrdinalIgnoreCase))
                    {
                        pt = ProductType.ReplicaKitAway;
                    }
                    else if (!string.IsNullOrEmpty(Request.QueryString["Type"]) &&
                             Request.QueryString["Type"].Equals("Cup", StringComparison.OrdinalIgnoreCase))
                    {
                        pt = ProductType.ReplicaKitCup;
                    }
                    else
                    {
                        pt = ProductType.ReplicaKitHome;
                    }
                }

                #endregion

                if (pt.Equals(ProductType.ReplicaKitAway))
                {
                    Page.Title = "阿森纳2015/16赛季客场PUMA球衣许愿单";
                    hlReplicaKitPage.NavigateUrl = "http://arsenaldirect.arsenal.com/puma-kit/puma-away-kit/icat/pumaaway";
                    ltrlBannerImage.Text = $"<img src=\"uploadfiles/banner/banner20150714.png\" alt=\"{Page.Title}\" />";
                }
                else if (pt.Equals(ProductType.ReplicaKitCup))
                {
                    Page.Title = "阿森纳2015/16赛季杯赛PUMA球衣许愿单";
                    hlReplicaKitPage.NavigateUrl = "http://arsenaldirect.arsenal.com/puma-kit/puma-cup-kit/icat/pumacup";
                    ltrlBannerImage.Text = $"<img src=\"uploadfiles/banner/banner20150813.png\" alt=\"{Page.Title}\" />";
                }
                else
                {
                    Page.Title = "阿森纳2016/17赛季主场PUMA球衣许愿单";
                    hlReplicaKitPage.NavigateUrl = "http://arsenaldirect.arsenal.com/puma-kit/puma-home-kit/icat/pumahome";
                    ltrlBannerImage.Text = $"<img src=\"uploadfiles/banner/banner20160524.png\" alt=\"{Page.Title}\" />";
                }

                return pt;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                #region Bind ddlPlayerDetail

                try
                {
                    var list = Arsenal_Player.Cache.PlayerList.FindAll(p => !p.IsLegend && !p.IsLoan && p.SquadNumber > 0);

                    if (list.Count > 0)
                    {
                        ddlPlayerDetail.Items.Clear();
                        ddlPlayerDetail.DataSource = list;
                        ddlPlayerDetail.DataValueField = "ID";
                        ddlPlayerDetail.DataBind();

                        ddlPlayerDetail.Items.Insert(0, new ListItem("--请选择印字印号--", string.Empty));
                        ddlPlayerDetail.Items.Insert(1, new ListItem("【自定义】", "custom"));
                    }
                }
                catch
                {
                    ddlPlayerDetail.Items.Clear();
                    ddlPlayerDetail.Items.Insert(0, new ListItem("--请选择印字印号--", string.Empty));
                    ddlPlayerDetail.Items.Insert(1, new ListItem("【自定义】", "custom"));
                }

                #endregion

                InitForm();
            }
        }

        private void InitForm()
        {
            try
            {
                lblMemberName.Text = $"<b>{MemberName}</b> (<em>NO.{MID}</em>)";
                lblMemberACNInfo.Text = $"<b>{Username}</b> (<em>ID.{UID}</em>)";

                var pNumber = Product.Cache.Load(ProductType.PlayerNumber).Find(p => p.IsActive);
                var pName = Product.Cache.Load(ProductType.PlayerName).Find(p => p.IsActive);
                var pPremierPatch = Product.Cache.Load(ProductType.PremiershipPatch).Find(p => p.IsActive);
                var pChampionPatch = Product.Cache.Load(ProductType.ChampionshipPatch).Find(p => p.IsActive);
                var pFont = Product.Cache.Load(ProductType.ArsenalFont).Find(p => p.IsActive);

                if (pNumber != null && pName != null && pPremierPatch != null && pChampionPatch != null && pFont != null)
                {
                    var pricePlayerDetail = pNumber.PriceCNY + pName.PriceCNY;

                    lblPricePlayerDetail.Text = $"<em>{pricePlayerDetail.ToString("f2")}</em>元";

                    // HARD CODE FOR HONGKONG HOME KIT
                    //lblPricePlayerDetailSale.Text = string.Format("<em>{0}</em>元", "122.00");

                    lblPriceArsenalFont.Text = $"<em>{pFont.PriceCNY.ToString("f2")}</em>元";
                    lblPricePremierPatch.Text = $"<em>{pPremierPatch.PriceCNY.ToString("f2")}</em>元/个";
                    lblPriceChampionPatch.Text = $"<em>{pChampionPatch.PriceCNY.ToString("f2")}</em>元/个";
                }
                else
                {
                    throw new Exception("无相关纪念品或缺货，请联系管理员");
                }

                if (OrderID > 0)
                {
                    var o = (OrdrReplicaKit)Order.Select(OrderID);

                    if (o == null || !o.IsActive)
                    {
                        throw new Exception("此订单无效");
                    }

                    // Whether Home or Away ReplicaKit
                    OrderItem oiReplicaKit;

                    if (CurrProductType.Equals(ProductType.ReplicaKitAway))
                    {
                        oiReplicaKit = o.OIReplicaKitAway;
                    }
                    else if (o.OIReplicaKitCup != null && o.OIReplicaKitCup.IsActive)
                    {
                        oiReplicaKit = o.OIReplicaKitCup;
                    }
                    else if (CurrProductType.Equals(ProductType.ReplicaKitHome))
                    {
                        oiReplicaKit = o.OIReplicaKitHome;
                    }
                    else
                    {
                        throw new Exception("此订单未购买球衣商品");
                    }

                    if (ConfigGlobal.IsPluginAdmin(UID) || o.MemberID.Equals(MID))
                    {
                        lblMemberName.Text = $"<b>{o.MemberName}</b> (<em>NO.{o.MemberID}</em>)";

                        var m = _repo.Single<Member>(o.MemberID);

                        if (m == null || !m.IsActive)
                        {
                            throw new Exception("无此会员信息");
                        }

                        lblMemberACNInfo.Text = $"<b>{m.AcnName}</b> (<em>ID.{m.AcnID}</em>)";
                        tbMemberWeChat.Text = m.WeChat;
                    }
                    else
                    {
                        throw new Exception("此订单非当前用户订单");
                    }

                    tbOrderMobile.Text = o.Mobile;
                    tbOrderAddress.Text = o.Address;
                    tbOrderDescription.Text = o.Description;

                    // Set Order Payment Info
                    //if (!string.IsNullOrEmpty(o.Payment))
                    //{
                    //    var payment = o.Payment.Substring(1, o.Payment.Length - 2).Split('|');
                    //    rblOrderPayment.SelectedValue = payment[0];

                    //    if (payment[0].Equals("Bank", StringComparison.OrdinalIgnoreCase))
                    //    {
                    //        tbBankName.Text = payment[1];
                    //        tbBankAccount.Text = payment[2];

                    //        trBank.Style.Add("display", "");
                    //        trAlipay.Style.Add("display", "none");
                    //    }
                    //    else
                    //    {
                    //        tbAlipay.Text = payment[1];

                    //        trBank.Style.Add("display", "none");
                    //        trAlipay.Style.Add("display", "");
                    //    }
                    //}
                    //else
                    //{
                    //    trBank.Style.Add("display", "none");
                    //    trAlipay.Style.Add("display", "none");
                    //}

                    var oiNumber = o.OIPlayerNumber;
                    var oiName = o.OIPlayerName;
                    var oiFont = o.OIArsenalFont;

                    var oiPremierPatch = o.OIPremiershipPatch;
                    var oiChampionPatch = o.OIChampionshipPatch;

                    var p = Product.Cache.Load(oiReplicaKit.ProductGuid);

                    ddlReplicaKit.Items.Insert(0, new ListItem(
                        $"({p.Code}) {p.DisplayName} - ￥{oiReplicaKit.UnitPrice.ToString("f2")}",
                        oiReplicaKit.ProductGuid.ToString()));

                    tbOrderItemSize.Text = oiReplicaKit.Size;
                    hlReplicaKitPage.Visible = false;

                    if (oiNumber != null && oiNumber.IsActive && oiName != null && oiName.IsActive)
                    {
                        if (oiFont != null && oiFont.IsActive)
                        {
                            cbArsenalFont.Checked = true;

                            cbArsenalFont.Text = Product.Cache.Load(oiFont.ProductGuid).DisplayName;
                        }
                        else
                        {
                            cbArsenalFont.Checked = false;
                        }

                        // Set Printing Number and Name

                        Guid playerGuid;

                        if (oiNumber.Remark.Equals(oiName.Remark, StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                playerGuid = new Guid(oiNumber.Remark);
                            }
                            catch
                            {
                                playerGuid = Guid.Empty;
                            }
                        }
                        else
                        {
                            playerGuid = Guid.Empty;
                        }

                        var player = Arsenal_Player.Cache.Load(playerGuid);

                        if (player != null)
                        {
                            ddlPlayerDetail.SelectedValue = player.ID.ToString();
                        }
                        else if (oiNumber.Remark.Equals("custom", StringComparison.OrdinalIgnoreCase)
                                 && oiName.Remark.Equals("custom", StringComparison.OrdinalIgnoreCase))
                        {
                            // Custom Player Number & Name Printing
                            ddlPlayerDetail.SelectedValue = "custom";

                            trCustomPrinting.Style.Add("display", "");
                            tbPlayerNumber.Text = oiNumber.Size;
                            tbPlayerName.Text = oiName.Size;
                        }
                        else
                        {
                            try
                            {
                                player = Arsenal_Player.Cache.PlayerList.Find(ap =>
                                    (ap.PrintingName.Equals(oiName.Size, StringComparison.OrdinalIgnoreCase)
                                     || ap.LastName.Equals(oiName.Size, StringComparison.OrdinalIgnoreCase)
                                     || ap.FirstName.Equals(oiName.Size, StringComparison.OrdinalIgnoreCase))
                                    && ap.SquadNumber.Equals(Convert.ToInt16(oiNumber.Size)));

                                ddlPlayerDetail.SelectedValue = player?.ID.ToString() ?? string.Empty;
                            }
                            catch
                            {
                                ddlPlayerDetail.SelectedValue = string.Empty;
                            }
                        }
                    }
                    else
                    {
                        ddlPlayerDetail.SelectedValue = string.Empty;
                    }

                    if (oiPremierPatch != null && oiPremierPatch.IsActive)
                    {
                        rblPremierPatch.SelectedValue = oiPremierPatch.Quantity.ToString();
                    }
                    else
                    {
                        rblPremierPatch.SelectedValue = "0";
                    }

                    if (oiChampionPatch != null && oiChampionPatch.IsActive)
                    {
                        rblChampionPatch.SelectedValue = oiChampionPatch.Quantity.ToString();
                    }
                    else
                    {
                        rblChampionPatch.SelectedValue = "0";
                    }

                    if (o.Postage > 0)
                    {
                        rblOrderPostage.SelectedValue = o.Postage.ToString("f0");
                    }
                    else
                    {
                        rblOrderPostage.SelectedValue = "0";
                    }
                }
                else
                {
                    //Fill Member draft information into textbox
                    var m = _repo.Single<Member>(MID);

                    tbOrderMobile.Text = m.Mobile;
                    tbMemberWeChat.Text = m.WeChat;
                    //tbAlipay.Text = m.TaobaoName;
                    tbOrderAddress.Text = m.Address;

                    var list = Product.Cache.Load(CurrProductType).FindAll(x => x.IsActive).OrderBy(x => x.Code).ToList();

                    if (list.Count > 0)
                    {
                        ddlReplicaKit.DataSource = list;
                        ddlReplicaKit.DataValueField = "ID";
                        ddlReplicaKit.DataBind();

                        ddlReplicaKit.Items.Insert(0, new ListItem("--请选择球衣类型--", string.Empty));
                    }
                    else
                    {
                        throw new Exception("无相关纪念品或缺货，请联系管理员");
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed",
                    $"alert('{ex.Message}');window.location.href = 'Default.aspx'", true);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            using (var conn = new SqlConnection(DataAccess.ConnectString))
            {
                conn.Open();
                var trans = conn.BeginTransaction();

                try
                {
                    if (string.IsNullOrEmpty(ddlReplicaKit.SelectedValue))
                    {
                        throw new Exception("请选择需要订购的球衣");
                    }

                    var m = _repo.Single<Member>(MID);

                    if (!string.IsNullOrEmpty(tbMemberWeChat.Text.Trim()))
                    {
                        m.Email = tbMemberWeChat.Text.Trim();

                        _repo.Update(m);
                    }
                    else
                    {
                        throw new Exception("请输入会员微信号");
                    }

                    //New Order
                    var o = new Order();
                    int newId;

                    if (OrderID > 0)
                    {
                        o = _repo.Single<Order>(OrderID);
                    }

                    o.Mobile = tbOrderMobile.Text.Trim();
                    o.Address = tbOrderAddress.Text.Trim();

                    //if (rblOrderPayment.SelectedValue.Equals("Bank", StringComparison.OrdinalIgnoreCase))
                    //{
                    //    o.Payment = "{" + rblOrderPayment.SelectedValue + "|" + tbBankName.Text.Trim() + "|" +
                    //                tbBankAccount.Text.Trim() + "}";
                    //}
                    //else
                    //{
                    //    o.Payment = "{" + rblOrderPayment.SelectedValue + "|" + tbAlipay.Text.Trim() + "}";
                    //}

                    o.Payment = string.Empty;

                    o.Postage = Convert.ToSingle(rblOrderPostage.SelectedValue);
                    o.UpdateTime = DateTime.Now;
                    o.Description = tbOrderDescription.Text.Trim();
                    o.OrderType = OrderBaseType.ReplicaKit;

                    if (OrderID > 0)
                    {
                        _repo.Update(o, trans);

                        // used by setting OrderItem foreign key
                        newId = OrderID;
                    }
                    else
                    {
                        o.MemberID = m.ID;
                        o.MemberName = m.Name;

                        o.Price = 0;
                        o.Sale = null;
                        o.Deposit = null;
                        o.Status = OrderStatusType.Draft;
                        o.Rate = 0;
                        o.CreateTime = DateTime.Now;
                        o.IsActive = true;
                        o.Remark = string.Empty;

                        //Get the Order ID after Insert new one
                        object key;
                        _repo.Insert(o, out key, trans);
                        newId = Convert.ToInt32(key);
                    }

                    //Remove Order Item of this Order
                    if (OrderID > 0 && o.ID.Equals(OrderID))
                    {
                        _repo.Query<OrderItem>(x => x.OrderID == OrderID).Delete(trans);
                    }

                    //New Order Item for ReplicaKit
                    var pReplicaKit = Product.Cache.Load(new Guid(ddlReplicaKit.SelectedValue));

                    var oi = new OrdrItmReplicaKit();

                    if (pReplicaKit == null)
                        throw new Exception("无相关纪念品或缺货，请联系管理员");

                    // HARD CORD for HongKong Home Kit
                    //bool isHongKongKit = pReplicaKit.Code.Equals("M74756HK", StringComparison.OrdinalIgnoreCase);
                    //float salePrinting = 61f;

                    oi.OrderID = newId;
                    oi.Size = tbOrderItemSize.Text.Trim().ToUpper();
                    oi.Quantity = 1;
                    oi.Sale = null;
                    oi.Remark = string.Empty;

                    oi.Place(m, pReplicaKit, trans);

                    // New Order Item for Home Player Number & Name
                    if (!string.IsNullOrEmpty(ddlPlayerDetail.SelectedValue))
                    {
                        var pNumber = Product.Cache.Load(ProductType.PlayerNumber).Find(p => p.IsActive);
                        var pName = Product.Cache.Load(ProductType.PlayerName).Find(p => p.IsActive);

                        if (pNumber == null || pName == null)
                            throw new Exception("无印号信息，请联系管理员");

                        var oiFont = new OrdrItmArsenalFont();
                        var oiNumber = new OrdrItmPlayerNumber();
                        var oiName = new OrdrItmPlayerName();

                        oiFont.OrderID = newId;
                        oiFont.Size = string.Empty;
                        oiFont.Quantity = 1;
                        oiFont.Sale = null;
                        oiFont.Remark = string.Empty;

                        oiNumber.OrderID = newId;
                        oiNumber.Quantity = 1;

                        oiName.OrderID = newId;
                        oiName.Quantity = 1;

                        if (ddlPlayerDetail.SelectedValue.Equals("custom"))
                        {
                            // Custom Printing

                            if (string.IsNullOrEmpty(tbPlayerNumber.Text.Trim()) ||
                                string.IsNullOrEmpty(tbPlayerName.Text.Trim()))
                                throw new Exception("请填写自定义印字印号");

                            // New Order Item for Arsenal Font
                            if (cbArsenalFont.Checked)
                            {
                                var pFont = Product.Cache.Load(ProductType.ArsenalFont).Find(p => p.IsActive);

                                if (pFont == null)
                                    throw new Exception("无特殊字体信息，请联系管理员");

                                oiFont.Place(m, trans);

                                oiNumber.Size = tbPlayerNumber.Text.Trim();
                                oiNumber.Sale = 0f;
                                oiNumber.Remark = "custom";
                                oiNumber.Place(m, trans);

                                oiName.Size = tbPlayerName.Text.Trim();
                                oiName.Sale = 0f;
                                oiName.Remark = "custom";
                                oiName.Place(m, trans);
                            }
                            else
                            {
                                oiNumber.Size = tbPlayerNumber.Text.Trim();

                                //if (isHongKongKit)
                                //{ oiNumber.Sale = salePrinting; }
                                //else
                                //{ oiNumber.Sale = null; }
                                oiNumber.Sale = null;

                                oiNumber.Remark = "custom";
                                oiNumber.Place(m, trans);

                                oiName.Size = tbPlayerName.Text.Trim();

                                //if (isHongKongKit)
                                //{ oiName.Sale = salePrinting; }
                                //else
                                //{ oiName.Sale = null; }
                                oiName.Sale = null;

                                oiName.Remark = "custom";
                                oiName.Place(m, trans);
                            }
                        }
                        else
                        {
                            // Arsenal Player Printing
                            var player = Arsenal_Player.Cache.Load(new Guid(ddlPlayerDetail.SelectedValue));

                            if (player == null)
                                throw new Exception("无球员信息，请联系管理员");

                            var printingName = GetArsenalPlayerPrintingName(player);

                            // New Order Item for Arsenal Font
                            if (cbArsenalFont.Checked)
                            {
                                var pFont = Product.Cache.Load(ProductType.ArsenalFont).Find(p => p.IsActive);

                                if (pFont == null)
                                    throw new Exception("无特殊字体信息，请联系管理员");

                                oiFont.Place(m, trans);

                                oiNumber.Size = player.SquadNumber.ToString();
                                oiNumber.Sale = 0f;
                                oiNumber.Remark = player.ID.ToString();
                                oiNumber.Place(m, trans);

                                oiName.Size = printingName;
                                oiName.Sale = 0f;
                                oiName.Remark = player.ID.ToString();
                                oiName.Place(m, trans);
                            }
                            else
                            {
                                oiNumber.Size = player.SquadNumber.ToString();

                                //if (isHongKongKit)
                                //{ oiNumber.Sale = salePrinting; }
                                //else
                                //{ oiNumber.Sale = null; }
                                oiNumber.Sale = null;

                                oiNumber.Remark = player.ID.ToString();
                                oiNumber.Place(m, trans);

                                oiName.Size = printingName;

                                //if (isHongKongKit)
                                //{ oiName.Sale = salePrinting; }
                                //else
                                //{ oiName.Sale = null; }
                                oiName.Sale = null;

                                oiName.Remark = player.ID.ToString();
                                oiName.Place(m, trans);
                            }
                        }
                    }

                    // HARD CODE FOR HongKong Home Kit
                    //if (!isHongKongKit)
                    //{

                    // New Order Item for Premiership Patch
                    if (Convert.ToInt32(rblPremierPatch.SelectedValue) > 0)
                    {
                        var pPremierPatch = Product.Cache.Load(ProductType.PremiershipPatch).Find(p => p.IsActive);

                        if (pPremierPatch == null)
                            throw new Exception("无英超袖标信息，请联系管理员");

                        var oiPremierPatch = new OrdrItmPremiershipPatch
                        {
                            OrderID = newId,
                            Size = string.Empty,
                            Quantity = Convert.ToInt32(rblPremierPatch.SelectedValue),
                            Sale = null,
                            Remark = string.Empty
                        };


                        oiPremierPatch.Place(m, trans);
                    }

                    // New Order Item for Championship Patch
                    if (Convert.ToInt32(rblChampionPatch.SelectedValue) > 0)
                    {
                        var pChampionPatch = Product.Cache.Load(ProductType.ChampionshipPatch).Find(p => p.IsActive);

                        if (pChampionPatch == null)
                            throw new Exception("无欧冠袖标信息，请联系管理员");

                        var oiChampionShipPatch = new OrdrItmChampionshipPatch
                        {
                            OrderID = newId,
                            Size = string.Empty,
                            Quantity = Convert.ToInt32(rblChampionPatch.SelectedValue),
                            Sale = null,
                            Remark = string.Empty
                        };

                        oiChampionShipPatch.Place(m, trans);
                    }

                    //}

                    trans.Commit();

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed",
                        string.Format("alert('订单({0})保存成功');window.location.href = 'ServerOrderView.ashx?OrderID={0}'",
                            newId), true);
                }
                catch (Exception ex)
                {
                    trans.Rollback();

                    ClientScript.RegisterClientScriptBlock(typeof(string), "failed", $"alert('{ex.Message}')", true);
                }

                //conn.Close();
            }
        }

        protected void ddlReplicaKit_DataBound(object sender, EventArgs e)
        {
            var dropDownList = sender as DropDownList;
            if (dropDownList != null)
            {
                foreach (ListItem li in dropDownList.Items)
                {
                    var p = Product.Cache.Load(new Guid(li.Value));

                    li.Text = $"({p.Code}) {p.DisplayName} - ￥{p.PriceCNY.ToString("f2")}";
                }
            }
        }

        protected void ddlPlayerDetail_DataBound(object sender, EventArgs e)
        {
            var dropDownList = sender as DropDownList;
            if (dropDownList != null)
            {
                foreach (ListItem li in dropDownList.Items)
                {
                    var p = Arsenal_Player.Cache.Load(new Guid(li.Value));

                    if (p != null)
                    {
                        li.Text = string.Format("{1} ({0})", p.SquadNumber, GetArsenalPlayerPrintingName(p).ToUpper());
                        //li.Value = string.Format("{0}|{1}", p.SquadNumber.ToString(), p.LastName);
                    }
                }
            }
        }

        private string GetArsenalPlayerPrintingName(ArsenalPlayer ap)
        {
            if (ap != null)
            {
                string strPrintingName;
                if (!string.IsNullOrEmpty(ap.PrintingName))
                {
                    strPrintingName = ap.PrintingName;
                }
                else
                {
                    if (!string.IsNullOrEmpty(ap.LastName))
                    {
                        strPrintingName = ap.LastName;
                    }
                    else if (!string.IsNullOrEmpty(ap.FirstName))
                    {
                        strPrintingName = ap.FirstName;
                    }
                    else
                    {
                        strPrintingName = ap.DisplayName;
                    }
                }

                return strPrintingName;
            }
            return null;
        }
    }
}
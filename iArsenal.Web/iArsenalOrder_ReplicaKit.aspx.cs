using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

using iArsenal.Entity;
using ArsenalPlayer = iArsenal.Entity.Arsenal.Player;

namespace iArsenal.Web
{
    public partial class iArsenalOrder_ReplicaKit : MemberPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                #region Bind ddlPlayerDetail

                try
                {
                    List<ArsenalPlayer> list = Arsenal_Player.Cache.PlayerList.FindAll(p => !p.IsLegend && !p.IsLoan && p.SquadNumber > 0);

                    if (list != null && list.Count > 0)
                    {
                        ddlPlayerDetail.Items.Clear();
                        ddlPlayerDetail.DataSource = list;
                        ddlPlayerDetail.DataValueField = "PlayerGuid";
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

        private ProductType CurrProductType
        {
            get
            {
                ProductType _pt = ProductType.ReplicaKitHome;

                # region Check whether home or away replicakit

                if (OrderID > 0)
                {
                    OrdrReplicaKit o = new OrdrReplicaKit(OrderID);
                    OrderItem oi_ReplicaKit = null;

                    if (o.OIReplicaKitAway != null && o.OIReplicaKitAway.IsActive)
                    {
                        oi_ReplicaKit = (OrdrItemReplicaKitAway)o.OIReplicaKitAway;
                    }
                    else if (o.OIReplicaKitHome != null && o.OIReplicaKitHome.IsActive)
                    {
                        oi_ReplicaKit = (OrdrItmReplicaKitHome)o.OIReplicaKitHome;
                    }
                    else
                    {
                        throw new Exception("此订单未购买球衣商品");
                    }

                    _pt = Product.Cache.Load(oi_ReplicaKit.ProductGuid).ProductType;
                }
                else
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["Type"]) && Request.QueryString["Type"].Equals("Away", StringComparison.OrdinalIgnoreCase))
                    {
                        _pt = ProductType.ReplicaKitAway;
                    }
                    else if (!string.IsNullOrEmpty(Request.QueryString["Type"]) && Request.QueryString["Type"].Equals("Cup", StringComparison.OrdinalIgnoreCase))
                    {
                        _pt = ProductType.ReplicaKitCup;
                    }
                    else
                    {
                        _pt = ProductType.ReplicaKitHome;
                    }
                }
                #endregion

                if (_pt.Equals(ProductType.ReplicaKitAway))
                {
                    Page.Title = "阿森纳2014/15赛季客场PUMA球衣许愿单";
                    hlReplicaKitPage.NavigateUrl = "http://arsenaldirect.arsenal.com/puma-kit/puma-away-kit/icat/pumaaway";
                    ltrlBannerImage.Text = string.Format("<img src=\"uploadfiles/banner/banner20140711.png\" alt=\"{0}\" />", Page.Title);
                }
                else if (_pt.Equals(ProductType.ReplicaKitCup))
                {
                    Page.Title = "阿森纳2014/15赛季杯赛PUMA球衣许愿单";
                    hlReplicaKitPage.NavigateUrl = "http://arsenaldirect.arsenal.com/puma-kit/puma-cup-kit/icat/pumacup";
                    ltrlBannerImage.Text = string.Format("<img src=\"uploadfiles/banner/banner20140815.png\" alt=\"{0}\" />", Page.Title);
                }
                else
                {
                    Page.Title = "阿森纳2014/15赛季主场PUMA球衣许愿单";
                    hlReplicaKitPage.NavigateUrl = "http://arsenaldirect.arsenal.com/puma-kit/puma-home-kit/icat/pumahome";
                    ltrlBannerImage.Text = string.Format("<img src=\"uploadfiles/banner/banner20140710.png\" alt=\"{0}\" />", Page.Title);
                }

                return _pt;
            }
        }

        private void InitForm()
        {
            try
            {
                lblMemberName.Text = string.Format("<b>{0}</b> (<em>NO.{1}</em>)", this.MemberName, this.MID.ToString());
                lblMemberACNInfo.Text = string.Format("<b>{0}</b> (<em>ID.{1}</em>)", this.Username, this.UID.ToString());

                Product pNumber = Product.Cache.Load(ProductType.PlayerNumber).Find(p => p.IsActive);
                Product pName = Product.Cache.Load(ProductType.PlayerName).Find(p => p.IsActive);
                Product pPremierPatch = Product.Cache.Load(ProductType.PremiershipPatch).Find(p => p.IsActive);
                Product pChampionPatch = Product.Cache.Load(ProductType.ChampionshipPatch).Find(p => p.IsActive);
                Product pFont = Product.Cache.Load(ProductType.ArsenalFont).Find(p => p.IsActive);

                if (pNumber != null && pName != null && pPremierPatch != null && pChampionPatch != null && pFont != null)
                {
                    float pricePlayerDetail = pNumber.PriceCNY + pName.PriceCNY;

                    lblPricePlayerDetail.Text = string.Format("<em>{0}</em>元", pricePlayerDetail.ToString("f2"));
                    lblPriceArsenalFont.Text = string.Format("<em>{0}</em>元", pFont.PriceCNY.ToString("f2"));
                    lblPricePremierPatch.Text = string.Format("<em>{0}</em>元/个", pPremierPatch.PriceCNY.ToString("f2"));
                    lblPriceChampionPatch.Text = string.Format("<em>{0}</em>元/个", pChampionPatch.PriceCNY.ToString("f2"));
                }
                else
                {
                    throw new Exception("无相关纪念品或缺货，请联系管理员");
                }

                if (OrderID > 0)
                {
                    //Order o = new Order();
                    //o.OrderID = OrderID;
                    //o.Select();
                    OrdrReplicaKit o = new OrdrReplicaKit(OrderID);

                    // Whether Home or Away ReplicaKit
                    OrderItem oiReplicaKit = null;

                    if (CurrProductType.Equals(ProductType.ReplicaKitAway))
                    {
                        oiReplicaKit = (OrdrItemReplicaKitAway)o.OIReplicaKitAway;
                    }
                    else if (o.OIReplicaKitCup != null && o.OIReplicaKitCup.IsActive)
                    {
                        oiReplicaKit = (OrdrItmReplicaKitCup)o.OIReplicaKitCup;
                    }
                    else if (CurrProductType.Equals(ProductType.ReplicaKitHome))
                    {
                        oiReplicaKit = (OrdrItmReplicaKitHome)o.OIReplicaKitHome;
                    }
                    else
                    {
                        throw new Exception("此订单未购买球衣商品");
                    }

                    if (ConfigAdmin.IsPluginAdmin(UID) && o != null)
                    {
                        lblMemberName.Text = string.Format("<b>{0}</b> (<em>NO.{1}</em>)", o.MemberName, o.MemberID.ToString());

                        Member m = new Member();
                        m.MemberID = o.MemberID;
                        m.Select();

                        if (m == null || !m.IsActive)
                        {
                            throw new Exception("无此会员信息");
                        }
                        else
                        {
                            lblMemberACNInfo.Text = string.Format("<b>{0}</b> (<em>ID.{1}</em>)", m.AcnName, m.AcnID.ToString());
                        }
                    }
                    else
                    {
                        if (o == null || !o.MemberID.Equals(MID) || !o.IsActive)
                            throw new Exception("此订单无效或非当前用户订单");
                    }

                    tbOrderMobile.Text = o.Mobile;
                    tbOrderAddress.Text = o.Address;
                    tbOrderDescription.Text = o.Description;

                    // Set Order Payment Info
                    if (!string.IsNullOrEmpty(o.Payment))
                    {
                        string[] payment = o.Payment.Substring(1, o.Payment.Length - 2).Split('|');
                        rblOrderPayment.SelectedValue = payment[0];

                        if (payment[0].Equals("Bank", StringComparison.OrdinalIgnoreCase))
                        {
                            tbBankName.Text = payment[1];
                            tbBankAccount.Text = payment[2];

                            trBank.Style.Add("display", "");
                            trAlipay.Style.Add("display", "none");
                        }
                        else
                        {
                            tbAlipay.Text = payment[1];

                            trBank.Style.Add("display", "none");
                            trAlipay.Style.Add("display", "");
                        }
                    }
                    else
                    {
                        trBank.Style.Add("display", "none");
                        trAlipay.Style.Add("display", "none");
                    }

                    OrdrItmPlayerNumber oiNumber = o.OIPlayerNumber;
                    OrdrItmPlayerName oiName = o.OIPlayerName;
                    OrdrItmArsenalFont oiFont = o.OIArsenalFont; ;
                    OrdrItmPremiershipPatch oiPremierPatch = o.OIPremiershipPatch;
                    OrdrItmChampionshipPatch oiChampionPatch = o.OIChampionshipPatch;

                    Product p = Product.Cache.Load(oiReplicaKit.ProductGuid);

                    ddlReplicaKit.Items.Insert(0, new ListItem(string.Format("({0}) {1} - 售价{2}元", p.Code, p.DisplayName, oiReplicaKit.UnitPrice.ToString("f2")), oiReplicaKit.ProductGuid.ToString()));
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

                        Guid _playerGuid = Guid.Empty;

                        if (oiNumber.Remark.Equals(oiName.Remark, StringComparison.OrdinalIgnoreCase))
                        {
                            try { _playerGuid = new Guid(oiNumber.Remark); }
                            catch { _playerGuid = Guid.Empty; };
                        }
                        else
                        {
                            _playerGuid = Guid.Empty;
                        }

                        ArsenalPlayer player = Arsenal_Player.Cache.Load(_playerGuid);

                        if (player != null)
                        {
                            ddlPlayerDetail.SelectedValue = player.PlayerGuid.ToString();
                        }
                        else if (oiNumber.Remark.Equals("custom") && oiName.Remark.Equals("custom"))
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

                                if (player != null)
                                    ddlPlayerDetail.SelectedValue = player.PlayerGuid.ToString();
                                else
                                    ddlPlayerDetail.SelectedValue = string.Empty;
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
                    Member m = new Member();
                    m.MemberID = this.MID;
                    m.Select();

                    tbOrderMobile.Text = m.Mobile;
                    tbAlipay.Text = m.TaobaoName;
                    tbOrderAddress.Text = m.Address;

                    List<Product> list = Product.Cache.Load(CurrProductType).FindAll(p => p.IsActive);
                    list.Sort(delegate(Product p1, Product p2) { return string.Compare(p1.Code, p2.Code); });

                    if (list != null && list.Count > 0)
                    {
                        ddlReplicaKit.DataSource = list;
                        ddlReplicaKit.DataValueField = "ProductGuid";
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
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}');window.location.href = 'Default.aspx'", ex.Message.ToString()), true);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = ConfigGlobal.SQLConnectionStrings)
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();

                try
                {
                    if (string.IsNullOrEmpty(ddlReplicaKit.SelectedValue))
                    { throw new Exception("请选择需要订购的球衣"); }

                    Member m = new Member();
                    m.MemberID = this.MID;
                    m.Select();

                    //New Order
                    Order o = new Order();

                    if (OrderID > 0)
                    {
                        o.OrderID = OrderID;
                        o.Select();
                    }

                    o.Mobile = tbOrderMobile.Text.Trim();
                    o.Address = tbOrderAddress.Text.Trim();

                    if (rblOrderPayment.SelectedValue.Equals("Bank", StringComparison.OrdinalIgnoreCase))
                    {
                        o.Payment = "{" + rblOrderPayment.SelectedValue + "|" + tbBankName.Text.Trim() + "|" + tbBankAccount.Text.Trim() + "}";
                    }
                    else
                    {
                        o.Payment = "{" + rblOrderPayment.SelectedValue + "|" + tbAlipay.Text.Trim() + "}";
                    }

                    o.Postage = Convert.ToSingle(rblOrderPostage.SelectedValue);
                    o.UpdateTime = DateTime.Now;
                    o.Description = tbOrderDescription.Text.Trim();

                    if (OrderID > 0)
                    {
                        o.Update(trans);
                    }
                    else
                    {
                        o.MemberID = m.MemberID;
                        o.MemberName = m.Name;

                        o.Price = 0;
                        o.Sale = null;
                        o.Deposit = null;
                        o.Status = OrderStatusType.Draft;
                        o.Rate = 0;
                        o.CreateTime = DateTime.Now;
                        o.IsActive = true;
                        o.Remark = string.Empty;

                        o.Insert(trans);
                        //o.Select();
                    }

                    //Get the Order ID after Insert new one

                    if (o.OrderID > 0)
                    {
                        //Remove Order Item of this Order
                        if (o.OrderID.Equals(OrderID))
                        {
                            int countOrderItem = OrderItem.RemoveOrderItemByOrderID(o.OrderID, trans);
                        }

                        //New Order Item for ReplicaKit
                        Product pReplicaKit = Product.Cache.Load(new Guid(ddlReplicaKit.SelectedValue));

                        OrdrItmReplicaKit oi = new OrdrItmReplicaKit();

                        if (pReplicaKit == null)
                            throw new Exception("无相关纪念品或缺货，请联系管理员");

                        oi.OrderID = o.OrderID;
                        oi.Size = tbOrderItemSize.Text.Trim().ToUpper();
                        oi.Quantity = 1;
                        oi.Sale = null;
                        oi.Remark = string.Empty;

                        oi.Place(m, pReplicaKit, trans);

                        // New Order Item for Home Player Number & Name
                        if (!string.IsNullOrEmpty(ddlPlayerDetail.SelectedValue))
                        {
                            Product pNumber = Product.Cache.Load(ProductType.PlayerNumber).Find(p => p.IsActive);
                            Product pName = Product.Cache.Load(ProductType.PlayerName).Find(p => p.IsActive);

                            if (pNumber == null || pName == null)
                                throw new Exception("无印号信息，请联系管理员");

                            OrdrItmArsenalFont oiFont = new OrdrItmArsenalFont();
                            OrdrItmPlayerNumber oiNumber = new OrdrItmPlayerNumber();
                            OrdrItmPlayerName oiName = new OrdrItmPlayerName();

                            oiFont.OrderID = o.OrderID;
                            oiFont.Size = string.Empty;
                            oiFont.Quantity = 1;
                            oiFont.Sale = null;
                            oiFont.Remark = string.Empty;

                            oiNumber.OrderID = o.OrderID;
                            oiNumber.Quantity = 1;

                            oiName.OrderID = o.OrderID;
                            oiName.Quantity = 1;

                            if (ddlPlayerDetail.SelectedValue.Equals("custom"))
                            {
                                // Custom Printing

                                if (string.IsNullOrEmpty(tbPlayerNumber.Text.Trim()) || string.IsNullOrEmpty(tbPlayerName.Text.Trim()))
                                    throw new Exception("请填写自定义印字印号");

                                // New Order Item for Arsenal Font
                                if (cbArsenalFont.Checked)
                                {
                                    Product pFont = Product.Cache.Load(ProductType.ArsenalFont).Find(p => p.IsActive);

                                    if (pFont == null)
                                        throw new Exception("无特殊字体信息，请联系管理员");

                                    oiFont.Place(m, trans);

                                    oiNumber.Size = tbPlayerNumber.Text.Trim();
                                    oiNumber.Sale = 0f;
                                    oiNumber.Remark = "CUSTOM";
                                    oiNumber.Place(m, trans);

                                    oiName.Size = tbPlayerName.Text.Trim();
                                    oiName.Sale = 0f;
                                    oiName.Remark = "CUSTOM";
                                    oiName.Place(m, trans);
                                }
                                else
                                {
                                    oiNumber.Size = tbPlayerNumber.Text.Trim();
                                    oiNumber.Sale = null;
                                    oiNumber.Remark = "CUSTOM";
                                    oiNumber.Place(m, trans);

                                    oiName.Size = tbPlayerName.Text.Trim();
                                    oiName.Sale = null;
                                    oiName.Remark = "CUSTOM";
                                    oiName.Place(m, trans);
                                }
                            }
                            else
                            {
                                // Arsenal Player Printing
                                ArsenalPlayer player = Arsenal_Player.Cache.Load(new Guid(ddlPlayerDetail.SelectedValue));

                                if (player == null)
                                    throw new Exception("无球员信息，请联系管理员");

                                string _printingName = GetArsenalPlayerPrintingName(player);

                                // New Order Item for Arsenal Font
                                if (cbArsenalFont.Checked)
                                {
                                    Product pFont = Product.Cache.Load(ProductType.ArsenalFont).Find(p => p.IsActive);

                                    if (pFont == null)
                                        throw new Exception("无特殊字体信息，请联系管理员");

                                    oiFont.Place(m, trans);

                                    oiNumber.Size = player.SquadNumber.ToString();
                                    oiNumber.Sale = 0f;
                                    oiNumber.Remark = player.PlayerGuid.ToString();
                                    oiNumber.Place(m, trans);

                                    oiName.Size = _printingName;
                                    oiName.Sale = 0f;
                                    oiName.Remark = player.PlayerGuid.ToString();
                                    oiName.Place(m, trans);
                                }
                                else
                                {
                                    oiNumber.Size = player.SquadNumber.ToString();
                                    oiNumber.Sale = null;
                                    oiNumber.Remark = player.PlayerGuid.ToString();
                                    oiNumber.Place(m, trans);

                                    oiName.Size = _printingName;
                                    oiName.Sale = null;
                                    oiName.Remark = player.PlayerGuid.ToString();
                                    oiName.Place(m, trans);
                                }
                            }
                        }

                        // New Order Item for Premiership Patch
                        if (Convert.ToInt32(rblPremierPatch.SelectedValue) > 0)
                        {
                            Product pPremierPatch = Product.Cache.Load(ProductType.PremiershipPatch).Find(p => p.IsActive);

                            if (pPremierPatch == null)
                                throw new Exception("无英超袖标信息，请联系管理员");

                            OrdrItmPremiershipPatch oiPremierPatch = new OrdrItmPremiershipPatch();

                            oiPremierPatch.OrderID = o.OrderID;
                            oiPremierPatch.Size = string.Empty;
                            oiPremierPatch.Quantity = Convert.ToInt32(rblPremierPatch.SelectedValue);
                            oiPremierPatch.Sale = null;
                            oiPremierPatch.Remark = string.Empty;

                            oiPremierPatch.Place(m, trans);
                        }

                        // New Order Item for Championship Patch
                        if (Convert.ToInt32(rblChampionPatch.SelectedValue) > 0)
                        {
                            Product pChampionPatch = Product.Cache.Load(ProductType.ChampionshipPatch).Find(p => p.IsActive);

                            if (pChampionPatch == null)
                                throw new Exception("无欧冠袖标信息，请联系管理员");

                            OrdrItmChampionshipPatch oiChampionShipPatch = new OrdrItmChampionshipPatch();

                            oiChampionShipPatch.OrderID = o.OrderID;
                            oiChampionShipPatch.Size = string.Empty;
                            oiChampionShipPatch.Quantity = Convert.ToInt32(rblPremierPatch.SelectedValue);
                            oiChampionShipPatch.Sale = null;
                            oiChampionShipPatch.Remark = string.Empty;

                            oiChampionShipPatch.Place(m, trans);
                        }
                    }

                    trans.Commit();

                    //Renew OrderType after Insert OrderItem and transcation commited
                    o.Update();

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", string.Format("alert('订单({0})保存成功');window.location.href = 'iArsenalOrderView_ReplicaKit.aspx?OrderID={0}'", o.OrderID.ToString()), true);
                }
                catch (Exception ex)
                {
                    trans.Rollback();

                    ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}')", ex.Message.ToString()), true);
                }

                conn.Close();
            }
        }

        protected void ddlReplicaKit_DataBound(object sender, EventArgs e)
        {
            foreach (ListItem li in (sender as DropDownList).Items)
            {
                Product p = Product.Cache.Load(new Guid(li.Value));

                li.Text = string.Format("({0}) {1} - 售价 ￥{2}", p.Code, p.DisplayName, p.PriceCNY.ToString("f2"));
            }
        }

        protected void ddlPlayerDetail_DataBound(object sender, EventArgs e)
        {
            foreach (ListItem li in (sender as DropDownList).Items)
            {
                ArsenalPlayer p = Arsenal_Player.Cache.Load(new Guid(li.Value));

                if (p != null)
                {
                    li.Text = string.Format("{1} ({0})", p.SquadNumber.ToString(), GetArsenalPlayerPrintingName(p).ToUpper());
                    //li.Value = string.Format("{0}|{1}", p.SquadNumber.ToString(), p.LastName);
                }
            }
        }

        private string GetArsenalPlayerPrintingName(ArsenalPlayer ap)
        {
            string _strPrintingName = string.Empty;

            if (ap != null)
            {
                if (!string.IsNullOrEmpty(ap.PrintingName))
                {
                    _strPrintingName = ap.PrintingName;
                }
                else
                {
                    if (!string.IsNullOrEmpty(ap.LastName))
                    {
                        _strPrintingName = ap.LastName;
                    }
                    else if (!string.IsNullOrEmpty(ap.FirstName))
                    {
                        _strPrintingName = ap.FirstName;

                    }
                    else
                    {
                        _strPrintingName = ap.DisplayName;
                    }
                }

                return _strPrintingName;
            }
            else
            {
                return null;
            }
        }
    }
}
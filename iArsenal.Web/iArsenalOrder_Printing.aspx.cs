using System;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using Arsenalcn.Core;
using iArsenal.Service;
using ArsenalPlayer = iArsenal.Service.Arsenal.Player;

namespace iArsenal.Web
{
    public partial class iArsenalOrder_Printing : MemberPageBase
    {
        private readonly IRepository _repo = new Repository();

        // 设置印字印号价格常量
        private readonly int _pricePremierLeague = 128;
        private readonly int _priceArsenalFont = 158;

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
                lblMemberName.Text = $"<b>{MemberName}</b> (<em>NO.{Mid}</em>)";
                lblMemberACNInfo.Text = $"<b>{Username}</b> (<em>ID.{Uid}</em>)";

                var pNumber = Product.Cache.Load(ProductType.PlayerNumber).Find(p => p.IsActive);
                var pName = Product.Cache.Load(ProductType.PlayerName).Find(p => p.IsActive);
                //var pPremierPatch = Product.Cache.Load(ProductType.PremiershipPatch).Find(p => p.IsActive);
                //var pChampionPatch = Product.Cache.Load(ProductType.ChampionshipPatch).Find(p => p.IsActive);
                var pFont = Product.Cache.Load(ProductType.ArsenalFont).Find(p => p.IsActive);

                if (pNumber != null && pName != null && pFont != null)
                {
                    //var pricePlayerDetail = pNumber.PriceCNY + pName.PriceCNY;

                    //lblPricePlayerDetail.Text = $"<em>{pricePremierLeague.ToString("f2")}</em>元";

                    //lblPriceArsenalFont.Text = $"<em>{priceArsenalFont.ToString("f2")}</em>元";
                    //lblPricePremierPatch.Text = $"<em>{pPremierPatch.PriceCNY.ToString("f2")}</em>元/个";
                    //lblPriceChampionPatch.Text = $"<em>{pChampionPatch.PriceCNY.ToString("f2")}</em>元/个";
                }
                else
                {
                    throw new Exception("无相关纪念品或缺货，请联系管理员");
                }

                if (OrderID > 0)
                {
                    var o = (OrdrPrinting)Order.Select(OrderID);

                    if (o == null || !o.IsActive)
                    {
                        throw new Exception("此订单无效");
                    }

                    if (ConfigGlobal.IsPluginAdmin(Uid) || o.MemberID.Equals(Mid))
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

                    var oiNumber = o.OIPlayerNumber;
                    var oiName = o.OIPlayerName;
                    var oiFont = o.OIArsenalFont;

                    //var oiPremierPatch = o.OIPremiershipPatch;
                    //var oiChampionPatch = o.OIChampionshipPatch;

                    if (oiNumber != null && oiNumber.IsActive && oiName != null && oiName.IsActive)
                    {
                        if (oiFont != null && oiFont.IsActive)
                        {
                            rblFontSelected.SelectedValue = "ArsenalFont";
                        }
                        else
                        {
                            rblFontSelected.SelectedValue = "PremierFont";
                        }

                        // Set Printing Number and Name

                        var playerGuid = oiNumber.ArsenalPlayerGuid == oiName.ArsenalPlayerGuid ? oiNumber.ArsenalPlayerGuid : Guid.Empty;

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
                            tbPlayerNumber.Text = oiNumber.PrintingNumber;
                            tbPlayerName.Text = oiName.PrintingName;
                        }
                        else
                        {
                            try
                            {
                                player = Arsenal_Player.Cache.PlayerList.Find(ap =>
                                    (ap.PrintingName.Equals(oiName.PrintingName, StringComparison.OrdinalIgnoreCase)
                                     || ap.LastName.Equals(oiName.PrintingName, StringComparison.OrdinalIgnoreCase)
                                     || ap.FirstName.Equals(oiName.PrintingName, StringComparison.OrdinalIgnoreCase))
                                    && ap.SquadNumber.Equals(Convert.ToInt16(oiNumber.PrintingNumber)));

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

                    //if (oiPremierPatch != null && oiPremierPatch.IsActive)
                    //{
                    //    rblPremierPatch.SelectedValue = oiPremierPatch.Quantity.ToString();
                    //}
                    //else
                    //{
                    //    rblPremierPatch.SelectedValue = "0";
                    //}

                    //if (oiChampionPatch != null && oiChampionPatch.IsActive)
                    //{
                    //    rblChampionPatch.SelectedValue = oiChampionPatch.Quantity.ToString();
                    //}
                    //else
                    //{
                    //    rblChampionPatch.SelectedValue = "0";
                    //}
                }
                else
                {
                    //Fill Member draft information into textbox
                    var m = _repo.Single<Member>(Mid);

                    tbOrderMobile.Text = m.Mobile;
                    tbMemberWeChat.Text = m.WeChat;
                    tbOrderAddress.Text = m.Address;
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", $"alert('{ex.Message}');window.location.href = 'Default.aspx'", true);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            using (var conn = new SqlConnection(DapperHelper.ConnectionString))
            {
                conn.Open();
                var trans = conn.BeginTransaction();

                try
                {
                    if (string.IsNullOrEmpty(ddlPlayerDetail.SelectedValue))
                    {
                        throw new Exception("请选择印字印号信息");
                    }

                    var m = _repo.Single<Member>(Mid);

                    if (!string.IsNullOrEmpty(tbMemberWeChat.Text.Trim()))
                    {
                        m.WeChat = tbMemberWeChat.Text.Trim();

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

                    o.Payment = string.Empty;

                    o.Postage = 0;
                    o.UpdateTime = DateTime.Now;
                    o.Description = tbOrderDescription.Text.Trim();
                    o.OrderType = OrderBaseType.Printing;

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

                    // New Order Item for Player Number & Name
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

                        if (ddlPlayerDetail.SelectedValue.Equals("custom", StringComparison.OrdinalIgnoreCase))
                        {
                            // Custom Printing

                            if (string.IsNullOrEmpty(tbPlayerNumber.Text.Trim()) || string.IsNullOrEmpty(tbPlayerName.Text.Trim()))
                            { throw new Exception("请填写自定义印字印号"); }

                            // New Order Item for Arsenal Font
                            if (rblFontSelected.SelectedValue.Equals("ArsenalFont", StringComparison.OrdinalIgnoreCase))
                            {
                                var pFont = Product.Cache.Load(ProductType.ArsenalFont).Find(p => p.IsActive);

                                if (pFont == null)
                                    throw new Exception("无特殊字体信息，请联系管理员");

                                // 设置个性化印字印号价格
                                oiFont.Sale = _priceArsenalFont;
                                oiFont.Place(m, trans);

                                oiNumber.PrintingNumber = tbPlayerNumber.Text.Trim();
                                oiNumber.Sale = 0f;
                                oiNumber.Remark = "custom";
                                oiNumber.Place(m, trans);

                                oiName.PrintingName = tbPlayerName.Text.Trim().ToUpper();
                                oiName.Sale = 0f;
                                oiName.Remark = "custom";
                                oiName.Place(m, trans);
                            }
                            else
                            {
                                oiNumber.PrintingNumber = tbPlayerNumber.Text.Trim();
                                // 设置个性化印字印号价格
                                oiNumber.Sale = _pricePremierLeague / 2;
                                oiNumber.Remark = "custom";
                                oiNumber.Place(m, trans);

                                oiName.PrintingName = tbPlayerName.Text.Trim().ToUpper();
                                // 设置个性化印字印号价格
                                oiName.Sale = _pricePremierLeague / 2;
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

                            var printingName = GetArsenalPlayerPrintingName(player).ToUpper();

                            // New Order Item for Arsenal Font
                            if (rblFontSelected.SelectedValue.Equals("ArsenalFont", StringComparison.OrdinalIgnoreCase))
                            {
                                var pFont = Product.Cache.Load(ProductType.ArsenalFont).Find(p => p.IsActive);

                                if (pFont == null)
                                    throw new Exception("无特殊字体信息，请联系管理员");

                                // 设置个性化印字印号价格
                                oiFont.Sale = _priceArsenalFont;
                                oiFont.Place(m, trans);

                                oiNumber.PrintingNumber = player.SquadNumber.ToString();
                                oiNumber.Sale = 0f;
                                oiNumber.ArsenalPlayerGuid = player.ID;
                                oiNumber.Place(m, trans);

                                oiName.PrintingName = printingName;
                                oiName.Sale = 0f;
                                oiName.ArsenalPlayerGuid = player.ID;
                                oiName.Place(m, trans);
                            }
                            else
                            {
                                oiNumber.PrintingNumber = player.SquadNumber.ToString();
                                // 设置个性化印字印号价格
                                oiNumber.Sale = _pricePremierLeague / 2;
                                oiNumber.ArsenalPlayerGuid = player.ID;
                                oiNumber.Place(m, trans);

                                oiName.PrintingName = printingName;
                                // 设置个性化印字印号价格
                                oiName.Sale = _pricePremierLeague / 2;
                                oiName.ArsenalPlayerGuid = player.ID;
                                oiName.Place(m, trans);
                            }
                        }
                    }

                    //// New Order Item for Premiership Patch
                    //if (Convert.ToInt32(rblPremierPatch.SelectedValue) > 0)
                    //{
                    //    var pPremierPatch = Product.Cache.Load(ProductType.PremiershipPatch).Find(p => p.IsActive);

                    //    if (pPremierPatch == null)
                    //        throw new Exception("无英超袖标信息，请联系管理员");

                    //    var oiPremierPatch = new OrdrItmPremiershipPatch
                    //    {
                    //        OrderID = newId,
                    //        Size = string.Empty,
                    //        Quantity = Convert.ToInt32(rblPremierPatch.SelectedValue),
                    //        Sale = null,
                    //        Remark = string.Empty
                    //    };

                    //    oiPremierPatch.Place(m, trans);
                    //}

                    //// New Order Item for Championship Patch
                    //if (Convert.ToInt32(rblChampionPatch.SelectedValue) > 0)
                    //{
                    //    var pChampionPatch = Product.Cache.Load(ProductType.ChampionshipPatch).Find(p => p.IsActive);

                    //    if (pChampionPatch == null)
                    //        throw new Exception("无欧冠袖标信息，请联系管理员");

                    //    var oiChampionShipPatch = new OrdrItmChampionshipPatch
                    //    {
                    //        OrderID = newId,
                    //        Size = string.Empty,
                    //        Quantity = Convert.ToInt32(rblChampionPatch.SelectedValue),
                    //        Sale = null,
                    //        Remark = string.Empty
                    //    };

                    //    oiChampionShipPatch.Place(m, trans);
                    //}

                    trans.Commit();

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed",
                        string.Format("alert('订单({0})保存成功');window.location.href = 'ServerOrderView.ashx?OrderID={0}'", newId), true);
                }
                catch (Exception ex)
                {
                    trans.Rollback();

                    ClientScript.RegisterClientScriptBlock(typeof(string), "failed", $"alert('{ex.Message}')", true);
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
                    }
                }
            }
        }

        private string GetArsenalPlayerPrintingName(ArsenalPlayer ap)
        {
            var strPrintingName = string.Empty;

            if (ap != null)
            {
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
            }

            return strPrintingName;
        }
    }
}
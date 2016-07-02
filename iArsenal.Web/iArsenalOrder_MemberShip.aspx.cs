using System;
using System.Data.SqlClient;
using Arsenalcn.Core;
using iArsenal.Service;

namespace iArsenal.Web
{
    public partial class iArsenalOrder_Membership : MemberPageBase
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

        private ProductType CurrProductType
        {
            get
            {
                ProductType pt;

                # region Check whether core or premier membership

                if (OrderID > 0)
                {
                    var o = (OrdrMembership)Order.Select(OrderID);

                    OrdrItmMembership oiMembership;

                    if (o.OIMembershipCore != null && o.OIMembershipCore.IsActive)
                    {
                        oiMembership = o.OIMembershipCore;
                    }
                    else if (o.OIMembershipPremier != null && o.OIMembershipPremier.IsActive)
                    {
                        oiMembership = o.OIMembershipPremier;
                    }
                    else
                    {
                        throw new Exception("此订单未登记会籍信息");
                    }

                    pt = Product.Cache.Load(oiMembership.ProductGuid).ProductType;
                }
                else
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["Type"]) &&
                        Request.QueryString["Type"].Equals("Core", StringComparison.OrdinalIgnoreCase))
                    {
                        pt = ProductType.MembershipCore;
                    }
                    else
                    {
                        pt = ProductType.MembershipPremier;
                    }
                }

                #endregion

                if (pt.Equals(ProductType.MembershipCore))
                {
                    Page.Title =
                        $"ACN{CurrSeasonDeadline.AddYears(-1).Year}/{CurrSeasonDeadline.ToString("yy")}赛季普通(Core)会员登记";
                    pnlMemberCore.Visible = true;
                    pnlMemberPremier.Visible = false;
                }
                else
                {
                    Page.Title =
                        $"ACN{CurrSeasonDeadline.AddYears(-1).Year}/{CurrSeasonDeadline.ToString("yy")}赛季高级(Premier)会员登记";
                    pnlMemberCore.Visible = false;
                    pnlMemberPremier.Visible = true;
                }

                return pt;
            }
        }

        private DateTime CurrSeasonDeadline
        {
            get
            {
                // Set Default Deadline yyyy-05-31 23:59:59
                var seasonDeadline = new DateTime(DateTime.Now.Year, 6, 1).AddSeconds(-1);

                if (DateTime.Now >= seasonDeadline)
                {
                    return seasonDeadline.AddYears(1);
                }
                return seasonDeadline;
            }
        }

        private bool IsUpgrade
        {
            get
            {
                var mp = MemberPeriod.GetCurrentMemberPeriodByMemberID(MID);

                if (mp == null) return false;

                CurrentCardNo = mp.MemberCardNo;

                return mp.MemberClass.Equals(MemberClassType.Core) &&
                       CurrProductType.Equals(ProductType.MembershipPremier);
            }
        }

        private bool IsRenew
        {
            get
            {
                var mp = MemberPeriod.GetCurrentMemberPeriodByMemberID(MID, -1);

                if (mp == null) return false;

                CurrentCardNo = mp.MemberCardNo;

                if (mp.MemberClass.Equals(MemberClassType.Core))
                {
                    return CurrProductType.Equals(ProductType.MembershipCore);
                }
                if (mp.MemberClass.Equals(MemberClassType.Premier))
                {
                    return CurrProductType.Equals(ProductType.MembershipPremier);
                }
                return false;
            }
        }

        private string CurrentCardNo { get; set; }

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
                lblMemberACNInfo.Text = $"<b>{Username}</b> (<em>ID.{UID}</em>)";

                if (OrderID > 0)
                {
                    var o = (OrdrMembership)Order.Select(OrderID);

                    if (o == null || !o.IsActive)
                    {
                        throw new Exception("此订单无效");
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

                        #region Set Member Nation & Region

                        if (!string.IsNullOrEmpty(m.Nation))
                        {
                            if (m.Nation.Equals("中国"))
                            {
                                ddlNation.SelectedValue = m.Nation;

                                var region = m.Region.Split('|');
                                if (region.Length > 1)
                                {
                                    tbRegion1.Text = region[0];
                                    tbRegion2.Text = region[1];
                                }
                                else
                                {
                                    tbRegion1.Text = region[0];
                                    tbRegion2.Text = string.Empty;
                                }
                            }
                            else
                            {
                                ddlNation.SelectedValue = "其他";
                                if (m.Nation.Equals("其他"))
                                    tbNation.Text = string.Empty;
                                else
                                    tbNation.Text = m.Nation;
                            }
                        }
                        else
                        {
                            ddlNation.SelectedValue = string.Empty;
                        }

                        #endregion

                        tbIDCardNo.Text = m.IDCardNo;
                        tbPassportNo.Text = m.PassportNo;
                        tbPassportName.Text = m.PassportName;
                        tbMobile.Text = m.Mobile;
                        tbQQ.Text = m.QQ;
                        tbEmail.Text = m.Email;

                        tbOrderDescription.Text = o.Description;
                    }
                    else
                    {
                        throw new Exception("此订单非当前用户订单");
                    }

                    // Whether Core or Premier Membership
                    OrdrItmMembership oiMembership;

                    if (CurrProductType.Equals(ProductType.MembershipCore))
                    {
                        oiMembership = o.OIMembershipCore;
                    }
                    else if (CurrProductType.Equals(ProductType.MembershipPremier))
                    {
                        oiMembership = o.OIMembershipPremier;
                    }
                    else
                    {
                        throw new Exception("此订单未登记会籍信息");
                    }

                    var pMembership = Product.Cache.Load(oiMembership.ProductGuid);

                    if (pMembership != null)
                    {
                        tbMemberClass.Text = ((int)pMembership.ProductType).ToString();
                        lblMemberClass.Text =
                            $"<em>ACN {oiMembership.Season}赛季【{pMembership.DisplayName}】- 售价 {pMembership.PriceInfo}</em>";

                        //tbMemberCardNo.Text = oiMembership.MemberCardNo;

                        lblDatePeriod.Text = $"<em>{DateTime.Now.ToString("yyyy-MM-dd")}</em> ~ <em>{CurrSeasonDeadline.ToString("yyyy-MM-dd")}</em>";
                    }
                    else
                    {
                        throw new Exception("无相关会籍可申请，请联系管理员");
                    }
                }
                else
                {
                    //Fill Member draft information into textbox
                    var m = _repo.Single<Member>(MID);

                    #region Set Member Nation & Region

                    if (!string.IsNullOrEmpty(m.Nation))
                    {
                        if (m.Nation.Equals("中国"))
                        {
                            ddlNation.SelectedValue = m.Nation;

                            var region = m.Region.Split('|');
                            if (region.Length > 1)
                            {
                                tbRegion1.Text = region[0];
                                tbRegion2.Text = region[1];
                            }
                            else
                            {
                                tbRegion1.Text = region[0];
                                tbRegion2.Text = string.Empty;
                            }
                        }
                        else
                        {
                            ddlNation.SelectedValue = "其他";
                            if (m.Nation.Equals("其他"))
                                tbNation.Text = string.Empty;
                            else
                                tbNation.Text = m.Nation;
                        }
                    }
                    else
                    {
                        ddlNation.SelectedValue = string.Empty;
                    }

                    #endregion

                    tbIDCardNo.Text = m.IDCardNo;
                    tbPassportNo.Text = m.PassportNo;
                    tbPassportName.Text = m.PassportName;
                    tbMobile.Text = m.Mobile;
                    tbQQ.Text = m.QQ;
                    tbEmail.Text = m.Email;

                    var pMembership = Product.Cache.Load(CurrProductType).Find(p => p.IsActive);

                    if (pMembership != null)
                    {
                        tbMemberClass.Text = ((int)pMembership.ProductType).ToString();
                        lblMemberClass.Text =
                            $"<em>ACN {CurrSeasonDeadline.AddYears(-1).Year}/{CurrSeasonDeadline.ToString("yy")}赛季【{pMembership.DisplayName}】- 售价 {pMembership.PriceInfo}</em>";

                        lblDatePeriod.Text = $"<em>{DateTime.Now.ToString("yyyy-MM-dd")}</em> ~ <em>{CurrSeasonDeadline.ToString("yyyy-MM-dd")}</em>";
                    }
                    else
                    {
                        throw new Exception("无相关会籍可申请，请联系管理员");
                    }

                    var pPremier = Product.Cache.Load("iMS2");
                    var pCore = Product.Cache.Load("iMS1");

                    if (IsUpgrade)
                    {
                        var sale = pPremier.PriceCNY - pCore.PriceCNY;

                        //tbMemberCardNo.Text = CurrentCardNo;

                        tbSale.Text = sale.ToString("f0");
                        lblSaleInfo.Text =
                            $"您只需支付<em>￥{sale.ToString("f2")}</em>，即可将您的会籍升级为<em>【{pPremier.DisplayName}】</em>";

                        phSaleInfo.Visible = true;
                    }
                    else if (IsRenew)
                    {
                        var sale = CurrProductType.Equals(ProductType.MembershipPremier)
                            ? Convert.ToSingle(Math.Floor(pPremier.PriceCNY * 0.88))
                            : pCore.PriceCNY;

                        //tbMemberCardNo.Text = CurrentCardNo;

                        tbSale.Text = sale.ToString("f0");
                        lblSaleInfo.Text = $"您只需支付<em>￥{sale.ToString("f2")}</em>，即可完成本赛季<em>同等会籍续期</em>";

                        phSaleInfo.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed",
                    $"alert('{ex.Message}');window.location.href = 'iArsenalMemberPeriod.aspx'", true);
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
                    var m = _repo.Single<Member>(MID);

                    // Update Member Information

                    #region Get Member Nation & Region

                    var nation = ddlNation.SelectedValue;

                    if (!string.IsNullOrEmpty(nation))
                    {
                        if (nation.Equals("中国"))
                        {
                            m.Nation = nation;
                            if (!string.IsNullOrEmpty(tbRegion1.Text.Trim()))
                            {
                                if (!string.IsNullOrEmpty(tbRegion2.Text.Trim()))
                                {
                                    m.Region = $"{tbRegion1.Text.Trim()}|{tbRegion2.Text.Trim()}";
                                }
                                else
                                {
                                    m.Region = tbRegion1.Text.Trim();
                                }
                            }
                            else
                            {
                                m.Region = string.Empty;
                            }
                        }
                        else if (!string.IsNullOrEmpty(tbNation.Text.Trim()))
                        {
                            m.Nation = tbNation.Text.Trim();
                            m.Region = string.Empty;
                        }
                        else
                        {
                            m.Nation = nation;
                            m.Region = string.Empty;
                        }
                    }
                    else
                    {
                        m.Nation = string.Empty;
                        m.Region = string.Empty;
                    }

                    #endregion

                    if (!string.IsNullOrEmpty(tbIDCardNo.Text.Trim()))
                        m.IDCardNo = tbIDCardNo.Text.Trim();
                    else
                        throw new Exception("请填写会员身份证号码");

                    if (!string.IsNullOrEmpty(tbPassportNo.Text.Trim()))
                        m.PassportNo = tbPassportNo.Text.Trim();
                    else
                        throw new Exception("请填写会员护照编号");

                    if (!string.IsNullOrEmpty(tbPassportName.Text.Trim()))
                        m.PassportName = tbPassportName.Text.Trim();
                    else
                        throw new Exception("请填写会员护照姓名");

                    if (!string.IsNullOrEmpty(tbMobile.Text.Trim()))
                        m.Mobile = tbMobile.Text.Trim();
                    else
                        throw new Exception("请填写会员手机");

                    if (!string.IsNullOrEmpty(tbQQ.Text.Trim()))
                        m.QQ = tbQQ.Text.Trim();
                    else
                        throw new Exception("请填写会员微信/QQ");

                    if (!string.IsNullOrEmpty(tbEmail.Text.Trim()))
                        m.Email = tbEmail.Text.Trim();
                    else
                        throw new Exception("请填写会员邮箱");

                    //m.MemberType = MemberType.Match;

                    _repo.Update(m);

                    // New Order
                    var o = new Order();
                    int newId;

                    if (OrderID > 0)
                    {
                        o = _repo.Single<Order>(OrderID);
                    }

                    o.Mobile = m.Mobile;
                    o.UpdateTime = DateTime.Now;
                    o.Description = tbOrderDescription.Text.Trim();
                    o.OrderType = OrderBaseType.Membership;

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

                        o.Address = m.Address;
                        o.Payment = string.Empty;

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

                    //New Order Items
                    var oi = new OrdrItmMembership();

                    if (!string.IsNullOrEmpty(tbMemberClass.Text.Trim()))
                    {
                        //Remove Order Item of this Order
                        if (OrderID > 0 && o.ID.Equals(OrderID))
                        {
                            _repo.Query<OrderItem>(x => x.OrderID == OrderID).Delete(trans);
                        }

                        var currProductType = (ProductType)Enum.Parse(typeof(ProductType), tbMemberClass.Text.Trim());
                        var pMembership = Product.Cache.Load(currProductType).Find(p => p.IsActive);

                        if (pMembership == null)
                            throw new Exception("无相关会籍可申请，请联系管理员");

                        // Set AlterMethod
                        if (IsUpgrade)
                        {
                            oi.AlterMethod = "Upgrade";
                        }
                        else if (IsRenew)
                        {
                            oi.AlterMethod = "Renew";
                        }
                        else
                        {
                            oi.AlterMethod = string.Empty;
                        }

                        // Set MemberCardNo
                        if (!string.IsNullOrEmpty(CurrentCardNo))
                        {
                            oi.MemberCardNo = CurrentCardNo;
                        }
                        else
                        {
                            var rand = new Random(Guid.NewGuid().GetHashCode());
                            oi.MemberCardNo = rand.Next(100, 999).ToString();
                        }

                        oi.EndDate = CurrSeasonDeadline;

                        oi.OrderID = newId;
                        oi.Quantity = 1;

                        if (IsUpgrade || IsRenew)
                        {
                            oi.Sale = Convert.ToSingle(tbSale.Text.Trim());
                        }
                        else
                        {
                            oi.Sale = null;
                        }

                        oi.Place(m, pMembership, trans);
                    }
                    else
                    {
                        throw new Exception("此订单未登记会籍信息");
                    }

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
    }
}
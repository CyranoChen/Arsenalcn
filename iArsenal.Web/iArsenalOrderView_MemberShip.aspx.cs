using System;
using System.Globalization;
using System.Linq;
using Arsenalcn.Core;
using iArsenal.Service;

namespace iArsenal.Web
{
    public partial class iArsenalOrderView_Membership : MemberPageBase
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
                lblMemberName.Text = $"<b>{MemberName}</b> (<em>NO.{Mid}</em>)";

                if (OrderID > 0)
                {
                    var o = (OrdrMembership)Order.Select(OrderID);

                    if (ConfigGlobal.IsPluginAdmin(Uid) && o != null)
                    {
                        lblMemberName.Text = $"<b>{o.MemberName}</b> (<em>NO.{o.MemberID}</em>)";

                        // Show the button of Generate Member Period
                        if (o.Status.Equals(OrderStatusType.Confirmed))
                        {
                            btnGenMemberPeriod.Visible = true;
                        }
                    }
                    else
                    {
                        if (o == null || !o.MemberID.Equals(Mid) || !o.IsActive)
                            throw new Exception("此订单无效或非当前用户订单");
                    }

                    #region Bind OrderView Status Workflow

                    if (ucPortalWorkflowInfo != null)
                    {
                        ucPortalWorkflowInfo.JSONOrderStatusList = $"[ {string.Join(",", o.StatusWorkflowInfo)} ]";
                        ucPortalWorkflowInfo.CurrOrderStatus = o.Status;
                    }

                    #endregion

                    var m = _repo.Single<Member>(o.MemberID);

                    lblOrderMobile.Text = $"<em>{o.Mobile}</em>";

                    #region Set Member Nation & Region

                    if (!string.IsNullOrEmpty(m.Nation))
                    {
                        if (m.Nation.Equals("中国"))
                        {
                            lblMemberRegion.Text = "中国 ";

                            var region = m.Region.Split('|');

                            foreach (var t in region)
                            {
                                int regionId;
                                if (int.TryParse(t, out regionId))
                                {
                                    lblMemberRegion.Text += DictionaryItem.Cache.Load(regionId).Name + " ";
                                }
                            }
                        }
                        else
                        {
                            lblMemberRegion.Text = m.Nation;
                        }
                    }
                    else
                    {
                        lblMemberRegion.Text = "无";
                    }

                    #endregion

                    lblMemberIDCardNo.Text = m.IDCardNo;
                    lblMemberPassportNo.Text = m.PassportNo;
                    lblMemberPassportName.Text = m.PassportName;
                    lblMemberWeChat.Text = $"<em>{m.WeChat}</em>";
                    lblMemberEmail.Text = $"<em>{m.Email}</em>";

                    lblOrderID.Text = $"<em>{o.ID}</em>";
                    lblOrderCreateTime.Text = o.CreateTime.ToString("yyyy-MM-dd HH:mm");
                    lblOrderDescription.Text = o.Description;

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

                    // Whether Core or Premier Membership
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

                    var p = Product.Cache.Load(oiMembership.ProductGuid);

                    if (p != null)
                    {
                        lblMemberClass.Text = $"<em>ACN {oiMembership.Season}赛季【{p.DisplayName}】</em>";

                        lblMemberCardNo.Text = $"<em>{oiMembership.MemberCardNo}</em>";
                        lblEndDate.Text = $"<em>{oiMembership.EndDate.ToString("yyyy-MM-dd")}</em>";
                    }
                    else
                    {
                        throw new Exception("无相关会籍可申请，请联系管理员");
                    }

                    var isUpgrade = oiMembership.AlterMethod.Equals("Upgrade", StringComparison.OrdinalIgnoreCase);
                    var isRenew = oiMembership.AlterMethod.Equals("Renew", StringComparison.OrdinalIgnoreCase);

                    // Set Order Price

                    price = oiMembership.TotalPrice;
                    priceInfo = string.Format("<合计> {2}：{0} × {1}", oiMembership.UnitPrice.ToString("f2"),
                        oiMembership.Quantity, Product.Cache.Load(oiMembership.ProductGuid).DisplayName);

                    tbOrderPrice.Text = price.ToString(CultureInfo.CurrentCulture);

                    if (isUpgrade)
                    {
                        lblOrderPrice.Text = $"{priceInfo}：<em>【升级】{price.ToString("f2")}</em>元";
                    }
                    else if (isRenew)
                    {
                        lblOrderPrice.Text = $"{priceInfo}<em>【续期】{price.ToString("f2")}</em>元";
                    }
                    else
                    {
                        lblOrderPrice.Text = $"{priceInfo} = <em>{price.ToString("f2")}</em>元";
                    }

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

                        ucPortalProductQrCode.QrCodeUrl = p.QrCodeUrl;
                        ucPortalProductQrCode.QrCodeProvider = "淘宝";
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
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed",
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

                    if (o == null || !o.MemberID.Equals(Mid) || !o.IsActive)
                        throw new Exception("此订单无效或非当前用户订单");

                    o.Status = OrderStatusType.Submitted;
                    o.UpdateTime = DateTime.Now;
                    o.Price = Convert.ToSingle(tbOrderPrice.Text.Trim());

                    _repo.Update(o);

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed",
                        $"alert('谢谢您的订购，您的订单已经提交成功。\\r\\n请尽快付款以完成订单确认，订单号为：{o.ID}'); window.location.href = window.location.href",
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

        protected void btnModify_Click(object sender, EventArgs e)
        {
            try
            {
                if (OrderID > 0)
                {
                    var o = _repo.Single<Order>(OrderID);

                    if (o == null || !o.MemberID.Equals(Mid) || !o.IsActive)
                        throw new Exception("此订单无效或非当前用户订单");

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed",
                        $"window.location.href = 'iArsenalOrder_Membership.aspx?OrderID={o.ID}'", true);
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

                    if (o == null || !o.MemberID.Equals(Mid) || !o.IsActive)
                        throw new Exception("此订单无效或非当前用户订单");

                    o.IsActive = false;
                    o.UpdateTime = DateTime.Now;
                    o.Price = Convert.ToSingle(tbOrderPrice.Text.Trim());

                    _repo.Update(o);

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed",
                        $"alert('此订单({o.ID})已经取消');window.location.href = 'iArsenalMemberPeriod.aspx'", true);
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

        protected void btnGenMemberPeriod_Click(object sender, EventArgs e)
        {
            using (var trans = DapperHelper.MarsConnection.BeginTransaction())
            {
                try
                {
                    if (OrderID > 0)
                    {
                        var o = (OrdrMembership)Order.Select(OrderID, trans);

                        if (ConfigGlobal.IsPluginAdmin(Uid) && o != null && o.Status.Equals(OrderStatusType.Confirmed))
                        {
                            // Whether Core or Premier Membership
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

                            var p = Product.Cache.Load(oiMembership.ProductGuid);

                            if (p == null)
                            {
                                throw new Exception("无相关会籍可申请，请联系管理员");
                            }

                            // Get all Member Period of current season
                            var list = _repo.Query<MemberPeriod>(x =>
                                x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now)
                                .FindAll(x => x.IsActive);

                            var updateFlag = false;

                            // Valiate the Member Period Information
                            if (list.Any())
                            {
                                if (list.Exists(x =>
                                    x.MemberID.Equals(o.MemberID) && x.MemberName.Equals(o.MemberName)
                                    && p.ProductType.Equals(ProductType.MembershipCore)))
                                {
                                    throw new Exception("此会员当前赛季已经有会籍信息");
                                }
                                if (list.Exists(x =>
                                    x.MemberID.Equals(o.MemberID) && x.MemberName.Equals(o.MemberName)
                                    && x.MemberClass.Equals(MemberClassType.Core))
                                    && p.ProductType.Equals(ProductType.MembershipPremier))
                                {
                                    updateFlag = true;
                                }

                                if (!updateFlag && list.Exists(x => !x.MemberID.Equals(o.MemberID) && 
                                    x.MemberCardNo.Equals(oiMembership.MemberCardNo, StringComparison.OrdinalIgnoreCase)))
                                {
                                    throw new Exception("此会员卡号已被其他会员占用");
                                }
                            }

                            if (updateFlag &&
                                oiMembership.AlterMethod.Equals("Upgrade", StringComparison.OrdinalIgnoreCase))
                            {
                                // Level up the core member to premier for current season
                                var mpCore = list.SingleOrDefault(x =>
                                    x.MemberID.Equals(o.MemberID) && x.MemberName.Equals(o.MemberName)
                                    && x.MemberClass.Equals(MemberClassType.Core));

                                if (mpCore != null)
                                {
                                    mpCore.MemberClass = MemberClassType.Premier;

                                    // not update MemberCardNo of the core member
                                    oiMembership.MemberCardNo = mpCore.MemberCardNo;

                                    mpCore.EndDate = oiMembership.EndDate;

                                    mpCore.Description =
                                        $"Season {oiMembership.Season} 于 {DateTime.Now.ToString("yyyy-MM-dd HH:mm")} 升级为【{mpCore.MemberClass}】会籍，原会籍订单号：{mpCore.OrderID}";

                                    mpCore.OrderID = OrderID;

                                    _repo.Update(mpCore, trans);
                                }
                            }
                            else
                            {
                                // Insert new Member Period for current season
                                var mp = new MemberPeriod();

                                mp.MemberID = o.MemberID;
                                mp.MemberName = o.MemberName;
                                mp.MemberCardNo = oiMembership.MemberCardNo;

                                if (p.ProductType.Equals(ProductType.MembershipCore))
                                {
                                    mp.MemberClass = MemberClassType.Core;
                                }
                                else if (p.ProductType.Equals(ProductType.MembershipPremier))
                                {
                                    mp.MemberClass = MemberClassType.Premier;
                                }
                                else
                                {
                                    throw new Exception("此订单无相关会籍信息");
                                }

                                mp.OrderID = OrderID;
                                mp.StartDate = DateTime.Now;
                                mp.EndDate = oiMembership.EndDate;
                                mp.IsActive = true;
                                mp.Description = $"Season {oiMembership.Season}";
                                mp.Remark = string.Empty;

                                _repo.Insert(mp, trans);
                            }

                            // Update Order Status
                            o.Status = OrderStatusType.Delivered;
                            o.UpdateTime = DateTime.Now;

                            _repo.Update(o, trans);

                            trans.Commit();

                            ClientScript.RegisterClientScriptBlock(typeof(string), "succeed",
                                $"alert('【{p.ProductType}】会籍 (卡号：{oiMembership.MemberCardNo}) 保存成功');window.location.href = window.location.href",
                                true);
                        }
                        else
                        {
                            throw new Exception("此订单无效,请联系管理员");
                        }
                    }
                    else
                    {
                        throw new Exception("此订单无效,请联系管理员");
                    }
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
using System;
using System.Data.SqlClient;
using System.Linq;
using Arsenalcn.Core;
using iArsenal.Service;

namespace iArsenal.Web
{
    public partial class iArsenalOrderView_MemberShip : MemberPageBase
    {
        private readonly IRepository repo = new Repository();

        private int OrderID
        {
            get
            {
                int _orderID;
                if (!string.IsNullOrEmpty(Request.QueryString["OrderID"]) &&
                    int.TryParse(Request.QueryString["OrderID"], out _orderID))
                {
                    return _orderID;
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
                    var o = (OrdrMembership) Order.Select(OrderID);

                    if (ConfigGlobal.IsPluginAdmin(UID) && o != null)
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

                    var m = repo.Single<Member>(o.MemberID);

                    lblOrderMobile.Text = $"<em>{o.Mobile}</em>";

                    #region Set Member Nation & Region

                    if (!string.IsNullOrEmpty(m.Nation))
                    {
                        if (m.Nation.Equals("中国"))
                        {
                            lblMemberRegion.Text = "中国 ";

                            var region = m.Region.Split('|');
                            var _regionID = int.MinValue;

                            for (var i = 0; i < region.Length; i++)
                            {
                                if (int.TryParse(region[i], out _regionID))
                                {
                                    lblMemberRegion.Text += DictionaryItem.Cache.Load(_regionID).Name + " ";
                                }
                                else
                                    continue;
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
                    lblMemberQQ.Text = $"<em>{m.QQ}</em>";
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
                    var price = default(double);
                    var priceInfo = string.Empty;

                    // Whether Core or Premier MemberShip
                    OrdrItmMemberShip oiMemberShip = null;

                    if (o.OIMemberShipCore != null && o.OIMemberShipCore.IsActive)
                    {
                        oiMemberShip = o.OIMemberShipCore;
                    }
                    else if (o.OIMemberShipPremier != null && o.OIMemberShipPremier.IsActive)
                    {
                        oiMemberShip = o.OIMemberShipPremier;
                    }
                    else
                    {
                        throw new Exception("此订单未登记会籍信息");
                    }

                    var p = Product.Cache.Load(oiMemberShip.ProductGuid);

                    if (p != null)
                    {
                        lblMemberClass.Text = $"<em>ACN {oiMemberShip.Season}赛季【{p.DisplayName}】</em>";

                        lblMemberCardNo.Text = $"<em>{oiMemberShip.MemberCardNo}</em>";
                        lblEndDate.Text = $"<em>{oiMemberShip.EndDate.ToString("yyyy-MM-dd")}</em>";
                    }
                    else
                    {
                        throw new Exception("无相关会籍可申请，请联系管理员");
                    }

                    var isUpgrade = oiMemberShip.AlterMethod.Equals("Upgrade", StringComparison.OrdinalIgnoreCase);
                    var isRenew = oiMemberShip.AlterMethod.Equals("Renew", StringComparison.OrdinalIgnoreCase);
                    ;

                    // Set Order Price

                    price = oiMemberShip.TotalPrice;
                    priceInfo = string.Format("<合计> {2}：{0} × {1}", oiMemberShip.UnitPrice.ToString("f2"),
                        oiMemberShip.Quantity, Product.Cache.Load(oiMemberShip.ProductGuid).DisplayName);

                    tbOrderPrice.Text = price.ToString();

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
                    var o = repo.Single<Order>(OrderID);

                    if (o == null || !o.MemberID.Equals(MID) || !o.IsActive)
                        throw new Exception("此订单无效或非当前用户订单");

                    o.Status = OrderStatusType.Submitted;
                    o.UpdateTime = DateTime.Now;
                    o.Price = Convert.ToSingle(tbOrderPrice.Text.Trim());

                    repo.Update(o);

                    ClientScript.RegisterClientScriptBlock(typeof (string), "succeed",
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
                ClientScript.RegisterClientScriptBlock(typeof (string), "failed", $"alert('{ex.Message}');", true);
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

                    ClientScript.RegisterClientScriptBlock(typeof (string), "succeed",
                        $"window.location.href = 'iArsenalOrder_MemberShip.aspx?OrderID={o.ID}'", true);
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
                    var o = repo.Single<Order>(OrderID);

                    if (o == null || !o.MemberID.Equals(MID) || !o.IsActive)
                        throw new Exception("此订单无效或非当前用户订单");

                    o.IsActive = false;
                    o.UpdateTime = DateTime.Now;
                    o.Price = Convert.ToSingle(tbOrderPrice.Text.Trim());

                    repo.Update(o);

                    ClientScript.RegisterClientScriptBlock(typeof (string), "succeed",
                        $"alert('此订单({o.ID})已经取消');window.location.href = 'iArsenalMemberPeriod.aspx'", true);
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

        protected void btnGenMemberPeriod_Click(object sender, EventArgs e)
        {
            using (var conn = new SqlConnection(DataAccess.ConnectString))
            {
                conn.Open();
                var trans = conn.BeginTransaction();

                try
                {
                    if (OrderID > 0)
                    {
                        var o = (OrdrMembership) Order.Select(OrderID);

                        if (ConfigGlobal.IsPluginAdmin(UID) && o != null && o.Status.Equals(OrderStatusType.Confirmed))
                        {
                            // Whether Core or Premier MemberShip
                            OrdrItmMemberShip oiMemberShip = null;

                            if (o.OIMemberShipCore != null && o.OIMemberShipCore.IsActive)
                            {
                                oiMemberShip = o.OIMemberShipCore;
                            }
                            else if (o.OIMemberShipPremier != null && o.OIMemberShipPremier.IsActive)
                            {
                                oiMemberShip = o.OIMemberShipPremier;
                            }
                            else
                            {
                                throw new Exception("此订单未登记会籍信息");
                            }

                            var p = Product.Cache.Load(oiMemberShip.ProductGuid);

                            if (p == null)
                            {
                                throw new Exception("无相关会籍可申请，请联系管理员");
                            }

                            // Get all Member Period of current season
                            var list = repo.Query<MemberPeriod>(x =>
                                x.IsActive && x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now);

                            var _updateFlag = false;

                            // Valiate the Member Period Information
                            if (list != null && list.Count > 0)
                            {
                                if (list.Exists(x =>
                                    x.MemberID.Equals(o.MemberID) && x.MemberName.Equals(o.MemberName)
                                    && p.ProductType.Equals(ProductType.MemberShipCore)))
                                {
                                    throw new Exception("此会员当前赛季已经有会籍信息");
                                }
                                if (list.Exists(x =>
                                    x.MemberID.Equals(o.MemberID) && x.MemberName.Equals(o.MemberName)
                                    && x.MemberClass.Equals(MemberClassType.Core))
                                    && p.ProductType.Equals(ProductType.MemberShipPremier))
                                {
                                    _updateFlag = true;
                                }

                                if (!_updateFlag && list.Exists(x => !x.MemberID.Equals(o.MemberID)
                                                                     &&
                                                                     x.MemberCardNo.Equals(oiMemberShip.MemberCardNo,
                                                                         StringComparison.OrdinalIgnoreCase)))
                                {
                                    throw new Exception("此会员卡号已被其他会员占用");
                                }
                            }

                            if (_updateFlag &&
                                oiMemberShip.AlterMethod.Equals("Upgrade", StringComparison.OrdinalIgnoreCase))
                            {
                                // Level up the core member to premier for current season
                                var mpCore = list.SingleOrDefault(x =>
                                    x.MemberID.Equals(o.MemberID) && x.MemberName.Equals(o.MemberName)
                                    && x.MemberClass.Equals(MemberClassType.Core));

                                mpCore.MemberClass = MemberClassType.Premier;

                                // not update MemberCardNo of the core member
                                oiMemberShip.MemberCardNo = mpCore.MemberCardNo;

                                mpCore.EndDate = oiMemberShip.EndDate;

                                mpCore.Description =
                                    $"Season {oiMemberShip.Season} 于 {DateTime.Now.ToString("yyyy-MM-dd HH:mm")} 升级为【{mpCore.MemberClass}】会籍，原会籍订单号：{mpCore.OrderID}";

                                mpCore.OrderID = OrderID;

                                repo.Update(mpCore, trans);
                            }
                            else
                            {
                                // Insert new Member Period for current season
                                var mp = new MemberPeriod();

                                mp.MemberID = o.MemberID;
                                mp.MemberName = o.MemberName;
                                mp.MemberCardNo = oiMemberShip.MemberCardNo;

                                if (p.ProductType.Equals(ProductType.MemberShipCore))
                                {
                                    mp.MemberClass = MemberClassType.Core;
                                }
                                else if (p.ProductType.Equals(ProductType.MemberShipPremier))
                                {
                                    mp.MemberClass = MemberClassType.Premier;
                                }
                                else
                                {
                                    throw new Exception("此订单无相关会籍信息");
                                }

                                mp.OrderID = OrderID;
                                mp.StartDate = DateTime.Now;
                                mp.EndDate = oiMemberShip.EndDate;
                                mp.IsActive = true;
                                mp.Description = $"Season {oiMemberShip.Season}";
                                mp.Remark = string.Empty;

                                repo.Insert(mp, trans);
                            }

                            // Update Order Status
                            o.Status = OrderStatusType.Delivered;
                            o.UpdateTime = DateTime.Now;

                            repo.Update(o, trans);

                            trans.Commit();

                            ClientScript.RegisterClientScriptBlock(typeof (string), "succeed",
                                $"alert('【{p.ProductType}】会籍 (卡号：{oiMemberShip.MemberCardNo}) 保存成功');window.location.href = window.location.href",
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

                    ClientScript.RegisterClientScriptBlock(typeof (string), "failed", $"alert('{ex.Message}')", true);
                }

                //conn.Close();
            }
        }
    }
}
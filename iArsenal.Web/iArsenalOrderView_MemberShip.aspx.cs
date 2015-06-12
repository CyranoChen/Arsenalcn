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
                    Order_MemberShip o = repo.Single<Order_MemberShip>(OrderID);

                    if (ConfigGlobal.IsPluginAdmin(UID) && o != null)
                    {
                        lblMemberName.Text = string.Format("<b>{0}</b> (<em>NO.{1}</em>)", o.MemberName, o.MemberID.ToString());

                        // Show the button of Generate Member Period
                        if (o.Status.Equals(OrderStatusType.Confirmed))
                        { btnGenMemberPeriod.Visible = true; }
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

                    Member m = repo.Single<Member>(o.MemberID);

                    lblOrderMobile.Text = string.Format("<em>{0}</em>", o.Mobile);

                    #region Set Member Nation & Region
                    if (!string.IsNullOrEmpty(m.Nation))
                    {
                        if (m.Nation.Equals("中国"))
                        {
                            lblMemberRegion.Text = "中国 ";

                            string[] region = m.Region.Split('|');
                            int _regionID = int.MinValue;

                            for (int i = 0; i < region.Length; i++)
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
                    lblMemberQQ.Text = string.Format("<em>{0}</em>", m.QQ);
                    lblMemberEmail.Text = string.Format("<em>{0}</em>", m.Email);

                    lblOrderID.Text = string.Format("<em>{0}</em>", o.ID.ToString());
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
                    float price = 0f;
                    string priceInfo = string.Empty;

                    // Whether Core or Premier MemberShip
                    OrdrItmMemberShip oiMemberShip = null;

                    if (o.OIMemberShipCore != null && o.OIMemberShipCore.IsActive)
                    {
                        oiMemberShip = (OrdrItmMemShipCore)o.OIMemberShipCore;
                    }
                    else if (o.OIMemberShipPremier != null && o.OIMemberShipPremier.IsActive)
                    {
                        oiMemberShip = (OrdrItmMemShipPremier)o.OIMemberShipPremier;
                    }
                    else
                    {
                        throw new Exception("此订单未登记会籍信息");
                    }

                    Product p = Product.Cache.Load(oiMemberShip.ProductGuid);

                    if (p != null)
                    {
                        lblMemberClass.Text = string.Format("<em>ACN {0}赛季【{1}】</em>", oiMemberShip.Season, p.DisplayName);

                        lblMemberCardNo.Text = string.Format("<em>{0}</em>", oiMemberShip.MemberCardNo);
                        lblEndDate.Text = string.Format("<em>{0}</em>", oiMemberShip.EndDate.ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        throw new Exception("无相关会籍可申请，请联系管理员");
                    }

                    bool isUpgrade = oiMemberShip.AlterMethod.Equals("Upgrade", StringComparison.OrdinalIgnoreCase);
                    bool isRenew = oiMemberShip.AlterMethod.Equals("Renew", StringComparison.OrdinalIgnoreCase); ;

                    // Set Order Price

                    price = oiMemberShip.TotalPrice;
                    priceInfo = string.Format("<合计> {2}：{0} × {1}", oiMemberShip.UnitPrice.ToString("f2"), oiMemberShip.Quantity.ToString(), Product.Cache.Load(oiMemberShip.ProductGuid).DisplayName);

                    tbOrderPrice.Text = price.ToString();

                    if (isUpgrade)
                    {
                        lblOrderPrice.Text = string.Format("{0}：<em>【升级】{1}</em>元", priceInfo, price.ToString("f2"));
                    }
                    else if (isRenew)
                    {
                        lblOrderPrice.Text = string.Format("{0}：<em>【续期】{1}</em>元", priceInfo, price.ToString("f2"));
                    }
                    else
                    {
                        lblOrderPrice.Text = string.Format("{0} = <em>{1}</em>元", priceInfo, price.ToString("f2"));
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
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}');window.location.href = 'iArsenalOrder.aspx'", ex.Message.ToString()), true);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (OrderID > 0)
                {
                    Order o = repo.Single<Order>(OrderID);

                    if (o == null || !o.MemberID.Equals(MID) || !o.IsActive)
                        throw new Exception("此订单无效或非当前用户订单");

                    o.Status = OrderStatusType.Submitted;
                    o.UpdateTime = DateTime.Now;
                    o.Price = Convert.ToSingle(tbOrderPrice.Text.Trim());

                    repo.Update(o);

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", string.Format("alert('谢谢您的订购，您的订单已经提交成功。\\r\\n请尽快付款以完成订单确认，订单号为：{0}'); window.location.href = window.location.href", o.ID.ToString()), true);
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

        protected void btnModify_Click(object sender, EventArgs e)
        {
            try
            {
                if (OrderID > 0)
                {
                    Order o = repo.Single<Order>(OrderID);

                    if (o == null || !o.MemberID.Equals(MID) || !o.IsActive)
                        throw new Exception("此订单无效或非当前用户订单");

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", string.Format("window.location.href = 'iArsenalOrder_MemberShip.aspx?OrderID={0}'", o.ID.ToString()), true);
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
                    Order o = repo.Single<Order>(OrderID);

                    if (o == null || !o.MemberID.Equals(MID) || !o.IsActive)
                        throw new Exception("此订单无效或非当前用户订单");

                    o.IsActive = false;
                    o.UpdateTime = DateTime.Now;
                    o.Price = Convert.ToSingle(tbOrderPrice.Text.Trim());

                    repo.Update(o);

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", string.Format("alert('此订单({0})已经取消');window.location.href = 'iArsenalMemberPeriod.aspx'", o.ID.ToString()), true);
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

        protected void btnGenMemberPeriod_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(DataAccess.ConnectString))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();

                try
                {
                    if (OrderID > 0)
                    {
                        Order_MemberShip o = repo.Single<Order_MemberShip>(OrderID);

                        if (ConfigGlobal.IsPluginAdmin(UID) && o != null && o.Status.Equals(OrderStatusType.Confirmed))
                        {
                            // Whether Core or Premier MemberShip
                            OrdrItmMemberShip oiMemberShip = null;

                            if (o.OIMemberShipCore != null && o.OIMemberShipCore.IsActive)
                            {
                                oiMemberShip = (OrdrItmMemShipCore)o.OIMemberShipCore;
                            }
                            else if (o.OIMemberShipPremier != null && o.OIMemberShipPremier.IsActive)
                            {
                                oiMemberShip = (OrdrItmMemShipPremier)o.OIMemberShipPremier;
                            }
                            else
                            {
                                throw new Exception("此订单未登记会籍信息");
                            }

                            Product p = Product.Cache.Load(oiMemberShip.ProductGuid);

                            if (p == null)
                            {
                                throw new Exception("无相关会籍可申请，请联系管理员");
                            }

                            // Get all Member Period of current season
                            var list = repo.Query<MemberPeriod>(x => x.IsActive && x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now).ToList();

                            bool _updateFlag = false;

                            // Valiate the Member Period Information
                            if (list != null && list.Count() > 0)
                            {
                                if (list.Any(x =>
                                    x.MemberID.Equals(o.MemberID) && x.MemberName.Equals(o.MemberName)
                                    && p.ProductType.Equals(ProductType.MemberShipCore)))
                                {
                                    throw new Exception("此会员当前赛季已经有会籍信息");
                                }
                                else if (list.Any(x =>
                                                x.MemberID.Equals(o.MemberID) && x.MemberName.Equals(o.MemberName)
                                                && x.MemberClass.Equals(MemberClassType.Core))
                                            && p.ProductType.Equals(ProductType.MemberShipPremier))
                                {
                                    _updateFlag = true;
                                }

                                if (!_updateFlag && list.Any(x => !x.MemberID.Equals(o.MemberID)
                                    && x.MemberCardNo.Equals(oiMemberShip.MemberCardNo, StringComparison.OrdinalIgnoreCase)))
                                {
                                    throw new Exception("此会员卡号已被其他会员占用");
                                }
                            }

                            if (_updateFlag && oiMemberShip.AlterMethod.Equals("Upgrade", StringComparison.OrdinalIgnoreCase))
                            {
                                // Level up the core member to premier for current season
                                MemberPeriod mpCore = list.SingleOrDefault(x =>
                                    x.MemberID.Equals(o.MemberID) && x.MemberName.Equals(o.MemberName)
                                    && x.MemberClass.Equals(MemberClassType.Core));

                                mpCore.MemberClass = MemberClassType.Premier;

                                // not update MemberCardNo of the core member
                                oiMemberShip.MemberCardNo = mpCore.MemberCardNo;

                                mpCore.EndDate = oiMemberShip.EndDate;

                                mpCore.Description = string.Format("Season {0} 于 {1} 升级为【{2}】会籍，原会籍订单号：{3}", oiMemberShip.Season,
                                    DateTime.Now.ToString("yyyy-MM-dd HH:mm"), mpCore.MemberClass.ToString(), mpCore.OrderID.ToString());

                                mpCore.OrderID = OrderID;

                                repo.Update(mpCore, trans);
                            }
                            else
                            {
                                // Insert new Member Period for current season
                                MemberPeriod mp = new MemberPeriod();

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
                                mp.Description = string.Format("Season {0}", oiMemberShip.Season);
                                mp.Remark = string.Empty;

                                repo.Insert(mp, trans);
                            }

                            // Update Order Status
                            o.Status = OrderStatusType.Delivered;
                            o.UpdateTime = DateTime.Now;

                            repo.Update(o, trans);

                            trans.Commit();

                            ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", string.Format("alert('【{0}】会籍 (卡号：{1}) 保存成功');window.location.href = window.location.href", p.ProductType.ToString(), oiMemberShip.MemberCardNo), true);
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

                    ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}')", ex.Message.ToString()), true);
                }

                //conn.Close();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

using Arsenalcn.Common.Entity;
using iArsenal.Entity;

namespace iArsenal.Web
{
    public partial class iArsenalOrderView_MemberShip : MemberPageBase
    {
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
                    Order_MemberShip o = new Order_MemberShip(OrderID);

                    if (ConfigAdmin.IsPluginAdmin(UID) && o != null)
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

                    Member m = new Member();
                    m.MemberID = o.MemberID;
                    m.Select();

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

                    lblOrderID.Text = string.Format("<em>{0}</em>", o.OrderID.ToString());
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


                    // Set Order Price

                    price = oiMemberShip.TotalPrice;
                    priceInfo = string.Format("<合计> {2}：{0} × {1}", oiMemberShip.UnitPrice.ToString("f2"), oiMemberShip.Quantity.ToString(), Product.Cache.Load(oiMemberShip.ProductGuid).DisplayName);

                    tbOrderPrice.Text = price.ToString();
                    lblOrderPrice.Text = string.Format("{0} = <em>{1}</em>元", priceInfo, price.ToString("f2"));

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
                    Order o = new Order();
                    o.OrderID = OrderID;
                    o.Select();

                    if (o == null || !o.MemberID.Equals(MID) || !o.IsActive)
                        throw new Exception("此订单无效或非当前用户订单");

                    o.Status = OrderStatusType.Submitted;
                    o.UpdateTime = DateTime.Now;
                    o.Price = Convert.ToSingle(tbOrderPrice.Text.Trim());
                    o.Update();

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", string.Format("alert('谢谢您的订购，您的订单已经提交成功。\\r\\n请尽快付款以完成订单确认，订单号为：{0}'); window.location.href = window.location.href", o.OrderID.ToString()), true);
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
                    Order o = new Order();
                    o.OrderID = OrderID;
                    o.Select();

                    if (o == null || !o.MemberID.Equals(MID) || !o.IsActive)
                        throw new Exception("此订单无效或非当前用户订单");

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", string.Format("window.location.href = 'iArsenalOrder_MemberShip.aspx?OrderID={0}'", o.OrderID.ToString()), true);
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
                    Order o = new Order();
                    o.OrderID = OrderID;
                    o.Select();

                    if (o == null || !o.MemberID.Equals(MID) || !o.IsActive)
                        throw new Exception("此订单无效或非当前用户订单");

                    o.IsActive = false;
                    o.UpdateTime = DateTime.Now;
                    o.Price = Convert.ToSingle(tbOrderPrice.Text.Trim());
                    o.Update();

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", string.Format("alert('此订单({0})已经取消');window.location.href = 'iArsenalMemberPeriod.aspx'", o.OrderID.ToString()), true);
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
            using (SqlConnection conn = ConfigGlobal.SQLConnectionStrings)
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();

                try
                {
                    if (OrderID > 0)
                    {
                        Order_MemberShip o = new Order_MemberShip(OrderID);

                        if (ConfigAdmin.IsPluginAdmin(UID) && o != null && o.Status.Equals(OrderStatusType.Confirmed))
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
                            List<MemberPeriod> list = MemberPeriod.GetMemberPeriods().FindAll(mp =>
                                mp.IsActive && mp.StartDate <= DateTime.Now && mp.EndDate >= DateTime.Now);

                            bool _updateFlag = false;

                            // Valiate the Member Period Information
                            if (list != null && list.Count > 0)
                            {
                                if (list.Exists(mp =>
                                    mp.MemberID.Equals(o.MemberID) && mp.MemberName.Equals(o.MemberName)
                                    && p.ProductType.Equals(ProductType.MemberShipCore)))
                                {
                                    throw new Exception("此会员当前赛季已经有会籍信息");
                                }
                                else if (list.Exists(mp =>
                                                mp.MemberID.Equals(o.MemberID) && mp.MemberName.Equals(o.MemberName)
                                                && mp.MemberClass.Equals(MemberClassType.Core))
                                            && p.ProductType.Equals(ProductType.MemberShipPremier))
                                {
                                    _updateFlag = true;
                                }

                                if (!_updateFlag && list.Exists(mp => !mp.MemberID.Equals(o.MemberID)
                                    && mp.MemberCardNo.Equals(oiMemberShip.MemberCardNo, StringComparison.OrdinalIgnoreCase)))
                                {
                                    throw new Exception("此会员卡号已被其他会员占用");
                                }
                            }

                            if (_updateFlag)
                            {
                                // Level up the core member to premier for current season
                                MemberPeriod mp = list.Find(mpCore =>
                                    mpCore.MemberID.Equals(o.MemberID) && mpCore.MemberName.Equals(o.MemberName)
                                    && mpCore.MemberClass.Equals(MemberClassType.Core));

                                mp.MemberClass = MemberClassType.Premier;

                                // not update MemberCardNo of the core member
                                oiMemberShip.MemberCardNo = mp.MemberCardNo;

                                mp.EndDate = oiMemberShip.EndDate;

                                mp.Description = string.Format("Season {0} \\r\\n于 {1} 升级为【{2}】会籍，原会籍订单号：{3}", oiMemberShip.Season,
                                    DateTime.Now.ToString("yyyy-MM-dd HH:mm"), mp.MemberClass.ToString(), mp.OrderID.ToString());

                                mp.OrderID = OrderID;

                                mp.Update(trans);
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

                                mp.Insert(trans);
                            }

                            // Update Order Status
                            o.Status = OrderStatusType.Delivered;
                            o.UpdateTime = DateTime.Now;
                            o.Update(trans);

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

                conn.Close();
            }
        }
    }
}
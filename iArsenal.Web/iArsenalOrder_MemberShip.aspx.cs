using System;
using System.Data.SqlClient;

using iArsenal.Entity;

namespace iArsenal.Web
{
    public partial class iArsenalOrder_MemberShip : PageBase.MemberPageBase
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

        private ProductType CurrProductType
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["Type"]) && Request.QueryString["Type"].Equals("Core", StringComparison.OrdinalIgnoreCase))
                {
                    Page.Title = string.Format("ACN{0}/{1}赛季普通(Core)会员登记", CurrSeasonDeadline.AddYears(-1).Year.ToString(), CurrSeasonDeadline.ToString("yy"));
                    //hlReplicaKitPage.NavigateUrl = "http://arsenaldirect.arsenal.com/puma-kit/puma-away-kit/icat/pumaaway";
                    //ltrlBannerImage.Text = string.Format("<img src=\"uploadfiles/banner/banner20140711.png\" alt=\"{0}\" />", Page.Title); ;

                    return ProductType.MemberShipCore;
                }
                else
                {
                    Page.Title = string.Format("ACN{0}/{1}赛季高级(Premier)会员登记", CurrSeasonDeadline.AddYears(-1).Year.ToString(), CurrSeasonDeadline.ToString("yy"));
                    //hlReplicaKitPage.NavigateUrl = "http://arsenaldirect.arsenal.com/puma-kit/puma-home-kit/icat/pumahome";
                    //ltrlBannerImage.Text = string.Format("<img src=\"uploadfiles/banner/banner20140710.png\" alt=\"{0}\" />", Page.Title); ;

                    return ProductType.MemberShipPremier;
                }
            }
        }

        private DateTime CurrSeasonDeadline
        {
            get
            {
                // Set Default Deadline yyyy-06-30 23:59:59
                DateTime _seasonDeadline = new DateTime(DateTime.Now.Year, 7, 1).AddSeconds(-1);

                if (DateTime.Now >= _seasonDeadline)
                {
                    return _seasonDeadline.AddYears(1);
                }
                else
                {
                    return _seasonDeadline;
                }
            }
        }

        private void InitForm()
        {
            try
            {
                lblMemberName.Text = string.Format("<b>{0}</b> (<em>NO.{1}</em>)", this.MemberName, this.MID.ToString());
                lblMemberACNInfo.Text = string.Format("<b>{0}</b> (<em>ID.{1}</em>)", this.Username, this.UID.ToString());

                if (OrderID > 0)
                {
                    //OrderBase o = new OrderBase();
                    //o.OrderID = OrderID;
                    //o.Select();
                    Order_MemberShip o = new Order_MemberShip(OrderID);

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

                            #region Set Member Nation & Region
                            if (!string.IsNullOrEmpty(m.Nation))
                            {
                                if (m.Nation.Equals("中国"))
                                {
                                    ddlNation.SelectedValue = m.Nation;

                                    string[] region = m.Region.Split('|');
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
                    }
                    else
                    {
                        if (o == null || !o.MemberID.Equals(MID) || !o.IsActive)
                            throw new Exception("此订单无效或非当前用户订单");
                    }

                    // Whether Core or Premier MemberShip
                    OrderItem_MemberShip oiMemberShip = null;

                    if (o.OIMemberShipCore != null && o.OIMemberShipCore.IsActive)
                    {
                        oiMemberShip = (OrderItem_Core)o.OIMemberShipCore;
                    }
                    else if (o.OIMemberShipPremier != null && o.OIMemberShipPremier.IsActive)
                    {
                        oiMemberShip = (OrderItem_Premier)o.OIMemberShipPremier;
                    }
                    else
                    {
                        throw new Exception("此订单未登记会籍信息");
                    }

                    Product p = Product.Cache.Load(oiMemberShip.ProductGuid);

                    if (p != null)
                    {
                        lblMemberClass.Text = string.Format("<em>ACN {0}赛季【{1}】- 售价 {2}</em>",
                            oiMemberShip.Season, p.DisplayName, p.PriceInfo);

                        tbMemberCardNo.Text = oiMemberShip.MemberCardNo;
                        lblEndDate.Text = string.Format("<em>{0}</em>", CurrSeasonDeadline.ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        throw new Exception("无相关会籍可申请，请联系管理员");
                    }
                }
                else
                {
                    //Fill Member draft information into textbox
                    Member m = new Member();
                    m.MemberID = this.MID;
                    m.Select();

                    #region Set Member Nation & Region
                    if (!string.IsNullOrEmpty(m.Nation))
                    {
                        if (m.Nation.Equals("中国"))
                        {
                            ddlNation.SelectedValue = m.Nation;

                            string[] region = m.Region.Split('|');
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

                    Product pMemberShip = Product.Cache.Load(CurrProductType).Find(p => p.IsActive);

                    if (pMemberShip != null)
                    {
                        lblMemberClass.Text = string.Format("<em>ACN {0}/{1}赛季【{2}】- 售价 {3}</em>",
                            CurrSeasonDeadline.AddYears(-1).Year.ToString(), CurrSeasonDeadline.ToString("yy"),
                            pMemberShip.DisplayName, pMemberShip.PriceInfo);

                        lblEndDate.Text = string.Format("<em>{0}</em>", CurrSeasonDeadline.ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        throw new Exception("无相关会籍可申请，请联系管理员");
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}');window.location.href = 'iArsenalMemberPeriod.aspx'", ex.Message.ToString()), true);
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
                    Member m = new Member();
                    m.MemberID = this.MID;
                    m.Select();

                    // Update Member Information

                    #region Get Member Nation & Region
                    string _nation = ddlNation.SelectedValue;

                    if (!string.IsNullOrEmpty(_nation))
                    {
                        if (_nation.Equals("中国"))
                        {
                            m.Nation = _nation;
                            if (!string.IsNullOrEmpty(tbRegion1.Text.Trim()))
                            {
                                if (!string.IsNullOrEmpty(tbRegion2.Text.Trim()))
                                {
                                    m.Region = string.Format("{0}|{1}", tbRegion1.Text.Trim(), tbRegion2.Text.Trim());
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
                            m.Nation = _nation;
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

                    m.MemberType = MemberType.Match;

                    m.Update();

                    // New Order
                    OrderBase o = new OrderBase();

                    if (OrderID > 0)
                    {
                        o.OrderID = OrderID;
                        o.Select();
                    }

                    o.Mobile = m.Mobile;
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

                        o.Insert(trans);
                        //o.Select();
                    }

                    //Get the Order ID after Insert new one

                    if (o.OrderID > 0)
                    {
                        //Remove Order Item of this Order
                        if (o.OrderID.Equals(OrderID))
                        {
                            int countOrderItem = OrderItemBase.RemoveOrderItemByOrderID(o.OrderID);
                        }

                        //New Order Items
                        Product pMembership = Product.Cache.Load(CurrProductType).Find(p => p.IsActive);

                        if (pMembership == null)
                            throw new Exception("无相关会籍可申请，请联系管理员");

                        // Validate Member Card No
                        int _cardNo = 0;
                        if (!string.IsNullOrEmpty(tbMemberCardNo.Text.Trim()) && int.TryParse(tbMemberCardNo.Text.Trim(), out _cardNo))
                        {

                        }
                        else
                        {
                            throw new Exception("请正确填写会员卡号");
                        }

                        OrderItemBase.WishOrderItem(m, pMembership, o, CurrSeasonDeadline.ToString("yyyy-MM-dd"), 1, null, _cardNo.ToString(), trans);
                    }

                    trans.Commit();

                    //Renew OrderType after Insert OrderItem and transcation commited
                    o.Update();

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", string.Format("alert('订单({0})保存成功');window.location.href = 'iArsenalOrderView_MemberShip.aspx?OrderID={0}'", o.OrderID.ToString()), true);
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
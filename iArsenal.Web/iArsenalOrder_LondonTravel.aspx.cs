using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using iArsenal.Entity;

namespace iArsenal.Web
{
    public partial class iArsenalOrder_LondonTravel : PageBase.MemberPageBase
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
                lblMemberACNInfo.Text = string.Format("<b>{0}</b> (<em>ID.{1}</em>)", this.Username, this.UID.ToString());

                Product pETPL = Product.Cache.Load(ProductType.TravelPlan).Find(p => p.IsActive);
                Product pETPA = Product.Cache.Load(ProductType.TravelPartner).Find(p => p.IsActive);

                if (pETPL == null || pETPA == null)
                {
                    throw new Exception("无相关商品信息，请联系管理员");
                }

                if (OrderID > 0)
                {
                    //OrderBase o = new OrderBase();
                    //o.OrderID = OrderID;
                    //o.Select();
                    Order_Travel o = new Order_Travel(OrderID);

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

                    OrderItemBase oiETPL = o.OITravelPlan;
                    OrderItemBase oiETPA = o.OITravelPartner;

                    if (oiETPL != null && oiETPL.IsActive)
                    {
                        // Set Order Travel Date

                        if (!string.IsNullOrEmpty(oiETPL.Size))
                        {
                            tbFromDate.Text = DateTime.Parse(oiETPL.Size.Split('|')[0]).ToString("yyyy-MM-dd");
                            tbToDate.Text = DateTime.Parse(oiETPL.Size.Split('|')[1]).ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            tbFromDate.Text = "2013-10-21";
                            tbToDate.Text = "2013-11-15";
                        }

                        // Set Order Travel Option

                        if (!string.IsNullOrEmpty(oiETPL.Remark))
                        {
                            string[] _strTravelOption = oiETPL.Remark.ToUpper().Split('|');

                            for (int j = 0; j < cblTravelOption.Items.Count; j++)
                            {
                                cblTravelOption.Items[j].Selected = false;
                            }

                            for (int i = 0; i < _strTravelOption.Length; i++)
                            {
                                cblTravelOption.Items.FindByValue(_strTravelOption[i]).Selected = true;
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("此订单未填写观赛信息");
                    }


                    if (oiETPA != null && oiETPA.IsActive && !string.IsNullOrEmpty(oiETPA.Remark))
                    {
                        cbPartner.Checked = true;

                        // Partner JSON Schema: {  "Name": "Cyrano",  "Relation": "1", "Gender": "0", "IDCardNo": "310101XXXX", "PassportNo": "", "PassportName","" }
                        // jsonSerializer.Deserialize String Partner
                        JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                        Partner pa = jsonSerializer.Deserialize<Partner>(oiETPA.Remark);

                        if (pa != null)
                        {
                            tbPartnerName.Text = pa.Name;
                            ddlPartnerRelation.SelectedValue = pa.Relation.ToString();
                            rblPartnerGender.SelectedValue = pa.Gender.ToString().ToLower();
                            tbPartnerIDCardNo.Text = pa.IDCardNo;
                            tbPartnerPassportNo.Text = pa.PassportNo;
                            tbPartnerPassportName.Text = pa.PassportName;

                            cbPartner.Checked = true;
                        }
                        else
                        {
                            cbPartner.Checked = false;
                        }
                    }
                    else
                    {
                        cbPartner.Checked = false;
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
                        throw new Exception("请填写会员QQ");

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
                        Product pETPL = Product.Cache.Load(ProductType.TravelPlan).Find(p => p.IsActive);
                        Product pETPA = Product.Cache.Load(ProductType.TravelPartner).Find(p => p.IsActive);

                        if (pETPL == null || pETPA == null)
                            throw new Exception("无观赛信息，请联系管理员");

                        // Partner JSON Schema: {  "Name": "Cyrano",  "Relation": "1", "Gender": "0", "IDCardNo": "310101XXXX", "PassportNo": "", "PassportName","" }
                        // jsonSerializer.Serialize Object Partner
                        string _strPartner = string.Empty;

                        if (cbPartner.Checked)
                        {
                            Partner p = new Partner();

                            if (!string.IsNullOrEmpty(tbPartnerName.Text.Trim()))
                                p.Name = tbPartnerName.Text.Trim();
                            else
                                throw new Exception("请填写同伴姓名");

                            if (!string.IsNullOrEmpty(ddlPartnerRelation.SelectedValue))
                                p.Relation = int.Parse(ddlPartnerRelation.SelectedValue);
                            else
                                throw new Exception("请选择同伴关系");

                            if (!string.IsNullOrEmpty(rblPartnerGender.SelectedValue))
                                p.Gender = bool.Parse(rblPartnerGender.SelectedValue);
                            else
                                p.Gender = true;

                            if (!string.IsNullOrEmpty(tbPartnerIDCardNo.Text.Trim()))
                                p.IDCardNo = tbPartnerIDCardNo.Text.Trim();
                            else
                                throw new Exception("请填写同伴身份证");

                            if (!string.IsNullOrEmpty(tbPartnerPassportNo.Text.Trim()))
                                p.PassportNo = tbPartnerPassportNo.Text.Trim();
                            else
                                throw new Exception("请填写同伴护照号码");

                            if (!string.IsNullOrEmpty(tbPartnerPassportName.Text.Trim()))
                                p.PassportName = tbPartnerPassportName.Text.Trim();
                            else
                                throw new Exception("请填写同伴护照姓名");

                            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                            _strPartner = jsonSerializer.Serialize(p);

                            OrderItemBase.WishOrderItem(m, pETPA, o, string.Empty, 1, null, _strPartner, trans);
                        }

                        // Genernate Travel Date
                        string _strTravelDate = string.Empty;

                        if (!string.IsNullOrEmpty(tbFromDate.Text.Trim()) && !string.IsNullOrEmpty(tbToDate.Text.Trim()))
                        {
                            DateTime _fDate = DateTime.Parse(tbFromDate.Text.Trim());
                            DateTime _tDate = DateTime.Parse(tbToDate.Text.Trim());

                            _strTravelDate = string.Format("{0}|{1}", _fDate.ToString("yyyy-MM-dd"), _tDate.ToString("yyyy-MM-dd"));
                        }
                        else
                        {
                            throw new Exception("请填写推荐出行时间");
                        }

                        // Generate Travel Option
                        string _strTravelOption = string.Empty;

                        for (int i = 0; i < cblTravelOption.Items.Count; i++)
                        {
                            if (cblTravelOption.Items[i].Selected)
                            {
                                _strTravelOption += string.Format("{0}|", cblTravelOption.Items[i].Value);
                            }
                        }

                        if (!string.IsNullOrEmpty(_strTravelOption))
                        {
                            _strTravelOption = _strTravelOption.Substring(0, _strTravelOption.Length - 1);
                        }

                        OrderItemBase.WishOrderItem(m, pETPL, o, _strTravelDate, 1, null, _strTravelOption, trans);
                    }

                    trans.Commit();

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", string.Format("alert('订单({0})保存成功');window.location.href = 'iArsenalOrderView_LondonTravel.aspx?OrderID={0}'", o.OrderID.ToString()), true);
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
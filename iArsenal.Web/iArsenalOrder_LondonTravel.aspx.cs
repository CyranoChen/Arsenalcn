using System;
using System.Collections.Generic;
using System.Data.SqlClient;

using Arsenalcn.Core;
using iArsenal.Service;

namespace iArsenal.Web
{
    public partial class iArsenalOrder_LondonTravel : MemberPageBase
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
                lblMemberACNInfo.Text = string.Format("<b>{0}</b> (<em>ID.{1}</em>)", this.Username, this.UID.ToString());

                Product pETPL = Product.Cache.Load("iETPL");
                Product pETPA = Product.Cache.Load("iETPA");

                if (pETPL == null || pETPA == null)
                {
                    throw new Exception("无相关商品信息，请联系管理员");
                }

                if (OrderID > 0)
                {
                    var o = (OrdrTravel)Order.Select(OrderID);

                    if (o == null || !o.IsActive) { throw new Exception("此订单无效"); }

                    if (ConfigGlobal.IsPluginAdmin(UID) || o.MemberID.Equals(MID))
                    {
                        lblMemberName.Text = string.Format("<b>{0}</b> (<em>NO.{1}</em>)", o.MemberName, o.MemberID.ToString());

                        Member m = repo.Single<Member>(o.MemberID);

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
                        throw new Exception("此订单非当前用户订单");
                    }

                    var oiTP = o.OITravelPlan.MapTo<OrdrItmTravelPlanLondon>();
                    oiTP.Init();

                    List<OrdrItmTravelPartner> listPartner = o.OITravelPartnerList.FindAll(oi =>
                        oi.IsActive && !string.IsNullOrEmpty(oi.Remark));

                    if (oiTP != null && oiTP.IsActive)
                    {
                        // Set Order Travel Date
                        tbFromDate.Text = oiTP.TravelFromDate.ToString("yyyy-MM-dd");
                        tbToDate.Text = oiTP.TravelToDate.ToString("yyyy-MM-dd");

                        // Set Order Travel Option
                        for (int j = 0; j < cblTravelOption.Items.Count; j++)
                        {
                            cblTravelOption.Items[j].Selected = false;
                        }

                        if (oiTP.TravelOption != null && oiTP.TravelOption.Length > 0)
                        {
                            for (int i = 0; i < oiTP.TravelOption.Length; i++)
                            {
                                cblTravelOption.Items.FindByValue(oiTP.TravelOption[i]).Selected = true;
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("此订单未填写观赛信息");
                    }

                    if (listPartner != null && listPartner.Count > 0)
                    {
                        cbPartner.Checked = true;

                        Partner pa = listPartner[0].Partner;

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
                    Member m = repo.Single<Member>(this.MID);

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
            using (SqlConnection conn = new SqlConnection(DataAccess.ConnectString))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();

                try
                {
                    Member m = repo.Single<Member>(this.MID);

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

                    repo.Update(m);

                    // New Order
                    Order o = new Order();
                    int _newID = int.MinValue;

                    if (OrderID > 0)
                    {
                        o = repo.Single<Order>(OrderID);
                    }

                    o.Mobile = m.Mobile;
                    o.UpdateTime = DateTime.Now;
                    o.Description = tbOrderDescription.Text.Trim();
                    o.OrderType = OrderBaseType.Travel;

                    if (OrderID > 0)
                    {
                        repo.Update(o, trans);

                        // used by setting OrderItem foreign key
                        _newID = OrderID;
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
                        object _key = null;
                        repo.Insert<Order>(o, out _key, trans);
                        _newID = Convert.ToInt32(_key);
                    }

                    //Remove Order Item of this Order
                    if (OrderID > 0 && o.ID.Equals(OrderID))
                    {
                        int _count = 0;
                        repo.Delete<OrderItem>(x => x.OrderID.Equals(OrderID), out _count, trans);
                    }

                    //New Order Items
                    //Product pETPL = Product.Cache.Load("iETPL");
                    Product pETPA = Product.Cache.Load("iETPA");

                    if (pETPA == null)
                        throw new Exception("无观赛信息，请联系管理员");

                    // Get Partner Information to Serialize Json

                    if (cbPartner.Checked)
                    {
                        OrdrItmTravelPartner oiPartner = new OrdrItmTravelPartner();

                        Partner pa = new Partner();

                        if (!string.IsNullOrEmpty(tbPartnerName.Text.Trim()))
                            pa.Name = tbPartnerName.Text.Trim();
                        else
                            throw new Exception("请填写同伴姓名");

                        if (!string.IsNullOrEmpty(ddlPartnerRelation.SelectedValue))
                            pa.Relation = int.Parse(ddlPartnerRelation.SelectedValue);
                        else
                            throw new Exception("请选择同伴关系");

                        if (!string.IsNullOrEmpty(rblPartnerGender.SelectedValue))
                            pa.Gender = bool.Parse(rblPartnerGender.SelectedValue);
                        else
                            pa.Gender = true;

                        if (!string.IsNullOrEmpty(tbPartnerIDCardNo.Text.Trim()))
                            pa.IDCardNo = tbPartnerIDCardNo.Text.Trim();
                        else
                            throw new Exception("请填写同伴身份证");

                        if (!string.IsNullOrEmpty(tbPartnerPassportNo.Text.Trim()))
                            pa.PassportNo = tbPartnerPassportNo.Text.Trim();
                        else
                            throw new Exception("请填写同伴护照号码");

                        if (!string.IsNullOrEmpty(tbPartnerPassportName.Text.Trim()))
                            pa.PassportName = tbPartnerPassportName.Text.Trim();
                        else
                            throw new Exception("请填写同伴护照姓名");

                        oiPartner.Partner = pa;

                        oiPartner.OrderID = _newID;
                        oiPartner.Size = string.Empty;
                        oiPartner.Quantity = 1;
                        oiPartner.Sale = null;

                        oiPartner.Place(m, pETPA, trans);
                    }

                    // Generate OrderItemTravelPlan
                    OrdrItmTravelPlanLondon oiPlan = new OrdrItmTravelPlanLondon();

                    // Genernate Travel Date
                    if (!string.IsNullOrEmpty(tbFromDate.Text.Trim()) && !string.IsNullOrEmpty(tbToDate.Text.Trim()))
                    {
                        oiPlan.TravelFromDate = DateTime.Parse(tbFromDate.Text.Trim());
                        oiPlan.TravelToDate = DateTime.Parse(tbToDate.Text.Trim());
                    }
                    else
                    {
                        throw new Exception("请填写推荐出行时间");
                    }

                    // Generate Travel Option
                    //string _strTravelOption = string.Empty;

                    List<string> listTravelOption = new List<string>();

                    for (int i = 0; i < cblTravelOption.Items.Count; i++)
                    {
                        if (cblTravelOption.Items[i].Selected)
                        {
                            listTravelOption.Add(cblTravelOption.Items[i].Value);
                        }
                    }

                    oiPlan.TravelOption = listTravelOption.Count > 0 ? listTravelOption.ToArray() : null;

                    oiPlan.OrderID = _newID;
                    oiPlan.Quantity = 1;
                    oiPlan.Sale = null;

                    oiPlan.Place(m, trans);

                    trans.Commit();

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", string.Format("alert('订单({0})保存成功');window.location.href = 'ServerOrderView.ashx?OrderID={0}'", _newID.ToString()), true);
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
using System;
using System.Collections.Generic;

using Arsenalcn.Core;
using iArsenal.Service;

namespace iArsenal.Web
{
    public partial class iArsenalOrderView_AsiaTrophy2015 : MemberPageBase
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
                    OrdrTravel o = repo.Single<OrdrTravel>(OrderID);

                    if (ConfigGlobal.IsPluginAdmin(UID) && o != null)
                    {
                        lblMemberName.Text = string.Format("<b>{0}</b> (<em>NO.{1}</em>)", o.MemberName, o.MemberID.ToString());
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

                    OrdrItmTravelPlan2015AsiaTrophy oiAsiaTrophy = new OrdrItmTravelPlan2015AsiaTrophy();
                    oiAsiaTrophy.Mapper(o.OITravelPlan);

                    if (oiAsiaTrophy.IsActive)
                    {
                        // Set Order Match Option
                        string _strMatchInfo = string.Empty;

                        if (oiAsiaTrophy.TravelOption.MatchOption.Equals(MatchOption.First))
                        {
                            _strMatchInfo = "7月15日 阿森纳 vs ?待定";
                        }
                        else if (oiAsiaTrophy.TravelOption.MatchOption.Equals(MatchOption.Second))
                        {
                            _strMatchInfo = "7月18日 阿森纳 vs ?待定";
                        }
                        else
                        {
                            _strMatchInfo = "7月15日 阿森纳 vs ?待定 + 7月18日 阿森纳 vs ?待定";
                        }

                        lblOrderItem_TravelInfo.Text = string.Format("<em>{0}：</em>{1}",
                            oiAsiaTrophy.IsTicketOnly ? "仅购票" : "观赛团", _strMatchInfo);

                        // Set Order Travel Option
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();

                        if (oiAsiaTrophy.TravelOption.IsVisa) { sb.Append("代办签证 "); }
                        if (oiAsiaTrophy.TravelOption.IsFlight) { sb.Append("| 购买机票 "); }
                        if (oiAsiaTrophy.TravelOption.IsHotel) { sb.Append("| 预订宾馆 "); }
                        if (oiAsiaTrophy.TravelOption.IsTraining) { sb.Append("| 训练课 "); }
                        if (oiAsiaTrophy.TravelOption.IsParty) { sb.Append("| 球员见面会 "); };
                        if (oiAsiaTrophy.TravelOption.IsSingapore) { sb.Append("| 当地团"); };

                        lblOrderItem_TravelOption.Text = sb.ToString();
                    }
                    else
                    {
                        throw new Exception("此订单未有正确观赛信息");
                    }

                    // Set Travel Partner
                    List<OrdrItmTravelPartner> listPartner = o.OITravelPartnerList.FindAll(oi =>
                        oi.IsActive && oi.Partner != null);

                    if (listPartner != null && listPartner.Count > 0)
                    {
                        OrdrItmTravelPartner oiPartner = listPartner[0];
                        Partner pa = oiPartner.Partner;

                        if (pa != null)
                        {
                            string _strParterRelation = "（{0}）";
                            if (pa.Relation.Equals(1))
                            {
                                _strParterRelation = string.Format(_strParterRelation, "亲属");
                            }
                            else if (pa.Relation.Equals(2))
                            {
                                _strParterRelation = string.Format(_strParterRelation, "朋友");
                            }
                            else
                            {
                                _strParterRelation = string.Empty;
                            }

                            lblOrderItem_TravelPartner.Text = string.Format("<em>{0}</em>{5}，{1}，{2}；护照：（{3}）{4}",
                                pa.Name, pa.Gender ? "男" : "女", pa.IDCardNo, pa.PassportNo, pa.PassportName, _strParterRelation);
                        }

                        phOrderPartner.Visible = true;
                    }
                    else
                    {
                        phOrderPartner.Visible = false;
                    }

                    // Set Travel Price

                    if (listPartner != null && listPartner.Count > 0)
                    {
                        OrdrItmTravelPartner oiPartner = listPartner[0];

                        price = oiPartner.TotalPrice + oiAsiaTrophy.TotalPrice;
                        priceInfo = string.Format("观赛团预订定金：{0}+ 同伴定金：{1} = <em>{2}</em>元 (CNY)",
                            oiAsiaTrophy.TotalPrice.ToString("f0"), oiPartner.TotalPrice.ToString("f0"), price.ToString("f2"));

                        phOrderPrice.Visible = true;
                    }
                    else
                    {
                        price = oiAsiaTrophy.TotalPrice;
                        priceInfo = string.Format("观赛团预订定金：<em>{0}</em>元 (CNY)", price.ToString("f2"));

                        phOrderPrice.Visible = true;
                    }

                    tbOrderPrice.Text = price.ToString();
                    lblOrderPrice.Text = priceInfo;

                    if (o.Status.Equals(OrderStatusType.Draft))
                    {
                        btnSubmit.Visible = true;
                        btnModify.Visible = true;
                        btnCancel.Visible = true;

                        phOrderPrice.Visible = false;
                    }
                    else if (o.Status.Equals(OrderStatusType.Submitted))
                    {
                        btnSubmit.Visible = false;
                        btnModify.Visible = false;
                        btnCancel.Visible = true;

                        phOrderPrice.Visible = false;
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

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", string.Format("alert('谢谢您的预订报名，您的订单已经提交成功。\\r\\n请耐心等待审核，并由观赛组织人会与您联系。\\r\\n订单号为：{0}'); window.location.href = window.location.href", o.ID.ToString()), true);
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

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", string.Format("window.location.href = 'iArsenalOrder_AsiaTrophy2015.aspx?OrderID={0}'", o.ID.ToString()), true);
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

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", string.Format("alert('此订单({0})已经取消');window.location.href = 'iArsenalOrder.aspx'", o.ID.ToString()), true);
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
    }
}
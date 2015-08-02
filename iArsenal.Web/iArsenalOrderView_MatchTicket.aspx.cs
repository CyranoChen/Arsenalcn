using System;

using Arsenalcn.Core;
using iArsenal.Service;

namespace iArsenal.Web
{
    public partial class iArsenalOrderView_MatchTicket : MemberPageBase
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

                bool _isMemberCouldPurchase = true;

                if (OrderID > 0)
                {
                    var o = (OrdrTicket)Order.Select(OrderID);

                    // For Vincent Song to View the MatchTickets Confirmation Page
                    if (ConfigGlobal.IsPluginAdmin(UID) || (UID.Equals(33067) && (int)o.Status >= 3) && o != null)
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
                    double price = default(double);
                    string priceInfo = string.Empty;

                    OrdrItmMatchTicket oiMatchTicket = o.OIMatchTicket;

                    // Get Order MatchTicket Info

                    if (oiMatchTicket != null && oiMatchTicket.IsActive)
                    {
                        if (oiMatchTicket.MatchGuid != null)
                        {
                            MatchTicket mt = MatchTicket.Cache.Load(oiMatchTicket.MatchGuid);

                            if (mt == null)
                            {
                                throw new Exception("无相关比赛信息，请联系管理员");
                            }

                            var mp = MemberPeriod.GetCurrentMemberPeriodByMemberID(this.MID);

                            _isMemberCouldPurchase = mt.CheckMemberCanPurchase(mp);

                            Product p = Product.Cache.Load(mt.ProductCode);

                            if (p == null)
                            {
                                throw new Exception("无相关商品信息，请联系管理员");
                            }
                            else
                            {
                                lblMatchTicketInfo.Text = string.Format("<em>【{0}】{1}({2})</em>", mt.LeagueName, mt.TeamName, Arsenal_Team.Cache.Load(mt.TeamGuid).TeamEnglishName);
                                lblMatchTicketPlayTime.Text = string.Format("<em>【伦敦】{0}</em>", mt.PlayTimeLocal.ToString("yyyy-MM-dd HH:mm"));

                                string _strRank = mt.ProductInfo.Trim();
                                if (lblMatchTicketRank != null && !string.IsNullOrEmpty(_strRank))
                                {
                                    lblMatchTicketRank.Text = string.Format("<em>{0}</em>", _strRank.Substring(_strRank.Length - 7, 7));
                                }
                                else
                                {
                                    lblMatchTicketRank.Text = string.Empty;
                                }

                                if (mt.AllowMemberClass.HasValue && mt.AllowMemberClass.Value == 2)
                                {
                                    lblMatchTicketAllowMemberClass.Text = "<em>只限高级会员(Premier)</em>";
                                }
                                else if (mt.AllowMemberClass.HasValue && mt.AllowMemberClass == 1)
                                {
                                    lblMatchTicketAllowMemberClass.Text = "<em>普通会员(Core)以上</em>";
                                }
                                else
                                {
                                    lblMatchTicketAllowMemberClass.Text = "无";
                                }

                                ctrlPortalMatchInfo.MatchGuid = mt.ID;
                            }
                        }
                        else
                        {
                            throw new Exception("无相关比赛信息，请联系管理员");
                        }

                        lblOrderItem_TravelDate.Text = oiMatchTicket.TravelDate.ToString("yyyy年MM月dd日");
                    }
                    else
                    {
                        throw new Exception("此订单未填写订票信息");
                    }

                    // Set Order Price

                    price = oiMatchTicket.TotalPrice;
                    priceInfo = string.Format("<合计> {2}：{0} × {1}", oiMatchTicket.UnitPrice.ToString("f2"), oiMatchTicket.Quantity.ToString(),
                        Product.Cache.Load(oiMatchTicket.ProductGuid).DisplayName);

                    tbOrderPrice.Text = price.ToString();
                    lblOrderPrice.Text = string.Format("{0} = <em>{1}</em>元", priceInfo, price.ToString("f2"));

                    if (o.Status.Equals(OrderStatusType.Draft))
                    {
                        btnSubmit.Visible = true;
                        btnModify.Visible = true;
                        btnCancel.Visible = true;

                        if (!_isMemberCouldPurchase)
                        {
                            lblOrderRemark.Text = @"<em style='line-height: 1.8'>由于球票供应有限，所有主场球票预订均只向(Core/Premier)会员开放。<br />
                                                                   <a href='iArsenalMemberPeriod.aspx' target='_blank' style='background: #fff48d'>【点击这里】请在续费或升级会员资格后，才能提交订单。</a></em>";
                            phOrderRemark.Visible = true;

                            btnSubmit.Visible = false;
                        }
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

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", string.Format("window.location.href = 'iArsenalOrder_MatchTicket.aspx?OrderID={0}'", o.ID.ToString()), true);
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
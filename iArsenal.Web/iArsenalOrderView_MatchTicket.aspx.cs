using System;
using System.Globalization;
using Arsenalcn.Core;
using iArsenal.Service;

namespace iArsenal.Web
{
    public partial class iArsenalOrderView_MatchTicket : MemberPageBase
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
                    var o = (OrdrTicket)Order.Select(OrderID);

                    // For Vincent Song to View the MatchTickets Confirmation Page
                    if (ConfigGlobal.IsPluginAdmin(Uid) || (Uid.Equals(33067) && (int)o.Status >= 3))
                    {
                        lblMemberName.Text = $"<b>{o.MemberName}</b> (<em>NO.{o.MemberID}</em>)";
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

                    var oiMatchTicket = o.OIMatchTicket;

                    // Get Order MatchTicket Info

                    bool isMemberCouldPurchase;

                    if (oiMatchTicket != null && oiMatchTicket.IsActive)
                    {
                        var mt = MatchTicket.Cache.Load(oiMatchTicket.MatchGuid);

                        if (mt == null)
                        {
                            throw new Exception("无相关比赛信息，请联系管理员");
                        }

                        var mp = MemberPeriod.GetCurrentMemberPeriodByMemberID(Mid);

                        isMemberCouldPurchase = mt.CheckMemberCanPurchase(mp);

                        var p = Product.Cache.Load(mt.ProductCode);

                        if (p == null)
                        {
                            throw new Exception("无相关商品信息，请联系管理员");
                        }

                        lblMatchTicketInfo.Text =
                            $"<em>【{mt.LeagueName}】{mt.TeamName}({Arsenal_Team.Cache.Load(mt.TeamGuid).TeamEnglishName})</em>";
                        lblMatchTicketPlayTime.Text =
                            $"<em>【伦敦】{mt.PlayTimeLocal.ToString("yyyy-MM-dd HH:mm")}</em>";

                        var strRank = mt.ProductInfo.Trim();
                        if (lblMatchTicketRank != null && !string.IsNullOrEmpty(strRank))
                        {
                            lblMatchTicketRank.Text = $"<em>{strRank.Substring(strRank.Length - 7, 7)}</em>";
                        }

                        if (mt.AllowMemberClass.HasValue && mt.AllowMemberClass.Value == 2)
                        {
                            lblMatchTicketAllowMemberClass.Text = "<em>只限高级会员 (Premier) </em>";
                        }
                        else if (mt.AllowMemberClass.HasValue && mt.AllowMemberClass == 1)
                        {
                            lblMatchTicketAllowMemberClass.Text = "<em>普通会员 (Core) 以上</em>";
                        }
                        else
                        {
                            lblMatchTicketAllowMemberClass.Text = "无";
                        }

                        ucPortalMatchInfo.MatchGuid = mt.ID;

                        lblOrderItem_TravelDate.Text = oiMatchTicket.TravelDate.ToString("yyyy年MM月dd日");


                        // Set Order Price

                        price = oiMatchTicket.TotalPrice;
                        priceInfo = string.Format("<合计> {2}：{0} × {1}", oiMatchTicket.UnitPrice.ToString("f2"),
                            oiMatchTicket.Quantity,
                            Product.Cache.Load(oiMatchTicket.ProductGuid).DisplayName);

                        tbOrderPrice.Text = price.ToString(CultureInfo.CurrentCulture);
                        lblOrderPrice.Text = $"{priceInfo} = <em>{price.ToString("f2")}</em>元";

                        if (o.Status.Equals(OrderStatusType.Draft))
                        {
                            btnSubmit.Visible = true;
                            btnModify.Visible = true;
                            btnCancel.Visible = true;

                            if (!isMemberCouldPurchase)
                            {
                                lblOrderRemark.Text =
                                    $@"<em style='line-height: 1.8'>由于球票供应有限，所有主场球票预订均只向(Core/Premier)会员开放。<br />
                                        <a href='ServerMembershipCheck.ashx?OrderID={OrderID}' target='_blank' style='background: #fff48d'>
                                        【点击这里】请在续费或升级会员资格后，才能提交订单。</a></em>";
                                phOrderRemark.Visible = true;

                                btnSubmit.Visible = false;
                            }
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
                        throw new Exception("此订单未填写订票信息");
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
                        $"window.location.href = 'iArsenalOrder_MatchTicket.aspx?OrderID={o.ID}'", true);
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
                        $"alert('此订单({o.ID})已经取消');window.location.href = 'iArsenalOrder.aspx'", true);
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
    }
}
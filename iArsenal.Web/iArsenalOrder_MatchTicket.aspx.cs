using System;
using System.Data.SqlClient;
using Arsenalcn.Core;
using iArsenal.Service;

namespace iArsenal.Web
{
    public partial class iArsenalOrder_MatchTicket : MemberPageBase
    {
        private readonly IRepository _repo = new Repository();

        private int OrderID
        {
            get
            {
                int orderID;
                if (!string.IsNullOrEmpty(Request.QueryString["OrderID"]) &&
                    int.TryParse(Request.QueryString["OrderID"], out orderID))
                {
                    return orderID;
                }
                return int.MinValue;
            }
        }

        private Guid MatchGuid
        {
            get
            {
                if (OrderID > 0)
                {
                    var o = (OrdrTicket)Order.Select(OrderID);

                    if (o.OIMatchTicket != null)
                    {
                        return o.OIMatchTicket.MatchGuid;
                    }
                    return Guid.Empty;
                }
                if (!string.IsNullOrEmpty(Request.QueryString["MatchGuid"]))
                {
                    try
                    {
                        return new Guid(Request.QueryString["MatchGuid"]);
                    }
                    catch
                    {
                        return Guid.Empty;
                    }
                }
                return Guid.Empty;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            #region Assign Control Property

            ctrlPortalMatchInfo.MatchGuid = MatchGuid;

            #endregion

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
                lblMemberACNInfo.Text = $"<b>{Username}</b> (<em>ID.{UID}</em>)";

                if (MatchGuid.Equals(Guid.Empty))
                {
                    Response.Redirect("iArsenalOrder_MatchList.aspx");
                    Response.Clear();
                }

                var mt = MatchTicket.Cache.Load(MatchGuid);

                if (mt == null)
                {
                    throw new Exception("无相关比赛信息，请联系管理员");
                }

                if (OrderID <= 0 && mt.Deadline < DateTime.Now)
                {
                    throw new Exception("此球票预定已过截至时间，请联系管理员");
                }

                //if (!mt.IsMemberCouldPurchase(this.CurrentMemberPeriod))
                //{
                //    ClientScript.RegisterClientScriptBlock(typeof(string), "failed",
                //        string.Format("alert('由于球票数量紧张，所有阿森纳主场球票预订，均只向收费会员开放，请在跳转页面后续费或升级会员资格');window.location.href = 'iArsenalMemberPeriod.aspx'"), true);
                //}

                var p = Product.Cache.Load(mt.ProductCode);

                if (p == null)
                {
                    throw new Exception("无相关商品信息，请联系管理员");
                }

                lblMatchTicketInfo.Text =
                    $"<em>【{mt.LeagueName}】{mt.TeamName}({Arsenal_Team.Cache.Load(mt.TeamGuid).TeamEnglishName})</em>";
                lblMatchTicketPlayTime.Text = $"<em>【伦敦】{mt.PlayTimeLocal.ToString("yyyy-MM-dd HH:mm")}</em>";

                var strRank = mt.ProductInfo.Trim();
                if (lblMatchTicketRank != null)
                {
                    lblMatchTicketRank.Text = !string.IsNullOrEmpty(strRank) ? $"<em>{strRank.Substring(strRank.Length - 7, 7)} - {p.PriceInfo}</em>" : string.Empty;
                }

                if (mt.AllowMemberClass.HasValue && mt.AllowMemberClass.Value == 2)
                {
                    lblAllowMemberClass.Text = "<em>只限高级会员(Premier)</em>";
                }
                else if (mt.AllowMemberClass.HasValue && mt.AllowMemberClass == 1)
                {
                    lblAllowMemberClass.Text = "<em>普通会员(Core)以上</em>";
                }
                else
                {
                    lblAllowMemberClass.Text = "无";
                }

                if (OrderID > 0)
                {
                    var o = (OrdrTicket)Order.Select(OrderID);

                    if (o == null || !o.IsActive)
                    {
                        throw new Exception("此订单无效");
                    }

                    if (ConfigGlobal.IsPluginAdmin(UID) || o.MemberID.Equals(MID))
                    {
                        lblMemberName.Text = $"<b>{o.MemberName}</b> (<em>NO.{o.MemberID}</em>)";

                        var m = _repo.Single<Member>(o.MemberID);

                        if (m == null || !m.IsActive)
                        {
                            throw new Exception("无此会员信息");
                        }
                        lblMemberACNInfo.Text = $"<b>{m.AcnName}</b> (<em>ID.{m.AcnID}</em>)";

                        #region Set Member Nation & Region

                        if (!string.IsNullOrEmpty(m.Nation))
                        {
                            if (m.Nation.Equals("中国"))
                            {
                                ddlNation.SelectedValue = m.Nation;

                                var region = m.Region.Split('|');
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
                                tbNation.Text = m.Nation.Equals("其他") ? string.Empty : m.Nation;
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
                        tbWeChat.Text = m.WeChat;
                        tbEmail.Text = m.Email;

                        tbOrderDescription.Text = o.Description;
                    }
                    else
                    {
                        throw new Exception("此订单非当前用户订单");
                    }

                    var oi = o.OIMatchTicket;

                    if (oi != null)
                    {
                        //tbQuantity.Text = oi.Quantity.ToString();
                        tbTravelDate.Text = oi.TravelDate.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        throw new Exception("此订单未填写订票信息");
                    }
                }
                else
                {
                    //Fill Member draft information into textbox
                    var m = _repo.Single<Member>(MID);

                    #region Set Member Nation & Region

                    if (!string.IsNullOrEmpty(m.Nation))
                    {
                        if (m.Nation.Equals("中国"))
                        {
                            ddlNation.SelectedValue = m.Nation;

                            var region = m.Region.Split('|');
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
                            tbNation.Text = m.Nation.Equals("其他") ? string.Empty : m.Nation;
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
                    tbWeChat.Text = m.WeChat;
                    tbEmail.Text = m.Email;

                    tbTravelDate.Text = mt.PlayTime.AddDays(-2).ToString("yyyy-MM-dd");
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed",
                    $"alert('{ex.Message}');window.location.href = 'iArsenalOrder_MatchList.aspx'", true);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            using (var conn = new SqlConnection(DataAccess.ConnectString))
            {
                conn.Open();
                var trans = conn.BeginTransaction();

                try
                {
                    if (MatchGuid.Equals(Guid.Empty))
                    {
                        Response.Redirect("iArsenalOrder_MatchList.aspx");
                        Response.Clear();
                    }

                    var mt = MatchTicket.Cache.Load(MatchGuid);

                    if (mt == null)
                    {
                        throw new Exception("无相关比赛信息，请联系管理员");
                    }

                    var m = _repo.Single<Member>(MID);

                    // Update Member Information

                    #region Get Member Nation & Region

                    var nation = ddlNation.SelectedValue;

                    if (!string.IsNullOrEmpty(nation))
                    {
                        if (nation.Equals("中国"))
                        {
                            m.Nation = nation;
                            if (!string.IsNullOrEmpty(tbRegion1.Text.Trim()))
                            {
                                m.Region = !string.IsNullOrEmpty(tbRegion2.Text.Trim()) ? $"{tbRegion1.Text.Trim()}|{tbRegion2.Text.Trim()}" : tbRegion1.Text.Trim();
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
                            m.Nation = nation;
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

                    if (!string.IsNullOrEmpty(tbWeChat.Text.Trim()))
                        m.WeChat = tbWeChat.Text.Trim();
                    else
                        throw new Exception("请填写会员微信号");

                    if (!string.IsNullOrEmpty(tbEmail.Text.Trim()))
                        m.Email = tbEmail.Text.Trim();
                    else
                        throw new Exception("请填写会员邮箱");

                    //m.MemberType = MemberType.Match;

                    _repo.Update(m);

                    // New Order
                    var o = new Order();
                    int newID;

                    if (OrderID > 0)
                    {
                        o = _repo.Single<Order>(OrderID);
                    }

                    o.Mobile = m.Mobile;
                    o.UpdateTime = DateTime.Now;
                    o.Description = tbOrderDescription.Text.Trim();
                    o.OrderType = OrderBaseType.Ticket;

                    if (OrderID > 0)
                    {
                        _repo.Update(o, trans);

                        // used by setting OrderItem foreign key
                        newID = OrderID;
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
                        object key;
                        _repo.Insert(o, out key, trans);
                        newID = Convert.ToInt32(key);
                    }

                    //New Order Items
                    var p = Product.Cache.Load(mt.ProductCode);

                    if (p == null)
                        throw new Exception("无相关商品信息，请联系管理员");

                    var oi = new OrdrItmMatchTicket();

                    //Remove Order Item of this Order
                    if (OrderID > 0 && o.ID.Equals(OrderID))
                    {
                        _repo.Query<OrderItem>(x => x.OrderID == OrderID).Delete(trans);
                    }

                    // Genernate Travel Date
                    DateTime date;
                    if (!string.IsNullOrEmpty(tbTravelDate.Text.Trim()) &&
                        DateTime.TryParse(tbTravelDate.Text.Trim(), out date))
                    {
                        oi.TravelDate = date;
                    }
                    else
                    {
                        throw new Exception("请正确填写计划出行时间");
                    }

                    // Every Member can only purchase ONE ticket of each match

                    oi.MatchGuid = mt.ID;
                    oi.OrderID = newID;
                    oi.Quantity = 1;
                    oi.Sale = null;

                    oi.Place(m, p, trans);

                    trans.Commit();

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed",
                        string.Format("alert('订单({0})保存成功');window.location.href = 'ServerOrderView.ashx?OrderID={0}'",
                            newID), true);
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
using System;
using System.Data.SqlClient;

using Arsenalcn.Core;
using iArsenal.Service;

namespace iArsenal.Web
{
    public partial class iArsenalOrder_MatchTicket : MemberPageBase
    {
        private readonly IRepository repo = new Repository();
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

        private Guid MatchGuid
        {
            get
            {
                if (OrderID > 0)
                {
                    var o = (OrdrTicket)Order.Select(OrderID);

                    if (o.OIMatchTicket != null)
                    { return o.OIMatchTicket.MatchGuid; }
                    else
                    { return Guid.Empty; }
                }
                else if (!string.IsNullOrEmpty(Request.QueryString["MatchGuid"]))
                {
                    try { return new Guid(Request.QueryString["MatchGuid"]); }
                    catch { return Guid.Empty; }
                }
                else
                    return Guid.Empty;
            }
        }

        private void InitForm()
        {
            try
            {
                lblMemberName.Text = $"<b>{this.MemberName}</b> (<em>NO.{this.MID.ToString()}</em>)";
                lblMemberACNInfo.Text = $"<b>{this.Username}</b> (<em>ID.{this.UID.ToString()}</em>)";

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

                var _strRank = mt.ProductInfo.Trim();
                if (lblMatchTicketRank != null && !string.IsNullOrEmpty(_strRank))
                {
                    lblMatchTicketRank.Text = $"<em>{_strRank.Substring(_strRank.Length - 7, 7)} - {p.PriceInfo}</em>";
                }
                else
                {
                    lblMatchTicketRank.Text = string.Empty;
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

                    if (o == null || !o.IsActive) { throw new Exception("此订单无效"); }

                    if (ConfigGlobal.IsPluginAdmin(UID) || o.MemberID.Equals(MID))
                    {
                        lblMemberName.Text = $"<b>{o.MemberName}</b> (<em>NO.{o.MemberID.ToString()}</em>)";

                        var m = repo.Single<Member>(o.MemberID);

                        if (m == null || !m.IsActive)
                        {
                            throw new Exception("无此会员信息");
                        }
                        else
                        {
                            lblMemberACNInfo.Text = $"<b>{m.AcnName}</b> (<em>ID.{m.AcnID.ToString()}</em>)";

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
                    var m = repo.Single<Member>(this.MID);

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

                    tbTravelDate.Text = mt.PlayTime.AddDays(-2).ToString("yyyy-MM-dd");
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed",
                    $"alert('{ex.Message.ToString()}');window.location.href = 'iArsenalOrder_MatchList.aspx'", true);
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

                    var m = repo.Single<Member>(this.MID);

                    // Update Member Information
                    #region Get Member Nation & Region
                    var _nation = ddlNation.SelectedValue;

                    if (!string.IsNullOrEmpty(_nation))
                    {
                        if (_nation.Equals("中国"))
                        {
                            m.Nation = _nation;
                            if (!string.IsNullOrEmpty(tbRegion1.Text.Trim()))
                            {
                                if (!string.IsNullOrEmpty(tbRegion2.Text.Trim()))
                                {
                                    m.Region = $"{tbRegion1.Text.Trim()}|{tbRegion2.Text.Trim()}";
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

                    repo.Update(m);

                    // New Order
                    var o = new Order();
                    var _newID = int.MinValue;

                    if (OrderID > 0)
                    {
                        o = repo.Single<Order>(OrderID);
                    }

                    o.Mobile = m.Mobile;
                    o.UpdateTime = DateTime.Now;
                    o.Description = tbOrderDescription.Text.Trim();
                    o.OrderType = OrderBaseType.Ticket;

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
                        repo.Insert(o, out _key, trans);
                        _newID = Convert.ToInt32(_key);
                    }

                    //New Order Items
                    var p = Product.Cache.Load(mt.ProductCode);

                    if (p == null)
                        throw new Exception("无相关商品信息，请联系管理员");

                    var oi = new OrdrItmMatchTicket();

                    //Remove Order Item of this Order
                    if (OrderID > 0 && o.ID.Equals(OrderID))
                    {
                        var count = repo.Query<OrderItem>(x => x.OrderID == OrderID).Delete(trans);
                    }

                    // Genernate Travel Date
                    DateTime _date;
                    if (!string.IsNullOrEmpty(tbTravelDate.Text.Trim()) && DateTime.TryParse(tbTravelDate.Text.Trim(), out _date))
                    {
                        oi.TravelDate = _date;
                    }
                    else
                    {
                        throw new Exception("请正确填写计划出行时间");
                    }

                    // Every Member can only purchase ONE ticket of each match

                    oi.MatchGuid = mt.ID;
                    oi.OrderID = _newID;
                    oi.Quantity = 1;
                    oi.Sale = null;

                    oi.Place(m, p, trans);

                    trans.Commit();

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", string.Format("alert('订单({0})保存成功');window.location.href = 'ServerOrderView.ashx?OrderID={0}'", _newID.ToString()), true);
                }
                catch (Exception ex)
                {
                    trans.Rollback();

                    ClientScript.RegisterClientScriptBlock(typeof(string), "failed", $"alert('{ex.Message.ToString()}')", true);
                }

                //conn.Close();
            }
        }
    }
}
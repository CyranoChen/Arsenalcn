using System;
using System.Linq;
using Arsenalcn.Core;
using iArsenal.Service;

namespace iArsenal.Web
{
    public partial class iArsenalOrder_2017TicketBeijing : MemberPageBase
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                #region Bind rblTicket

                var list = Product.Cache.Load(ProductType.TicketFriendly)
                    .FindAll(p => p.IsActive && p.Code.StartsWith("20170722"));

                if (list.Count > 0)
                {
                    var query = from p in list
                                orderby p.Code ascending
                                select new { p.ID, Text = $" {p.Description} {p.PriceInfo} " };

                    rblTicket.Items.Clear();
                    rblTicket.DataSource = query;
                    rblTicket.DataTextField = "Text";
                    rblTicket.DataTextFormatString = "<em>{0}</em>";
                    rblTicket.DataValueField = "ID";
                    rblTicket.DataBind();
                }

                #endregion

                InitForm();
            }
        }

        private void InitForm()
        {
            try
            {
                lblMemberName.Text = $"<b>{MemberName}</b> (<em>NO.{Mid}</em>)";
                lblMemberACNInfo.Text = $"<b>{Username}</b> (<em>ID.{Uid}</em>)";

                if (OrderID > 0)
                {
                    var o = (OrdrTicket)Order.Select(OrderID);

                    if (o == null || !o.IsActive)
                    {
                        throw new Exception("此订单无效");
                    }

                    if (ConfigGlobal.IsPluginAdmin(Uid) || o.MemberID.Equals(Mid))
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
                        tbMemberWeChat.Text = m.WeChat;
                    }
                    else
                    {
                        throw new Exception("此订单非当前用户订单");
                    }

                    tbOrderMobile.Text = o.Mobile;
                    tbOrderDescription.Text = o.Description;

                    var oi = o.OI2017TicketBeijing;

                    rblTicket.SelectedValue = oi.ProductGuid.ToString();
                    tbQuantity.Text = oi.Quantity.ToString();
                }
                else
                {
                    //Fill Member draft information into textbox
                    var m = _repo.Single<Member>(Mid);

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

                    tbOrderMobile.Text = m.Mobile;
                    tbMemberWeChat.Text = m.WeChat;
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", $"alert('{ex.Message}');window.location.href = 'Default.aspx'", true);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            using (var trans = DapperHelper.MarsConnection.BeginTransaction())
            {
                try
                {
                    Guid pid;

                    if (!string.IsNullOrEmpty(rblTicket.SelectedValue))
                        pid = new Guid(rblTicket.SelectedValue);
                    else
                        throw new Exception("请选择对应球票");

                    var m = _repo.Single<Member>(Mid, trans);

                    m.IDCardNo = tbIDCardNo.Text.Trim();

                    if (!string.IsNullOrEmpty(tbMemberWeChat.Text.Trim()))
                    {
                        m.WeChat = tbMemberWeChat.Text.Trim();
                    }
                    else
                    {
                        throw new Exception("请输入会员微信号");
                    }

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

                    _repo.Update(m, trans);

                    //New Order
                    var o = new Order();
                    int newId;

                    if (OrderID > 0)
                    {
                        o = _repo.Single<Order>(OrderID, trans);
                    }

                    o.Mobile = tbOrderMobile.Text.Trim();

                    o.Payment = string.Empty;

                    o.Postage = 0;
                    o.UpdateTime = DateTime.Now;
                    o.Description = tbOrderDescription.Text.Trim();
                    o.OrderType = OrderBaseType.Ticket;

                    if (OrderID > 0)
                    {
                        _repo.Update(o, trans);

                        // used by setting OrderItem foreign key
                        newId = OrderID;
                    }
                    else
                    {
                        o.MemberID = m.ID;
                        o.MemberName = m.Name;

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
                        newId = Convert.ToInt32(key);
                    }

                    //New Order Items
                    var p = Product.Cache.Load(pid);

                    if (p == null)
                        throw new Exception("无相关商品信息，请联系管理员");

                    var oi = new OrdrItm2017TicketBeijing();

                    //Remove Order Item of this Order
                    if (OrderID > 0 && o.ID.Equals(OrderID))
                    {
                        _repo.Query<OrderItem>(x => x.OrderID == OrderID, trans).Delete(trans);
                    }

                    oi.OrderID = newId;
                    oi.Size = string.Empty;

                    int quantity;

                    if (!string.IsNullOrEmpty(tbQuantity.Text.Trim()) &&
                        int.TryParse(tbQuantity.Text.Trim(), out quantity))
                    {
                        oi.Quantity = quantity;
                    }
                    else
                    {
                        throw new Exception("请填写球票数量");
                    }

                    oi.Sale = null;
                    oi.Remark = string.Empty;

                    oi.Place(m, p, trans);

                    trans.Commit();

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed",
                        string.Format("alert('订单({0})保存成功');window.location.href = 'ServerOrderView.ashx?OrderID={0}'", newId), true);
                }
                catch (Exception ex)
                {
                    trans.Rollback();

                    ClientScript.RegisterClientScriptBlock(typeof(string), "failed", $"alert('{ex.Message}')", true);
                }
            }
        }
    }
}
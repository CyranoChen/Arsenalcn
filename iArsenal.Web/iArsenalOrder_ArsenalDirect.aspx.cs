using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Script.Serialization;
using Arsenalcn.Core;
using iArsenal.Service;

namespace iArsenal.Web
{
    public partial class iArsenalOrder_ArsenalDirect : MemberPageBase
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
                    var o = (OrdrWish)Order.Select(OrderID);

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
                        tbMemberWeChat.Text = m.WeChat;
                    }
                    else
                    {
                        throw new Exception("此订单非当前用户订单");
                    }

                    tbOrderMobile.Text = o.Mobile;
                    tbOrderAddress.Text = o.Address;

                    rblOrderPostage.SelectedValue = o.Postage > 0 ? o.Postage.ToString("f0") : "23";

                    tbOrderDescription.Text = o.Description;

                    var query = _repo.Query<OrderItem>(x => x.OrderID == o.ID).FindAll(x => x.IsActive).OrderBy(x => x.ID);

                    if (query.Any())
                    {
                        var jsonSerializer = new JavaScriptSerializer();
                        tbWishOrderItemListInfo.Text = jsonSerializer.Serialize(query.ToList());
                    }
                    else
                    {
                        tbWishOrderItemListInfo.Text = string.Empty;
                    }
                }
                else
                {
                    //Fill Member draft information into textbox
                    var m = _repo.Single<Member>(Mid);

                    tbOrderMobile.Text = m.Mobile;
                    tbMemberWeChat.Text = m.WeChat;
                    tbOrderAddress.Text = m.Address;
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed",
                    $"alert('{ex.Message}');window.location.href = 'Default.aspx'", true);
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
                    if (string.IsNullOrEmpty(tbWishOrderItemListInfo.Text.Trim()))
                    {
                        throw new Exception("请填写订购纪念品信息");
                    }

                    // Convert ProductListInfo to List<OrderItem>
                    var json = $"[ {tbWishOrderItemListInfo.Text.Trim()} ]";

                    var jsonSerializer = new JavaScriptSerializer();

                    var wishList = jsonSerializer.Deserialize<List<OrderItem>>(json);

                    // Validate the OrderItemBase Code & Quantity in oiList
                    if (wishList.Count > 0)
                    {
                        if (wishList.Exists(oi => string.IsNullOrEmpty(oi.Code)))
                        {
                            throw new Exception("请填写订购纪念品的编号信息");
                        }
                        if (wishList.Exists(oi => (oi.Quantity <= 0)))
                        {
                            throw new Exception("请正确填写订购纪念品的数量");
                        }
                    }
                    else
                    {
                        throw new Exception("请填写订购纪念品信息");
                    }

                    var m = _repo.Single<Member>(Mid);

                    if (!string.IsNullOrEmpty(tbMemberWeChat.Text.Trim()))
                    {
                        m.WeChat = tbMemberWeChat.Text.Trim();

                        _repo.Update(m);
                    }
                    else
                    {
                        throw new Exception("请输入会员微信号");
                    }

                    //New Order
                    var o = new Order();
                    int newID;

                    if (OrderID > 0)
                    {
                        o = _repo.Single<Order>(OrderID);
                    }

                    if (!string.IsNullOrEmpty(tbOrderMobile.Text.Trim()))
                        o.Mobile = tbOrderMobile.Text.Trim();
                    else
                        throw new Exception("请填写会员手机");

                    if (!string.IsNullOrEmpty(tbOrderAddress.Text.Trim()))
                        o.Address = tbOrderAddress.Text.Trim();
                    else
                        throw new Exception("请填写收货地址");

                    o.UpdateTime = DateTime.Now;
                    o.Postage = Convert.ToSingle(rblOrderPostage.SelectedValue);
                    o.Description = tbOrderDescription.Text.Trim();
                    o.OrderType = OrderBaseType.Wish;

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

                    if (wishList.Count > 0)
                    {
                        //Remove Order Item of this Order
                        if (OrderID > 0 && o.ID.Equals(OrderID))
                        {
                            _repo.Query<OrderItem>(x => x.OrderID == OrderID).Delete(trans);
                        }

                        //New Order Item for each WishOrderItem
                        foreach (var oi in wishList)
                        {
                            if (!oi.ProductGuid.Equals(Guid.Empty))
                            {
                                // Exist Product Wished
                                var p = Product.Cache.Load(oi.ProductGuid);

                                if (p != null && p.IsActive && p.ProductType.Equals(ProductType.Other))
                                {
                                    oi.Size = oi.Size ?? string.Empty;
                                    oi.Remark = oi.Remark ?? string.Empty;
                                    oi.OrderID = newID;

                                    oi.Place(m, p, trans);
                                }
                                else
                                {
                                    throw new Exception("无对应纪念品信息，请联系管理员");
                                }
                            }
                            else
                            {
                                // New Product Wished
                                oi.MemberID = m.ID;
                                oi.MemberName = m.Name;
                                oi.CreateTime = DateTime.Now;
                                oi.OrderID = newID;
                                oi.IsActive = true;
                                //oi.Code = oi.Code; // Exist
                                oi.ProductGuid = Guid.Empty;
                                oi.ProductName = oi.ProductName ?? string.Empty;
                                oi.Size = oi.Size ?? string.Empty;
                                //oi.Quantity = oi.Quantity; // Exist
                                oi.Sale = null;
                                oi.Remark = new JavaScriptSerializer().Serialize(oi);

                                _repo.Insert(oi, trans);
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("请输入需要订购的纪念品");
                    }

                    trans.Commit();

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed",
                        string.Format(
                            "alert('订单({0})保存成功');window.location.href = 'iArsenalOrderView_ArsenalDirect.aspx?OrderID={0}'",
                            newID), true);
                }
                catch (Exception ex)
                {
                    trans.Rollback();

                    ClientScript.RegisterClientScriptBlock(typeof(string), "failed",
                        $"alert('{ex.Message}');window.location.href = window.location.href", true);
                }
            }
        }
    }
}
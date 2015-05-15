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

                if (OrderID > 0)
                {
                    OrdrWish o = repo.Single<OrdrWish>(OrderID);

                    if (ConfigGlobal.IsPluginAdmin(UID) && o != null)
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
                            tbEmail.Text = m.Email;
                        }
                    }
                    else
                    {
                        if (o == null || !o.MemberID.Equals(MID) || !o.IsActive)
                            throw new Exception("此订单无效或非当前用户订单");
                    }

                    tbOrderMobile.Text = o.Mobile;
                    tbOrderAddress.Text = o.Address;
                    tbOrderDescription.Text = o.Description;

                    var query = repo.Query<OrderItem>(x => x.OrderID.Equals(o.ID) && x.IsActive).OrderBy(x => x.ID);

                    if (query != null && query.Count() > 0)
                    {
                        JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
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
                    Member m = repo.Single<Member>(this.MID);

                    tbOrderMobile.Text = m.Mobile;
                    tbEmail.Text = m.Email;
                    tbOrderAddress.Text = m.Address;
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
                    if (string.IsNullOrEmpty(tbWishOrderItemListInfo.Text.Trim()))
                    { throw new Exception("请填写订购纪念品信息"); }

                    // Convert ProductListInfo to List<OrderItem>
                    string _strWishOrderItemListInfo = string.Format("[ {0} ]", tbWishOrderItemListInfo.Text.Trim());

                    JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

                    List<OrderItem> wishList = jsonSerializer.Deserialize<List<OrderItem>>(_strWishOrderItemListInfo);

                    // Validate the OrderItemBase Code & Quantity in oiList
                    if (wishList.Count > 0)
                    {
                        if (wishList.Exists(oi => string.IsNullOrEmpty(oi.Code)))
                        {
                            throw new Exception("请填写订购纪念品的编号信息");
                        }
                        else if (wishList.Exists(oi => (oi.Quantity <= 0)))
                        {
                            throw new Exception("请正确填写订购纪念品的数量");
                        }
                    }
                    else
                    {
                        throw new Exception("请填写订购纪念品信息");
                    }

                    Member m = repo.Single<Member>(this.MID);

                    if (!string.IsNullOrEmpty(tbEmail.Text.Trim()))
                    {
                        m.Email = tbEmail.Text.Trim();

                        repo.Update(m);
                    }
                    else
                    {
                        throw new Exception("请输入会员邮箱信息");
                    }

                    //New Order
                    Order o = new Order();
                    int _newID = int.MinValue;

                    if (OrderID > 0)
                    {
                        o = repo.Single<Order>(OrderID);
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
                    o.Description = tbOrderDescription.Text.Trim();
                    o.OrderType = OrderBaseType.Wish;

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
                        _newID = (int)repo.InsertOutKey<Order>(o, trans);
                    }

                    if (wishList != null && wishList.Count > 0)
                    {
                        //Remove Order Item of this Order
                        if (OrderID > 0 && o.ID.Equals(OrderID))
                        {
                            repo.Delete<OrderItem>(x => x.OrderID.Equals(OrderID), trans);
                        }

                        //New Order Item for each WishOrderItem
                        foreach (OrderItem oi in wishList)
                        {
                            if (!oi.ProductGuid.Equals(Guid.Empty))
                            {
                                // Exist Product Wished
                                Product p = Product.Cache.Load(oi.ProductGuid);

                                if (p != null && p.IsActive && p.ProductType.Equals(ProductType.Other))
                                {
                                    oi.Size = (oi.Size != null) ? oi.Size : string.Empty;
                                    oi.Remark = (oi.Remark != null) ? oi.Remark : string.Empty;
                                    oi.OrderID = _newID;

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
                                oi.OrderID = _newID;
                                oi.IsActive = true;
                                //oi.Code = oi.Code;
                                oi.ProductGuid = Guid.Empty;
                                oi.ProductName = (oi.ProductName != null) ? oi.ProductName : string.Empty;
                                oi.Size = (oi.Size != null) ? oi.Size : string.Empty;
                                //oi.Quantity = oi.Quantity;
                                oi.Sale = null;
                                oi.Remark = new JavaScriptSerializer().Serialize(oi);

                                repo.Insert(oi, trans);
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("请输入需要订购的纪念品");
                    }

                    trans.Commit();

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", string.Format("alert('订单({0})保存成功');window.location.href = 'ServerOrderView.ashx?OrderID={0}'", _newID.ToString()), true);

                }
                catch (Exception ex)
                {
                    trans.Rollback();

                    ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}');window.location.href = window.location.href", ex.Message.ToString()), true);
                }

                //conn.Close();
            }
        }
    }
}
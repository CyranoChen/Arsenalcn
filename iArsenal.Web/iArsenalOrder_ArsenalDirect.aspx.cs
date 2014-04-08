using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using iArsenal.Entity;

namespace iArsenal.Web
{
    public partial class iArsenalOrder_ArsenalDirect : PageBase.MemberPageBase
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

                if (OrderID > 0)
                {
                    Order_Wish o = new Order_Wish(OrderID);

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

                    List<OrderItemBase> oiList = OrderItemBase.GetOrderItems(o.OrderID).FindAll(oi => oi.IsActive);
                    oiList.Sort(delegate(OrderItemBase oi1, OrderItemBase oi2) { return oi1.OrderItemID - oi2.OrderItemID; });

                    if (oiList != null && oiList.Count > 0)
                    {
                        JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                        tbWishOrderItemListInfo.Text = jsonSerializer.Serialize(oiList);
                    }
                    else
                    {
                        tbWishOrderItemListInfo.Text = string.Empty;
                    }
                }
                else
                {
                    //Fill Member draft information into textbox
                    Member m = new Member();
                    m.MemberID = this.MID;
                    m.Select();

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
            using (SqlConnection conn = ConfigGlobal.SQLConnectionStrings)
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

                    List<OrderItemBase> oiList = jsonSerializer.Deserialize<List<OrderItemBase>>(_strWishOrderItemListInfo);

                    // Validate the OrderItemBase Code & Quantity in oiList
                    if (oiList.Count > 0)
                    {
                        if (oiList.Exists(oi => string.IsNullOrEmpty(oi.Code)))
                        {
                            throw new Exception("请填写订购纪念品的编号信息");
                        }
                        else if (oiList.Exists(oi => (oi.Quantity <= 0)))
                        {
                            throw new Exception("请正确填写订购纪念品的数量");
                        }
                    }
                    else
                    {
                        throw new Exception("请填写订购纪念品信息");
                    }

                    Member m = new Member();
                    m.MemberID = this.MID;
                    m.Select();

                    if (!string.IsNullOrEmpty(tbEmail.Text.Trim()))
                    {
                        m.Email = tbEmail.Text.Trim();
                        m.Update();
                    }
                    else
                    {
                        throw new Exception("请输入会员邮箱信息");
                    }

                    //New Order
                    OrderBase o = new OrderBase();

                    if (OrderID > 0)
                    {
                        o.OrderID = OrderID;
                        o.Select();
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

                    if (OrderID > 0)
                    {
                        o.Update(trans);
                    }
                    else
                    {
                        o.MemberID = m.MemberID;
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

                        o.Insert(trans);
                        //o.Select();
                    }

                    //Get the Order ID after Insert new one

                    if (o.OrderID > 0)
                    {
                        //Remove Order Item of this Order
                        if (o.OrderID.Equals(OrderID))
                        {
                            int countOrderItem = OrderItemBase.RemoveOrderItemByOrderID(o.OrderID, trans);
                        }

                        //New Order Item for each WishOrderItem
                        foreach (OrderItemBase oi in oiList)
                        {
                            if (!oi.ProductGuid.Equals(Guid.Empty))
                            {
                                // Exist Product Wished
                                Product p = Product.Cache.Load(oi.ProductGuid);

                                if (p != null && p.IsActive && p.ProductType.Equals(ProductType.Other))
                                {
                                    OrderItemBase.WishOrderItem(m, p, o, oi.Size != null ? oi.Size : string.Empty, oi.Quantity, oi.Sale, oi.Remark != null ? oi.Remark : string.Empty, trans);
                                }
                                else
                                {
                                    throw new Exception("无对应纪念品信息，请联系管理员");
                                }
                            }
                            else
                            {
                                // New Product Wished
                                oi.MemberID = m.MemberID;
                                oi.MemberName = m.Name;
                                oi.CreateTime = DateTime.Now;
                                oi.OrderID = o.OrderID;
                                oi.IsActive = true;
                                //oi.Code = oi.Code;
                                oi.ProductGuid = Guid.Empty;
                                oi.ProductName = oi.ProductName != null ? oi.ProductName : string.Empty;
                                oi.Size = oi.Size != null ? oi.Size : string.Empty;
                                //oi.Quantity = oi.Quantity;
                                oi.Sale = null;
                                oi.Remark = new JavaScriptSerializer().Serialize(oi);

                                oi.Insert(trans);
                            }
                        }

                        trans.Commit();

                        ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", string.Format("alert('订单({0})保存成功');window.location.href = 'iArsenalOrderView_ArsenalDirect.aspx?OrderID={0}'", o.OrderID.ToString()), true);
                    }
                    else
                    {
                        throw new Exception("请输入需要订购的纪念品");
                    }
                }
                catch (Exception ex)
                {
                    trans.Rollback();

                    ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}');window.location.href = window.location.href", ex.Message.ToString()), true);
                }

                conn.Close();
            }
        }
    }
}
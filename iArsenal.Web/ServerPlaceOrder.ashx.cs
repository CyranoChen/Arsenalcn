using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Script.Serialization;
using Arsenalcn.Core.Dapper;
using iArsenal.Service;

namespace iArsenal.Web
{
    public class ServerPlaceOrder : HandlerBase
    {
        private readonly IRepository _repo = new Repository();

        public override void ProcessRequest(HttpContext context)
        {
            var responseText = string.Empty;
            var jsonSerializer = new JavaScriptSerializer();

            using (var dapper = DapperHelper.GetInstance())
            {
                var trans = dapper.BeginTransaction();

                try
                {
                    var oid = 0;

                    if (context.Request.RequestType.Equals("POST", StringComparison.OrdinalIgnoreCase))
                    {
                        // Request should have been sent successfully
                        if (Uid > 0 && !string.IsNullOrEmpty(Username))
                        {
                            // Load Member Info
                            var m = Member.Cache.LoadByAcnID(Uid);

                            if (m == null)
                            {
                                context.Response.Redirect("iArsenalMemberRegister.aspx", false);
                                context.ApplicationInstance.CompleteRequest();

                                throw new Exception("无当前会员信息");
                            }

                            if (!OrderType.HasValue)
                            {
                                throw new Exception("订单类型异常");
                            }

                            if (OrderType.Value == OrderBaseType.Wish &&
                                !string.IsNullOrEmpty(context.Request.Form["items"]))
                            {
                                var items = jsonSerializer.Deserialize<List<Item>>(context.Request.Form["items"]);

                                if (items != null && items.Count > 0)
                                {
                                    // Place Order
                                    var o = new OrdrWish
                                    {
                                        Mobile = m.Mobile,
                                        Address = m.Address,
                                        UpdateTime = DateTime.Now,
                                        Description = string.Empty,
                                        OrderType = OrderBaseType.Wish,
                                        MemberID = m.ID,
                                        MemberName = m.Name,
                                        Payment = string.Empty,
                                        Price = 0,
                                        Sale = null,
                                        Deposit = null,
                                        Status = OrderStatusType.Draft,
                                        Rate = 0,
                                        CreateTime = DateTime.Now,
                                        IsActive = true,
                                        Remark = "来自结算购物车"
                                    };

                                    //Get the Order ID after Insert new one
                                    object key;
                                    _repo.Insert(o, out key);
                                    oid = Convert.ToInt32(key);

                                    // Place OrderItem
                                    var dict = new Dictionary<string, int>();

                                    // Organize Dictionary
                                    foreach (var item in items)
                                    {
                                        if (dict.ContainsKey(item.code))
                                        {
                                            dict[item.code]++;
                                        }
                                        else
                                        {
                                            dict.Add(item.code, 1);
                                        }
                                    }

                                    // Place every product into orderItem
                                    if (dict.Count > 0)
                                    {
                                        foreach (var d in dict)
                                        {
                                            var p = Product.Cache.Load(d.Key);

                                            if (p != null && p.IsActive && p.ProductType.Equals(ProductType.Other))
                                            {
                                                var oi = new OrderItem
                                                {
                                                    Quantity = d.Value,
                                                    Size = string.Empty,
                                                    Remark = string.Empty,
                                                    OrderID = oid
                                                };

                                                oi.Place(m, p);
                                            }
                                        }
                                    }
                                }

                                trans.Commit();

                                responseText = jsonSerializer.Serialize(new { OrderID = oid });
                            }
                        }
                        else
                        {
                            throw new Exception("当前用户处于未登录状态");
                        }
                    }
                    else
                    {
                        // Request was sent incorrectly somehow
                        throw new Exception("客户端应发起POST请求");
                    }
                }
                catch (Exception ex)
                {
                    trans.Rollback();

                    responseText = jsonSerializer.Serialize(new { result = "error", error_msg = ex.Message });
                }
            }

            context.Response.Clear();
            context.Response.ContentType = "text/json";
            context.Response.Write(responseText);
            context.Response.End();
        }

        private class Item
        {
            public string code { get; set; }
            public string price { get; set; }
        }

        private OrderBaseType? OrderType
        {
            get
            {
                if (!string.IsNullOrEmpty(Context.Request.QueryString["OrderType"]))
                {
                    OrderBaseType type;
                    if (Enum.TryParse(Context.Request.QueryString["OrderType"], out type))
                    { return type; }
                }

                return null;
            }
        }
    }
}
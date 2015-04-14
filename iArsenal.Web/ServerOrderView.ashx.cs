using System;
using System.Web;

using iArsenal.Entity;

namespace iArsenal.Web
{
    public class ServerOrderView : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string _strOrderID = context.Request.QueryString["OrderID"];
                int _orderID = int.MinValue;

                if (!string.IsNullOrEmpty(_strOrderID) && int.TryParse(_strOrderID, out _orderID))
                {
                    Order o = Order.Select(_orderID);

                    if (o != null)
                    {
                        if (!string.IsNullOrEmpty(o.URLOrderView))
                        {
                            context.Response.Redirect(string.Format("{0}?OrderID={1}", o.URLOrderView, o.OrderID.ToString()), false);
                            context.ApplicationInstance.CompleteRequest();
                        }
                        else
                        {
                            throw new Exception("No Page");
                        }
                    }
                    else
                    {
                        throw new Exception("No Valid Order");
                    }
                }
            }
            catch
            {
                context.Response.Redirect("iArsenalOrder.aspx", false);
                context.ApplicationInstance.CompleteRequest();
            }
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}
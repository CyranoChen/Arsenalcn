using System;
using System.Web;
using iArsenal.Service;

namespace iArsenal.Web
{
    public class ServerOrderView : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                var strOrderId = context.Request.QueryString["OrderID"];
                int orderId;

                if (!string.IsNullOrEmpty(strOrderId) && int.TryParse(strOrderId, out orderId))
                {
                    var o = Order.Select(orderId);

                    if (o != null)
                    {
                        if (!string.IsNullOrEmpty(o.UrlOrderView))
                        {
                            context.Response.Redirect($"{o.UrlOrderView}?OrderID={o.ID}", false);
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

        public bool IsReusable => true;
    }
}
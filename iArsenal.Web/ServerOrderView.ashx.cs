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
                var _strOrderID = context.Request.QueryString["OrderID"];
                var _orderID = int.MinValue;

                if (!string.IsNullOrEmpty(_strOrderID) && int.TryParse(_strOrderID, out _orderID))
                {
                    var o = Order.Select(_orderID);

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

        public bool IsReusable
        {
            get { return true; }
        }
    }
}
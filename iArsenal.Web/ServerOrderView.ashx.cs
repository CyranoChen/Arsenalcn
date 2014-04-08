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
                if (!string.IsNullOrEmpty(context.Request.QueryString["OrderID"]))
                {
                    int orderID = Convert.ToInt32(context.Request.QueryString["OrderID"]);

                    OrderBase o = OrderBase.Select(orderID);

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
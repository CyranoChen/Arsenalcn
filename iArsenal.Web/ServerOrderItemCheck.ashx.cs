using System;
using System.Web;
using System.Web.Script.Serialization;

using iArsenal.Entity;

namespace iArsenal.Web
{
    public class ServerOrderItemCheck : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string responseText = string.Empty;

            if (!string.IsNullOrEmpty(context.Request.QueryString["OrderItem"]))
            {
                try
                {
                    OrderItem oi = new OrderItem();

                    JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                    responseText = jsonSerializer.Serialize(oi);

                }
                catch (Exception ex)
                {
                    responseText = string.Format("{{  \"result\": \"error\", \"error_msg\": \"{0}\" }}", ex.Message);
                }
            }

            //responseText = "{  \"result\": \"success\",  \"ProductGuid\": \"cyrano\" }";

            context.Response.Clear();
            context.Response.ContentType = "text/plain";
            context.Response.Write(responseText);
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
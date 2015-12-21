using System;
using System.Web;
using System.Web.Script.Serialization;

using iArsenal.Service;

namespace iArsenal.Web
{
    public class ServerOrderItemCheck : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var responseText = string.Empty;

            if (!string.IsNullOrEmpty(context.Request.QueryString["OrderItem"]))
            {
                try
                {
                    var oi = new OrderItem();

                    var jsonSerializer = new JavaScriptSerializer();
                    responseText = jsonSerializer.Serialize(oi);

                }
                catch (Exception ex)
                {
                    responseText = $"{{  \"result\": \"error\", \"error_msg\": \"{ex.Message}\" }}";
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
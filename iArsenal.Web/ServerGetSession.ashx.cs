using System;
using System.Web;
using System.Web.Script.Serialization;

namespace iArsenal.Web
{
    public class ServerGetSession : HandlerBase
    {
        public override void ProcessRequest(HttpContext context)
        {
            string responseText;

            try
            {
                if (Uid > 0 && !string.IsNullOrEmpty(Username))
                {
                    var jsonSerializer = new JavaScriptSerializer();
                    responseText = jsonSerializer.Serialize(new { UserID = Uid, Username, SessionKey });
                }
                else
                {
                    throw new Exception("当前用户处于未登录状态");
                }

            }
            catch (Exception ex)
            {
                responseText = $"{{  \"result\": \"error\", \"error_msg\": \"{ex.Message}\" }}";
            }

            context.Response.Clear();
            context.Response.ContentType = "text/plain";
            context.Response.Write(responseText);
            context.Response.End();
        }
    }
}
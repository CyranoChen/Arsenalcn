using System;
using System.Web;
using System.Web.Script.Serialization;
using Arsenalcn.Core;
using iArsenal.Service;

namespace iArsenal.Web
{
    public class ServerMemberCheck : IHttpHandler
    {
        private readonly IRepository repo = new Repository();

        public void ProcessRequest(HttpContext context)
        {
            var responseText = string.Empty;

            if (!string.IsNullOrEmpty(context.Request.QueryString["MemberID"]))
            {
                try
                {
                    var mID = context.Request.QueryString["MemberID"];

                    var m = repo.Single<Member>(Convert.ToInt32(mID));

                    if (m != null)
                    {
                        var jsonSerializer = new JavaScriptSerializer();
                        responseText = jsonSerializer.Serialize(m);
                    }
                    else
                    {
                        throw new Exception("invalid Member ID");
                    }
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
            get { return true; }
        }
    }
}
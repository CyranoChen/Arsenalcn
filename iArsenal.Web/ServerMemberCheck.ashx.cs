using System;
using System.Web;
using System.Web.Script.Serialization;

using iArsenal.Entity;

namespace iArsenal.Web
{
    public class ServerMemberCheck : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string responseText = string.Empty;

            if (!string.IsNullOrEmpty(context.Request.QueryString["MemberID"]))
            {
                try
                {
                    string mID = context.Request.QueryString["MemberID"];

                    Member m = new Member();
                    m.MemberID = Convert.ToInt32(mID);
                    m.Select();

                    if (m != null)
                    {
                        JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                        responseText = jsonSerializer.Serialize(m);
                    }
                    else
                    {
                        throw new Exception("invalid Member ID");
                    }
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
                return true;
            }
        }
    }
}
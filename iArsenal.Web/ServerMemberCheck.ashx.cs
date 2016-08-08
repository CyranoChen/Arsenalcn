using System;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Arsenalcn.Core;
using iArsenal.Service;

namespace iArsenal.Web
{
    public class ServerMemberCheck : IHttpHandler
    {
        private readonly IRepository _repo = new Repository();

        public void ProcessRequest(HttpContext context)
        {
            var responseText = string.Empty;

            if (!string.IsNullOrEmpty(context.Request.QueryString["MemberID"]))
            {
                try
                {
                    var id = context.Request.QueryString["MemberID"];

                    var m = _repo.Single<Member>(Convert.ToInt32(id));

                    if (m != null)
                    {
                        var jsonSerializer = new JavaScriptSerializer();
                        responseText = jsonSerializer.Serialize(new { m.Name });
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
            else if (!string.IsNullOrEmpty(context.Request.QueryString["AcnID"]))
            {
                try
                {
                    var id = context.Request.QueryString["AcnID"];

                    var m = _repo.Query<Member>(x => x.AcnID == Convert.ToInt32(id)).FirstOrDefault();

                    if (m != null)
                    {
                        var jsonSerializer = new JavaScriptSerializer();
                        responseText = jsonSerializer.Serialize(new { m.ID, m.AcnName, m.Name, m.Mobile, m.Email });
                    }
                    else
                    {
                        throw new Exception("invalid Acn ID");
                    }
                }
                catch (Exception ex)
                {
                    responseText = $"{{  \"result\": \"error\", \"error_msg\": \"{ex.Message}\" }}";
                }
            }

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
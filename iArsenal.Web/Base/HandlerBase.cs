using System.Web;

namespace iArsenal.Web
{
    public class HandlerBase : IHttpHandler
    {
        protected readonly HttpContext Context = HttpContext.Current;

        public virtual void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
        }

        protected int Uid
        {
            get
            {
                if (!string.IsNullOrEmpty(Context.Request.Cookies["uid"]?.Value))
                {
                    //already login
                    return int.Parse(Context.Request.Cookies["uid"].Value);
                }
                return -1;
            }
        }

        protected string Username
        {
            get
            {
                if (!string.IsNullOrEmpty(Context.Request.Cookies["user_name"]?.Value))
                { return HttpUtility.UrlDecode(Context.Request.Cookies["user_name"].Value); }

                return string.Empty;
            }
        }

        protected string SessionKey
        {
            get
            {
                if (!string.IsNullOrEmpty(Context.Request.Cookies["session_key"]?.Value))
                { return Context.Request.Cookies["session_key"].Value; }

                return string.Empty;
            }
        }

        protected int Mid
        {
            get
            {
                if (!string.IsNullOrEmpty(Context.Request.Cookies["mid"]?.Value))
                {
                    //already login
                    return int.Parse(Context.Request.Cookies["mid"].Value);
                }

                return -1;
            }
        }

        protected string MemberName
        {
            get
            {
                if (!string.IsNullOrEmpty(Context.Request.Cookies["member_name"]?.Value))
                {
                    return HttpUtility.UrlDecode(Context.Request.Cookies["member_name"].Value);
                }

                return string.Empty;
            }
        }

        public virtual bool IsReusable => true;
    }
}
using System.Web;

namespace iArsenal.Web
{
    public class HandlerBase: IHttpHandler
    {
        private readonly HttpContext _context = HttpContext.Current;

        public virtual void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
        }

        protected int Uid
        {
            get
            {
                if (!string.IsNullOrEmpty(_context.Request.Cookies["uid"]?.Value))
                {
                    //already login
                    return int.Parse(_context.Request.Cookies["uid"].Value);
                }
                return -1;
            }
        }

        protected string Username
        {
            get
            {
                if (!string.IsNullOrEmpty(_context.Request.Cookies["user_name"]?.Value))
                { return HttpUtility.UrlDecode(_context.Request.Cookies["user_name"].Value); }

                return string.Empty;
            }
        }

        protected string SessionKey
        {
            get
            {
                if (!string.IsNullOrEmpty(_context.Request.Cookies["session_key"]?.Value))
                { return _context.Request.Cookies["session_key"].Value; }

                return string.Empty;
            }
        }

        public virtual bool IsReusable => true;
    }
}
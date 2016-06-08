using System.Web.Mvc;

namespace Arsenal.Mobile.Extensions
{
    public class JsonpResult : JsonResult
    {
        public JsonpResult()
        {
            JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        }

        public string Callback { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            var httpContext = context.HttpContext;
            var callback = Callback;

            if (string.IsNullOrWhiteSpace(callback))
                callback = httpContext.Request["callback"];

            httpContext.Response.Write(callback + "(");
            base.ExecuteResult(context);
            httpContext.Response.Write(");");
        }
    }
}
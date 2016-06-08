using System;
using System.Web;

namespace Arsenal.Mobile.Extensions
{
    public static class JsonRequestExtensions
    {
        public static bool IsJsonRequest(this HttpRequestBase request)
        {
            return string.Equals(request["format"], "json", StringComparison.OrdinalIgnoreCase);
        }
    }
}
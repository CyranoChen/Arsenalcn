using System;
using System.Web;

namespace Arsenalcn.Core
{
    public static class JsonRequestExtensions
    {
        public static bool IsJsonRequest(this HttpRequestBase request)
        {
            return string.Equals(request["format"], "json");
        }
    }
}
using System;
using System.Web;
using System.Web.Script.Serialization;
using Arsenalcn.Core;

namespace iArsenal.Web
{
    public class ServerRegionCheck : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var responseText = string.Empty;

            if (!string.IsNullOrEmpty(context.Request.QueryString["RegionID"]))
            {
                try
                {
                    var rid = int.Parse(context.Request.QueryString["RegionID"]);

                    var itemList = DictionaryItem.Cache.DictionaryItemListRegion;

                    itemList = itemList.FindAll(item => item.ParentID.Equals(rid));
                    itemList.RemoveAll(
                        item =>
                            item.Name.Equals("市辖区") || item.Name.Equals("县") || item.Name.Equals("市") ||
                            item.Name.Contains("行政单位"));

                    if (itemList.Count > 0)
                    {
                        var jsonSerializer = new JavaScriptSerializer();
                        responseText = jsonSerializer.Serialize(itemList);
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
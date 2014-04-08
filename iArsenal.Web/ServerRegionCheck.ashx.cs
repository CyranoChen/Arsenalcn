using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Script.Serialization;

using Arsenalcn.Common.Entity;

namespace iArsenal.Web
{
    public class ServerRegionCheck : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string responseText = string.Empty;

            if (!string.IsNullOrEmpty(context.Request.QueryString["RegionID"]))
            {
                try
                {
                    int rid = int.Parse(context.Request.QueryString["RegionID"]);

                    List<DictionaryItem> itemList = DictionaryItem.Cache.DictionaryItemList_Region;

                    itemList = itemList.FindAll(item => item.ParentID.Equals(rid));
                    itemList.RemoveAll(item => item.Name.Equals("市辖区") || item.Name.Equals("县") || item.Name.Equals("市") || item.Name.Contains("行政单位"));

                    if (itemList != null)
                    {
                        JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                        responseText = jsonSerializer.Serialize(itemList);
                    }
                }
                catch (Exception ex)
                {
                    responseText = string.Format("{{  \"result\": \"error\", \"error_msg\": \"{0}\" }}", ex.Message);
                }
            }

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
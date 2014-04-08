using System.Collections.Generic;
using System.Web;

using Arsenalcn.Common.Entity;

namespace iArsenal.Web
{
    public class ServerGetRegion : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string responseText = string.Empty;
            string xmlContent = string.Empty;

            if (!string.IsNullOrEmpty(context.Request.QueryString["RegionID"]))
            {
                int regionID = int.MinValue;
                if (int.TryParse(context.Request.QueryString["RegionID"], out regionID))
                {
                    List<DictionaryItem> itemList = DictionaryItem.Cache.DictionaryItemList_Region;
                    
                    itemList = itemList.FindAll(delegate(DictionaryItem item) { return item.ParentID == regionID; });
                    itemList.RemoveAll(delegate(DictionaryItem item) { return item.Name == "市辖区" || item.Name == "县" || item.Name == "市"; });

                    if (itemList.Count > 0)
                    {
                        foreach (DictionaryItem item in itemList)
                        {
                            xmlContent += string.Format("<RegionItem ID=\"{0}\" Name=\"{1}\" Code=\"{2}\" OrderNum=\"{3}\" />", item.ItemID, item.Name, item.Code, item.OrderNum);
                        }

                        responseText = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?><Region>{0}</Region>", xmlContent);
                    }
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
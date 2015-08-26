using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Script.Serialization;

using iArsenal.Service;

namespace iArsenal.Web
{
    public class ServerProductCheck : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var responseText = string.Empty;

            if (!string.IsNullOrEmpty(context.Request.QueryString["ProductCode"]))
            {
                try
                {
                    var code = context.Request.QueryString["ProductCode"];

                    var p = Product.Cache.Load(code);

                    if (p != null)
                    {
                        var jsonSerializer = new JavaScriptSerializer();
                        responseText = jsonSerializer.Serialize(p);
                    }
                    else
                    {
                        throw new Exception("invalid product code");
                    }
                }
                catch (Exception ex)
                {
                    responseText = string.Format("{{  \"result\": \"error\", \"error_msg\": \"{0}\" }}", ex.Message);
                }
            }

            if (!string.IsNullOrEmpty(context.Request.QueryString["ProductGuid"]))
            {
                try
                {
                    var guid = new Guid(context.Request.QueryString["ProductGuid"]);

                    var p = Product.Cache.Load(guid);

                    if (p != null)
                    {
                        var jsonSerializer = new JavaScriptSerializer();
                        responseText = jsonSerializer.Serialize(p);
                    }
                    else
                    {
                        throw new Exception("invalid product guid");
                    }
                }
                catch (Exception ex)
                {
                    responseText = string.Format("{{  \"result\": \"error\", \"error_msg\": \"{0}\" }}", ex.Message);
                }
            }

            if (!string.IsNullOrEmpty(context.Request.QueryString["IsActive"]) && !string.IsNullOrEmpty(context.Request.QueryString["ProductType"]))
            {
                try
                {
                    var _isActive = bool.Parse(context.Request.QueryString["IsActive"]);
                    var _productType = int.Parse(context.Request.QueryString["ProductType"]);

                    var list = Product.Cache.ProductList.FindAll(
                        p => p.IsActive.Equals(_isActive) && ((int)p.ProductType).Equals(_productType)); ;

                    if (list != null && list.Count > 0)
                    {
                        var alCode = new ArrayList();

                        foreach (var p in list) { alCode.Add(p.Code); }

                        var jsonSerializer = new JavaScriptSerializer();
                        responseText = jsonSerializer.Serialize(alCode);
                    }
                    else
                    {
                        responseText = string.Empty;
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
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Xml.Serialization;

using ArsenalPlayer = Arsenal.Entity.Player;

namespace Arsenalcn.ClubSys.Web
{
    public class ServerXML : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string responseText = string.Empty;
            StringWriter xmlContent = new StringWriter();

            Guid? aPlayerGuid = null;
            if (!string.IsNullOrEmpty(context.Request.QueryString["PlayerGuid"]))
            {
                try { aPlayerGuid = new Guid(context.Request.QueryString["PlayerGuid"]); }
                catch { aPlayerGuid = null; }
            }

            if (aPlayerGuid.HasValue)
            {
                ArsenalPlayer p = ArsenalPlayer.Cache.Load(aPlayerGuid.Value);

                XmlSerializer xml = new XmlSerializer(typeof(ArsenalPlayer));
                xml.Serialize(xmlContent, p);
            }

            if (!string.IsNullOrEmpty(xmlContent.ToString()))
                responseText = xmlContent.ToString();

            context.Response.Clear();
            context.Response.ContentType = "text/xml";
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
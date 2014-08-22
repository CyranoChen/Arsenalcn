using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

using Arsenalcn.ClubSys.Service;

namespace Arsenalcn.ClubSys.Web
{
    public partial class ServerGoogleAdv : Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["url"]))
            {
                string url = HttpUtility.UrlDecode(Request.QueryString["url"], Encoding.UTF8);
                string retValue = CrawlPageContent(url);
                retValue = retValue.Replace("href=\"/", "href=\"http://googleads.g.doubleclick.net/");
                retValue = retValue.Replace("src=\"/", "src=\"http://googleads.g.doubleclick.net/");
                retValue = retValue.Replace("target=\"_top\"", "target=\"_blank\"");
                retValue = retValue.Replace("onclick=\"ha('aw", "onclick=\"parent.GoogleAdvClick(this.href);ha('aw");
                retValue = retValue.Replace("onClick=\"ha('aw", "onClick=\"parent.GoogleAdvClick(this.href);ha('aw");

                if (retValue.IndexOf("google_flash_obj") != -1)
                {
                    string clickURL = retValue.Substring(retValue.IndexOf("clickTAG=") + 9);
                    clickURL = clickURL.Substring(0, clickURL.IndexOf("\""));
                    clickURL = HttpUtility.UrlDecode(clickURL, Encoding.UTF8);
                    clickURL = string.Format("<a href=\"{0}\" target=\"_blank\" onclick=\"parent.GoogleAdvClick(this.href);\" style=\"position:absolute;left:0px;z-index:1101;display:block;width:468px;height:60px;background:#fff;filter:alpha(opacity=1);-moz-opacity:0.01;opacity:0.01;\"></a>", clickURL.Replace("\r\n", ""));

                    retValue = retValue.Replace("<div id=\"google_flash_div", string.Format("{0}<div id=\"google_flash_div", clickURL));
                }

                Response.Clear();
                Response.ContentType = "text/html";
                Response.Charset = "UTF-8";
                Response.Write(retValue);
                Response.End();
            }
            else if (!string.IsNullOrEmpty(Request.QueryString["logAdv"]))
            {
                bool isLog = false;
                bool.TryParse(Request.QueryString["logAdv"], out isLog);

                string advURL = string.Empty;
                if (!string.IsNullOrEmpty(Request.QueryString["advURL"]))
                    advURL = HttpUtility.UrlDecode(Request.QueryString["advURL"], Encoding.UTF8);

                Response.Clear();
                Response.ContentType = "text/plain";

                try
                {
                    if (isLog && this.userid != -1)
                    {
                        AdvLog.LogHistory(this.userid, this.username, AdvHistoryType.GoogleAdv, advURL, Request.UserHostAddress.ToString());
                        Response.Write("success");
                    }
                    else
                        Response.Write("failed");
                }
                catch
                {
                    Response.Write("failed");
                }

                Response.End();
            }
        }

        public string CrawlPageContent(string url)
        {
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;

            if (request != null)
            {
                StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream(), Encoding.GetEncoding("UTF-8"));

                return reader.ReadToEnd();
            }
            else
                return string.Empty;
        }

        public string CrawlPageContent(string url, string begin, string end, bool includeBegin, bool includeEnd)
        {
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;

            if (request != null)
            {
                StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream(), Encoding.GetEncoding("UTF-8"));

                string pageContent = reader.ReadToEnd();
                string retValue = string.Empty;
                if (begin != string.Empty && end != string.Empty)
                    retValue = GetStringBetweenBeginAndEnd(pageContent, begin, end);

                return string.Format("{0}{1}{2}",
                    includeBegin ? begin : string.Empty,
                    retValue,
                    includeEnd ? end : string.Empty);
            }
            else
            {
                return string.Empty;
            }
        }

        private string GetStringBetweenBeginAndEnd(string input, string begin, string end)
        {
            string pattern = string.Format("(?<={0})[\\W\\w]*?(?={1})", begin, end);
            if (Regex.IsMatch(input, pattern))
                return Regex.Match(input, pattern).Value;
            else
                return string.Empty;
        }
    }
}

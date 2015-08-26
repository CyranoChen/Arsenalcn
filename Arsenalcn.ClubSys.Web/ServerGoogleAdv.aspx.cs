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
                var url = HttpUtility.UrlDecode(Request.QueryString["url"], Encoding.UTF8);
                var retValue = CrawlPageContent(url);
                retValue = retValue.Replace("href=\"/", "href=\"http://googleads.g.doubleclick.net/");
                retValue = retValue.Replace("src=\"/", "src=\"http://googleads.g.doubleclick.net/");
                retValue = retValue.Replace("target=\"_top\"", "target=\"_blank\"");
                retValue = retValue.Replace("onclick=\"ha('aw", "onclick=\"parent.GoogleAdvClick(this.href);ha('aw");
                retValue = retValue.Replace("onClick=\"ha('aw", "onClick=\"parent.GoogleAdvClick(this.href);ha('aw");

                if (retValue.IndexOf("google_flash_obj") != -1)
                {
                    var clickURL = retValue.Substring(retValue.IndexOf("clickTAG=") + 9);
                    clickURL = clickURL.Substring(0, clickURL.IndexOf("\""));
                    clickURL = HttpUtility.UrlDecode(clickURL, Encoding.UTF8);
                    clickURL =
                        $"<a href=\"{clickURL.Replace("\r\n", "")}\" target=\"_blank\" onclick=\"parent.GoogleAdvClick(this.href);\" style=\"position:absolute;left:0px;z-index:1101;display:block;width:468px;height:60px;background:#fff;filter:alpha(opacity=1);-moz-opacity:0.01;opacity:0.01;\"></a>";

                    retValue = retValue.Replace("<div id=\"google_flash_div", $"{clickURL}<div id=\"google_flash_div");
                }

                Response.Clear();
                Response.ContentType = "text/html";
                Response.Charset = "UTF-8";
                Response.Write(retValue);
                Response.End();
            }
            else if (!string.IsNullOrEmpty(Request.QueryString["logAdv"]))
            {
                var isLog = false;
                bool.TryParse(Request.QueryString["logAdv"], out isLog);

                var advURL = string.Empty;
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

        public static string CrawlPageContent(string url)
        {
            var request = HttpWebRequest.Create(url) as HttpWebRequest;

            if (request != null)
            {
                var reader = new StreamReader(request.GetResponse().GetResponseStream(), Encoding.GetEncoding("UTF-8"));

                return reader.ReadToEnd();
            }
            else
                return string.Empty;
        }

        public static string CrawlPageContent(string url, string begin, string end, bool includeBegin, bool includeEnd)
        {
            var request = HttpWebRequest.Create(url) as HttpWebRequest;

            if (request != null)
            {
                var reader = new StreamReader(request.GetResponse().GetResponseStream(), Encoding.GetEncoding("UTF-8"));

                var pageContent = reader.ReadToEnd();
                var retValue = string.Empty;
                if (begin != string.Empty && end != string.Empty)
                    retValue = GetStringBetweenBeginAndEnd(pageContent, begin, end);

                return $"{(includeBegin ? begin : string.Empty)}{retValue}{(includeEnd ? end : string.Empty)}";
            }
            else
            {
                return string.Empty;
            }
        }

        private static string GetStringBetweenBeginAndEnd(string input, string begin, string end)
        {
            var pattern = $"(?<={begin})[\\W\\w]*?(?={end})";
            if (Regex.IsMatch(input, pattern))
                return Regex.Match(input, pattern).Value;
            else
                return string.Empty;
        }
    }
}

using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Xml;
using Arsenal.Service;
using Arsenalcn.Core.Logger;
using Arsenalcn.Core.Utility;

namespace Arsenal.Web
{
    public class ServerACNCallback : IHttpHandler
    {
        private readonly ILog _log = new UserLog();

        public void ProcessRequest(HttpContext context)
        {
            string authToken;
            var nextUrl = "/default.aspx";
            string gotoURL;

            try
            {
                if (!string.IsNullOrEmpty(context.Request.QueryString["auth_token"]))
                    authToken = context.Request.QueryString["auth_token"];
                else
                    throw new Exception("auth_token is invalid");

                if (!string.IsNullOrEmpty(context.Request.QueryString["next"]))
                    nextUrl = context.Request.QueryString["next"];

                //New HttpWebRequest for DiscuzNT Service API
                var req = (HttpWebRequest)WebRequest.Create(ConfigGlobal_Arsenal.APIServiceURL);

                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";

                //Gen Digital Signature
                var sig = string.Format("api_key={0}auth_token={1}method={2}{3}", ConfigGlobal_Arsenal.APIAppKey, authToken,
                    "auth.getSession", ConfigGlobal_Arsenal.APICryptographicKey);

                //Set WebRequest Parameter
                var para = string.Format("method={0}&api_key={1}&auth_token={2}&sig={3}", "auth.getSession",
                    ConfigGlobal_Arsenal.APIAppKey, authToken, getMd5Hash(sig));

                var encodedBytes = Encoding.UTF8.GetBytes(para);
                req.ContentLength = encodedBytes.Length;

                // Write encoded data into request stream
                var requestStream = req.GetRequestStream();
                requestStream.Write(encodedBytes, 0, encodedBytes.Length);
                requestStream.Close();

                using (var response = req.GetResponse())
                {
                    var receiveStream = response.GetResponseStream();
                    var readStream = new StreamReader(receiveStream, Encoding.UTF8);
                    var responseResult = readStream.ReadToEnd();

                    if (!string.IsNullOrEmpty(responseResult))
                    {
                        var xml = new XmlDocument();
                        var sr = new StringReader(responseResult);
                        xml.Load(sr);

                        //Build Member & ACNUser Cookie Information
                        if (xml.HasChildNodes && xml.FirstChild.NextSibling != null && !xml.FirstChild.NextSibling.Name.Equals("error_response"))
                        {
                            var nodeSessionKey = xml.GetElementsByTagName("session_key").Item(0);
                            var nodeUid = xml.GetElementsByTagName("uid").Item(0);
                            var nodeUserName = xml.GetElementsByTagName("user_name").Item(0);

                            if (nodeSessionKey != null && nodeUid != null && nodeUserName != null)
                            {
                                context.Response.SetCookie(new HttpCookie("session_key", nodeSessionKey.InnerText));
                                context.Response.SetCookie(new HttpCookie("uid", nodeUid.InnerText));
                                context.Response.SetCookie(new HttpCookie("user_name", HttpUtility.UrlEncode(nodeUserName.InnerText)));

                                var logPara = new LogInfo
                                {
                                    MethodInstance = MethodBase.GetCurrentMethod(),
                                    ThreadInstance = Thread.CurrentThread,
                                    UserClient = new UserClientInfo
                                    {
                                        UserID = Convert.ToInt32(nodeUid.InnerText),
                                        UserName = HttpUtility.UrlEncode(nodeUserName.InnerText),
                                        UserIP = IPLocation.GetIP(),
                                        UserBrowser = BrowserInfo.GetBrowser(),
                                        UserOS = OSInfo.GetOS()
                                    }
                                };

                                _log.Info("ACN用户验证登录成功", logPara);
                            }
                        }
                        else
                        {
                            var nodeErrorCode = xml.GetElementsByTagName("error_code").Item(0);
                            var nodeMsg = xml.GetElementsByTagName("error_msg").Item(0);
                            if (nodeErrorCode != null && nodeMsg != null)
                            {
                                throw new Exception($"({nodeErrorCode.InnerText}) {nodeMsg.InnerText}");
                            }
                        }

                        gotoURL = nextUrl;
                    }
                    else
                    {
                        gotoURL =
                            $"{ConfigGlobal_Arsenal.APILoginURL}?api_key={ConfigGlobal_Arsenal.APIAppKey}&next={nextUrl}";
                    }

                    context.Response.Redirect(gotoURL, false);
                    context.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                var logPara = new LogInfo
                {
                    MethodInstance = MethodBase.GetCurrentMethod(),
                    ThreadInstance = Thread.CurrentThread,
                    UserClient = new UserClientInfo
                    {
                        UserID = -1,
                        UserName = string.Empty,
                        UserIP = IPLocation.GetIP(),
                        UserBrowser = BrowserInfo.GetBrowser(),
                        UserOS = OSInfo.GetOS()
                    }
                };

                _log.Warn(ex, logPara);

                context.Response.Redirect(nextUrl);
            }
        }

        public bool IsReusable => true;

        private string getMd5Hash(string input)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            var md5Hasher = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (var i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;

using iArsenal.Service;

namespace iArsenal.Web
{
    public class ServerACNCallback : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var authToken = string.Empty;
            var nextURL = "/default.aspx";
            var gotoURL = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(context.Request.QueryString["auth_token"]))
                    authToken = context.Request.QueryString["auth_token"];
                else
                    throw new Exception("auth_token is invalid");

                if (!string.IsNullOrEmpty(context.Request.QueryString["next"]))
                    nextURL = context.Request.QueryString["next"];

                //New HttpWebRequest for DiscuzNT Service API
                var req = (HttpWebRequest)WebRequest.Create(ConfigGlobal.APIServiceURL);

                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";

                //Gen Digital Signature
                var sig =
                    $"api_key={ConfigGlobal.APIAppKey}auth_token={authToken}method={"auth.getSession"}{ConfigGlobal.APICryptographicKey}";

                //Set WebRequest Parameter
                var para =
                    $"method={"auth.getSession"}&api_key={ConfigGlobal.APIAppKey}&auth_token={authToken}&sig={getMd5Hash(sig)}";

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
                        if (xml.HasChildNodes && !xml.FirstChild.NextSibling.Name.Equals("error_response"))
                        {
                            context.Response.SetCookie(new HttpCookie("session_key", xml.GetElementsByTagName("session_key").Item(0).InnerText.Trim()));
                            context.Response.SetCookie(new HttpCookie("uid", xml.GetElementsByTagName("uid").Item(0).InnerText.Trim()));
                            context.Response.SetCookie(new HttpCookie("user_name", HttpUtility.UrlEncode(xml.GetElementsByTagName("user_name").Item(0).InnerText.Trim())));
                        }
                        else
                        {
                            var error_code = xml.GetElementsByTagName("error_code").Item(0).InnerText;
                            var error_msg = xml.GetElementsByTagName("error_msg").Item(0).InnerText;
                            throw new Exception($"({error_code}) {error_msg}");
                        }

                        gotoURL = nextURL.Contains("default.aspx?method=logout") ? "/default.aspx" : nextURL;
                    }
                    else
                    {
                        gotoURL = $"{ConfigGlobal.APILoginURL}?api_key={ConfigGlobal.APIAppKey}&next={nextURL}";
                    }

                    context.Response.Redirect(gotoURL, false);
                    context.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                var errorMsg = ex.Message.ToString();
                context.Response.Redirect(nextURL);
            }
        }

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

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}
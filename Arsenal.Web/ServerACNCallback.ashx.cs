using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;

using Arsenal.Service;

namespace Arsenal.Web
{
    public class ServerACNCallback : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string authToken = string.Empty;
            string nextURL = "/default.aspx";
            string gotoURL = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(context.Request.QueryString["auth_token"]))
                    authToken = context.Request.QueryString["auth_token"];
                else
                    throw new Exception("auth_token is invalid");

                if (!string.IsNullOrEmpty(context.Request.QueryString["next"]))
                    nextURL = context.Request.QueryString["next"];

                //New HttpWebRequest for DiscuzNT Service API
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(ConfigGlobal.APIServiceURL);

                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";

                //Gen Digital Signature
                string sig = string.Format("api_key={0}auth_token={1}method={2}{3}", ConfigGlobal.APIAppKey, authToken, "auth.getSession", ConfigGlobal.APICryptographicKey);

                //Set WebRequest Parameter
                string para = string.Format("method={0}&api_key={1}&auth_token={2}&sig={3}", "auth.getSession", ConfigGlobal.APIAppKey, authToken, getMd5Hash(sig));

                byte[] encodedBytes = Encoding.UTF8.GetBytes(para);
                req.ContentLength = encodedBytes.Length;

                // Write encoded data into request stream
                Stream requestStream = req.GetRequestStream();
                requestStream.Write(encodedBytes, 0, encodedBytes.Length);
                requestStream.Close();

                using (var response = req.GetResponse())
                {
                    var receiveStream = response.GetResponseStream();
                    StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                    string responseResult = readStream.ReadToEnd();

                    if (!string.IsNullOrEmpty(responseResult))
                    {
                        XmlDocument xml = new XmlDocument();
                        StringReader sr = new StringReader(responseResult);
                        xml.Load(sr);

                        //Build Member & ACNUser Cookie Information
                        if (xml.HasChildNodes && !xml.FirstChild.NextSibling.Name.Equals("error_response"))
                        {
                            context.Response.SetCookie(new HttpCookie("session_key", xml.GetElementsByTagName("session_key").Item(0).InnerText));
                            context.Response.SetCookie(new HttpCookie("uid", xml.GetElementsByTagName("uid").Item(0).InnerText));
                            context.Response.SetCookie(new HttpCookie("user_name", HttpUtility.UrlEncode(xml.GetElementsByTagName("user_name").Item(0).InnerText)));
                        }
                        else
                        {
                            string error_code = xml.GetElementsByTagName("error_code").Item(0).InnerText;
                            string error_msg = xml.GetElementsByTagName("error_msg").Item(0).InnerText;
                            throw new Exception(string.Format("({0}) {1}", error_code, error_msg));
                        }

                        gotoURL = nextURL;
                    }
                    else
                    {
                        gotoURL = string.Format("{0}?api_key={1}&next={2}", ConfigGlobal.APILoginURL, ConfigGlobal.APIAppKey, nextURL);
                    }

                    context.Response.Redirect(gotoURL, false);
                    context.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message.ToString();
                context.Response.Redirect(nextURL);
            }
        }

        private string getMd5Hash(string input)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5 md5Hasher = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
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
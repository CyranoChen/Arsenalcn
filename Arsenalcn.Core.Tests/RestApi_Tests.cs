using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Arsenal.Service;
using Arsenalcn.Core;
using System.Web;
using Arsenalcn.Core.Utility;

namespace Arsenalcn.Core.Tests
{
    [TestClass()]
    public class RestApi_Tests
    {
        [TestInitialize]
        public void MyTestInitialize()
        {
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://localhost", ""), new HttpResponse(new StringWriter(new StringBuilder())));
        }

        [TestMethod()]
        public void ApiValidate_Test()
        {
            string authToken = string.Empty;
            //string nextURL = "/default.aspx";
            string gotoURL = string.Empty;

            string _apiServiceUrl = "http://vm-win2008r2/services/restserver.aspx";
            string _apiAppKey = "e5b551b11b65fd03bf8e9afe14a092c5";
            string _apiCryptographicKey = "68a9b3a904bc09ce89a62310e9ebbd3c";
            string _method = "auth.validate";
            //string _strAuthToken = !string.IsNullOrEmpty(authToken) ? string.Format("authToken={0}", authToken) : string.Empty;

            string _username = "cyrano";
            string _password = "linfeng";

            try
            {
                //if (!string.IsNullOrEmpty(context.Request.QueryString["auth_token"]))
                //    authToken = context.Request.QueryString["auth_token"];
                //else
                //    throw new Exception("auth_token is invalid");

                //if (!string.IsNullOrEmpty(context.Request.QueryString["next"]))
                //    nextURL = context.Request.QueryString["next"];

                //New HttpWebRequest for DiscuzNT Service API
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(_apiServiceUrl);

                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";

                //Gen Digital Signature
                string sig = string.Format("api_key={0}format=jsonmethod={1}password={2}user_name={3}{4}",
                    _apiAppKey, _method, _password, _username, _apiCryptographicKey);

                //Set WebRequest Parameter
                string para = string.Format("method={0}&format=json&api_key={1}&sig={2}&user_name={3}&password={4}",
                    _method, _apiAppKey, Encrypt.getMd5Hash(sig), _username, _password);

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
                        Assert.AreEqual("\"443\"", responseResult);

                        //XmlDocument xml = new XmlDocument();
                        //StringReader sr = new StringReader(responseResult);
                        //xml.Load(sr);

                        //Build Member & ACNUser Cookie Information
                        //if (xml.HasChildNodes && !xml.FirstChild.NextSibling.Name.Equals("error_response"))
                        //{
                        //    context.Response.SetCookie(new HttpCookie("session_key", xml.GetElementsByTagName("session_key").Item(0).InnerText.Trim()));
                        //    context.Response.SetCookie(new HttpCookie("uid", xml.GetElementsByTagName("uid").Item(0).InnerText.Trim()));
                        //    context.Response.SetCookie(new HttpCookie("user_name", HttpUtility.UrlEncode(xml.GetElementsByTagName("user_name").Item(0).InnerText.Trim())));
                        //}
                        //else
                        //{
                        //    string error_code = xml.GetElementsByTagName("error_code").Item(0).InnerText;
                        //    string error_msg = xml.GetElementsByTagName("error_msg").Item(0).InnerText;
                        //    throw new Exception(string.Format("({0}) {1}", error_code, error_msg));
                        //}

                        //gotoURL = nextURL;

                        //Console.WriteLine(responseResult);
                    }
                    else
                    {
                        //gotoURL = string.Format("{0}?api_key={1}&next={2}", ConfigGlobal.APILoginURL, ConfigGlobal.APIAppKey, nextURL);
                    }

                    //context.Response.Redirect(gotoURL, false);
                    //context.ApplicationInstance.CompleteRequest();

                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
                //string errorMsg = ex.Message.ToString();
                //context.Response.Redirect(nextURL);
            }
        }

        [TestMethod()]
        public void ApiUsersGetID_Test()
        {
            var client = new DiscuzApiClient();

            var responseResult = client.UsersGetID("cyrano");

            Assert.AreEqual("\"443\"", responseResult);
        }

        [TestMethod()]
        public void ApiAuthValidate_Test()
        {
            var client = new DiscuzApiClient();

            var uid = client.AuthValidate("cyrano", Encrypt.getMd5Hash("linfeng"));

            Assert.AreEqual(443, Convert.ToInt32(uid.Replace("\"", "")));
        }

        [TestMethod()]
        public void ApiUsersGetInfo_Test()
        {
            var client = new DiscuzApiClient();

            int[] uids = { 443, 17650 };
            string[] fields = { "uid", "user_name", "password", "email", "mobile", "join_date" };

            var responseResult = client.UsersGetInfo(uids, fields);

            Assert.IsNotNull(responseResult);
        }

        [TestMethod()]
        public void WeChatGetAccessToken()
        {
            var client1 = new WeChatApiClient();

            var token1 = client1.AccessToken;

            var client2 = new WeChatApiClient();

            var token2 = client2.AccessToken;

            Assert.AreSame(token1, token2);
        }
    }
}

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
    public class ServerAcnCheck : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var responseText = string.Empty;

            if (!string.IsNullOrEmpty(context.Request.QueryString["AcnID"]) &&
                !string.IsNullOrEmpty(context.Request.QueryString["SessionKey"]))
            {
                try
                {
                    var acnID = context.Request.QueryString["AcnID"];
                    var sessionKey = context.Request.QueryString["SessionKey"];
                    float callID = DateTime.Now.Millisecond;
                    var fields = "user_name";

                    //New HttpWebRequest for DiscuzNT Service API
                    var req = (HttpWebRequest) WebRequest.Create(ConfigGlobal.APIServiceURL);

                    req.Method = "POST";
                    req.ContentType = "application/x-www-form-urlencoded";

                    //Gen Digital Signature
                    var sig =
                        $"api_key={ConfigGlobal.APIAppKey}call_id={callID}fields={fields}method={"users.getInfo"}session_key={sessionKey}uids={acnID}{ConfigGlobal.APICryptographicKey}";

                    //Set WebRequest Parameter
                    var para =
                        $"method={"users.getInfo"}&api_key={ConfigGlobal.APIAppKey}&session_key={sessionKey}&call_id={callID}&uids={acnID}&fields={fields}&sig={getMd5Hash(sig)}";

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

                            //Build ACNUser Information
                            if (xml.HasChildNodes && !xml.FirstChild.NextSibling.Name.Equals("error_response"))
                            {
                                responseText =
                                    $"{{  \"result\": \"success\",  \"username\": \"{xml.GetElementsByTagName("user_name").Item(0).InnerText}\" }}";
                            }
                            else
                            {
                                var error_code = xml.GetElementsByTagName("error_code").Item(0).InnerText;
                                var error_msg = xml.GetElementsByTagName("error_msg").Item(0).InnerText;
                                throw new Exception($"({error_code}) {error_msg}");
                            }
                        }
                        else
                        {
                            throw new Exception("no response result");
                        }
                        //context.ApplicationInstance.CompleteRequest();
                    }
                }
                catch (Exception ex)
                {
                    responseText = $"{{  \"result\": \"error\", \"error_msg\": \"{ex.Message}\" }}";
                }
            }

            //responseText = "{  \"result\": \"success\",  \"username\": \"cyrano\" }";

            context.Response.Clear();
            context.Response.ContentType = "text/plain";
            context.Response.Write(responseText);
            context.Response.End();
        }

        public bool IsReusable
        {
            get { return true; }
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
    }
}
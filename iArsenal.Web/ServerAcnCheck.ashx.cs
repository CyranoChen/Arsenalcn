﻿using System;
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
            string responseText = string.Empty;

            if (!string.IsNullOrEmpty(context.Request.QueryString["AcnID"]) && !string.IsNullOrEmpty(context.Request.QueryString["SessionKey"]))
            {
                try
                {
                    string acnID = context.Request.QueryString["AcnID"];
                    string sessionKey = context.Request.QueryString["SessionKey"];
                    float callID = DateTime.Now.Millisecond;
                    string fields = "user_name";

                    //New HttpWebRequest for DiscuzNT Service API
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(ConfigGlobal.APIServiceURL);

                    req.Method = "POST";
                    req.ContentType = "application/x-www-form-urlencoded";

                    //Gen Digital Signature
                    string sig = string.Format("api_key={0}call_id={1}fields={2}method={3}session_key={4}uids={5}{6}", ConfigGlobal.APIAppKey, callID.ToString(), fields, "users.getInfo", sessionKey, acnID, ConfigGlobal.APICryptographicKey);

                    //Set WebRequest Parameter
                    string para = string.Format("method={0}&api_key={1}&session_key={2}&call_id={3}&uids={4}&fields={5}&sig={6}", "users.getInfo", ConfigGlobal.APIAppKey, sessionKey, callID.ToString(), acnID, fields, getMd5Hash(sig));

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

                            //Build ACNUser Information
                            if (xml.HasChildNodes && !xml.FirstChild.NextSibling.Name.Equals("error_response"))
                            {
                                responseText = string.Format("{{  \"result\": \"success\",  \"username\": \"{0}\" }}", xml.GetElementsByTagName("user_name").Item(0).InnerText);
                            }
                            else
                            {
                                string error_code = xml.GetElementsByTagName("error_code").Item(0).InnerText;
                                string error_msg = xml.GetElementsByTagName("error_msg").Item(0).InnerText;
                                throw new Exception(string.Format("({0}) {1}", error_code, error_msg));
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
                    responseText = string.Format("{{  \"result\": \"error\", \"error_msg\": \"{0}\" }}", ex.Message);
                }
            }

            //responseText = "{  \"result\": \"success\",  \"username\": \"cyrano\" }";

            context.Response.Clear();
            context.Response.ContentType = "text/plain";
            context.Response.Write(responseText);
            context.Response.End();
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
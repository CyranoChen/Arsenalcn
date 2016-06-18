﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Arsenalcn.Core.Utility;

namespace Arsenalcn.Core
{
    public class RestClient
    {
        protected virtual string GetResponse(RequestMethod method = RequestMethod.POST,
            string contentType = "application/x-www-form-urlencoded")
        {
            //New HttpWebRequest for DiscuzNT Service API
            var req = (HttpWebRequest)WebRequest.Create(ServiceUrl);

            req.Method = method.ToString();
            req.ContentType = contentType;

            #region Set Signature & PostParas

            var sig = new StringBuilder();
            var postData = new StringBuilder();

            foreach (var para in Parameters)
            {
                if (!string.IsNullOrEmpty(para.Value))
                {
                    sig.Append(string.Format("{0}={1}", para.Key, para.Value));
                    postData.Append(string.Format("&{0}={1}", para.Key, para.Value));
                }
            }

            sig.Append(CryptographicKey);

            var strParameter = string.Format("sig={0}{1}", Encrypt.GetMd5Hash(sig.ToString()), postData);

            #endregion

            var encodedBytes = Encoding.UTF8.GetBytes(strParameter);
            req.ContentLength = encodedBytes.Length;

            // Write encoded data into request stream
            var requestStream = req.GetRequestStream();
            requestStream.Write(encodedBytes, 0, encodedBytes.Length);
            requestStream.Close();

            using (var response = req.GetResponse())
            {
                var receiveStream = response.GetResponseStream();

                if (receiveStream != null)
                {
                    var readStream = new StreamReader(receiveStream, Encoding.UTF8);
                    return readStream.ReadToEnd();
                }
                else
                {
                    return null;
                }
            }
        }

        protected virtual void SetDefaultParameters()
        {
            Parameters = new SortedDictionary<string, string>();

            if (!string.IsNullOrEmpty(AppKey))
            {
                Parameters.Add("api_key", AppKey);
            }
            else
            {
                throw new Exception("AppKey is null");
            }

            if (!string.IsNullOrEmpty(Method))
            {
                Parameters.Add("method", Method);
            }
            else
            {
                throw new Exception("Method is null");
            }

            if (!string.IsNullOrEmpty(Format.ToString()))
            {
                Parameters.Add("format", Format.ToString());
            }
        }

        #region Members and Properties

        public string ServiceUrl { get; set; }

        public string AppKey { get; set; }

        public string CryptographicKey { get; set; }

        public string Method { get; set; }

        public ResponseType Format { get; set; }

        public SortedDictionary<string, string> Parameters { get; set; }

        #endregion
    }

    public enum ResponseType
    {
        XML,
        JSON
    }

    public enum RequestMethod
    {
        GET,
        POST
    }
}
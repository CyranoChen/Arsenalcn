using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

using Arsenalcn.Core.Utility;

namespace Arsenalcn.Core
{
    public class RestClient
    {
        public RestClient() { }

        protected virtual string GetReponse(RequestMethod method = RequestMethod.POST, string contentType = "application/x-www-form-urlencoded")
        {
            //New HttpWebRequest for DiscuzNT Service API
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(ServiceUrl);

            req.Method = method.ToString();
            req.ContentType = contentType;

            #region Set Signature & PostParas
            StringBuilder sig = new StringBuilder();
            StringBuilder postData = new StringBuilder();

            foreach (var para in Parameters)
            {
                if (!string.IsNullOrEmpty(para.Value))
                {
                    sig.Append(string.Format("{0}={1}", para.Key, para.Value));
                    postData.Append(string.Format("&{0}={1}", para.Key, para.Value));
                }
            }

            sig.Append(CryptographicKey);

            var _strParameter = string.Format("sig={0}{1}", Encrypt.getMd5Hash(sig.ToString()), postData.ToString());

            #endregion

            byte[] encodedBytes = Encoding.UTF8.GetBytes(_strParameter);
            req.ContentLength = encodedBytes.Length;

            // Write encoded data into request stream
            Stream requestStream = req.GetRequestStream();
            requestStream.Write(encodedBytes, 0, encodedBytes.Length);
            requestStream.Close();

            using (var response = req.GetResponse())
            {
                var receiveStream = response.GetResponseStream();
                var readStream = new StreamReader(receiveStream, Encoding.UTF8);

                return readStream.ReadToEnd();
            }
        }

        protected virtual void SetDefaultParameters()
        {
            Parameters = new SortedDictionary<string, string>();

            if (!string.IsNullOrEmpty(AppKey))
            { Parameters.Add("api_key", AppKey); }
            else
            { throw new Exception("AppKey is null"); }

            if (!string.IsNullOrEmpty(Method))
            { Parameters.Add("method", Method); }
            else
            { throw new Exception("Method is null"); }

            if (!string.IsNullOrEmpty(Format.ToString()))
            { Parameters.Add("format", Format.ToString()); }
        }


        #region Members and Properties
        public string ServiceUrl
        { get; set; }

        public string AppKey
        { get; set; }

        public string CryptographicKey
        { get; set; }

        public string Method
        { get; set; }

        public ReponseType Format
        { get; set; }

        public SortedDictionary<string, string> Parameters
        { get; set; }

        #endregion
    }

    public enum ReponseType
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

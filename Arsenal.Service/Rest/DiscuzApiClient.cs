using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

using Arsenalcn.Core.Utility;
using System.Net;
using System.IO;

namespace Arsenal.Service
{
    public class DiscuzApiClient
    {
        public DiscuzApiClient()
        {
            ServiceUrl = ConfigGlobal.APIServiceURL;
            AppKey = ConfigGlobal.APIAppKey;
            CryptographicKey = ConfigGlobal.APICryptographicKey;
        }

        public string AuthValidate(string username, string password, string passwordFormat = "md5")
        {
            Contract.Requires(!string.IsNullOrEmpty(username));
            Contract.Requires(!string.IsNullOrEmpty(password));

            Method = "auth.validate";
            Format = ReponseType.JSON;

            SetDefaultParameters();

            Parameters.Add("user_name", username);
            Parameters.Add("password", password);

            if (!string.IsNullOrEmpty(passwordFormat))
            { Parameters.Add("password_format", passwordFormat); }

            return GetReponse();
        }

        public string AuthRegister(string username, string password, string email, string passwordFormat = "md5")
        {
            Contract.Requires(!string.IsNullOrEmpty(username));
            Contract.Requires(!string.IsNullOrEmpty(password));
            Contract.Requires(!string.IsNullOrEmpty(email));

            Method = "auth.register";
            Format = ReponseType.JSON;

            SetDefaultParameters();

            Parameters.Add("user_name", username);
            Parameters.Add("password", password);
            Parameters.Add("email", email);

            if (!string.IsNullOrEmpty(passwordFormat))
            { Parameters.Add("password_format", passwordFormat); }

            return GetReponse();
        }

        public string UsersGetID(string username)
        {
            Contract.Requires(!string.IsNullOrEmpty(username));

            Method = "users.getid";
            Format = ReponseType.JSON;

            SetDefaultParameters();

            Parameters.Add("user_name", username);

            return GetReponse();
        }

        public string UsersGetInfo(int[] uids, string[] fields)
        {
            Contract.Requires(uids.Length > 0);
            Contract.Requires(fields.Length > 0);

            Method = "users.getinfo";
            Format = ReponseType.JSON;

            SetDefaultParameters();

            Parameters.Add("uids", string.Join(",", uids.ToArray()));
            Parameters.Add("fields", string.Join(",", fields.ToArray()));

            return GetReponse();
        }

        private string GetReponse()
        {
            //New HttpWebRequest for DiscuzNT Service API
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(ServiceUrl);

            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";

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

            Signature = Encrypt.getMd5Hash(sig.ToString());
            PostParameters = string.Format("sig={0}{1}", Signature, postData.ToString());

            #endregion

            byte[] encodedBytes = Encoding.UTF8.GetBytes(PostParameters);
            req.ContentLength = encodedBytes.Length;

            // Write encoded data into request stream
            Stream requestStream = req.GetRequestStream();
            requestStream.Write(encodedBytes, 0, encodedBytes.Length);
            requestStream.Close();

            var response = req.GetResponse();
            var receiveStream = response.GetResponseStream();
            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

            return readStream.ReadToEnd();
        }

        private void SetDefaultParameters()
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

            if (!string.IsNullOrEmpty(AuthToken))
            { Parameters.Add("auth_token", AuthToken); }
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

        public string AuthToken
        { get; set; }

        public string Signature
        { get; set; }

        public SortedDictionary<string, string> Parameters
        { get; set; }

        public string PostParameters
        { get; set; }

        #endregion

    }

    public enum ReponseType
    {
        XML,
        JSON
    }
}

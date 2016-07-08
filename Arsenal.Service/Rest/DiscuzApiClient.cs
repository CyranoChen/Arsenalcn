using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Arsenalcn.Core;
using Arsenalcn.Core.Utility;

namespace Arsenal.Service
{
    public class DiscuzApiClient : RestClient
    {
        public DiscuzApiClient()
        {
            ServiceUrl = ConfigGlobal_Arsenal.APIServiceURL;
            AppKey = ConfigGlobal_Arsenal.APIAppKey;
            CryptographicKey = ConfigGlobal_Arsenal.APICryptographicKey;
        }

        private string ApiPost()
        {
            //New HttpWebRequest for DiscuzNT Service API
            var req = (HttpWebRequest)WebRequest.Create(ServiceUrl);

            req.Method = RequestMethod.Post.ToString();
            req.ContentType = "application/x-www-form-urlencoded";

            #region Set Signature & PostParas

            var sig = new StringBuilder();
            var postData = new StringBuilder();

            foreach (var para in Parameters)
            {
                if (!string.IsNullOrEmpty(para.Value))
                {
                    sig.Append($"{para.Key}={para.Value}");
                    postData.Append($"&{para.Key}={para.Value}");
                }
            }

            sig.Append(CryptographicKey);

            var strParameter = $"sig={Encrypt.GetMd5Hash(sig.ToString())}{postData}";

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

                return null;
            }
        }

        private void SetDefaultParameters()
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


        public string AuthValidate(string username, string password, string passwordFormat = "md5")
        {
            Contract.Requires(!string.IsNullOrEmpty(username));
            Contract.Requires(!string.IsNullOrEmpty(password));

            Method = "auth.validate";
            Format = ResponseType.Json;

            SetDefaultParameters();

            Parameters.Add("user_name", username);
            Parameters.Add("password", password);

            if (!string.IsNullOrEmpty(passwordFormat))
            {
                Parameters.Add("password_format", passwordFormat);
            }

            return ApiPost();
        }

        public string AuthRegister(string username, string password, string email, string passwordFormat = "md5")
        {
            Contract.Requires(!string.IsNullOrEmpty(username));
            Contract.Requires(!string.IsNullOrEmpty(password));
            Contract.Requires(!string.IsNullOrEmpty(email));

            Method = "auth.register";
            Format = ResponseType.Json;

            SetDefaultParameters();

            Parameters.Add("user_name", username);
            Parameters.Add("password", password);
            Parameters.Add("email", email);

            if (!string.IsNullOrEmpty(passwordFormat))
            {
                Parameters.Add("password_format", passwordFormat);
            }

            return ApiPost();
        }

        public string UsersChangePassword(int uid, string oldPassword, string newPassword, string passwordFormat = "md5")
        {
            Contract.Requires(uid > 0);
            Contract.Requires(!string.IsNullOrEmpty(oldPassword));
            Contract.Requires(!string.IsNullOrEmpty(newPassword));

            Method = "users.changepassword";
            Format = ResponseType.Json;

            SetDefaultParameters();

            Parameters.Add("uid", uid.ToString());
            Parameters.Add("original_password", oldPassword);
            Parameters.Add("new_password", newPassword);
            Parameters.Add("confirm_new_password", newPassword);

            if (!string.IsNullOrEmpty(passwordFormat))
            {
                Parameters.Add("password_format", passwordFormat);
            }

            return ApiPost();
        }

        // ReSharper disable once InconsistentNaming
        public string UsersGetID(string username)
        {
            Contract.Requires(!string.IsNullOrEmpty(username));

            Method = "users.getid";
            Format = ResponseType.Json;

            SetDefaultParameters();

            Parameters.Add("user_name", username);

            return ApiPost();
        }

        public string UsersGetInfo(int[] uids, string[] fields)
        {
            Contract.Requires(uids.Length > 0);
            Contract.Requires(fields.Length > 0);

            Method = "users.getinfo";
            Format = ResponseType.Json;

            SetDefaultParameters();

            Parameters.Add("uids", string.Join(",", uids.ToArray()));
            Parameters.Add("fields", string.Join(",", fields.ToArray()));

            return ApiPost();
        }
    }
}
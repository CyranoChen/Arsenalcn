using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Caching;
using Arsenalcn.Core;

namespace Arsenal.Service
{
    public class WeChatApiClient : RestClient
    {
        public WeChatApiClient()
        {
            ServiceUrl = ConfigGlobal.WeChatServiceURL;
            AppKey = ConfigGlobal.WeChatAppKey;
            CryptographicKey = ConfigGlobal.WeChatAppSecret;

            Init();
        }

        #region Members and Properties

        public string AccessToken { get; set; }

        #endregion

        private void Init()
        {
            var context = HttpContext.Current;

            // Get access_token by using System.Web.Caching
            if (context.Cache["AccessToken"] != null)
            {
                AccessToken = context.Cache["AccessToken"].ToString();
            }
            else
            {
                // Get access token
                // https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=APPID&secret=APPSECRET
                // {"access_token":"ACCESS_TOKEN","expires_in":7200}

                var uri = string.Format("{0}token?grant_type=client_credential&appid={1}&secret={2}",
                    ServiceUrl, AppKey, CryptographicKey);

                var client = new WebClient();

                using (var responseStream = client.OpenRead(uri))
                {
                    var readStream = new StreamReader(responseStream, Encoding.UTF8);

                    var responseResult = readStream.ReadToEnd();

                    // Set access_token by using System.Web.Caching
                    AddItemToCache("AccessToken", responseResult, 3600);

                    AccessToken = context.Cache["AccessToken"].ToString();
                }
            }
        }

        //static bool itemRemoved = false;
        //static CacheItemRemovedReason reason;
        //CacheItemRemovedCallback onRemove = null;

        private void RemovedCallback(string k, object v, CacheItemRemovedReason r)
        {
            //itemRemoved = true;
            //reason = r;
        }

        private void AddItemToCache(string key, object value, double expires)
        {
            //itemRemoved = false;

            var onRemove = new CacheItemRemovedCallback(RemovedCallback);

            if (HttpContext.Current.Cache[key] == null)
                HttpContext.Current.Cache.Add(key, value, null, DateTime.Now.AddSeconds(expires), TimeSpan.Zero,
                    CacheItemPriority.High, onRemove);
        }

        //public void RemoveItemFromCache(Object sender, EventArgs e)
        //{
        //    if (Cache["Key1"] != null)
        //        Cache.Remove("Key1");
        //}

        protected override void SetDefaultParameters()
        {
            Parameters = new SortedDictionary<string, string>();

            if (!string.IsNullOrEmpty(AppKey))
            {
                Parameters.Add("api_key", AppKey);
            }
            else
            {
                throw new ArgumentNullException("AppKey");
            }

            if (!string.IsNullOrEmpty(Method))
            {
                Parameters.Add("method", Method);
            }
            else
            {
                throw new ArgumentNullException("Method");
            }

            if (!string.IsNullOrEmpty(AccessToken))
            {
                Parameters.Add("access_token", AccessToken);
            }
            else
            {
                throw new ArgumentNullException("AccessToken");
            }

            if (!string.IsNullOrEmpty(Format.ToString()))
            {
                Parameters.Add("format", Format.ToString());
            }
        }
    }
}
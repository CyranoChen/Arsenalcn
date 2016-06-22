using System;
using System.Web;
using System.Web.Caching;
using Arsenalcn.Core;
using Newtonsoft.Json.Linq;

namespace Arsenal.Service
{
    public class WeChatApiClient : RestClient
    {
        public WeChatApiClient()
        {
            ServiceUrl = ConfigGlobal_Arsenal.WeChatServiceURL;
            AppKey = ConfigGlobal_Arsenal.WeChatAppKey;
            CryptographicKey = ConfigGlobal_Arsenal.WeChatAppSecret;
        }

        #region Members and Properties

        protected string AccessToken { get; set; }

        #endregion

        private void Init()
        {
            if (!ConfigGlobal_Arsenal.WeChatActive) { return; }

            var context = HttpContext.Current;

            // Get access_token by using System.Web.Caching
            if (context.Cache["AccessToken"] != null)
            {
                AccessToken = context.Cache["AccessToken"].ToString();
            }
            else
            {
                // Get access token
                // http请求方式: GET
                // https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=APPID&secret=APPSECRET
                // {"access_token":"ACCESS_TOKEN","expires_in":7200}

                var uri = $"{ServiceUrl}token?grant_type=client_credential&appid={AppKey}&secret={CryptographicKey}";

                var responseResult = ApiGet(uri);

                if (string.IsNullOrEmpty(responseResult))
                {
                    throw new Exception("WeChatApiClient.Init() responseResult is null");
                }

                var json = JToken.Parse(responseResult);

                if (json["access_token"] != null && json["expires_in"] != null)
                {
                    // Set access_token by using System.Web.Caching
                    AccessToken = AddItemToCache("AccessToken",
                        json["access_token"], json["expires_in"].Value<double>()).ToString();
                }
                else
                {
                    // TODO 反序列化成异常对象，并抛出
                }
            }
        }

        private object AddItemToCache(string key, object value, double expires)
        {
            if (HttpContext.Current.Cache[key] == null)
                HttpContext.Current.Cache.Add(key, value, null, DateTime.Now.AddSeconds(expires), TimeSpan.Zero,
                    CacheItemPriority.High, null);

            return value;
        }
    }

    public enum ScopeType
    {
        snsapi_base,
        snsapi_userinfo
    }
}
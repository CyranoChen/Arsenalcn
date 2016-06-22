using System;
using System.Text;
using System.Web;
using Newtonsoft.Json.Linq;

namespace Arsenal.Service.Rest
{
    public class WeChatSnsClient : WeChatApiClient
    {
        public string GetOpenUrl(string redirectUri, ScopeType scope, string state)
        {
            if (!ConfigGlobal_Arsenal.WeChatActive) { return null; }

            //https://open.weixin.qq.com/connect/oauth2/authorize?appid=APPID&redirect_uri=REDIRECT_URI&response_type=code&scope=SCOPE&state=STATE#wechat_redirect 

            var serverUrl = "https://open.weixin.qq.com/connect/oauth2/";

            return $"{serverUrl}authorize?appid={AppKey}&redirect_uri={HttpUtility.UrlEncode(redirectUri, Encoding.UTF8)}&response_type=code&scope={scope}&state={HttpUtility.UrlEncode(state, Encoding.UTF8)}#wechat_redirect";
        }

        public void SetUserBaseInfo(string code, Guid userGuid)
        {
            if (!ConfigGlobal_Arsenal.WeChatActive) { return; }

            //https://api.weixin.qq.com/sns/oauth2/access_token?appid=APPID&secret=SECRET&code=CODE&grant_type=authorization_code 

            var serverUrl = "https://api.weixin.qq.com/sns/oauth2/";

            var uri = $"{serverUrl}access_token?appid={AppKey}&secret={CryptographicKey}&code={code}&grant_type=authorization_code";

            var responseResult = ApiGet(uri);

            if (string.IsNullOrEmpty(responseResult))
            {
                throw new Exception("WeChatApiClient.Init() responseResult is null");
            }

            var json = JToken.Parse(responseResult);

            if (json["access_token"] != null && json["expires_in"] != null 
                && json["refresh_token"] != null && json["openid"] != null && json["scope"] != null)
            {
                // TODO 持久化
                HttpContext.Current.Session["access_token"] = json["access_token"];
                HttpContext.Current.Session["expires_in"] = json["expires_in"];
                HttpContext.Current.Session["refresh_token"] = json["refresh_token"];
                HttpContext.Current.Session["openid"] = json["openid"];
                HttpContext.Current.Session["scope"] = json["scope"];
            }
            else
            {
                // TODO 反序列化成异常对象，并抛出
            }
        }

    }
}

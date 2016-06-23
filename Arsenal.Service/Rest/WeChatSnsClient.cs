using System;
using System.Text;
using System.Web;

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

        public string GetAccessToken(string code)
        {
            if (!ConfigGlobal_Arsenal.WeChatActive) { return null; }

            //https://api.weixin.qq.com/sns/oauth2/access_token?appid=APPID&secret=SECRET&code=CODE&grant_type=authorization_code 

            var serverUrl = "https://api.weixin.qq.com/sns/oauth2/";

            var uri = $"{serverUrl}access_token?appid={AppKey}&secret={CryptographicKey}&code={code}&grant_type=authorization_code";

            var responseResult = ApiGet(uri);

            if (string.IsNullOrEmpty(responseResult))
            {
                throw new Exception("WeChatSnsClient.GetAccessToken() responseResult is null");
            }

            return responseResult;
        }

        public string GetUserInfo(string accessToken, string openId)
        {
            if (!ConfigGlobal_Arsenal.WeChatActive) { return null; }

            //https://api.weixin.qq.com/sns/userinfo?access_token=ACCESS_TOKEN&openid=OPENID&lang=zh_CN

            var serverUrl = "https://api.weixin.qq.com/sns/";

            var uri = $"{serverUrl}userinfo?access_token={accessToken}&openid={openId}&lang=zh_CN";

            var responseResult = ApiGet(uri);

            if (string.IsNullOrEmpty(responseResult))
            {
                throw new Exception("WeChatSnsClient.GetUserInfo() responseResult is null");
            }

            return responseResult;
        }
    }
}

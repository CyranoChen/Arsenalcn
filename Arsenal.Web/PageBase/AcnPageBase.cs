using System;
using System.Web;
using System.Web.UI;
using Arsenal.Service;

namespace Arsenal.Web
{
    public class AcnPageBase : Page
    {
        protected bool _adminPage = false;

        public int UID
        {
            get
            {
                if (Request.Cookies["uid"] != null && !string.IsNullOrEmpty(Request.Cookies["uid"].Value))
                {
                    //already login
                    return int.Parse(Request.Cookies["uid"].Value);
                }
                return -1;
            }
        }

        public string Username
        {
            get
            {
                if (Request.Cookies["user_name"] != null && !string.IsNullOrEmpty(Request.Cookies["user_name"].Value))
                    return HttpUtility.UrlDecode(Request.Cookies["user_name"].Value);
                return string.Empty;
            }
        }

        public string SessionKey
        {
            get
            {
                if (Request.Cookies["session_key"] != null &&
                    !string.IsNullOrEmpty(Request.Cookies["session_key"].Value))
                    return Request.Cookies["session_key"].Value;
                return string.Empty;
            }
        }

        protected bool AnonymousRedirect { get; set; }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);

            if (!ConfigGlobal_Arsenal.PluginActive && Request.Url.LocalPath.ToLower().IndexOf("default.aspx") < 0)
            {
                if (!_adminPage)
                    Response.Redirect("Default.aspx");
            }

            //not Login
            if (AnonymousRedirect && UID == -1)
            {
                //Response.SetCookie(new HttpCookie("session_key", "1234567890"));
                //Response.SetCookie(new HttpCookie("uid", "443"));
                //Response.SetCookie(new HttpCookie("user_name", "cyrano"));

                //string loginURL = "default.aspx";

                Response.Clear();

                var loginURL = string.Format("{0}?api_key={1}&next={2}", ConfigGlobal_Arsenal.APILoginURL,
                    ConfigGlobal_Arsenal.APIAppKey, Request.Url.PathAndQuery);

                Response.Redirect(loginURL, false);

                Context.ApplicationInstance.CompleteRequest();
            }

            //Set Master Page Info
            if (Master != null && Master is DefaultMaster)
            {
                var masterPage = Master as DefaultMaster;

                masterPage.UserID = UID;
                masterPage.UserName = Username;
                masterPage.UserKey = SessionKey;
            }

            //if (this.Master != null && this.Master is ArsenalMaster)
            //{
            //    ArsenalMaster masterPage = this.Master as ArsenalMaster;

            //    masterPage.UserID = UID;
            //    masterPage.UserName = Username;
            //    masterPage.UserKey = SessionKey;
            //}
        }
    }
}
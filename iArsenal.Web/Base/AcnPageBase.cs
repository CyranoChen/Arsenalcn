using System;
using System.Web;
using System.Web.UI;
using iArsenal.Service;

namespace iArsenal.Web
{
    public class AcnPageBase : Page
    {
        protected bool AdminPage = false;

        protected int Uid
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.Cookies["uid"]?.Value))
                {
                    //already login
                    return int.Parse(Request.Cookies["uid"].Value);
                }

                return -1;
            }
        }

        protected string Username
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.Cookies["user_name"]?.Value))
                { return HttpUtility.UrlDecode(Request.Cookies["user_name"].Value); }

                return string.Empty;
            }
        }

        protected string SessionKey
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.Cookies["session_key"]?.Value))
                { return Request.Cookies["session_key"].Value; }

                return string.Empty;
            }
        }

        protected bool AnonymousRedirect { get; set; }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);

            if (!ConfigGlobal.PluginActive && Request.Url.LocalPath.ToLower().IndexOf("default.aspx") < 0)
            {
                if (!AdminPage)
                    Response.Redirect("Default.aspx");
            }

            //not Login
            if (AnonymousRedirect && Uid == -1)
            {
                //Response.SetCookie(new HttpCookie("session_key", "1234567890"));
                //Response.SetCookie(new HttpCookie("uid", "443"));
                //Response.SetCookie(new HttpCookie("user_name", "cyrano"));

                //string loginURL = "default.aspx";

                Response.Clear();

                var loginURL =
                    $"{ConfigGlobal.APILoginURL}?api_key={ConfigGlobal.APIAppKey}&next={Request.Url.PathAndQuery}";

                Response.Redirect(loginURL, false);

                Context.ApplicationInstance.CompleteRequest();
            }

            //Set Master Page Info
            if (Master != null && Master is DefaultMaster)
            {
                var masterPage = Master as DefaultMaster;

                masterPage.UserID = Uid;
                masterPage.UserName = Username;
                masterPage.UserKey = SessionKey;
            }

            if (Master != null && Master is iArsenalMaster)
            {
                var masterPage = Master as iArsenalMaster;

                masterPage.UserID = Uid;
                masterPage.UserName = Username;
                masterPage.UserKey = SessionKey;
            }
        }
    }
}
using System;
using Arsenalcn.CasinoSys.Entity;
using Discuz.Forum;

namespace Arsenalcn.CasinoSys.Web.Common
{
    public class BasePage : PageBase //Page 
    {
        protected bool AdminPage = false;
        //public int userid = 1;
        //public string username = "cao262";
        //public string userkey = "kkkk222";


        /// <summary>
        ///     anonymous user will be redirected to login page if set true
        /// </summary>
        public bool AnonymousRedirect { get; set; }

        public Gambler CurrentGambler { get; set; }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);

            if (!ConfigGlobal.PluginActive && Request.Url.LocalPath.ToLower().IndexOf("default.aspx") < 0)
            {
                if (!AdminPage)
                    Response.Redirect("Default.aspx");
            }

            if (AnonymousRedirect && userid == -1)
                Response.Redirect("/login.aspx");

            //Set Master Page Info
            if (Master != null && Master is DefaultMaster)
            {
                var masterPage = Master as DefaultMaster;

                masterPage.UserId = userid;
                masterPage.UserName = username;
                masterPage.UserKey = userkey;
            }

            if (userid != -1)
            {
                CurrentGambler = new Gambler(userid);
            }
        }
    }
}
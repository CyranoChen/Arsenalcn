using System;

using Arsenalcn.ClubSys.Entity;

namespace Arsenalcn.ClubSys.Web.Common
{
    public class BasePage : Discuz.Forum.PageBase //Page
    {
        //public int userid = 1;
        //public string username = "cao262";
        //public string userkey = "kkkk222";

        
        private bool _anonymousRedirect = false;
        /// <summary>
        /// anonymous user will be redirected to login page if set true
        /// </summary>
        public bool AnonymousRedirect
        {
            get
            {
                return _anonymousRedirect;
            }
            set
            {
                _anonymousRedirect = value;
            }
        }

        protected bool _adminPage = false;

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);

            if (!ConfigGlobal.PluginActive && Request.Url.LocalPath.ToLower().IndexOf("default.aspx") < 0)
            {
                if (!_adminPage)
                    Response.Redirect("Default.aspx");
            }

            if (AnonymousRedirect && userid == -1)
                Response.Redirect("/login.aspx");

            //Set Master Page Info
            if (this.Master != null && this.Master is DefaultMaster)
            {
                var masterPage = this.Master as DefaultMaster;

                masterPage.UserID = userid;
                masterPage.UserName = username;
                masterPage.UserKey = userkey;
            }
        }
    }
}

using System;
using Arsenalcn.CasinoSys.Entity;
using Discuz.Forum;

namespace Arsenalcn.CasinoSys.Web.Common
{
    public class BasePage : PageBase
    {
        protected bool AdminPage = false;

        /// <summary>
        ///     anonymous user will be redirected to login page if set true
        /// </summary>
        protected bool AnonymousRedirect { get; set; }

        protected Gambler CurrentGambler { get; set; }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);

            if (!ConfigGlobal.PluginActive
                && Request.Url.LocalPath.ToLower().IndexOf("default.aspx", StringComparison.OrdinalIgnoreCase) < 0)
            {
                if (!AdminPage)
                    Response.Redirect("Default.aspx");
            }

            // ReSharper disable once Html.PathError
            if (AnonymousRedirect && userid == -1) { Response.Redirect("/login.aspx"); }

            //Set Master Page Info
            var master = Master as DefaultMaster;

            if (master != null)
            {
                var masterPage = master;

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
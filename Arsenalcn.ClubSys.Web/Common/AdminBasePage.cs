using System;
using Arsenalcn.ClubSys.Entity;

namespace Arsenalcn.ClubSys.Web.Common
{
    public class AdminBasePage : BasePage
    {
        protected override void OnInitComplete(EventArgs e)
        {
            _adminPage = true;
            AnonymousRedirect = true;

            base.OnInitComplete(e);

            if (!ConfigAdmin.IsPluginAdmin(userid))
            {
                Response.Redirect("ClubPortal.aspx");
            }
        }
    }
}
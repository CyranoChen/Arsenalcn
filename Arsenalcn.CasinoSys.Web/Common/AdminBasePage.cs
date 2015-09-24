using System;

using Arsenalcn.CasinoSys.Entity;

namespace Arsenalcn.CasinoSys.Web.Common
{
    public class AdminBasePage : BasePage
    {
        protected override void OnInitComplete(EventArgs e)
        {
            AdminPage = true;
            AnonymousRedirect = true;

            base.OnInitComplete(e);

            if (!ConfigAdmin.IsPluginAdmin(userid))
            {
                Response.Redirect("CasinoPortal.aspx");
            }
        }
    }
}

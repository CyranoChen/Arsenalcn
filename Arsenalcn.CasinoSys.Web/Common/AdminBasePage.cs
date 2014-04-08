using System;

using Arsenalcn.CasinoSys.Entity;

namespace Arsenalcn.CasinoSys.Web.Common
{
    public class AdminBasePage : BasePage
    {
        protected override void OnInitComplete(EventArgs e)
        {
            _adminPage = true;
            AnonymousRedirect = true;

            base.OnInitComplete(e);

            if (!ConfigAdmin.IsPluginAdmin(this.userid))
            {
                Response.Redirect("CasinoPortal.aspx");
            }
        }
    }
}

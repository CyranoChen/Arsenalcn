using System;

using iArsenal.Entity;

namespace iArsenal.Web.PageBase
{
    public class AdminPageBase : AcnPageBase
    {
        protected override void OnInitComplete(EventArgs e)
        {
            _adminPage = true;
            AnonymousRedirect = true;

            base.OnInitComplete(e);

            if (!ConfigAdmin.IsPluginAdmin(this.UID))
            {
                Response.Redirect("default.aspx");
            }
        }
    }
}

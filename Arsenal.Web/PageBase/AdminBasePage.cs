using System;

using Arsenal.Service;

namespace Arsenal.Web
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
                Response.Redirect("Default.aspx");
            }
        }
    }
}

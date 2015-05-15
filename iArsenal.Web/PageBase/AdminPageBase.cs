using System;

using iArsenal.Service;

namespace iArsenal.Web
{
    public class AdminPageBase : AcnPageBase
    {
        protected override void OnInitComplete(EventArgs e)
        {
            _adminPage = true;
            AnonymousRedirect = true;

            base.OnInitComplete(e);

            if (!ConfigGlobal.IsPluginAdmin(this.UID))
            {
                Response.Redirect("Default.aspx");
            }
        }
    }
}

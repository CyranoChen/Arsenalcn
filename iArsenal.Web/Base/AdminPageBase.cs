using System;
using iArsenal.Service;

namespace iArsenal.Web
{
    public class AdminPageBase : AcnPageBase
    {
        protected override void OnInitComplete(EventArgs e)
        {
            AdminPage = true;
            AnonymousRedirect = true;

            base.OnInitComplete(e);

            if (!ConfigGlobal.IsPluginAdmin(Uid))
            {
                Response.Redirect("Default.aspx");
            }
        }
    }
}
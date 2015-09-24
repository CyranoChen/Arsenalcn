using System;

using Arsenalcn.CasinoSys.Entity;

namespace Arsenalcn.CasinoSys.Web
{
    public partial class Default : Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (ConfigGlobal.PluginActive)
                Response.Redirect("CasinoPortal.aspx");
            else
            {
                ltrlPluginName.Text = $"<strong>欢迎进入{ConfigGlobal.PluginDisplayName}</strong>";

                if (ConfigAdmin.IsPluginAdmin(userid))
                    pnlAdmin.Visible = true;
                else
                    pnlAdmin.Visible = false;
            }
        }
    }
}

using System;

using Arsenalcn.CasinoSys.Entity;

namespace Arsenalcn.CasinoSys.Web
{
    public partial class _Default : Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Entity.ConfigGlobal.PluginActive)
                Response.Redirect("CasinoPortal.aspx");
            else
            {
                ltrlPluginName.Text = string.Format("<strong>欢迎进入{0}</strong>", ConfigGlobal.PluginDisplayName);

                if (ConfigAdmin.IsPluginAdmin(this.userid))
                    pnlAdmin.Visible = true;
                else
                    pnlAdmin.Visible = false;
            }
        }
    }
}

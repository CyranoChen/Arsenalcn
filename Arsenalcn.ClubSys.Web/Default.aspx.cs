using System;
using Arsenalcn.ClubSys.Entity;
using Arsenalcn.ClubSys.Web.Common;

namespace Arsenalcn.ClubSys.Web
{
    public partial class _Default : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (ConfigGlobal.PluginActive)
            {
                Response.Redirect("ClubPortal.aspx");
            }
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
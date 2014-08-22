using System;
using System.Collections.Generic;

using Arsenalcn.ClubSys.Service;
using Arsenalcn.ClubSys.Entity;

namespace Arsenalcn.ClubSys.Web
{
    public partial class _Default : Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (ConfigGlobal.PluginActive)
                Response.Redirect("ClubPortal.aspx");
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

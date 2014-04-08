using System;

using iArsenal.Entity;

namespace iArsenal.Web
{
    public partial class _Default : PageBase.AcnPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (ConfigGlobal.PluginActive)
            {   
                //Response.Redirect("index.aspx");
            }
            else
            {
                //ltrlPluginName.Text = string.Format("<strong>欢迎进入{0}</strong>", ConfigGlobal.PluginDisplayName);

                //if (ConfigAdmin.IsPluginAdmin(this.UID))
                //    pnlAdmin.Visible = true;
                //else
                //    pnlAdmin.Visible = false;
            }
        }
    }
}

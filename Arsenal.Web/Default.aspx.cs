using System;
using Arsenal.Service;

namespace Arsenal.Web
{
    public partial class _Default : AcnPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (ConfigGlobal_Arsenal.PluginActive)
            {
                Response.Redirect("http://www.arsenalcn.com");
                //Response.Redirect("http://bbs.arsenalcn.com/plugin/acncasino/");
            }
            else
            {
                ltrlPluginName.Text = string.Format("<strong>欢迎进入{0}</strong>", ConfigGlobal_Arsenal.PluginDisplayName);

                if (ConfigGlobal_Arsenal.IsPluginAdmin(UID))
                    pnlAdmin.Visible = true;
                else
                    pnlAdmin.Visible = false;
            }
        }
    }
}
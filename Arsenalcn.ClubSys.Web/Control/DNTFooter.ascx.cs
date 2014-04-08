using System;

using Arsenalcn.ClubSys.Entity;

namespace Arsenalcn.ClubSys.Web.Control
{
    public partial class DNTFooter : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            pluginName = ConfigGlobal.PluginName;
            pluginVersion = ConfigGlobal.PluginVersion;
        }

        public string pluginName = string.Empty;
        public string pluginVersion = string.Empty;
    }
}
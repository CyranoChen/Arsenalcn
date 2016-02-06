using System;
using System.Web.UI;
using Arsenalcn.ClubSys.Entity;

namespace Arsenalcn.ClubSys.Web.Control
{
    public partial class DNTFooter : UserControl
    {
        public string pluginName = string.Empty;
        public string pluginVersion = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            pluginName = ConfigGlobal.PluginName;
            pluginVersion = ConfigGlobal.PluginVersion;
        }
    }
}
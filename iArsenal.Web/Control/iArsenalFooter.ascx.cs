using System;

using iArsenal.Service;

namespace iArsenal.Web.Control
{
    public partial class iArsenalFooter : System.Web.UI.UserControl
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
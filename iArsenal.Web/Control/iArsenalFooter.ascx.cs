using System;
using System.Web.UI;
using iArsenal.Service;

namespace iArsenal.Web.Control
{
    public partial class iArsenalFooter : UserControl
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
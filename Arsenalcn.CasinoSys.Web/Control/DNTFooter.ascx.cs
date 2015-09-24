using System;

using Arsenalcn.CasinoSys.Entity;

namespace Arsenalcn.CasinoSys.Web.Control
{
    public partial class DntFooter : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PluginName = ConfigGlobal.PluginName;
            PluginVersion = ConfigGlobal.PluginVersion;
        }

        public string PluginName = string.Empty;
        public string PluginVersion = string.Empty;
    }
}
using System;
using System.Web.UI;
using Arsenalcn.CasinoSys.Entity;

namespace Arsenalcn.CasinoSys.Web.Control
{
    public partial class DntFooter : UserControl
    {
        public string PluginName = string.Empty;
        public string PluginVersion = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            PluginName = ConfigGlobal.PluginName;
            PluginVersion = ConfigGlobal.PluginVersion;
        }
    }
}
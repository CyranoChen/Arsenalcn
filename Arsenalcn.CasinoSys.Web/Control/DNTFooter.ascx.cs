using System;
using System.Web.UI;
using Arsenalcn.CasinoSys.Entity;

namespace Arsenalcn.CasinoSys.Web.Control
{
    public partial class DntFooter : UserControl
    {
        protected string PluginName = string.Empty;
        protected string PluginVersion = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            PluginName = ConfigGlobal.PluginName;
            PluginVersion = ConfigGlobal.PluginVersion;
        }
    }
}
using System;

using Arsenalcn.CasinoSys.Entity;

namespace Arsenalcn.CasinoSys.Web.Control
{
    public partial class AdminFieldToolBar : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ltrlAdminInfo.Text = string.Format("<strong>欢迎<em>{0}</em>, 进入{1} 后台管理</strong>", AdminUserName, ConfigGlobal.PluginDisplayName);
        }

        public string AdminUserName
        { get; set; }
    }
}
using System;

using Arsenalcn.CasinoSys.Entity;

namespace Arsenalcn.CasinoSys.Web.Control
{
    public partial class AdminFieldToolBar : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ltrlAdminInfo.Text = $"<strong>欢迎<em>{AdminUserName}</em>, 进入{ConfigGlobal.PluginDisplayName} 后台管理</strong>";
        }

        public string AdminUserName
        { get; set; }
    }
}
using System;
using System.Web.UI;
using Arsenalcn.CasinoSys.Entity;

namespace Arsenalcn.CasinoSys.Web.Control
{
    public partial class AdminFieldToolBar : UserControl
    {
        public string AdminUserName { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            ltrlAdminInfo.Text = $"<strong>欢迎<em>{AdminUserName}</em>, 进入{ConfigGlobal.PluginDisplayName} 后台管理</strong>";
        }
    }
}
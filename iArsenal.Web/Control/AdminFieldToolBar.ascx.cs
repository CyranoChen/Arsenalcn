using System;
using Arsenalcn.Core.Utility;
using iArsenal.Service;

namespace iArsenal.Web.Control
{
    public partial class AdminFieldToolBar : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ltrlAdminInfo.Text = $"<strong>欢迎<em>{AdminUserName}</em>, 进入{ConfigGlobal.PluginDisplayName} 后台管理</strong>";

            var _ip = IPLocation.GetIP();

            ltrlMyIPInfo.Text = $"<a>IP: {_ip} - {IPLocation.GetIPInfo(_ip)}</a>";
        }

        public string AdminUserName
        { get; set; }
    }
}
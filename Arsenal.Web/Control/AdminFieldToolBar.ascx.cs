using System;
using System.Web.UI;
using Arsenal.Service;
using Arsenalcn.Core.Utility;

namespace Arsenal.Web.Control
{
    public partial class AdminFieldToolBar : UserControl
    {
        public string AdminUserName { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            ltrlAdminInfo.Text = string.Format("<strong>欢迎<em>{0}</em>, 进入{1} 后台管理</strong>", AdminUserName,
                ConfigGlobal_Arsenal.PluginDisplayName);

            var _ip = IPLocation.GetIP();

            ltrlMyIPInfo.Text = string.Format("<a>IP: {0} - {1}</a>", _ip, IPLocation.GetIPInfo(_ip));
        }
    }
}
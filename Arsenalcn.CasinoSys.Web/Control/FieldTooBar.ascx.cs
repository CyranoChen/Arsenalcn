using System;
using System.Web.UI;
using Arsenalcn.CasinoSys.Entity;

namespace Arsenalcn.CasinoSys.Web.Control
{
    public partial class FieldTooBar : UserControl
    {
        public int UserId { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (UserId == -1)
                ltrlToolBarTip.Text = "<strong>欢迎进入，请在<a href=\"/login.aspx\" target=\"_self\">登录</a>后使用全部功能</strong>";
            else
                ltrlToolBarTip.Text = $"<strong>提醒：您可投注即将于<em>{ConfigGlobal.CasinoValidDays}</em>天内开始的比赛</strong>";

            if (ConfigAdmin.IsPluginAdmin(UserId))
                pnlFuncLink.Visible = true;
            else
                pnlFuncLink.Visible = false;
        }
    }
}
using System;

using iArsenal.Service;

namespace iArsenal.Web.Control
{
    public partial class PortalSitePath : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ltrlTitle.Text =
                $"<a href=\"http://www.arsenalcn.com\">{"阿森纳中国官方球迷会"}</a> &raquo; <a href=\"default.aspx\">{ConfigGlobal.PluginDisplayName}</a> &raquo; <em>{this.Page.Title}</em>";
        }
    }
}
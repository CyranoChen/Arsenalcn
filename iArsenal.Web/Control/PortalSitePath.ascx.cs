using System;

using iArsenal.Entity;

namespace iArsenal.Web.Control
{
    public partial class PortalSitePath : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ltrlTitle.Text = string.Format("<a href=\"http://www.arsenalcn.com\">{0}</a> &raquo; <a href=\"default.aspx\">{1}</a> &raquo; <em>{2}</em>", "阿森纳中国官方球迷会", ConfigGlobal.PluginDisplayName, this.Page.Title);
        }
    }
}
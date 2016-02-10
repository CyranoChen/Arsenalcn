using System;
using System.Web.UI;
using Arsenal.Service;

namespace Arsenal.Web.Control
{
    public partial class DNTFooter : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ltrlCopyright.Text = $"<p id=\"copyright\">Powered by " +
                                 $"<a href=\"default.aspx\" title=\"{ConfigGlobal.Assembly.Title} {ConfigGlobal.Assembly.Version} (.NET Framework 4.5)\">" +
                                 $"{ConfigGlobal.Assembly.Title} </a> <em>{ConfigGlobal.Assembly.Version}</em> " +
                                 $"{ConfigGlobal.Assembly.Copyright} - {DateTime.Now.Year} " +
                                 $"<a href =\"http://www.arsenal.cn\" target =\"_blank\">{ConfigGlobal.Assembly.Product}</a></p>";

            ltrlDebugInfo.Text = $"<p id=\"debuginfo\">{ConfigGlobal.Assembly.Company} &copy; 2003 - {DateTime.Now.Year} Willing co., ltd.</p>";
        }
    }
}
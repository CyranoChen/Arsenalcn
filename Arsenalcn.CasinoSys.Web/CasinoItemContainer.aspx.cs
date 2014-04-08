using System;
using System.Web.UI;
using System.IO;

using Arsenalcn.CasinoSys.Web.Control;

namespace Arsenalcn.CasinoSys.Web
{
    public partial class CasinoItemContainer : Discuz.Forum.PageBase
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            CasinoHeader ctrlCasinoHeader = LoadControl("Control/CasinoHeader.ascx") as CasinoHeader;

            Guid matchGuid = Entity.Match.GetRandomOpenMatch();

            if (Entity.ConfigGlobal.PluginActive && Entity.ConfigGlobal.PluginContainerActive && ctrlCasinoHeader != null && matchGuid != Guid.Empty && userid > 0)
            {
                ctrlCasinoHeader.UserID = userid;
                ctrlCasinoHeader.MatchGuid = matchGuid;
                ctrlCasinoHeader.IsHistoryView = false;

                Controls.Add(ctrlCasinoHeader);

                ctrlCasinoHeader.RenderControl(htw);

                Response.Write("document.write('<link href=\"../../App_Themes/Arsenalcn/casinosys.css\" type=\"text/css\" rel=\"stylesheet\" />');");
                Response.Write(string.Format("document.write('{0}');", sw.ToString().Replace("<div class=\"CasinoSys_Header\">", "<div class=\"CasinoSys_Header\" style=\"border:none;background:#fff;\">").Replace("<img src=\"", "<img src=\"plugin/AcnCasino/").Replace("'", "\"").Replace("\r\n", "")));
                //Response.Write(string.Format("document.write('<div><a class=\"CasinoSys_HeaderMore\" href=\"plugin/AcnCasino/CasinoGameBet.aspx?Match={0}\" target=\"_blank\">比赛投注</a></div>');", matchGuid.ToString()));
            }
            else
            {
                //Response.Write("document.write('Google Adv');");
                //Response.Write("document.write('<script type=\"text/javascript\" src=\"http://www.arsenalcn.com/scripts/show_ads.js\"></script>');");
            }

            Response.End();
        }
    }
}

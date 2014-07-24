using System;
using System.Data;

using Arsenalcn.CasinoSys.Entity;

namespace Arsenalcn.CasinoSys.Web.Control
{
    public partial class TopContestList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ConfigGlobal.DefaultLeagueID.Equals(Guid.Empty))
            {
                League l = new League(ConfigGlobal.DefaultLeagueID);
                ltrlContestTitle.Text = string.Format("<a title=\"{0}{1}\">ACN博彩竞赛排行榜</a>", l.LeagueName, l.LeagueSeason);

                pnlContestTop.Visible = true;
            }
            else
            {
                pnlContestTop.Visible = false;
            }
        }
    }
}
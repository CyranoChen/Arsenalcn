using System;
using System.Web.UI;
using iArsenal.Service;
using ArsenalTeam = iArsenal.Service.Arsenal.Team;

namespace iArsenal.Web.Control
{
    public partial class PortalMatchInfo : UserControl
    {
        public Guid MatchGuid { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (MatchGuid != Guid.Empty)
            {
                var mt = MatchTicket.Cache.Load(MatchGuid);
                var tHome = Arsenal_Team.Cache.Load(ConfigGlobal.ArsenalTeamGuid);
                var tAway = Arsenal_Team.Cache.Load(mt.TeamGuid);

                if (mt != null)
                {
                    var _strLeagueInfo = mt.LeagueName;

                    if (mt.Round.HasValue)
                        _strLeagueInfo += $" 第{mt.Round.Value}轮";

                    lblLeagueSeason.Text = _strLeagueInfo;

                    var _strTeamInfo =
                        "<div class=\"MatchTicket_MatchInfo\"><a class=\"StrongLink\" title=\"{1}\">{0}</a><img src=\"{2}\" alt=\"{0}\" /><a><em>vs</em></a><img src=\"{5}\" alt=\"{3}\" /><a class=\"StrongLink\" title=\"{4}\">{3}</a></div>";

                    ltrlTeamInfo.Text = string.Format(_strTeamInfo, tHome.TeamDisplayName, tHome.TeamEnglishName,
                        ConfigGlobal.AcnCasinoURL + tHome.TeamLogo, tAway.TeamDisplayName, tAway.TeamEnglishName,
                        ConfigGlobal.AcnCasinoURL + tAway.TeamLogo);

                    lblTicketInfo.Text = mt.ProductInfo;
                    lblGameTime.Text = mt.PlayTimeLocal.ToString("yyyy-MM-dd HH:mm");
                }
            }
            else
            {
                lblLeagueSeason.Visible = false;
                ltrlTeamInfo.Visible = false;
                lblTicketInfo.Visible = false;
                lblGameTime.Visible = false;
            }
        }
    }
}
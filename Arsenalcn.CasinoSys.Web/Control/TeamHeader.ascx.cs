using System;
using System.Data;
using System.Web.UI;
using Arsenalcn.CasinoSys.Entity;

namespace Arsenalcn.CasinoSys.Web.Control
{
    public partial class TeamHeader : UserControl
    {
        public Guid TeamGuid { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            var t = Team.Cache.Load(TeamGuid);

            if (t != null)
            {
                var dt = CasinoItem.GetEndViewByTeam(t.ID);
                var wonCount = 0;
                var drawCount = 0;
                var lostCount = 0;

                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        var m = new Match((Guid) dr["MatchGuid"]);
                        if (t.ID == m.Home)
                        {
                            if (m.ResultHome > m.ResultAway)
                                wonCount++;
                            else if (m.ResultHome < m.ResultAway)
                                lostCount++;
                            else
                                drawCount++;
                        }
                        else if (t.ID == m.Away)
                        {
                            if (m.ResultHome < m.ResultAway)
                                wonCount++;
                            else if (m.ResultHome > m.ResultAway)
                                lostCount++;
                            else
                                drawCount++;
                        }
                    }
                    var matchCount = wonCount + lostCount + drawCount;

                    ltrlMatchCount.Text = "<em>" + matchCount + "</em>";
                    ltrlWonCount.Text = "<em>" + wonCount + "</em>";
                    ltrlDrawCount.Text = "<em>" + drawCount + "</em>";
                    ltrlLostCount.Text = "<em>" + lostCount + "</em>";
                }
                else
                    pnlHistoryResult.Visible = false;

                ltrlTeamDisplayName.Text = $"<em>{t.TeamDisplayName}({t.TeamEnglishName})</em>";
                ltrlTeamLogo.Text = string.Format("<img src=\"{0}\" alt=\"{1}\" title=\"{1}\" />", t.TeamLogo,
                    t.TeamDisplayName);
                ltrlTeamCoach.Text = "<em>" + t.Manager + "</em>";
                ltrlGround.Text = "<em>" + t.Ground + "</em>";
                ltrlCapacity.Text = "<em>" + t.Capacity + "</em>";
            }
        }
    }
}
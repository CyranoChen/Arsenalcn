using System;
using System.Data;

using Arsenalcn.CasinoSys.Entity;

namespace Arsenalcn.CasinoSys.Web.Control
{
    public partial class TeamHeader : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Team t = Team.Cache.Load(TeamGuid);

            if (t != null)
            {
                DataTable dt = CasinoItem.GetEndViewByTeam(t.ID);
                int matchCount = 0;
                int wonCount = 0;
                int drawCount = 0;
                int lostCount = 0;

                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Match m = new Match((Guid)dr["MatchGuid"]);
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
                    matchCount = wonCount + lostCount + drawCount;

                    ltrlMatchCount.Text = "<em>" + matchCount.ToString() + "</em>";
                    ltrlWonCount.Text = "<em>" + wonCount.ToString() + "</em>";
                    ltrlDrawCount.Text = "<em>" + drawCount.ToString() + "</em>";
                    ltrlLostCount.Text = "<em>" + lostCount.ToString() + "</em>";
                }
                else
                    pnlHistoryResult.Visible = false;

                ltrlTeamDisplayName.Text = string.Format("<em>{0}({1})</em>", t.TeamDisplayName, t.TeamEnglishName);
                ltrlTeamLogo.Text = string.Format("<img src=\"{0}\" alt=\"{1}\" title=\"{1}\" />", t.TeamLogo, t.TeamDisplayName);
                ltrlTeamCoach.Text = "<em>" + t.Manager + "</em>";
                ltrlGround.Text = "<em>" + t.Ground + "</em>";
                ltrlCapacity.Text = "<em>" + t.Capacity.ToString() + "</em>";
            }
        }

        public Guid TeamGuid
        {
            get;
            set;
        }
    }
}
using System;
using System.Data;
using System.Web.UI.WebControls;
using Arsenalcn.CasinoSys.Entity;
using Arsenalcn.CasinoSys.Web.Common;
using Arsenalcn.CasinoSys.Web.Control;

namespace Arsenalcn.CasinoSys.Web
{
    public partial class CasinoTeam : BasePage
    {
        public Guid CurrentTeam
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["Team"]))
                {
                    try
                    {
                        return new Guid(Request.QueryString["Team"]);
                    }
                    catch
                    {
                        return Guid.Empty;
                    }
                }
                return Guid.Empty;
            }
        }

        public Guid CurrentMatch
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["Match"]))
                {
                    try
                    {
                        return new Guid(Request.QueryString["Match"]);
                    }
                    catch
                    {
                        return Guid.Empty;
                    }
                }
                return Guid.Empty;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            #region Assign Control Property

            ctrlLeftPanel.UserId = userid;
            ctrlLeftPanel.UserName = username;

            ctrlFieldTooBar.UserId = userid;

            ctrlMenuTabBar.CurrentMenu = CasinoMenuType.CasinoGame;

            #endregion

            BindData();
        }

        private void BindData()
        {
            DataTable dt = null;

            if (CurrentTeam != Guid.Empty)
            {
                dt = CasinoItem.GetEndViewByTeam(CurrentTeam);
                ctrlTeamHeader.TeamGuid = CurrentTeam;
                ctrlTeamHeader.Visible = true;
                ctrlCasinoHeader.Visible = false;
            }
            else if (CurrentMatch != Guid.Empty)
            {
                dt = CasinoItem.GetHistoryViewByMatch(CurrentMatch);
                ctrlCasinoHeader.MatchGuid = CurrentMatch;
                ctrlCasinoHeader.IsHistoryView = true;
                ctrlCasinoHeader.Visible = true;
                ctrlTeamHeader.Visible = false;
            }
            else
                Response.Redirect("CasinoPortal.aspx");

            if (dt != null)
            {
                gvMatch.DataSource = dt;
                gvMatch.DataBind();

                ltrlMatchCount.Text = dt.Rows.Count.ToString();
            }
            else
                Response.Redirect("CasinoPortal.aspx");
        }

        protected void gvMatch_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var drv = e.Row.DataItem as DataRowView;

                if (drv != null)
                {
                    var m = new Match((Guid) drv["MatchGuid"]);

                    var ltrlLeagueInfo = e.Row.FindControl("ltrlLeagueInfo") as Literal;

                    if (ltrlLeagueInfo != null)
                    {
                        var strLeague =
                            "<a href=\"CasinoGame.aspx?League={0}\" title=\"{1}\"><img src=\"{2}\" alt=\"{1}\" class=\"CasinoSys_CategoryImg\" /></a>";

                        var strLeagueName = $"{m.LeagueName}{(m.Round.HasValue ? $" 第{m.Round}轮" : string.Empty)}";

                        ltrlLeagueInfo.Text = string.Format(strLeague, m.LeagueGuid, strLeagueName,
                            League.Cache.Load(m.LeagueGuid).LeagueLogo);
                    }

                    var lblHome = e.Row.FindControl("lblHome") as Label;
                    var lblAway = e.Row.FindControl("lblAway") as Label;
                    var hlVersus = e.Row.FindControl("hlVersus") as HyperLink;
                    var ltrlResult = e.Row.FindControl("ltrlResult") as Literal;

                    if (lblHome != null && lblAway != null && hlVersus != null)
                    {
                        var tHome = Team.Cache.Load(m.Home);
                        var tAway = Team.Cache.Load(m.Away);

                        var strTeamName =
                            "<a class=\"StrongLink\" href=\"CasinoTeam.aspx?Team={0}\"  title=\"{1}\">{2}</a> ";
                        var strTeamLogo = "<img src=\"{3}\" alt=\"{1}\" /> ";

                        lblHome.Text = string.Format(strTeamName + strTeamLogo,
                            tHome.ID, tHome.TeamEnglishName, tHome.TeamDisplayName, tHome.TeamLogo);
                        lblAway.Text = string.Format(strTeamLogo + strTeamName,
                            tAway.ID, tAway.TeamEnglishName, tAway.TeamDisplayName, tAway.TeamLogo);

                        hlVersus.NavigateUrl = $"CasinoTeam.aspx?Match={m.MatchGuid}";
                        hlVersus.Text =
                            $"<em title=\"{tHome.Ground}{(tHome.Capacity.HasValue ? ("(" + tHome.Capacity.Value + ")") : string.Empty)}\">vs</em>";
                    }

                    if (ltrlResult != null && m.ResultHome.HasValue && m.ResultAway.HasValue)
                    {
                        ltrlResult.Text = $"{m.ResultHome.Value} : {m.ResultAway.Value}";
                    }

                    var ltrlTotalBetCount = e.Row.FindControl("ltrlTotalBetCount") as Literal;
                    var ltrlTotalBetCash = e.Row.FindControl("ltrlTotalBetCash") as Literal;
                    var ltrlTotalWin = e.Row.FindControl("ltrlTotalWin") as Literal;

                    var betList = Bet.GetMatchAllBet(m.MatchGuid);

                    if (ltrlTotalBetCount != null)
                    {
                        ltrlTotalBetCount.Text = betList.Count.ToString();
                    }

                    if (ltrlTotalBetCash != null)
                    {
                        float totalbetCash = 0;
                        betList.ForEach(delegate(Bet bet) { totalbetCash += bet.BetAmount.GetValueOrDefault(0f); });
                        ltrlTotalBetCash.Text = totalbetCash.ToString("N0");
                    }

                    if (ltrlTotalWin != null)
                    {
                        float totalWin = 0;
                        betList.ForEach(
                            delegate(Bet bet)
                            {
                                totalWin += (bet.BetAmount.GetValueOrDefault(0f) - bet.Earning.GetValueOrDefault(0f));
                            });
                        ltrlTotalWin.Text = totalWin.ToString("N2");
                    }
                }
            }
        }

        protected void gvMatch_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMatch.PageIndex = e.NewPageIndex;

            BindData();
        }
    }
}
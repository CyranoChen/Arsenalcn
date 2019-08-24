using System;
using System.Data;
using System.Web.UI.WebControls;
using Arsenalcn.CasinoSys.Entity;
using Arsenalcn.CasinoSys.Web.Common;
using Arsenalcn.CasinoSys.Web.Control;

namespace Arsenalcn.CasinoSys.Web
{
    public partial class CasinoGame : BasePage
    {
        public Guid CurrentLeague
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["League"]))
                {
                    try
                    {
                        return new Guid(Request.QueryString["League"]);
                    }
                    catch
                    {
                        return Guid.Empty;
                    }
                }
                if (CurrentGroup != Guid.Empty)
                {
                    try
                    {
                        return new Group(CurrentGroup).LeagueGuid;
                    }
                    catch
                    {
                        return Guid.Empty;
                    }
                }
                return Guid.Empty;
            }
        }

        public Guid CurrentGroup
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["Group"]))
                {
                    try
                    {
                        return new Guid(Request.QueryString["Group"]);
                    }
                    catch
                    {
                        return Guid.Empty;
                    }
                }
                return Guid.Empty;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            AnonymousRedirect = true;

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            #region Assign Control Property

            ctrlLeftPanel.UserId = userid;
            ctrlLeftPanel.UserName = username;

            ctrlFieldTooBar.UserId = userid;

            ctrlMenuTabBar.CurrentMenu = CasinoMenuType.CasinoGame;

            ctrlLeagueHeader.CurrLeagueGuid = CurrentLeague;
            ctrlLeagueHeader.PageUrl = "CasinoGame.aspx";

            #endregion

            if (!IsPostBack)
            {
                if (CurrentLeague != Guid.Empty)
                {
                    var list = League.Cache.GetSeasonsByLeagueGuid(CurrentLeague);

                    ddlSeason.DataSource = list;
                    ddlSeason.DataTextField = "LeagueSeason";
                    ddlSeason.DataValueField = "ID";
                    ddlSeason.DataBind();

                    ddlSeason.SelectedValue = CurrentLeague.ToString();
                }
                else
                {
                    ddlSeason.Visible = false;
                    ddlGameTip.Visible = true;
                }

                BindData();
            }
        }

        private void BindData()
        {
            DataTable dt;

            if (CurrentGroup != Guid.Empty)
            {
                var group = new Group(CurrentGroup);
                dt = CasinoItem.GetEndViewByMatch(group.LeagueGuid, group.GroupGuid, group.IsTable);
            }
            else if (ddlSeason.Items.Count != 0)
            {
                dt = CasinoItem.GetEndViewByMatch(new Guid(ddlSeason.SelectedValue));
            }
            else
            {
                dt = CasinoItem.GetEndViewByMatch();
            }

            ltrlMatchCount.Text = dt?.Rows.Count.ToString() ?? "0";

            gvMatch.DataSource = dt;
            gvMatch.DataBind();
        }

        protected void ddlSeason_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvMatch.PageIndex = 0;

            BindData();
        }

        protected void gvMatch_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var drv = e.Row.DataItem as DataRowView;

                if (drv != null)
                {
                    var m = new Match((Guid) drv["MatchGuid"]);

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
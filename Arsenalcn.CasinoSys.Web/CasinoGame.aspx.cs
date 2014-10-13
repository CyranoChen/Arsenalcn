using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

using Arsenalcn.CasinoSys.Entity;
using ArsenalLeauge = Arsenalcn.CasinoSys.Entity.Arsenal.League;
using ArsenalTeam = Arsenalcn.CasinoSys.Entity.Arsenal.Team;

namespace Arsenalcn.CasinoSys.Web
{
    public partial class CasinoGame : Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region Assign Control Property

            ctrlLeftPanel.UserID = userid;
            ctrlLeftPanel.UserName = username;

            ctrlFieldTooBar.UserID = userid;

            ctrlMenuTabBar.CurrentMenu = Arsenalcn.CasinoSys.Web.Control.CasinoMenuType.CasinoGame;

            ctrlLeagueHeader.CurrLeagueGuid = CurrentLeague;
            ctrlLeagueHeader.PageURL = "CasinoGame.aspx";

            #endregion

            if (!IsPostBack)
            {
                if (CurrentLeague != Guid.Empty)
                {
                    List<ArsenalLeauge> list = Entity.Arsenal_League.Cache.GetSeasonsByLeagueGuid(CurrentLeague);

                    ddlSeason.DataSource = list;
                    ddlSeason.DataTextField = "LeagueSeason";
                    ddlSeason.DataValueField = "LeagueGuid";
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
                Entity.Group group = new Group(CurrentGroup);
                dt = Entity.CasinoItem.GetEndViewByMatch(group.LeagueGuid, group.GroupGuid, group.IsTable);
            }
            else if (ddlSeason.Items.Count != 0)
            {
                dt = Entity.CasinoItem.GetEndViewByMatch(new Guid(ddlSeason.SelectedValue));
            }
            else
            {
                dt = Entity.CasinoItem.GetEndViewByMatch();
            }

            if (dt != null)
                ltrlMatchCount.Text = dt.Rows.Count.ToString();
            else
                ltrlMatchCount.Text = "0";

            gvMatch.DataSource = dt;
            gvMatch.DataBind();
        }

        public Guid CurrentLeague
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["League"]))
                {
                    try { return new Guid(Request.QueryString["League"]); }
                    catch { return Guid.Empty; }
                }
                else if (CurrentGroup != Guid.Empty)
                {
                    try { return new Group(CurrentGroup).LeagueGuid; }
                    catch { return Guid.Empty; }
                }
                else
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
                else
                    return Guid.Empty;
            }
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
                DataRowView drv = e.Row.DataItem as DataRowView;

                Match m = new Match((Guid)drv["MatchGuid"]);

                Label lblHome = e.Row.FindControl("lblHome") as Label;
                Label lblAway = e.Row.FindControl("lblAway") as Label;
                HyperLink hlVersus = e.Row.FindControl("hlVersus") as HyperLink;
                Literal ltrlResult = e.Row.FindControl("ltrlResult") as Literal;

                if (lblHome != null && lblAway != null && hlVersus != null)
                {
                    ArsenalTeam tHome = Arsenal_Team.Cache.Load(m.Home);
                    ArsenalTeam tAway = Arsenal_Team.Cache.Load(m.Away);

                    string _strTeamName = "<a class=\"StrongLink\" href=\"CasinoTeam.aspx?Team={0}\"  title=\"{1}\">{2}</a> ";
                    string _strTeamLogo = "<img src=\"{3}\" alt=\"{1}\" /> ";

                    lblHome.Text = string.Format(_strTeamName + _strTeamLogo,
                        tHome.TeamGuid.ToString(), tHome.TeamEnglishName, tHome.TeamDisplayName, tHome.TeamLogo);
                    lblAway.Text = string.Format(_strTeamLogo + _strTeamName,
                        tAway.TeamGuid.ToString(), tAway.TeamEnglishName, tAway.TeamDisplayName, tAway.TeamLogo);

                    hlVersus.NavigateUrl = string.Format("CasinoTeam.aspx?Match={0}", m.MatchGuid.ToString());
                    hlVersus.Text = string.Format("<em title=\"{0}{1}\">vs</em>", tHome.Ground,
                        tHome.Capacity.HasValue ? ("(" + tHome.Capacity.Value.ToString() + ")") : string.Empty);
                }

                if (ltrlResult != null)
                {
                    ltrlResult.Text = string.Format("{0} : {1}", m.ResultHome.Value.ToString(), m.ResultAway.Value.ToString());
                }

                Literal ltrlTotalBetCount = e.Row.FindControl("ltrlTotalBetCount") as Literal;
                Literal ltrlTotalBetCash = e.Row.FindControl("ltrlTotalBetCash") as Literal;
                Literal ltrlTotalWin = e.Row.FindControl("ltrlTotalWin") as Literal;

                List<Entity.Bet> betList = Entity.Bet.GetMatchAllBet(m.MatchGuid);

                if (ltrlTotalBetCount != null)
                { ltrlTotalBetCount.Text = betList.Count.ToString(); }

                if (ltrlTotalBetCash != null)
                {
                    float _totalbetCash = 0;
                    betList.ForEach(delegate(Entity.Bet bet) { _totalbetCash += bet.BetAmount.GetValueOrDefault(0f); });
                    ltrlTotalBetCash.Text = _totalbetCash.ToString("N0");
                }

                if (ltrlTotalWin != null)
                {
                    float _totalWin = 0;
                    betList.ForEach(delegate(Entity.Bet bet) { _totalWin += (bet.BetAmount.GetValueOrDefault(0f) - bet.Earning.GetValueOrDefault(0f)); });
                    ltrlTotalWin.Text = _totalWin.ToString("N2");
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

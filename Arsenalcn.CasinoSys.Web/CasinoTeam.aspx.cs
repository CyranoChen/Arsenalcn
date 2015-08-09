using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

using Arsenalcn.CasinoSys.Entity;

namespace Arsenalcn.CasinoSys.Web
{
    public partial class CasinoTeam : Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region Assign Control Property

            ctrlLeftPanel.UserID = userid;
            ctrlLeftPanel.UserName = username;

            ctrlFieldTooBar.UserID = userid;

            ctrlMenuTabBar.CurrentMenu = Arsenalcn.CasinoSys.Web.Control.CasinoMenuType.CasinoGame;

            #endregion

            BindData();
        }

        private void BindData()
        {
            DataTable dt = null;

            if (CurrentTeam != Guid.Empty)
            {
                dt = Entity.CasinoItem.GetEndViewByTeam(CurrentTeam);
                ctrlTeamHeader.TeamGuid = CurrentTeam;
                ctrlTeamHeader.Visible = true;
                ctrlCasinoHeader.Visible = false;
            }
            else if (CurrentMatch != Guid.Empty)
            {
                dt = Entity.CasinoItem.GetHistoryViewByMatch(CurrentMatch);
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
                else
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
                else
                    return Guid.Empty;
            }
        }

        protected void gvMatch_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = e.Row.DataItem as DataRowView;

                Match m = new Match((Guid)drv["MatchGuid"]);

                Literal ltrlLeagueInfo = e.Row.FindControl("ltrlLeagueInfo") as Literal;

                if (ltrlLeagueInfo != null)
                {
                    string _strLeague = "<a href=\"CasinoGame.aspx?League={0}\" title=\"{1}\"><img src=\"{2}\" alt=\"{1}\" class=\"CasinoSys_CategoryImg\" /></a>";

                    string _strLeagueName = string.Format("{0}{1}", m.LeagueName, m.Round.HasValue ?
                        string.Format(" 第{0}轮", m.Round.ToString()) : string.Empty);

                    ltrlLeagueInfo.Text = string.Format(_strLeague, m.LeagueGuid.ToString(), _strLeagueName,
                        League.Cache.Load(m.LeagueGuid).LeagueLogo);
                }

                Label lblHome = e.Row.FindControl("lblHome") as Label;
                Label lblAway = e.Row.FindControl("lblAway") as Label;
                HyperLink hlVersus = e.Row.FindControl("hlVersus") as HyperLink;
                Literal ltrlResult = e.Row.FindControl("ltrlResult") as Literal;

                if (lblHome != null && lblAway != null && hlVersus != null)
                {
                    Team tHome = Team.Cache.Load(m.Home);
                    Team tAway = Team.Cache.Load(m.Away);

                    string _strTeamName = "<a class=\"StrongLink\" href=\"CasinoTeam.aspx?Team={0}\"  title=\"{1}\">{2}</a> ";
                    string _strTeamLogo = "<img src=\"{3}\" alt=\"{1}\" /> ";

                    lblHome.Text = string.Format(_strTeamName + _strTeamLogo,
                        tHome.ID.ToString(), tHome.TeamEnglishName, tHome.TeamDisplayName, tHome.TeamLogo);
                    lblAway.Text = string.Format(_strTeamLogo + _strTeamName,
                        tAway.ID.ToString(), tAway.TeamEnglishName, tAway.TeamDisplayName, tAway.TeamLogo);

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

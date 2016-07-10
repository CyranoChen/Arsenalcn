using System;
using System.Data;
using System.Web.UI.WebControls;
using Arsenalcn.CasinoSys.Entity;
using Arsenalcn.CasinoSys.Web.Common;
using Arsenalcn.CasinoSys.Web.Control;

namespace Arsenalcn.CasinoSys.Web
{
    public partial class CasinoPortal : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region Assign Control Property

            ctrlLeftPanel.UserId = userid;
            ctrlLeftPanel.UserName = username;

            ctrlFieldTooBar.UserId = userid;

            ctrlMenuTabBar.CurrentMenu = CasinoMenuType.CasinoPortal;

            #endregion

            if (userid == -1)
            {
                gvMatch.Columns[gvMatch.Columns.Count - 1].Visible = false;
            }
            else if (CurrentGambler.Cash <= 0)
            {
                gvMatch.Columns[gvMatch.Columns.Count - 1].Visible = false;
            }

            var dtMatch = CasinoItem.GetMatchCasinoItemView(true);

            gvMatch.DataSource = dtMatch;
            gvMatch.DataBind();
        }

        protected void gvMatch_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var drv = e.Row.DataItem as DataRowView;

                if (drv != null)
                {
                    var m = new Match((Guid)drv["MatchGuid"]);

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

                    var guid = CasinoItem.GetCasinoItemGuidByMatch(m.MatchGuid, CasinoType.SingleChoice);

                    if (guid.HasValue)
                    {
                        var item = CasinoItem.GetCasinoItem(guid.Value);

                        if (item != null)
                        {
                            var options = ((SingleChoice)item).Options;

                            var winOption = options.Find(option => option.OptionValue == MatchChoiceOption.HomeWinValue);
                            var drawOption = options.Find(option => option.OptionValue == MatchChoiceOption.DrawValue);
                            var loseOption = options.Find(option => option.OptionValue == MatchChoiceOption.AwayWinValue);

                            if (!string.IsNullOrEmpty(winOption.OptionValue) &&
                                !string.IsNullOrEmpty(drawOption.OptionValue) &&
                                !string.IsNullOrEmpty(loseOption.OptionValue))
                            {
                                var ltrlWinRate = e.Row.FindControl("ltrlWinRate") as Literal;
                                var ltrlDrawRate = e.Row.FindControl("ltrlDrawRate") as Literal;
                                var ltrlLoseRate = e.Row.FindControl("ltrlLoseRate") as Literal;

                                if (ltrlWinRate != null && ltrlDrawRate != null && ltrlLoseRate != null &&
                                    winOption.OptionRate.HasValue && drawOption.OptionRate.HasValue && loseOption.OptionRate.HasValue)
                                {
                                    ltrlWinRate.Text =
                                        $"<em title=\"主队胜赔率\">{Convert.ToSingle(winOption.OptionRate.Value).ToString("f2")}</em>";
                                    ltrlDrawRate.Text =
                                        $"<em title=\"双方平赔率\">{Convert.ToSingle(drawOption.OptionRate.Value).ToString("f2")}</em>";
                                    ltrlLoseRate.Text =
                                        $"<em title=\"客队胜赔率\">{Convert.ToSingle(loseOption.OptionRate.Value).ToString("f2")}</em>";

                                    var lbWinInfo = e.Row.FindControl("lbWinInfo") as Label;
                                    var lbDrawInfo = e.Row.FindControl("lbDrawInfo") as Label;
                                    var lbLoseInfo = e.Row.FindControl("lbLoseInfo") as Label;

                                    if (lbWinInfo != null && lbDrawInfo != null && lbLoseInfo != null)
                                    {
                                        lbWinInfo.Text =
                                            $"{ChoiceOption.GetOptionTotalCount(guid.Value, winOption.OptionValue)} | {ChoiceOption.GetOptionTotalBet(guid.Value, winOption.OptionValue).ToString("N0")}";
                                        lbDrawInfo.Text =
                                            $"{ChoiceOption.GetOptionTotalCount(guid.Value, drawOption.OptionValue)} | {ChoiceOption.GetOptionTotalBet(guid.Value, drawOption.OptionValue).ToString("N0")}";
                                        lbLoseInfo.Text =
                                            $"{ChoiceOption.GetOptionTotalCount(guid.Value, loseOption.OptionValue)} | {ChoiceOption.GetOptionTotalBet(guid.Value, loseOption.OptionValue).ToString("N0")}";
                                    }

                                    var btnBet = e.Row.FindControl("btnBet") as HyperLink;

                                    if (btnBet != null)
                                    {
                                        var betList = Bet.GetUserMatchAllBet(userid, m.MatchGuid);
                                        int betCount;

                                        if (betList != null && betList.Count > 0)
                                            betCount = betList.Count;
                                        else
                                            betCount = 0;

                                        btnBet.Text =
                                            $"投注 <span class=\"CasinoSys_BetInfo\">{betCount} | {Bet.GetUserMatchTotalBet(userid, m.MatchGuid).ToString("N0")}</span>";
                                        btnBet.NavigateUrl = $"CasinoGameBet.aspx?Match={m.MatchGuid}";
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
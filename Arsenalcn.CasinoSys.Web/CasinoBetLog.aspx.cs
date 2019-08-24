using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Arsenalcn.CasinoSys.Entity;
using Arsenalcn.CasinoSys.Web.Common;
using Arsenalcn.CasinoSys.Web.Control;

namespace Arsenalcn.CasinoSys.Web
{
    public partial class CasinoBetLog : BasePage
    {
        private string _away = string.Empty;

        private string _home = string.Empty;

        public int TimeDiff => int.Parse(ddlTimeDiff.SelectedValue);

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

            ctrlMenuTabBar.CurrentMenu = CasinoMenuType.CasinoBetLog;

            if (CurrentMatch != Guid.Empty)
            {
                ctrlCasinoHeader.UserId = userid;
                ctrlCasinoHeader.MatchGuid = CurrentMatch;
                ctrlCasinoHeader.IsHistoryView = false;

                hlBetClubLog.NavigateUrl = $"CasinoBetClubLog.aspx?Match={CurrentMatch}";
                hlBetClubLog.Target = "_self";
                hlBetClubLog.Visible = true;
            }
            else
            {
                ctrlCasinoHeader.Visible = false;
                hlBetClubLog.Visible = false;
            }

            #endregion

            BindData();
        }

        private void BindData()
        {
            List<Bet> betList;

            if (CurrentMatch == Guid.Empty)
            {
                //display all based on time diff
                ddlTimeDiff.Visible = true;

                betList = Bet.GetAllBetByTimeDiff(TimeDiff);
            }
            else
            {
                //display all match bet
                ddlTimeDiff.Visible = false;

                var m = new Match(CurrentMatch);

                var homeT = Team.Cache.Load(m.Home);
                var awayT = Team.Cache.Load(m.Away);
                _home = homeT.TeamDisplayName;
                _away = awayT.TeamDisplayName;

                betList = Bet.GetMatchAllBet(CurrentMatch);
            }

            ltrlSingleChoiceCount.Text = betList.FindAll(bet => bet.BetAmount.HasValue).Count.ToString();
            ltrlMatchResultCount.Text = betList.FindAll(bet => !bet.BetAmount.HasValue).Count.ToString();

            float totalBetCount = 0;
            betList.ForEach(delegate (Bet bet) { totalBetCount += bet.BetAmount.GetValueOrDefault(0f); });
            ltrlTotalBetCount.Text = totalBetCount.ToString("N0");

            gvBet.DataSource = betList;
            gvBet.DataBind();

            if (CurrentMatch != Guid.Empty)
            {
                gvBet.Columns[2].Visible = false;
                gvBet.Columns[3].Visible = false;
                gvBet.Columns[4].Visible = false;
            }
            else
            {
                gvBet.Columns[2].Visible = true;
                gvBet.Columns[3].Visible = true;
                gvBet.Columns[4].Visible = true;
            }
        }

        protected void gvBet_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var bet = e.Row.DataItem as Bet;

                var ltrlHome = e.Row.FindControl("ltrlHome") as Literal;
                var ltrlAway = e.Row.FindControl("ltrlAway") as Literal;
                var ltrlVs = e.Row.FindControl("ltrlVS") as Literal;

                if (bet != null && ltrlHome != null && ltrlAway != null && ltrlVs != null)
                {
                    var item = CasinoItem.GetCasinoItem(bet.CasinoItemGuid);

                    if (CurrentMatch != Guid.Empty)
                    {
                        ltrlHome.Text = _home;
                        ltrlAway.Text = _away;
                    }
                    else
                    {
                        if (item.MatchGuid != null)
                        {
                            var m = new Match(item.MatchGuid.Value);

                            var homeT = Team.Cache.Load(m.Home);
                            var awayT = Team.Cache.Load(m.Away);

                            ltrlHome.Text =
                                $"<a class=\"StrongLink\" href=\"CasinoTeam.aspx?Team={homeT.ID}\">{homeT.TeamDisplayName}</a>";
                            ltrlAway.Text =
                                $"<a class=\"StrongLink\" href=\"CasinoTeam.aspx?Team={awayT.ID}\">{awayT.TeamDisplayName}</a>";

                            ltrlVs.Text =
                                $"<a href=\"CasinoTeam.aspx?Match={m.MatchGuid}\"><em title=\"{homeT.Ground}({homeT.Capacity})\">vs</em></a>";
                        }
                    }

                    var ltrlResult = e.Row.FindControl("ltrlResult") as Literal;
                    var dt = BetDetail.GetBetDetailByBetId(bet.ID);

                    if (dt != null && ltrlResult != null)
                    {
                        var dr = dt.Rows[0];

                        switch (item.ItemType)
                        {
                            case CasinoType.SingleChoice:
                                if (dr["DetailName"].ToString() == MatchChoiceOption.HomeWinValue)
                                    ltrlResult.Text = "主队胜";
                                else if (dr["DetailName"].ToString() == MatchChoiceOption.DrawValue)
                                    ltrlResult.Text = "双方平";
                                else if (dr["DetailName"].ToString() == MatchChoiceOption.AwayWinValue)
                                    ltrlResult.Text = "客队胜";

                                break;
                            case CasinoType.MatchResult:
                                var betDetail = new MatchResultBetDetail(dt);
                                ltrlResult.Text = $"{betDetail.Home}：{betDetail.Away}";
                                break;
                        }
                    }

                    var ltrlBetResult = e.Row.FindControl("ltrlBetResult") as Literal;

                    if (ltrlBetResult != null)
                    {
                        if (!bet.IsWin.HasValue)
                        {
                            ltrlBetResult.Text = string.Empty;
                        }
                        else
                        {
                            if (bet.IsWin != null && bet.IsWin.Value)
                            {
                                if (item.ItemType == CasinoType.SingleChoice)
                                    ltrlBetResult.Text = "<span class=\"CasinoSys_True\" title=\"猜对输赢\"></span>";
                                else if (item.ItemType == CasinoType.MatchResult)
                                    ltrlBetResult.Text = "<span class=\"CasinoSys_Good\" title=\"猜对比分\"></span>";

                                e.Row.CssClass = "RowCasinoSys_True";
                            }
                            else
                            {
                                ltrlBetResult.Text = "<span class=\"CasinoSys_False\" title=\"失败\"></span>";
                            }
                        }
                    }

                    var ltrlBetRate = e.Row.FindControl("ltrlBetRate") as Literal;

                    if (ltrlBetRate != null)
                    {
                        ltrlBetRate.Text = bet.BetRate.HasValue ? Convert.ToSingle(bet.BetRate).ToString("f2") : "/";
                    }
                }
            }
        }

        protected void ddlTimeDiff_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvBet.PageIndex = 0;

            BindData();
        }

        protected void gvBet_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvBet.PageIndex = e.NewPageIndex;

            BindData();
        }
    }
}
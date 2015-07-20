using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

using Arsenalcn.CasinoSys.Entity;
using ArsenalTeam = Arsenalcn.CasinoSys.Entity.Arsenal.Team;

namespace Arsenalcn.CasinoSys.Web
{
    public partial class CasinoBetLog : Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region Assign Control Property

            ctrlLeftPanel.UserID = userid;
            ctrlLeftPanel.UserName = username;

            ctrlFieldTooBar.UserID = userid;

            ctrlMenuTabBar.CurrentMenu = Arsenalcn.CasinoSys.Web.Control.CasinoMenuType.CasinoBetLog;

            if (CurrentMatch != Guid.Empty)
            {
                ctrlCasinoHeader.UserID = userid;
                ctrlCasinoHeader.MatchGuid = CurrentMatch;
                ctrlCasinoHeader.IsHistoryView = false;

                hlBetClubLog.NavigateUrl = string.Format("CasinoBetClubLog.aspx?Match={0}", CurrentMatch.ToString());
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

        private string home = string.Empty;
        private string away = string.Empty;

        private void BindData()
        {
            List<Entity.Bet> betList;

            if (CurrentMatch == Guid.Empty)
            {
                //display all based on time diff
                ddlTimeDiff.Visible = true;

                betList = Entity.Bet.GetAllBetByTimeDiff(TimeDiff);
            }
            else
            {
                //display all match bet
                ddlTimeDiff.Visible = false;

                Match m = new Match(CurrentMatch);

                ArsenalTeam homeT = Arsenal_Team.Cache.Load(m.Home);
                ArsenalTeam awayT = Arsenal_Team.Cache.Load(m.Away);
                home = homeT.TeamDisplayName;
                away = awayT.TeamDisplayName;

                betList = Entity.Bet.GetMatchAllBet(CurrentMatch);
            }

            ltrlSingleChoiceCount.Text = betList.FindAll(delegate(Entity.Bet bet) { return bet.BetAmount.HasValue; }).Count.ToString();
            ltrlMatchResultCount.Text = betList.FindAll(delegate(Entity.Bet bet) { return !bet.BetAmount.HasValue; }).Count.ToString();

            float totalBetCount = 0;
            betList.ForEach(delegate(Entity.Bet bet) { totalBetCount += bet.BetAmount.GetValueOrDefault(0f); });
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

        public int TimeDiff
        {
            get
            {
                return int.Parse(ddlTimeDiff.SelectedValue);
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

        protected void gvBet_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Bet bet = e.Row.DataItem as Bet;

                Literal ltrlHome = e.Row.FindControl("ltrlHome") as Literal;
                Literal ltrlAway = e.Row.FindControl("ltrlAway") as Literal;
                Literal ltrlVS = e.Row.FindControl("ltrlVS") as Literal;

                Entity.CasinoItem item = Entity.CasinoItem.GetCasinoItem(bet.CasinoItemGuid);

                if (CurrentMatch != Guid.Empty)
                {
                    ltrlHome.Text = home;
                    ltrlAway.Text = away;
                }
                else
                {
                    Match m = new Match(item.MatchGuid.Value);

                    ArsenalTeam homeT = Arsenal_Team.Cache.Load(m.Home);
                    ArsenalTeam awayT = Arsenal_Team.Cache.Load(m.Away);

                    ltrlHome.Text = string.Format("<a class=\"StrongLink\" href=\"CasinoTeam.aspx?Team={0}\">{1}</a>", homeT.ID.ToString(), homeT.TeamDisplayName);
                    ltrlAway.Text = string.Format("<a class=\"StrongLink\" href=\"CasinoTeam.aspx?Team={0}\">{1}</a>", awayT.ID.ToString(), awayT.TeamDisplayName);

                    ltrlVS.Text = string.Format("<a href=\"CasinoTeam.aspx?Match={0}\"><em title=\"{1}({2})\">vs</em></a>", m.MatchGuid.ToString(), homeT.Ground, homeT.Capacity.ToString());
                }

                Literal ltrlResult = e.Row.FindControl("ltrlResult") as Literal;
                DataTable dt = Entity.BetDetail.GetBetDetailByBetID(bet.ID);

                if (dt != null)
                {
                    DataRow dr = dt.Rows[0];

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
                            Entity.MatchResultBetDetail betDetail = new MatchResultBetDetail(dt);
                            ltrlResult.Text = string.Format("{0}：{1}", betDetail.Home, betDetail.Away);
                            break;
                    }
                }

                Literal ltrlBetResult = e.Row.FindControl("ltrlBetResult") as Literal;

                if (!bet.IsWin.HasValue)
                {
                    ltrlBetResult.Text = string.Empty;
                }
                else
                {
                    if (bet.IsWin.Value)
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

                Literal ltrlBetRate = e.Row.FindControl("ltrlBetRate") as Literal;

                if (bet.BetRate.HasValue)
                {
                    ltrlBetRate.Text = Convert.ToSingle(bet.BetRate).ToString("f2");
                }
                else
                {
                    ltrlBetRate.Text = "/";
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

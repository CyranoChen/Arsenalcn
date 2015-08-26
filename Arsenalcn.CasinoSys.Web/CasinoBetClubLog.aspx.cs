using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

using Arsenalcn.CasinoSys.Entity;

namespace Arsenalcn.CasinoSys.Web
{
    public partial class CasinoBetClubLog : Common.BasePage
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

                hlBetLog.NavigateUrl = string.Format("CasinoBetLog.aspx?Match={0}", CurrentMatch.ToString());
                hlBetLog.Target = "_self";
                hlBetLog.Visible = true;
            }
            else
            {
                ctrlCasinoHeader.Visible = false;
                hlBetLog.Visible = false;
            }

            #endregion

            if (!IsPostBack)
            {
                // Bind ddlGroup
                var dtClub = Entity.UserClub.GetAllClubs();
                if (dtClub != null)
                {
                    ddlClub.DataSource = dtClub;
                    ddlClub.DataTextField = "FullName";
                    ddlClub.DataValueField = "ClubUid";
                    ddlClub.DataBind();

                    var item = new ListItem("所有球会", "0");
                    ddlClub.Items.Insert(0, item);
                }
                else
                    ddlClub.Visible = false;
            }

            BindData();
        }

        private string home = string.Empty;
        private string away = string.Empty;

        private void BindData()
        {
            List<Entity.Bet> betList;

            if (CurrentMatch != Guid.Empty)
            {
                var m = new Match(CurrentMatch);

                var homeT = Team.Cache.Load(m.Home);
                var awayT = Team.Cache.Load(m.Away);
                home = homeT.TeamDisplayName;
                away = awayT.TeamDisplayName;

                betList = Entity.Bet.GetMatchAllBet(CurrentMatch);

                if (betList != null && betList.Count > 0)
                {
                    ltrlSingleChoiceCount.Text = betList.FindAll(delegate(Entity.Bet bet) { return bet.BetAmount.HasValue; }).Count.ToString();
                    ltrlMatchResultCount.Text = betList.FindAll(delegate(Entity.Bet bet) { return !bet.BetAmount.HasValue; }).Count.ToString();

                    float totalBetCount = 0;
                    betList.ForEach(delegate(Entity.Bet bet) { totalBetCount += bet.BetAmount.GetValueOrDefault(0f); });
                    ltrlTotalBetCount.Text = totalBetCount.ToString("N0");

                    var itemGuid = Entity.CasinoItem.GetCasinoItemGuidByMatch(CurrentMatch, CasinoType.MatchResult);

                    if (itemGuid.HasValue)
                        betList = Entity.Bet.GetBetByCasinoItemGuid(itemGuid.Value, null);
                    else
                        betList = null;

                    gvBet.PageSize = betList.Count;
                    gvBet.DataSource = betList;
                    gvBet.DataBind();
                }
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
                var bet = e.Row.DataItem as Entity.Bet;

                var item = Entity.CasinoItem.GetCasinoItem(bet.CasinoItemGuid);

                var ltrlClub = e.Row.FindControl("ltrlClub") as Literal;

                var pathAcnClub = Entity.ConfigGlobal.PluginAcnClubPath;

                if ((!string.IsNullOrEmpty(pathAcnClub)) && (pathAcnClub != Boolean.FalseString.ToLower()))
                {
                    var drClub = Entity.UserClub.GetUserClubHistoryInfo(bet.UserID, bet.BetTime);
                    if ((drClub != null) && ((drClub["ClubUid"].ToString() == ddlClub.SelectedValue) || (ddlClub.SelectedValue == "0")))
                    {
                        ltrlClub.Text = string.Format("<a href=\"/{0}/ClubView.aspx?ClubID={1}\" target=\"_blank\">{2}</a>", pathAcnClub, drClub["ClubUid"].ToString(), drClub["ClubName"]);
                    }
                    else if ((drClub == null) && (ddlClub.SelectedValue == "0"))
                    {
                        ltrlClub.Text = "/";
                    }
                    else
                    {
                        e.Row.Visible = false;
                        return;
                    }
                }
                else
                    Response.Redirect(string.Format("CasinoBetLog.aspx?Match={0}", CurrentMatch));


                var ltrlResult = e.Row.FindControl("ltrlResult") as Literal;
                var dt = Entity.BetDetail.GetBetDetailByBetID(bet.ID);

                if (dt != null)
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
                            ltrlResult.Text = string.Format("{0}：{1}", betDetail.Home, betDetail.Away);
                            break;
                    }
                }

                var ltrlBetResult = e.Row.FindControl("ltrlBetResult") as Literal;

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
            }
        }

        protected void ddlClub_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvBet.PageIndex = 0;

            BindData();
        }
    }
}
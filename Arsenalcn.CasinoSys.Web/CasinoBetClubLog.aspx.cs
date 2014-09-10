using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

using Arsenalcn.CasinoSys.Entity;
using ArsenalTeam = Arsenalcn.CasinoSys.Entity.Arsenal.Team;

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
                DataTable dtClub = Entity.UserClub.GetAllClubs();
                if (dtClub != null)
                {
                    ddlClub.DataSource = dtClub;
                    ddlClub.DataTextField = "FullName";
                    ddlClub.DataValueField = "ClubUid";
                    ddlClub.DataBind();

                    ListItem item = new ListItem("所有球会", "0");
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
                Match m = new Match(CurrentMatch);

                ArsenalTeam homeT = Team.Cache.Load(m.Home);
                ArsenalTeam awayT = Team.Cache.Load(m.Away);
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

                    Guid? itemGuid = Entity.CasinoItem.GetCasinoItemGuidByMatch(CurrentMatch, CasinoItem.CasinoType.MatchResult);

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
                Entity.Bet bet = e.Row.DataItem as Entity.Bet;

                Entity.CasinoItem item = Entity.CasinoItem.GetCasinoItem(bet.CasinoItemGuid);

                Literal ltrlClub = e.Row.FindControl("ltrlClub") as Literal;

                string pathAcnClub = Entity.ConfigGlobal.PluginAcnClubPath;

                if ((!string.IsNullOrEmpty(pathAcnClub)) && (pathAcnClub != Boolean.FalseString.ToLower()))
                {
                    DataRow drClub = Entity.UserClub.GetUserClubHistoryInfo(bet.UserID, bet.BetTime);
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


                Literal ltrlResult = e.Row.FindControl("ltrlResult") as Literal;
                DataTable dt = Entity.BetDetail.GetBetDetailByBetID(bet.ID);

                if (dt != null)
                {
                    DataRow dr = dt.Rows[0];

                    switch (item.ItemType)
                    {
                        case CasinoItem.CasinoType.SingleChoice:
                            if (dr["DetailName"].ToString() == MatchChoiceOption.HomeWinValue)
                                ltrlResult.Text = "主队胜";
                            else if (dr["DetailName"].ToString() == MatchChoiceOption.DrawValue)
                                ltrlResult.Text = "双方平";
                            else if (dr["DetailName"].ToString() == MatchChoiceOption.AwayWinValue)
                                ltrlResult.Text = "客队胜";

                            break;
                        case CasinoItem.CasinoType.MatchResult:
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
                        if (item.ItemType == CasinoItem.CasinoType.SingleChoice)
                            ltrlBetResult.Text = "<span class=\"CasinoSys_True\" title=\"猜对输赢\"></span>";
                        else if (item.ItemType == CasinoItem.CasinoType.MatchResult)
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
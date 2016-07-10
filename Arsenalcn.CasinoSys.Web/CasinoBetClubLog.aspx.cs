using System;
using System.Web.UI.WebControls;
using Arsenalcn.CasinoSys.Entity;
using Arsenalcn.CasinoSys.Web.Common;
using Arsenalcn.CasinoSys.Web.Control;

namespace Arsenalcn.CasinoSys.Web
{
    public partial class CasinoBetClubLog : BasePage
    {
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

            ctrlMenuTabBar.CurrentMenu = CasinoMenuType.CasinoBetLog;

            if (CurrentMatch != Guid.Empty)
            {
                ctrlCasinoHeader.UserId = userid;
                ctrlCasinoHeader.MatchGuid = CurrentMatch;
                ctrlCasinoHeader.IsHistoryView = false;

                hlBetLog.NavigateUrl = $"CasinoBetLog.aspx?Match={CurrentMatch}";
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
                var dtClub = UserClub.GetAllClubs();
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

        private void BindData()
        {
            if (CurrentMatch != Guid.Empty)
            {
                //var m = new Match(CurrentMatch);

                //var homeT = Team.Cache.Load(m.Home);
                //var awayT = Team.Cache.Load(m.Away);

                var betList = Bet.GetMatchAllBet(CurrentMatch);

                if (betList != null && betList.Count > 0)
                {
                    ltrlSingleChoiceCount.Text = betList.FindAll(bet => bet.BetAmount.HasValue).Count.ToString();
                    ltrlMatchResultCount.Text = betList.FindAll(bet => !bet.BetAmount.HasValue).Count.ToString();

                    float totalBetCount = 0;
                    betList.ForEach(delegate (Bet bet) { totalBetCount += bet.BetAmount.GetValueOrDefault(0f); });
                    ltrlTotalBetCount.Text = totalBetCount.ToString("N0");

                    var itemGuid = CasinoItem.GetCasinoItemGuidByMatch(CurrentMatch, CasinoType.MatchResult);

                    betList = itemGuid.HasValue ? Bet.GetBetByCasinoItemGuid(itemGuid.Value, null) : null;

                    if (betList != null)
                    {
                        gvBet.PageSize = betList.Count;
                        gvBet.DataSource = betList;
                    }

                    gvBet.DataBind();
                }
            }
        }

        protected void gvBet_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var bet = e.Row.DataItem as Bet;

                if (bet != null)
                {
                    var item = CasinoItem.GetCasinoItem(bet.CasinoItemGuid);

                    var ltrlClub = e.Row.FindControl("ltrlClub") as Literal;

                    if (ltrlClub != null)
                    {
                        var pathAcnClub = ConfigGlobal.PluginAcnClubPath;

                        if ((!string.IsNullOrEmpty(pathAcnClub)) && (pathAcnClub != bool.FalseString.ToLower()))
                        {
                            var drClub = UserClub.GetUserClubHistoryInfo(bet.UserID, bet.BetTime);
                            if ((drClub != null) &&
                                ((drClub["ClubUid"].ToString() == ddlClub.SelectedValue) ||
                                 (ddlClub.SelectedValue == "0")))
                            {
                                ltrlClub.Text =
                                    $"<a href=\"/{pathAcnClub}/ClubView.aspx?ClubID={drClub["ClubUid"]}\" target=\"_blank\">{drClub["ClubName"]}</a>";
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
                            Response.Redirect($"CasinoBetLog.aspx?Match={CurrentMatch}");
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
            }
        }

        protected void ddlClub_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvBet.PageIndex = 0;

            BindData();
        }
    }
}
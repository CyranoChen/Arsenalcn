using System;
using System.Data;
using System.Web.UI.WebControls;
using Arsenalcn.CasinoSys.Entity;
using Arsenalcn.CasinoSys.Web.Common;
using Arsenalcn.CasinoSys.Web.Control;
using Discuz.Forum;

namespace Arsenalcn.CasinoSys.Web
{
    public partial class MyBetLog : BasePage
    {
        private int CurrentUserId
        {
            get
            {
                int uid;
                if (!string.IsNullOrEmpty(Request.QueryString["UserID"]) &&
                    int.TryParse(Request.QueryString["UserID"], out uid))
                {
                    return uid;
                }
                return userid;
            }
        }

        private string CurrentUserName
        {
            get
            {
                var sUser = Users.GetShortUserInfo(CurrentUserId);
                return sUser.Username.Trim();
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

            ctrlMenuTabBar.CurrentMenu = CasinoMenuType.CasinoPortal;

            ctrlGamblerHeader.UserId = CurrentUserId;
            ctrlGamblerHeader.UserName = CurrentUserName;

            #endregion

            BindData();
        }

        private void BindData()
        {
            gvBetLog.DataSource = Bet.GetUserBetHistoryView(CurrentUserId);
            gvBetLog.DataBind();
        }

        protected void gvBetLog_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var drv = e.Row.DataItem as DataRowView;

                if (drv != null)
                {
                    var m = new Match((Guid) drv["MatchGuid"]);

                    var hlHome = e.Row.FindControl("hlHome") as HyperLink;
                    var hlAway = e.Row.FindControl("hlAway") as HyperLink;
                    var hlVersus = e.Row.FindControl("hlVersus") as HyperLink;

                    if (hlHome != null && hlAway != null && hlVersus != null)
                    {
                        var tHome = Team.Cache.Load(m.Home);
                        var tAway = Team.Cache.Load(m.Away);

                        hlHome.Text = tHome.TeamDisplayName;
                        hlHome.NavigateUrl = $"CasinoTeam.aspx?Team={m.Home}";

                        hlAway.Text = tAway.TeamDisplayName;
                        hlAway.NavigateUrl = $"CasinoTeam.aspx?Team={m.Away}";

                        hlVersus.NavigateUrl = $"CasinoTeam.aspx?Match={m.MatchGuid}";
                        hlVersus.Text =
                            $"<em title=\"{tHome.Ground}{(tHome.Capacity.HasValue ? ("(" + tHome.Capacity.Value + ")") : string.Empty)}\">vs</em>";
                    }
                }

                var ltrlResult = e.Row.FindControl("ltrlResult") as Literal;
                var ltrlBetResult = e.Row.FindControl("ltrlBetResult") as Literal;
                var ltrlBetRate = e.Row.FindControl("ltrlBetRate") as Literal;
                var btnReturnBet = e.Row.FindControl("btnReturnBet") as LinkButton;

                if (drv != null && ltrlResult != null && ltrlBetResult != null && ltrlBetRate != null &&
                    btnReturnBet != null)
                {
                    var itemGuid = (Guid) drv["CasinoItemGuid"];

                    var item = CasinoItem.GetCasinoItem(itemGuid);
                    var dt = BetDetail.GetBetDetailByBetId((int) drv["ID"]);

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
                                ltrlResult.Text = $"{betDetail.Home}：{betDetail.Away}";

                                break;
                        }
                    }

                    if (!Convert.IsDBNull(drv["IsWin"]))
                    {
                        ltrlBetResult.Visible = true;
                        btnReturnBet.Visible = false;
                        if (Convert.ToBoolean(drv["IsWin"]))
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
                    else if (Convert.IsDBNull(drv["IsWin"]) && (CurrentUserId == userid) &&
                             (item.CloseTime > DateTime.Now))
                    {
                        btnReturnBet.Visible = true;
                        btnReturnBet.CommandArgument = drv["ID"].ToString();
                    }
                    else
                    {
                        ltrlBetResult.Visible = false;
                        btnReturnBet.Visible = false;
                    }
                }

                if (drv != null && ltrlBetRate != null)
                {
                    ltrlBetRate.Text = Convert.IsDBNull(drv["BetRate"]) ? "/" : Convert.ToSingle(drv["BetRate"]).ToString("f2");
                }
            }
        }

        protected void gvBetLog_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvBetLog.PageIndex = e.NewPageIndex;
            gvBetLog.DataBind();
        }

        protected void gvBetLog_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ReturnBet")
            {
                var bet = new Bet(Convert.ToInt32(e.CommandArgument.ToString()));

                try
                {
                    if (bet.UserID == userid)
                    {
                        Bet.ReturnBet(bet.ID);
                        ClientScript.RegisterClientScriptBlock(typeof (string), "success",
                            "alert('投注退还成功');window.location.href=window.location.href", true);
                    }
                    else
                        throw new Exception("非本人投注项");
                }
                catch
                {
                    ClientScript.RegisterClientScriptBlock(typeof (string), "failed", "alert('投注退还失败');", true);
                }

                BindData();
            }
        }
    }
}
using System;
using System.Data;
using System.Web.UI.WebControls;
using Arsenalcn.CasinoSys.Entity;
using Arsenalcn.CasinoSys.Web.Common;
using Arsenalcn.CasinoSys.Web.Control;
using Discuz.Forum;

namespace Arsenalcn.CasinoSys.Web
{
    public partial class MyBonusLog : BasePage
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
            gvBonusLog.DataSource = Bet.GetUserBetMatch(CurrentUserId);
            gvBonusLog.DataBind();
        }

        protected void gvBonusLog_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvBonusLog.PageIndex = e.NewPageIndex;

            BindData();
        }

        protected void gvBonusLog_RowDataBound(object sender, GridViewRowEventArgs e)
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

                    var betList = Bet.GetUserMatchAllBet(CurrentUserId, m.MatchGuid);
                    betList.RemoveAll(bet => !bet.IsWin.HasValue);

                    float totalBetCount = 0, totalWin = 0;
                    var rpBonus = false;

                    foreach (var bet in betList)
                    {
                        totalBetCount += bet.BetAmount.GetValueOrDefault(0f);
                        if (bet.Earning != null) totalWin += (bet.Earning.Value - bet.BetAmount.GetValueOrDefault(0f));

                        if (bet.IsWin != null && (bet.IsWin.Value && !bet.BetAmount.HasValue))
                            rpBonus = true;
                    }

                    var ltrlResult = e.Row.FindControl("ltrlResult") as Literal;
                    var ltrlBetCount = e.Row.FindControl("ltrlBetCount") as Literal;
                    var ltrWinLose = e.Row.FindControl("ltrWinLose") as Literal;
                    var ltrlExtraBonus = e.Row.FindControl("ltrlExtraBonus") as Literal;

                    ltrlBetCount.Text = totalBetCount.ToString("N0");
                    ltrWinLose.Text = totalWin.ToString("N2");

                    ltrlExtraBonus.Text = rpBonus ? "RP+1" : "/";

                    if (totalWin > 0)
                    {
                        ltrlResult.Text = "<span class=\"CasinoSys_Agree\" title=\"盈余\"></span>";
                        e.Row.CssClass = "RowCasinoSys_True";
                    }
                    else
                    {
                        ltrlResult.Text = "<span class=\"CasinoSys_Disagree\" title=\"亏损\"></span>";
                    }
                }
            }
        }
    }
}
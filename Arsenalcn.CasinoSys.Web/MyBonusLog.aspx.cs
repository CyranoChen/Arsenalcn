using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

using Discuz.Entity;
using Discuz.Forum;

namespace Arsenalcn.CasinoSys.Web
{
    public partial class MyBonusLog : Common.BasePage
    {
        protected override void OnInit(EventArgs e)
        {
            AnonymousRedirect = true;

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            #region Assign Control Property

            ctrlLeftPanel.UserID = userid;
            ctrlLeftPanel.UserName = username;

            ctrlFieldTooBar.UserID = userid;

            ctrlMenuTabBar.CurrentMenu = Arsenalcn.CasinoSys.Web.Control.CasinoMenuType.CasinoPortal;

            ctrlGamblerHeader.UserID = CurrentUserID;
            ctrlGamblerHeader.UserName = CurrentUserName;

            #endregion

            BindData();
        }

        private void BindData()
        {
            gvBonusLog.DataSource = Entity.Bet.GetUserBetMatch(CurrentUserID);
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
                DataRowView drv = e.Row.DataItem as DataRowView;

                Guid matchGuid = (Guid)drv["MatchGuid"];

                List<Entity.Bet> betList = Entity.Bet.GetUserMatchAllBet(CurrentUserID, matchGuid);
                betList.RemoveAll(delegate(Entity.Bet bet) { return !bet.IsWin.HasValue; });

                float totalBetCount = 0, totalWin = 0;
                bool rpBonus = false;

                foreach (Entity.Bet bet in betList)
                {
                    totalBetCount += bet.BetAmount.GetValueOrDefault(0f);
                    totalWin += (bet.Earning.Value - bet.BetAmount.GetValueOrDefault(0f));

                    if (bet.IsWin.Value && !bet.BetAmount.HasValue)
                        rpBonus = true;
                }

                Literal ltrlResult = e.Row.FindControl("ltrlResult") as Literal;
                Literal ltrlBetCount = e.Row.FindControl("ltrlBetCount") as Literal;
                Literal ltrWinLose = e.Row.FindControl("ltrWinLose") as Literal;
                Literal ltrlExtraBonus = e.Row.FindControl("ltrlExtraBonus") as Literal;

                ltrlBetCount.Text = totalBetCount.ToString("N0");
                ltrWinLose.Text = totalWin.ToString("N2");

                if (rpBonus)
                    ltrlExtraBonus.Text = "RP+1";
                else
                    ltrlExtraBonus.Text = "/";

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

        private int CurrentUserID
        {
            get
            {
                int _userid;
                if (!string.IsNullOrEmpty(Request.QueryString["UserID"]) && int.TryParse(Request.QueryString["UserID"], out _userid))
                {
                    return _userid;
                }
                else
                    return this.userid;
            }
        }

        private string CurrentUserName
        {
            get
            {
                ShortUserInfo sUser = AdminUsers.GetShortUserInfo(CurrentUserID);
                return sUser.Username.Trim();
            }
        }
    }
}

using System;
using System.Data;
using System.Web.UI.WebControls;

using Arsenalcn.CasinoSys.Entity;
using ArsenalTeam = Arsenalcn.CasinoSys.Entity.Arsenal.Team;

using Discuz.Entity;
using Discuz.Forum;

namespace Arsenalcn.CasinoSys.Web
{
    public partial class MyBetLog : Common.BasePage
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
            gvBetLog.DataSource = Entity.Bet.GetUserBetHistoryView(CurrentUserID);
            gvBetLog.DataBind();
        }

        protected void gvBetLog_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = e.Row.DataItem as DataRowView;

                Match m = new Match((Guid)drv["MatchGuid"]);

                HyperLink hlHome = e.Row.FindControl("hlHome") as HyperLink;
                HyperLink hlAway = e.Row.FindControl("hlAway") as HyperLink;
                HyperLink hlVersus = e.Row.FindControl("hlVersus") as HyperLink;

                if (hlHome != null && hlAway != null && hlVersus != null)
                {
                    ArsenalTeam tHome = Arsenal_Team.Cache.Load(m.Home);
                    ArsenalTeam tAway = Arsenal_Team.Cache.Load(m.Away);

                    hlHome.Text = tHome.TeamDisplayName;
                    hlHome.NavigateUrl = string.Format("CasinoTeam.aspx?Team={0}", m.Home);

                    hlAway.Text = tAway.TeamDisplayName;
                    hlAway.NavigateUrl = string.Format("CasinoTeam.aspx?Team={0}", m.Away);

                    hlVersus.NavigateUrl = string.Format("CasinoTeam.aspx?Match={0}", m.MatchGuid.ToString());
                    hlVersus.Text = string.Format("<em title=\"{0}{1}\">vs</em>", tHome.Ground,
                        tHome.Capacity.HasValue ? ("(" + tHome.Capacity.Value.ToString() + ")") : string.Empty);
                }

                Literal ltrlResult = e.Row.FindControl("ltrlResult") as Literal;
                Literal ltrlBetResult = e.Row.FindControl("ltrlBetResult") as Literal;
                Literal ltrlBetRate = e.Row.FindControl("ltrlBetRate") as Literal;
                LinkButton btnReturnBet = e.Row.FindControl("btnReturnBet") as LinkButton;

                Guid itemGuid = (Guid)drv["CasinoItemGuid"];

                Entity.CasinoItem item = Entity.CasinoItem.GetCasinoItem(itemGuid);
                DataTable dt = Entity.BetDetail.GetBetDetailByBetID((int)drv["ID"]);

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
                else if (Convert.IsDBNull(drv["IsWin"]) && (CurrentUserID == userid) && (item.CloseTime > DateTime.Now))
                {
                    btnReturnBet.Visible = true;
                    btnReturnBet.CommandArgument = drv["ID"].ToString();
                }
                else
                {
                    ltrlBetResult.Visible = false;
                    btnReturnBet.Visible = false;
                }

                if (Convert.IsDBNull(drv["BetRate"]))
                {
                    ltrlBetRate.Text = "/";
                }
                else
                {
                    ltrlBetRate.Text = Convert.ToSingle(drv["BetRate"]).ToString("f2");
                }
            }
        }

        protected void gvBetLog_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvBetLog.PageIndex = e.NewPageIndex;
            gvBetLog.DataBind();

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

        protected void gvBetLog_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ReturnBet")
            {
                Bet bet = new Bet(Convert.ToInt32(e.CommandArgument.ToString()));

                try
                {
                    if (bet.UserID == userid)
                    {
                        Bet.ReturnBet(bet.ID);
                        this.ClientScript.RegisterClientScriptBlock(typeof(string), "success", "alert('投注退还成功');window.location.href=window.location.href", true);
                    }
                    else
                        throw new Exception("非本人投注项");
                }
                catch
                {
                    this.ClientScript.RegisterClientScriptBlock(typeof(string), "failed", "alert('投注退还失败');", true);
                }

                BindData();
            }
        }
    }
}

using System;
using System.Web.UI.WebControls;
using Arsenalcn.CasinoSys.Entity;
using Arsenalcn.CasinoSys.Web.Common;

namespace Arsenalcn.CasinoSys.Web
{
    public partial class AdminBet : AdminBasePage
    {
        private int TimeDiff => int.Parse(ddlTimeDiff.SelectedValue);

        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = username;

            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            //display all based on time diff
            var betList = Bet.GetAllBetByTimeDiff(TimeDiff);
            gvBet.DataSource = betList;
            gvBet.DataBind();
        }

        protected void gvBet_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var bet = e.Row.DataItem as Bet;

                var ltrlHome = e.Row.FindControl("ltrlHome") as Literal;
                var ltrlAway = e.Row.FindControl("ltrlAway") as Literal;
                var btnReturnBet = e.Row.FindControl("btnReturnBet") as LinkButton;

                if (bet != null)
                {
                    var item = CasinoItem.GetCasinoItem(bet.CasinoItemGuid);

                    if (item.MatchGuid != null)
                    {
                        var m = new Match(item.MatchGuid.Value);
                        {
                            var homeT = Team.Cache.Load(m.Home);
                            var awayT = Team.Cache.Load(m.Away);

                            if (ltrlHome != null) ltrlHome.Text = homeT.TeamDisplayName;
                            if (ltrlAway != null) ltrlAway.Text = awayT.TeamDisplayName;
                        }
                    }

                    var ltrlResult = e.Row.FindControl("ltrlResult") as Literal;
                    var dt = BetDetail.GetBetDetailByBetID(bet.ID);

                    if (dt != null && ltrlResult != null)
                    {
                        var dr = dt.Rows[0];

                        switch (item.ItemType)
                        {
                            case CasinoType.SingleChoice:
                                if (dr["DetailName"].ToString() == MatchChoiceOption.HomeWinValue)
                                {
                                    ltrlResult.Text = "主队胜";
                                }
                                else if (dr["DetailName"].ToString() == MatchChoiceOption.DrawValue)
                                {
                                    ltrlResult.Text = "双方平";
                                }
                                else if (dr["DetailName"].ToString() == MatchChoiceOption.AwayWinValue)
                                {
                                    ltrlResult.Text = "客队胜";
                                }

                                break;
                            case CasinoType.MatchResult:
                                var betDetail = new MatchResultBetDetail(dt);
                                ltrlResult.Text = $"{betDetail.Home}：{betDetail.Away}";
                                break;
                        }
                    }
                }

                var ltrlBetRate = e.Row.FindControl("ltrlBetRate") as Literal;

                if (bet != null)
                {
                    if (ltrlBetRate != null)
                        ltrlBetRate.Text = bet.BetRate.HasValue ? Convert.ToSingle(bet.BetRate).ToString("f2") : "/";

                    if (btnReturnBet != null)
                    {
                        btnReturnBet.Visible = true;
                        btnReturnBet.CommandArgument = bet.ID.ToString();
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

        protected void gvBet_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ReturnBet")
            {
                var bet = new Bet(Convert.ToInt32(e.CommandArgument.ToString()));

                try
                {
                    Bet.ReturnBet(bet.ID);
                    ClientScript.RegisterClientScriptBlock(typeof (string), "success",
                        "alert('投注退还成功');window.location.href=window.location.href", true);
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterClientScriptBlock(typeof (string), "failed", $"alert('{ex.Message}');", true);
                }

                BindData();
            }
        }
    }
}
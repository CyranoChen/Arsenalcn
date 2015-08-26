using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

using Arsenalcn.CasinoSys.Entity;

namespace Arsenalcn.CasinoSys.Web
{
    public partial class AdminBet : Common.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = this.username;

            if (!IsPostBack)
            {
                BindData();
            }
        }

        private string home = string.Empty;
        private string away = string.Empty;

        private void BindData()
        {
            List<Entity.Bet> betList;

            //display all based on time diff
            betList = Bet.GetAllBetByTimeDiff(TimeDiff);
            gvBet.DataSource = betList;
            gvBet.DataBind();

        }

        public int TimeDiff
        {
            get
            {
                return int.Parse(ddlTimeDiff.SelectedValue);
            }
        }

        protected void gvBet_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var bet = e.Row.DataItem as Bet;

                var ltrlHome = e.Row.FindControl("ltrlHome") as Literal;
                var ltrlAway = e.Row.FindControl("ltrlAway") as Literal;
                var btnReturnBet = e.Row.FindControl("btnReturnBet") as LinkButton;

                var item = Entity.CasinoItem.GetCasinoItem(bet.CasinoItemGuid);

                var m = new Match(item.MatchGuid.Value);
                if (m != null)
                {
                    var homeT = Team.Cache.Load(m.Home);
                    var awayT = Team.Cache.Load(m.Away);

                    ltrlHome.Text = homeT.TeamDisplayName;
                    ltrlAway.Text = awayT.TeamDisplayName;
                }

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

                var ltrlBetRate = e.Row.FindControl("ltrlBetRate") as Literal;

                if (bet.BetRate.HasValue)
                {
                    ltrlBetRate.Text = Convert.ToSingle(bet.BetRate).ToString("f2");
                }
                else
                {
                    ltrlBetRate.Text = "/";
                }

                btnReturnBet.Visible = true;
                btnReturnBet.CommandArgument = bet.ID.ToString();
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
                    this.ClientScript.RegisterClientScriptBlock(typeof(string), "success", "alert('投注退还成功');window.location.href=window.location.href", true);
                }
                catch (Exception ex)
                {
                    this.ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}');", ex.Message.ToString()), true);
                }

                BindData();
            }
        }
    }
}

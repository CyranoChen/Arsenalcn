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
                Bet bet = e.Row.DataItem as Bet;

                Literal ltrlHome = e.Row.FindControl("ltrlHome") as Literal;
                Literal ltrlAway = e.Row.FindControl("ltrlAway") as Literal;
                LinkButton btnReturnBet = e.Row.FindControl("btnReturnBet") as LinkButton;

                CasinoItem item = Entity.CasinoItem.GetCasinoItem(bet.CasinoItemGuid);

                Match m = new Match(item.MatchGuid.Value);
                if (m != null)
                {
                    Team homeT = new Team(m.Home);
                    Team awayT = new Team(m.Away);

                    ltrlHome.Text = homeT.TeamDisplayName;
                    ltrlAway.Text = awayT.TeamDisplayName;
                }

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

                Literal ltrlBetRate = e.Row.FindControl("ltrlBetRate") as Literal;

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
                Bet bet = new Bet(Convert.ToInt32(e.CommandArgument.ToString()));

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

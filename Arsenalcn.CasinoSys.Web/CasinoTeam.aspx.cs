using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace Arsenalcn.CasinoSys.Web
{
    public partial class CasinoTeam : Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region Assign Control Property

            ctrlLeftPanel.UserID = userid;
            ctrlLeftPanel.UserName = username;

            ctrlFieldTooBar.UserID = userid;

            ctrlMenuTabBar.CurrentMenu = Arsenalcn.CasinoSys.Web.Control.CasinoMenuType.CasinoGame;

            #endregion

            BindData();
        }

        private void BindData()
        {
            DataTable dt = null;

            if (CurrentTeam != Guid.Empty)
            {
                dt = Entity.CasinoItem.GetEndViewByTeam(CurrentTeam);
                ctrlTeamHeader.TeamGuid = CurrentTeam;
                ctrlTeamHeader.Visible = true;
                ctrlCasinoHeader.Visible = false;
            }
            else if (CurrentMatch != Guid.Empty)
            {
                dt = Entity.CasinoItem.GetHistoryViewByMatch(CurrentMatch);
                ctrlCasinoHeader.MatchGuid = CurrentMatch;
                ctrlCasinoHeader.IsHistoryView = true;
                ctrlCasinoHeader.Visible = true;
                ctrlTeamHeader.Visible = false;
            }
            else
                Response.Redirect("CasinoPortal.aspx");

            if (dt != null)
            {
                gvMatch.DataSource = dt;
                gvMatch.DataBind();

                ltrlMatchCount.Text = dt.Rows.Count.ToString();
            }
            else
                Response.Redirect("CasinoPortal.aspx");
        }

        public Guid CurrentTeam
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["Team"]))
                {
                    try
                    {
                        return new Guid(Request.QueryString["Team"]);
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

        protected void gvMatch_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = e.Row.DataItem as DataRowView;

                Guid matchGuid = (Guid)drv["MatchGuid"];

                List<Entity.Bet> betList = Entity.Bet.GetMatchAllBet(matchGuid);

                Literal ltrlTotalBetCount = e.Row.FindControl("ltrlTotalBetCount") as Literal;
                Literal ltrlTotalBetCash = e.Row.FindControl("ltrlTotalBetCash") as Literal;
                Literal ltrlTotalWin = e.Row.FindControl("ltrlTotalWin") as Literal;

                ltrlTotalBetCount.Text = betList.Count.ToString();

                float totalbetCash = 0;
                betList.ForEach(delegate(Entity.Bet bet) { totalbetCash += bet.BetAmount.GetValueOrDefault(0f); });
                ltrlTotalBetCash.Text = totalbetCash.ToString("N0");

                float totalWin = 0;
                betList.ForEach(delegate(Entity.Bet bet) { totalWin += (bet.BetAmount.GetValueOrDefault(0f) - bet.Earning.GetValueOrDefault(0f)); });
                ltrlTotalWin.Text = totalWin.ToString("N2");
            }
        }

        protected void gvMatch_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMatch.PageIndex = e.NewPageIndex;

            BindData();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

using Arsenalcn.CasinoSys.Entity;
using ArsenalLeauge = Arsenalcn.CasinoSys.Entity.Arsenal.League;

namespace Arsenalcn.CasinoSys.Web
{
    public partial class CasinoGame : Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region Assign Control Property

            ctrlLeftPanel.UserID = userid;
            ctrlLeftPanel.UserName = username;

            ctrlFieldTooBar.UserID = userid;

            ctrlMenuTabBar.CurrentMenu = Arsenalcn.CasinoSys.Web.Control.CasinoMenuType.CasinoGame;

            ctrlLeagueHeader.CurrLeagueGuid = CurrentLeague;
            ctrlLeagueHeader.PageURL = "CasinoGame.aspx";

            #endregion

            if (!IsPostBack)
            {
                if (CurrentLeague != Guid.Empty)
                {
                    List<ArsenalLeauge> list = Entity.League.Cache.GetSeasonsByLeagueGuid(CurrentLeague);

                    ddlSeason.DataSource = list;
                    ddlSeason.DataTextField = "LeagueSeason";
                    ddlSeason.DataValueField = "LeagueGuid";
                    ddlSeason.DataBind();

                    ddlSeason.SelectedValue = CurrentLeague.ToString();
                }
                else
                {
                    ddlSeason.Visible = false;
                    ddlGameTip.Visible = true;
                }

                BindData();
            }

        }

        private void BindData()
        {
            DataTable dt;

            if (CurrentGroup != Guid.Empty)
            {
                Entity.Group group = new Group(CurrentGroup);
                dt = Entity.CasinoItem.GetEndViewByMatch(group.LeagueGuid, group.GroupGuid, group.IsTable);
            }
            else if (ddlSeason.Items.Count != 0)
                dt = Entity.CasinoItem.GetEndViewByMatch(new Guid(ddlSeason.SelectedValue));
            else
                dt = Entity.CasinoItem.GetEndViewByMatch();

            if (dt != null)
                ltrlMatchCount.Text = dt.Rows.Count.ToString();
            else
                ltrlMatchCount.Text = "0";

            gvMatch.DataSource = dt;
            gvMatch.DataBind();
        }

        public Guid CurrentLeague
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["League"]))
                {
                    try { return new Guid(Request.QueryString["League"]); }
                    catch { return Guid.Empty; }
                }
                else if (CurrentGroup != Guid.Empty)
                {
                    try { return new Group(CurrentGroup).LeagueGuid; }
                    catch { return Guid.Empty; }
                }
                else
                    return Guid.Empty;
            }
        }

        public Guid CurrentGroup
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["Group"]))
                {
                    try
                    {
                        return new Guid(Request.QueryString["Group"]);
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

        protected void ddlSeason_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvMatch.PageIndex = 0;

            BindData();
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

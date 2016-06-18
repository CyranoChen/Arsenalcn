using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Arsenalcn.CasinoSys.Entity;
using Arsenalcn.CasinoSys.Web.Common;
using Arsenalcn.CasinoSys.Web.Control;

namespace Arsenalcn.CasinoSys.Web
{
    public partial class CasinoGambler : BasePage
    {
        private Guid CurrentLeague
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["League"]))
                {
                    try
                    {
                        return new Guid(Request.QueryString["League"]);
                    }
                    catch
                    {
                        return Guid.Empty;
                    }
                }
                return Guid.Empty;
            }
        }

        private Guid ContestLeague
        {
            get
            {
                if (CurrentLeague != Guid.Empty)
                    return CurrentLeague;
                return ConfigGlobal.DefaultLeagueID;
            }
        }

        private int ContestArea
        {
            get
            {
                if (!string.IsNullOrEmpty(ddlContestArea.SelectedValue))
                {
                    if (ddlContestArea.SelectedValue.Equals("Upper", StringComparison.OrdinalIgnoreCase))
                    {
                        return 1;
                    }
                    if (ddlContestArea.SelectedValue.Equals("Lower", StringComparison.OrdinalIgnoreCase))
                    {
                        return 2;
                    }
                    return 0;
                }
                return 0;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            #region Assign Control Property

            ctrlLeftPanel.UserId = userid;
            ctrlLeftPanel.UserName = username;

            ctrlFieldTooBar.UserId = userid;

            ctrlMenuTabBar.CurrentMenu = CasinoMenuType.CasinoGambler;

            #endregion

            if (!IsPostBack)
                BindData();
        }

        private void BindData()
        {
            var l = League.Cache.Load(ContestLeague);

            if (l != null)
            {
                hlGamblerLeagueView.Text = $"{l.LeagueName}{l.LeagueSeason}排行榜";
                hlGamblerLeagueView.NavigateUrl = $"CasinoGambler.aspx?League={ContestLeague}";
                hlGamblerLeagueView.Target = "_self";
            }

            List<Entity.CasinoGambler> list;

            if (!CurrentLeague.Equals(Guid.Empty))
            {
                if (ContestArea > 0)
                {
                    var tbs = ConfigGlobal.TotalBetStandard;

                    list =
                        Entity.CasinoGambler.GetCasinoGamblers(CurrentLeague).FindAll(delegate(Entity.CasinoGambler cg)
                        {
                            if (ContestArea == 1)
                                return cg.TotalBet >= tbs;
                            if (ContestArea == 2)
                                return cg.TotalBet < tbs;
                            return true;
                        });
                }
                else
                {
                    list = Entity.CasinoGambler.GetCasinoGamblers(CurrentLeague);
                }

                ddlContestArea.Visible = true;
            }
            else
            {
                list = Entity.CasinoGambler.GetCasinoGamblers();

                ddlContestArea.Visible = false;
            }

            if (list != null && list.Count > 0)
            {
                list = !string.IsNullOrEmpty(ddlOrderClause.SelectedValue)
                    ? Entity.CasinoGambler.SortCasinoGambler(list, ddlOrderClause.SelectedValue)
                    : Entity.CasinoGambler.SortCasinoGambler(list);
            }

            gvGambler.DataSource = list;
            gvGambler.DataBind();
        }

        protected void gvGambler_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvGambler.PageIndex = e.NewPageIndex;

            BindData();
        }

        protected void ddlOrderClause_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvGambler.PageIndex = 0;

            BindData();
        }

        protected void ddlContestArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvGambler.PageIndex = 0;

            BindData();
        }

        protected void gvGambler_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var cg = e.Row.DataItem as Entity.CasinoGambler;

                var lblRank = e.Row.FindControl("lblRank") as Label;

                if (lblRank == null) return;

                if (cg != null)
                    lblRank.Text = !string.IsNullOrEmpty(ddlOrderClause.SelectedValue)
                        ? $"<em>{cg.Rank}</em>"
                        : $"<em>{cg.Rank}</em>({cg.Credit})";
            }
        }
    }
}
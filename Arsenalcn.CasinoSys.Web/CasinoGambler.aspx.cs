using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Arsenalcn.CasinoSys.Entity;

namespace Arsenalcn.CasinoSys.Web
{
    public partial class CasinoGambler : Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region Assign Control Property

            ctrlLeftPanel.UserID = userid;
            ctrlLeftPanel.UserName = username;

            ctrlFieldTooBar.UserID = userid;

            ctrlMenuTabBar.CurrentMenu = Arsenalcn.CasinoSys.Web.Control.CasinoMenuType.CasinoGambler;

            #endregion

            if (!IsPostBack)
                BindData();
        }

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
                else
                    return Guid.Empty;
            }
        }

        private Guid ContestLeague
        {
            get
            {
                if (CurrentLeague != Guid.Empty)
                    return CurrentLeague;
                else
                    return Entity.ConfigGlobal.DefaultLeagueID;
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
                    else if (ddlContestArea.SelectedValue.Equals("Lower", StringComparison.OrdinalIgnoreCase))
                    {
                        return 2;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
        }

        private void BindData()
        {
            League l = new League(ContestLeague);

            hlGamblerLeagueView.Text = string.Format("{0}{1}排行榜", l.LeagueName.ToString(), l.LeagueSeason.ToString());
            hlGamblerLeagueView.NavigateUrl = string.Format("CasinoGambler.aspx?League={0}", ContestLeague.ToString());
            hlGamblerLeagueView.Target = "_self";

            List<Entity.CasinoGambler> list = null;

            if (!CurrentLeague.Equals(Guid.Empty))
            {
                if (ContestArea > 0)
                {
                    float _tbs = ConfigGlobal.TotalBetStandard;

                    list = Entity.CasinoGambler.GetCasinoGamblers(CurrentLeague).FindAll(delegate(Entity.CasinoGambler cg)
                    {
                        if (ContestArea == 1)
                            return cg.TotalBet >= _tbs;
                        else if (ContestArea == 2)
                            return cg.TotalBet < _tbs;
                        else
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
                if (!string.IsNullOrEmpty(ddlOrderClause.SelectedValue))
                {
                    list = Entity.CasinoGambler.SortCasinoGambler(list, ddlOrderClause.SelectedValue);
                }
                else
                {
                    list = Entity.CasinoGambler.SortCasinoGambler(list);
                }
            }

            gvGamlber.DataSource = list;
            gvGamlber.DataBind();
        }

        protected void gvGamlber_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvGamlber.PageIndex = e.NewPageIndex;

            BindData();
        }

        protected void ddlOrderClause_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvGamlber.PageIndex = 0;

            BindData();
        }

        protected void ddlContestArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvGamlber.PageIndex = 0;

            BindData();
        }

        protected void gvGamlber_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Entity.CasinoGambler cg = e.Row.DataItem as Entity.CasinoGambler;

                Label lblRank = e.Row.FindControl("lblRank") as Label;

                if (lblRank != null)
                {
                    if (!string.IsNullOrEmpty(ddlOrderClause.SelectedValue))
                    {
                        lblRank.Text = string.Format("<em>{0}</em>", cg.Rank.ToString());
                    }
                    else
                    {
                        lblRank.Text = string.Format("<em>{0}</em>({1})", cg.Rank.ToString(), cg.Credit.ToString());
                    }
                }
            }
        }
    }
}

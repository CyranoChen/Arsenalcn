using System;
using System.Data;
using System.Web.UI.WebControls;

using Arsenalcn.CasinoSys.Entity;
using System.Collections.Generic;

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
                list = SortCasinoGambler(list, ddlOrderClause.SelectedValue);
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

        private List<Entity.CasinoGambler> SortCasinoGambler(List<Entity.CasinoGambler> list, string orderKeyword)
        {
            if (orderKeyword.Equals("ProfitRate", StringComparison.OrdinalIgnoreCase))
            {
                list.Sort(delegate(Entity.CasinoGambler cg1, Entity.CasinoGambler cg2)
                {
                    return !cg2.ProfitRate.Equals(cg1.ProfitRate) ?
                        cg2.ProfitRate.CompareTo(cg1.ProfitRate) : cg2.Profit.CompareTo(cg1.Profit);
                });
            }
            else if (orderKeyword.Equals("TotalBet", StringComparison.OrdinalIgnoreCase))
            {
                list.Sort(delegate(Entity.CasinoGambler cg1, Entity.CasinoGambler cg2)
                {
                    return !cg2.TotalBet.Equals(cg1.TotalBet) ?
                        cg2.TotalBet.CompareTo(cg1.TotalBet) : cg2.Profit.CompareTo(cg1.Profit);
                });
            }
            else if (orderKeyword.Equals("RPBonus", StringComparison.OrdinalIgnoreCase))
            {
                list.Sort(delegate(Entity.CasinoGambler cg1, Entity.CasinoGambler cg2)
                {
                    if (!cg1.RPBonus.HasValue && !cg2.RPBonus.HasValue)
                    {
                        return cg2.Profit.CompareTo(cg1.Profit);
                    }
                    else if (cg1.RPBonus.HasValue && !cg2.RPBonus.HasValue)
                    {
                        return -1;
                    }
                    else if (!cg1.RPBonus.HasValue && cg2.RPBonus.HasValue)
                    {
                        return 1;
                    }
                    else
                    {
                        return !cg2.RPBonus.Value.Equals(cg1.RPBonus.Value) ?
                            cg2.RPBonus.Value - cg1.RPBonus.Value : cg2.Profit.CompareTo(cg1.Profit);
                    }
                });
            }
            else
            {
                list.Sort(delegate(Entity.CasinoGambler cg1, Entity.CasinoGambler cg2)
                {
                    return cg2.Profit.CompareTo(cg1.Profit);
                });
            }

            //    orderClause = string.Format("{0} DESC, Profit DESC, RPBonus DESC, TotalBet DESC", orderKeyword);
            //else if (orderKeyword == "TotalBet")
            //    orderClause = string.Format("{0} DESC, Profit DESC, RPBonus DESC, ProfitRate DESC", orderKeyword);
            //else if (orderKeyword == "RPBonus")
            //    orderClause = string.Format("{0} DESC, Profit DESC, ProfitRate DESC, TotalBet DESC", orderKeyword);
            //else
            //    orderClause = "Profit DESC, RPBonus DESC, ProfitRate DESC, TotalBet DESC";

            int _rank = 1;

            foreach (Entity.CasinoGambler cg in list)
            {
                cg.Rank = _rank++;

                //dr["Profit"] = Convert.ToSingle(dr["Earning"]) - Convert.ToSingle(dr["TotalBet"]);
                //if (Convert.ToSingle(dr["TotalBet"]) > 0f)
                //    dr["ProfitRate"] = Convert.ToSingle(dr["Profit"]) / Convert.ToSingle(dr["TotalBet"]) * 100;
                //else
                //    dr["ProfitRate"] = 0;

                //int RPBonus = Entity.Gambler.GetGamblerRPByUserID(Convert.ToInt32(dr["UserID"]), CurrentLeague);

                //if (RPBonus > 0)
                //    dr["RPBonus"] = RPBonus;
            }

            return list;
        }
    }
}

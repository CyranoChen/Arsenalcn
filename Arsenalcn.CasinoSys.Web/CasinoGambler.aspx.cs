using System;
using System.Data;
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

        private Guid HLLeague
        {
            get
            {
                if (CurrentLeague != Guid.Empty)
                    return CurrentLeague;
                else
                    return Entity.ConfigGlobal.DefaultLeagueID;
            }
        }

        private void BindData()
        {
            DataTable dt = Entity.Gambler.GetGamblerProfitView(CurrentLeague);
            League l = new League(HLLeague);

            hlGamblerLeagueView.Text = string.Format("{0}{1}排行榜", l.LeagueName.ToString(), l.LeagueSeason.ToString());
            hlGamblerLeagueView.NavigateUrl = string.Format("CasinoGambler.aspx?League={0}", HLLeague.ToString());
            hlGamblerLeagueView.Target = "_self";

            if (dt != null)
            {
                dt.Columns.Add("Rank", typeof(int));
                dt.Columns.Add("Profit", typeof(float));
                dt.Columns.Add("ProfitRate", typeof(float));
                dt.Columns.Add("RPBonus", typeof(int));

                string orderClause = string.Empty;
                string orderKeyword = ddlOrderClause.SelectedValue.ToString();
                if (orderKeyword == "ProfitRate")
                    orderClause = string.Format("{0} DESC, Profit DESC, RPBonus DESC, TotalBet DESC", orderKeyword);
                else if (orderKeyword == "TotalBet")
                    orderClause = string.Format("{0} DESC, Profit DESC, RPBonus DESC, ProfitRate DESC", orderKeyword);
                else if (orderKeyword == "RPBonus")
                    orderClause = string.Format("{0} DESC, Profit DESC, ProfitRate DESC, TotalBet DESC", orderKeyword);
                else
                    orderClause = "Profit DESC, RPBonus DESC, ProfitRate DESC, TotalBet DESC";

                foreach (DataRow dr in dt.Rows)
                {
                    dr["Profit"] = Convert.ToSingle(dr["Earning"]) - Convert.ToSingle(dr["TotalBet"]);
                    if (Convert.ToSingle(dr["TotalBet"]) > 0f)
                        dr["ProfitRate"] = Convert.ToSingle(dr["Profit"]) / Convert.ToSingle(dr["TotalBet"]) * 100;
                    else
                        dr["ProfitRate"] = 0;

                    int RPBonus = Entity.Gambler.GetGamblerRPByUserID(Convert.ToInt32(dr["UserID"]), CurrentLeague);

                    if (RPBonus > 0)
                        dr["RPBonus"] = RPBonus;
                }

                DataView dv = new DataView(dt);
                dv.Sort = orderClause;
                dt = dv.ToTable();

                int rank = 1;
                foreach (DataRow dr in dt.Rows)
                {
                    dr["Rank"] = rank;
                    rank++;
                }
            }

            gvGamlber.DataSource = dt;
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
    }
}

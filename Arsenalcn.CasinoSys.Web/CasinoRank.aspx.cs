using System;
using System.Data;
using System.Web.UI.WebControls;

namespace Arsenalcn.CasinoSys.Web
{
    public partial class CasinoRank : Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region Assign Control Property

            ctrlLeftPanel.UserID = userid;
            ctrlLeftPanel.UserName = username;

            ctrlFieldTooBar.UserID = userid;

            ctrlMenuTabBar.CurrentMenu = Arsenalcn.CasinoSys.Web.Control.CasinoMenuType.CasinoRank;

            #endregion

            var dt = Entity.Rank.GetTopGamblerMonthly();

            if (dt != null)
            {
                dt.Columns.Add("RankDate", typeof(string));
                dt.Columns.Add("WinnerProfitRate", typeof(float));
                dt.Columns.Add("LoserProfitRate", typeof(float));

                var rMonth = string.Empty;

                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["RankMonth"].ToString().Length == 1)
                        rMonth = string.Format("0{0}", dr["RankMonth"].ToString());
                    else
                        rMonth = dr["RankMonth"].ToString();

                    dr["RankDate"] = string.Format("{0}/{1}", dr["RankYear"].ToString(), rMonth);
                    dr["WinnerProfitRate"] = Convert.ToSingle(dr["WinnerProfit"]) / Convert.ToSingle(dr["WinnerTotalBet"]) * 100;
                    dr["LoserProfitRate"] = Convert.ToSingle(dr["LoserProfit"]) / Convert.ToSingle(dr["LoserTotalBet"]) * 100;
                }
            }

            gvRank.DataSource = dt;
            gvRank.DataBind();
        }

        protected void gvRank_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvRank.PageIndex = e.NewPageIndex;

            gvRank.DataBind();
        }
    }
}

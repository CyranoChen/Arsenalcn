using System;
using System.Data;

namespace Arsenalcn.CasinoSys.Web.Control
{
    public partial class TopGamblerList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region Top Gambler Profit
            var rank = 1;
            var months = 0;
            var dtTopProfit = Entity.Rank.GetTopGamblerProfit(out months);

            if (dtTopProfit != null)
            {
                dtTopProfit.Columns.Add("Rank", typeof(int));
                foreach (DataRow dr in dtTopProfit.Rows)
                {
                    dr["Rank"] = rank;
                    rank++;
                }
            }

            rptGamblerProfit.DataSource = dtTopProfit;
            rptGamblerProfit.DataBind();

            this.ltrlProfit.Text = string.Format("收益({0}月)", DateTime.Today.AddMonths(months).Month.ToString());
            #endregion

            #region Top Gambler RP
            rank = 1;
            months = 0;
            var dtTopRP = Entity.Rank.GetTopGamblerRP(out months);

            if (dtTopRP != null)
            {
                dtTopRP.Columns.Add("Rank", typeof(int));
                foreach (DataRow dr in dtTopRP.Rows)
                {
                    dr["Rank"] = rank;
                    rank++;
                }
            }

            rptGamblerRP.DataSource = dtTopRP;
            rptGamblerRP.DataBind();

            this.ltrlRP.Text = string.Format("RP({0}月)", DateTime.Today.AddMonths(months).Month.ToString());
            #endregion

            #region Top Gambler Bet
            rank = 1;
            months = 0;
            var dtTopBet = Entity.Rank.GetTopGamblerTotalBet(out months);

            if (dtTopBet != null)
            {
                dtTopBet.Columns.Add("Rank", typeof(int));
                foreach (DataRow dr in dtTopBet.Rows)
                {
                    dr["Rank"] = rank;
                    rank++;
                }
            }

            rptGamblerBet.DataSource = dtTopBet;
            rptGamblerBet.DataBind();

            this.ltrlBet.Text = string.Format("投注({0}月)", DateTime.Today.AddMonths(months).Month.ToString());
            #endregion
        }
    }
}
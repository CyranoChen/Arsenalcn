using System;
using System.Data;
using System.Web.UI;
using Arsenalcn.CasinoSys.Entity;

namespace Arsenalcn.CasinoSys.Web.Control
{
    public partial class TopGamblerList : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region Top Gambler Profit

            var rank = 1;
            int months;
            var dtTopProfit = Rank.GetTopGamblerProfit(out months);

            if (dtTopProfit != null)
            {
                dtTopProfit.Columns.Add("Rank", typeof (int));
                foreach (DataRow dr in dtTopProfit.Rows)
                {
                    dr["Rank"] = rank;
                    rank++;
                }
            }

            rptGamblerProfit.DataSource = dtTopProfit;
            rptGamblerProfit.DataBind();

            ltrlProfit.Text = $"收益({DateTime.Today.AddMonths(months).Month}月)";

            #endregion

            #region Top Gambler RP

            rank = 1;
            var dtTopRp = Rank.GetTopGamblerRP(out months);

            if (dtTopRp != null)
            {
                dtTopRp.Columns.Add("Rank", typeof (int));
                foreach (DataRow dr in dtTopRp.Rows)
                {
                    dr["Rank"] = rank;
                    rank++;
                }
            }

            rptGamblerRP.DataSource = dtTopRp;
            rptGamblerRP.DataBind();

            ltrlRP.Text = $"RP({DateTime.Today.AddMonths(months).Month}月)";

            #endregion

            #region Top Gambler Bet

            rank = 1;
            var dtTopBet = Rank.GetTopGamblerTotalBet(out months);

            if (dtTopBet != null)
            {
                dtTopBet.Columns.Add("Rank", typeof (int));
                foreach (DataRow dr in dtTopBet.Rows)
                {
                    dr["Rank"] = rank;
                    rank++;
                }
            }

            rptGamblerBet.DataSource = dtTopBet;
            rptGamblerBet.DataBind();

            ltrlBet.Text = $"投注({DateTime.Today.AddMonths(months).Month}月)";

            #endregion
        }
    }
}
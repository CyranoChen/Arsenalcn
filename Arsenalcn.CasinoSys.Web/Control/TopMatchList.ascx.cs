using System;
using System.Data;

namespace Arsenalcn.CasinoSys.Web.Control
{
    public partial class TopMatchList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region Top Match Earning
            int rank = 1;
            int months = 0;
            DataTable dtTopEarning = Entity.CasinoItem.GetTopMatchEarning(out months);

            if (dtTopEarning != null)
            {
                dtTopEarning.Columns.Add("Rank", typeof(int));
                foreach (DataRow dr in dtTopEarning.Rows)
                {
                    dr["Rank"] = rank;
                    rank++;
                }
            }

            rptMatchEarning.DataSource = dtTopEarning;
            rptMatchEarning.DataBind();

            this.ltrlEarning.Text = string.Format("盈余排行({0}月)", DateTime.Today.AddMonths(months).Month.ToString());
            #endregion

            #region Top Match Loss
            rank = 1;
            months = 0;
            DataTable dtTopLoss = Entity.CasinoItem.GetTopMatchLoss(out months);

            if (dtTopLoss != null)
            {
                dtTopLoss.Columns.Add("Rank", typeof(int));
                foreach (DataRow dr in dtTopLoss.Rows)
                {
                    dr["Rank"] = rank;
                    rank++;
                }
            }

            rptMatchLoss.DataSource = dtTopLoss;
            rptMatchLoss.DataBind();

            this.ltrlLoss.Text = string.Format("亏损排行({0}月)", DateTime.Today.AddMonths(months).Month.ToString());
            #endregion
        }
    }
}
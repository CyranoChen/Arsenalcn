using System;
using System.Data;
using System.Web.UI.WebControls;

using Arsenalcn.CasinoSys.Entity;

namespace Arsenalcn.CasinoSys.Web.Control
{
    public partial class TopMatchList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region Top Match Earning
            var rank = 1;
            var months = 0;
            var dtTopEarning = Entity.CasinoItem.GetTopMatchEarning(out months);

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
            var dtTopLoss = Entity.CasinoItem.GetTopMatchLoss(out months);

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

        protected void rptMatchEarning_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                var drv = e.Item.DataItem as DataRowView;

                var m = new Match((Guid)drv["MatchGuid"]);

                var ltrlMatchInfo = e.Item.FindControl("ltrlMatchInfo") as Literal;

                if (ltrlMatchInfo != null && m != null)
                {
                    var tHome = Team.Cache.Load(m.Home);
                    var tAway = Team.Cache.Load(m.Away);

                    var _strMatchInfo = "<li class=\"IconTop{0}\"><a href=\"CasinoBetLog.aspx?Match={1}\" title=\"{2} {3}\">{4} <em>vs</em> {5}</a><em title=\"比赛盈余\"  class=\"CasinoSys_TopRankEM\">{6}</em></li>";

                    ltrlMatchInfo.Text = string.Format(_strMatchInfo, drv["Rank"].ToString(), m.MatchGuid.ToString(), m.LeagueName,
                        m.PlayTime.ToString("yyyy-MM-dd HH:mm"), tHome.TeamDisplayName, tAway.TeamDisplayName,
                        Convert.ToSingle(drv["Earning"]).ToString("N2"));
                }
            }
        }

        protected void rptMatchLoss_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                var drv = e.Item.DataItem as DataRowView;

                var m = new Match((Guid)drv["MatchGuid"]);

                var ltrlMatchInfo = e.Item.FindControl("ltrlMatchInfo") as Literal;

                if (ltrlMatchInfo != null && m != null)
                {
                    var tHome = Team.Cache.Load(m.Home);
                    var tAway = Team.Cache.Load(m.Away);

                    var _strMatchInfo = "<li class=\"IconTop{0}\"><a href=\"CasinoBetLog.aspx?Match={1}\" title=\"{2} {3}\">{4} <em>vs</em> {5}</a><em title=\"比赛亏损\"  class=\"CasinoSys_TopRankEM\">{6}</em></li>";

                    ltrlMatchInfo.Text = string.Format(_strMatchInfo, drv["Rank"].ToString(), m.MatchGuid.ToString(), m.LeagueName,
                        m.PlayTime.ToString("yyyy-MM-dd HH:mm"), tHome.TeamDisplayName, tAway.TeamDisplayName,
                        Convert.ToSingle(drv["Earning"]).ToString("N2"));
                }
            }
        }
    }
}
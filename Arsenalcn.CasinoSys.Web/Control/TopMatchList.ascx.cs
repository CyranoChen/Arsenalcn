using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Arsenalcn.CasinoSys.Entity;

namespace Arsenalcn.CasinoSys.Web.Control
{
    public partial class TopMatchList : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region Top Match Earning

            var rank = 1;
            var months = 0;
            var dtTopEarning = CasinoItem.GetTopMatchEarning(out months);

            if (dtTopEarning != null)
            {
                dtTopEarning.Columns.Add("Rank", typeof (int));
                foreach (DataRow dr in dtTopEarning.Rows)
                {
                    dr["Rank"] = rank;
                    rank++;
                }
            }

            rptMatchEarning.DataSource = dtTopEarning;
            rptMatchEarning.DataBind();

            ltrlEarning.Text = $"盈余排行({DateTime.Today.AddMonths(months).Month}月)";

            #endregion

            #region Top Match Loss

            rank = 1;
            months = 0;
            var dtTopLoss = CasinoItem.GetTopMatchLoss(out months);

            if (dtTopLoss != null)
            {
                dtTopLoss.Columns.Add("Rank", typeof (int));
                foreach (DataRow dr in dtTopLoss.Rows)
                {
                    dr["Rank"] = rank;
                    rank++;
                }
            }

            rptMatchLoss.DataSource = dtTopLoss;
            rptMatchLoss.DataBind();

            ltrlLoss.Text = $"亏损排行({DateTime.Today.AddMonths(months).Month}月)";

            #endregion
        }

        protected void rptMatchEarning_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                var drv = e.Item.DataItem as DataRowView;

                var m = new Match((Guid) drv["MatchGuid"]);

                var ltrlMatchInfo = e.Item.FindControl("ltrlMatchInfo") as Literal;

                if (ltrlMatchInfo != null && m != null)
                {
                    var tHome = Team.Cache.Load(m.Home);
                    var tAway = Team.Cache.Load(m.Away);

                    var strMatchInfo =
                        "<li class=\"IconTop{0}\"><a href=\"CasinoBetLog.aspx?Match={1}\" title=\"{2} {3}\">{4} <em>vs</em> {5}</a><em title=\"比赛盈余\"  class=\"CasinoSys_TopRankEM\">{6}</em></li>";

                    ltrlMatchInfo.Text = string.Format(strMatchInfo, drv["Rank"], m.MatchGuid, m.LeagueName,
                        m.PlayTime.ToString("yyyy-MM-dd HH:mm"), tHome.TeamDisplayName, tAway.TeamDisplayName,
                        Convert.ToSingle(drv["Earning"]).ToString("N2"));
                }
            }
        }

        protected void rptMatchLoss_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                var drv = e.Item.DataItem as DataRowView;

                var m = new Match((Guid) drv["MatchGuid"]);

                var ltrlMatchInfo = e.Item.FindControl("ltrlMatchInfo") as Literal;

                if (ltrlMatchInfo != null && m != null)
                {
                    var tHome = Team.Cache.Load(m.Home);
                    var tAway = Team.Cache.Load(m.Away);

                    var strMatchInfo =
                        "<li class=\"IconTop{0}\"><a href=\"CasinoBetLog.aspx?Match={1}\" title=\"{2} {3}\">{4} <em>vs</em> {5}</a><em title=\"比赛亏损\"  class=\"CasinoSys_TopRankEM\">{6}</em></li>";

                    ltrlMatchInfo.Text = string.Format(strMatchInfo, drv["Rank"], m.MatchGuid, m.LeagueName,
                        m.PlayTime.ToString("yyyy-MM-dd HH:mm"), tHome.TeamDisplayName, tAway.TeamDisplayName,
                        Convert.ToSingle(drv["Earning"]).ToString("N2"));
                }
            }
        }
    }
}
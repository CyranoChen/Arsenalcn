using System;
using System.Data;
using System.Web.UI.WebControls;
using Arsenalcn.CasinoSys.Entity;
using Arsenalcn.CasinoSys.Web.Common;
using Arsenalcn.CasinoSys.Web.Control;

namespace Arsenalcn.CasinoSys.Web
{
    public partial class CasinoRank : BasePage
    {
        protected override void OnInit(EventArgs e)
        {
            AnonymousRedirect = true;

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            #region Assign Control Property

            ctrlLeftPanel.UserId = userid;
            ctrlLeftPanel.UserName = username;

            ctrlFieldTooBar.UserId = userid;

            ctrlMenuTabBar.CurrentMenu = CasinoMenuType.CasinoRank;

            #endregion

            var dt = Rank.GetTopGamblerMonthly();

            if (dt != null)
            {
                dt.Columns.Add("RankDate", typeof(string));
                dt.Columns.Add("WinnerProfitRate", typeof(float));
                dt.Columns.Add("LoserProfitRate", typeof(float));

                foreach (DataRow dr in dt.Rows)
                {
                    var month = dr["RankMonth"].ToString().Length == 1 ? $"0{dr["RankMonth"]}" : dr["RankMonth"].ToString();

                    dr["RankDate"] = $"{dr["RankYear"]}/{month}";
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
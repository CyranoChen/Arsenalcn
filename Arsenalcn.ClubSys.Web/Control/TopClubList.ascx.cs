using System;
using System.Web.UI;
using Arsenalcn.ClubSys.Service;

namespace Arsenalcn.ClubSys.Web.Control
{
    public partial class TopClubList : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region Top Rank Club

            var topRankClubs = ClubLogic.GetTopRankClubs();

            var rank = 1;
            foreach (var topClub in topRankClubs)
            {
                topClub.AdditionalData2 = rank.ToString();
                rank++;
            }

            rptTopClubs.DataSource = topRankClubs;
            rptTopClubs.DataBind();

            #endregion

            #region Top Lv Rank Club

            var topLvClubs = ClubLogic.GetTopLvClubs();

            rank = 1;
            foreach (var topClub in topLvClubs)
            {
                topClub.AdditionalData2 = rank.ToString();
                rank++;
            }

            rptLv.DataSource = topLvClubs;
            rptLv.DataBind();

            #endregion

            #region Top Fortune Rank Club

            var topFortuneClubs = ClubLogic.GetTopFortuneClubs();

            rank = 1;
            foreach (var topClub in topFortuneClubs)
            {
                topClub.AdditionalData2 = rank.ToString();
                rank++;
            }

            rptFortune.DataSource = topFortuneClubs;
            rptFortune.DataBind();

            #endregion
        }
    }
}
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;

using Arsenalcn.ClubSys.DataAccess;
using Arsenalcn.ClubSys.Entity;

namespace Arsenalcn.ClubSys.Web.Control
{
    public partial class TopClubList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region Top Rank Club
            List<Club> topRankClubs = ClubLogic.GetTopRankClubs();

            int rank = 1;
            foreach (Club topClub in topRankClubs)
            {
                topClub.AdditionalData2 = rank.ToString();
                rank++;
            }

            rptTopClubs.DataSource = topRankClubs;
            rptTopClubs.DataBind();
            #endregion

            #region Top Lv Rank Club
            List<Club> topLvClubs = ClubLogic.GetTopLvClubs();

            rank = 1;
            foreach (Club topClub in topLvClubs)
            {
                topClub.AdditionalData2 = rank.ToString();
                rank++;
            }

            rptLv.DataSource = topLvClubs;
            rptLv.DataBind();
            #endregion

            #region Top Fortune Rank Club
            List<Club> topFortuneClubs = ClubLogic.GetTopFortuneClubs();

            rank = 1;
            foreach (Club topClub in topFortuneClubs)
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
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

using Arsenalcn.ClubSys.Service;
using Arsenalcn.ClubSys.Entity;

namespace Arsenalcn.ClubSys.Web.Control
{
    public partial class TopPlayerList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region Top Rp Player
            int rank = 1;
            DataTable dtTopRp = PlayerStrip.GetTopRpPlayers();

            if (dtTopRp != null)
            {
                dtTopRp.Columns.Add("Rank", typeof(int));
                foreach (DataRow dr in dtTopRp.Rows)
                {
                    dr["Rank"] = rank;
                    rank++;
                }
            }

            rptRP.DataSource = dtTopRp;
            rptRP.DataBind();
            #endregion

            #region Top Card Player
            rank = 1;
            DataTable dtTopCard = PlayerStrip.GetTopCardPlayers();

            if (dtTopCard != null)
            {
                dtTopCard.Columns.Add("Rank", typeof(int));
                foreach (DataRow dr in dtTopCard.Rows)
                {
                    dr["Rank"] = rank;
                    rank++;
                }
            }

            rptCard.DataSource = dtTopCard;
            rptCard.DataBind();
            #endregion

            #region Top Video Player
            rank = 1;
            DataTable dtTopVideo = PlayerStrip.GetTopVideoPlayers();

            if (dtTopVideo != null)
            {
                dtTopVideo.Columns.Add("Rank", typeof(int));
                foreach (DataRow dr in dtTopVideo.Rows)
                {
                    dr["Rank"] = rank;
                    rank++;
                }
            }

            rptVideo.DataSource = dtTopVideo;
            rptVideo.DataBind();
            #endregion
        }
    }
}
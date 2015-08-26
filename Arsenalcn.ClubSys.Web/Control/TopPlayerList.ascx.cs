using System;
using System.Data;
using Arsenalcn.ClubSys.Service;

namespace Arsenalcn.ClubSys.Web.Control
{
    public partial class TopPlayerList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region Top Rp Player
            var rank = 1;
            var dtTopRp = PlayerStrip.GetTopRpPlayers();

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
            var dtTopCard = PlayerStrip.GetTopCardPlayers();

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
            var dtTopVideo = PlayerStrip.GetTopVideoPlayers();

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
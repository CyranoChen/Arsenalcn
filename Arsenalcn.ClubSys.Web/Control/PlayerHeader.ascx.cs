using System;
using System.Web.UI;
using Arsenalcn.ClubSys.Entity;
using Arsenalcn.ClubSys.Service;

namespace Arsenalcn.ClubSys.Web.Control
{
    public partial class PlayerHeader : UserControl
    {
        public int shirtLv;
        public int shortsLv;
        public int sockLv;

        /// <summary>
        ///     Current Profile User ID
        /// </summary>
        public int ProfileUserID { get; set; } = -1;

        public int UserID { get; set; } = -1;

        public int PlayerLv
        {
            get
            {
                var lv = 0;

                if (shirtLv < shortsLv)
                    lv = shirtLv;
                else
                    lv = shortsLv;

                if (lv < sockLv)
                    return lv;
                return sockLv;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var clubs = ClubLogic.GetActiveUserClubs(ProfileUserID);

            if (clubs.Count == 0)
            {
                btnClub.Visible = false;
                btnGetStrip.Visible = false;
                btnCardFusion.Visible = false;
            }
            else
            {
                var club = clubs[0];

                btnClub.Text = club.FullName;
                btnClub.PostBackUrl = $"../ClubView.aspx?ClubID={club.ID.Value}";

                if (ProfileUserID != UserID)
                {
                    btnGetStrip.Visible = false;
                    btnCardFusion.Visible = false;
                }
            }

            var _playerInfo = PlayerStrip.GetPlayerInfo(ProfileUserID);

            if (_playerInfo == null)
                pnlSwf.Visible = false;
            else
            {
                pnlSwf.Visible = true;

                shirtLv = _playerInfo.Shirt;
                shortsLv = _playerInfo.Shorts;
                sockLv = _playerInfo.Sock;
            }

            var TotalCount = PlayerStrip.GetUserBingoPlayCount(ProfileUserID);
            if (TotalCount <= 0)
            {
                ltrlRP.Text = "0%";
            }
            else
            {
                ltrlRP.Text = (PlayerStrip.GetUserBingoGainCount(ProfileUserID)*100/TotalCount) + "%";
            }

            var _playerLV = Math.Min(PlayerLv, ConfigGlobal.PlayerMaxLv);

            ltrlPlayerLV.Text =
                $"<div class=\"ClubSys_PlayerLV\" style=\"width: {(_playerLV*20)}px;\" title=\"球员等级\"></div>";
        }
    }
}
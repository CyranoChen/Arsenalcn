using System;
using System.Collections.Generic;

using Arsenalcn.ClubSys.Service;
using Arsenalcn.ClubSys.Entity;

namespace Arsenalcn.ClubSys.Web.Control
{
    public partial class PlayerHeader : System.Web.UI.UserControl
    {
        private int _profileUserID = -1;
        /// <summary>
        /// Current Profile User ID
        /// </summary>
        public int ProfileUserID
        {
            get
            {
                return _profileUserID;
            }
            set
            {
                _profileUserID = value;
            }
        }

        private int _userID = -1;

        public int UserID
        {
            get
            {
                return _userID;
            }
            set
            {
                _userID = value;
            }
        }

        public int shirtLv = 0;
        public int shortsLv = 0;
        public int sockLv = 0;

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
                else
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

                if (ProfileUserID != _userID)
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
                ltrlRP.Text = (PlayerStrip.GetUserBingoGainCount(ProfileUserID) * 100 / TotalCount).ToString() + "%";
            }

            var _playerLV = Math.Min(PlayerLv, ConfigGlobal.PlayerMaxLv);

            ltrlPlayerLV.Text =
                $"<div class=\"ClubSys_PlayerLV\" style=\"width: {(_playerLV*20).ToString()}px;\" title=\"球员等级\"></div>";
        }
    }
}
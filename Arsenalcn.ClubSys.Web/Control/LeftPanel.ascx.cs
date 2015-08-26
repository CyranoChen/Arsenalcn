using System;
using System.Collections.Generic;

using Arsenalcn.ClubSys.Service;
using Arsenalcn.ClubSys.Entity;

namespace Arsenalcn.ClubSys.Web.Control
{
    public partial class LeftPanel : System.Web.UI.UserControl
    {
        private string _userName = string.Empty;
        /// <summary>
        /// Current User Name
        /// </summary>
        public string UserName
        {
            set
            {
                _userName = value;
            }
        }

        private int _userId = -1;
        /// <summary>
        /// Current User ID
        /// </summary>
        public int UserID
        {
            get
            {
                return _userId;
            }
            set
            {
                _userId = value;
            }
        }

        private string _userKey = string.Empty;
        /// <summary>
        /// Current User Key
        /// </summary>
        public string UserKey
        {
            set
            {
                _userKey = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (_userId == -1)
            {
                //unauthenticated

                pnlMyClub.Visible = false;
            }
            else
            {
                //authenticated

                pnlMyClub.Visible = true;


                #region MyClubPanel
                var myClubs = ClubLogic.GetActiveUserClubs(this._userId);

                if (myClubs.Count == 0)
                {
                    //do not have a joined club
                    phMyClub.Visible = false;
                    hlNoClub.Visible = true;

                    phCreateClub.Visible = false;
                    phClubAdmin.Visible = false;
                }
                else
                {
                    phMyClub.Visible = true;
                    hlNoClub.Visible = false;

                    var myClub = myClubs[0];
                    hlMyClub.Text = myClub.FullName;
                    hlMyClub.NavigateUrl = $"../ClubView.aspx?ClubID={myClub.ID.Value.ToString()}";
                    //ltrlMyClubName.Text = myClub.FullName;
                    //ltrlMyClubID.Text = myClub.ID.Value.ToString();
                    ltrlMyClubRankScore.Text =
                        $" <a href=\"ClubRank.aspx?ClubID={myClub.ID.Value.ToString()}\"><em title=\"评价分 RankPoint\">RPos: {myClub.RankScore.Value.ToString()}</em></a>";

                    phCreateClub.Visible = false;

                    var managedClubs = ClubLogic.GetUserManagedClubs(this._userId);
                    if (managedClubs.Count == 0)
                    {
                        phClubAdmin.Visible = false;
                    }
                    else
                    {
                        phClubAdmin.Visible = true;
                        hlMyAdminClub.NavigateUrl =
                            $"../ManageApplication.aspx?ClubID={managedClubs[0].ID.Value.ToString()}";
                    }
                }
                #endregion
            }


            #region HideClubSysNotice
            if (string.IsNullOrEmpty(ConfigGlobal.SysNotice))
            {
                pnlClubNotice.Visible = false;
            }
            #endregion

        }
    }
}
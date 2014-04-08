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
                List<Club> myClubs = ClubLogic.GetActiveUserClubs(this._userId);

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

                    Club myClub = myClubs[0];
                    hlMyClub.Text = myClub.FullName;
                    hlMyClub.NavigateUrl = string.Format("../ClubView.aspx?ClubID={0}", myClub.ID.Value.ToString());
                    //ltrlMyClubName.Text = myClub.FullName;
                    //ltrlMyClubID.Text = myClub.ID.Value.ToString();
                    ltrlMyClubRankScore.Text = string.Format(" <a href=\"ClubRank.aspx?ClubID={0}\"><em title=\"评价分 RankPoint\">RPos: {1}</em></a>", myClub.ID.Value.ToString(), myClub.RankScore.Value.ToString());

                    phCreateClub.Visible = false;

                    List<Club> managedClubs = ClubLogic.GetUserManagedClubs(this._userId);
                    if (managedClubs.Count == 0)
                    {
                        phClubAdmin.Visible = false;
                    }
                    else
                    {
                        phClubAdmin.Visible = true;
                        hlMyAdminClub.NavigateUrl = string.Format("../ManageApplication.aspx?ClubID={0}", managedClubs[0].ID.Value.ToString());
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
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

using Arsenalcn.ClubSys.DataAccess;
using Arsenalcn.ClubSys.Entity;

namespace Arsenalcn.ClubSys.Web.Control
{
    public partial class ManageMenuTabBar : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Club club = ClubLogic.GetClubInfo(ClubID);

            if (club != null)
            {
                if (club.ManagerUid.Value != UserID)
                    liManageClub.Visible = false;
                else
                    liManageClub.Visible = true;
            }
            else
                Response.Redirect("ClubPortal.aspx");
        }

        public ManageClubMenuItem CurrentMenu
        {
            set
            {
                switch (value)
                {
                    case ManageClubMenuItem.ManageClub:
                        liManageClub.Attributes.Add("class", "Current");
                        break;
                    case ManageClubMenuItem.ManageMember:
                        liManageMemeber.Attributes.Add("class", "Current");
                        break;
                    case ManageClubMenuItem.ManageApplication:
                        liManageApplication.Attributes.Add("class", "Current");
                        break;
                    default:
                        liManageClub.Attributes.Add("class", "Current");
                        break;
                }
            }
        }

        public int ClubID
        {
            get
            {
                int tmp;
                if (int.TryParse(Request.QueryString["ClubID"], out tmp))
                    return tmp;
                else
                {
                    Response.Redirect("ClubPortal.aspx");

                    return -1;
                }
            }
        }

        private int userID = -1;
        public int UserID
        {
            get
            {
                return userID;
            }
            set
            {
                userID = value;
            }
        }
    }

    public enum ManageClubMenuItem
    {
        ManageClub,
        ManageMember,
        ManageApplication
    }
}
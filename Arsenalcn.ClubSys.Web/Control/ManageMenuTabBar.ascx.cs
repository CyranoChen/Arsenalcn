using System;
using System.Web.UI;
using Arsenalcn.ClubSys.Service;

namespace Arsenalcn.ClubSys.Web.Control
{
    public partial class ManageMenuTabBar : UserControl
    {
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
                Response.Redirect("ClubPortal.aspx");

                return -1;
            }
        }

        public int UserID { get; set; } = -1;

        protected void Page_Load(object sender, EventArgs e)
        {
            var club = ClubLogic.GetClubInfo(ClubID);

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
    }

    public enum ManageClubMenuItem
    {
        ManageClub,
        ManageMember,
        ManageApplication
    }
}
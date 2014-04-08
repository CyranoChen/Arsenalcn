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

namespace Arsenalcn.ClubSys.Web.Control
{
    public partial class MenuTabBar : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public ClubMenuItem CurrentMenu
        {
            set
            {
                switch (value)
                {
                    case ClubMenuItem.ClubInfo:
                        liClubInfo.Attributes.Add("class", "Current");
                        break;
                    case ClubMenuItem.ClubMemeber:
                        liClubMember.Attributes.Add("class", "Current");
                        break;
                    case ClubMenuItem.ClubRank:
                        liClubRank.Attributes.Add("class", "Current");
                        break;
                    case ClubMenuItem.ClubHistory:
                        liClubHistory.Attributes.Add("class", "Current");
                        break;
                    case ClubMenuItem.ClubPlayer:
                        liClubPlayer.Attributes.Add("class", "Current");
                        break;
                    case ClubMenuItem.ClubStrip:
                        liClubStrip.Attributes.Add("class", "Current");
                        break;
                    case ClubMenuItem.ClubVideo:
                        liClubVideo.Attributes.Add("class", "Current");
                        break;
                    default:
                        liClubInfo.Attributes.Add("class", "Current");
                        break;
                }
            }
        }

        private int _clubID = -1;
        public int ClubID
        {
            get
            {
                //int tmp;
                //if (int.TryParse(Request.QueryString["ClubID"], out tmp))
                //    return tmp;
                //else
                //{
                //    Response.Redirect("ClubPortal.aspx");

                //    return -1;
                //}

                return _clubID;
            }
            set
            {
                _clubID = value;
            }
        }
    }

    public enum ClubMenuItem
    {
        ClubInfo,
        ClubMemeber,
        ClubRank,
        ClubHistory,
        ClubPlayer,
        ClubStrip,
        ClubVideo
    }
}
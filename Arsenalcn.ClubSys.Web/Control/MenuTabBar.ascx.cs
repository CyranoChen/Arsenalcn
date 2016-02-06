using System;
using System.Web.UI;

namespace Arsenalcn.ClubSys.Web.Control
{
    public partial class MenuTabBar : UserControl
    {
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

        public int ClubID { get; set; } = -1;

        protected void Page_Load(object sender, EventArgs e)
        {
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
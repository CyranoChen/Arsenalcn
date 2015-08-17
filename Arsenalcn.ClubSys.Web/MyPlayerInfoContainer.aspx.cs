using System;
using System.Collections.Generic;

using Arsenalcn.ClubSys.Service;
using Arsenalcn.ClubSys.Entity;

namespace Arsenalcn.ClubSys.Web
{
    public partial class MyPlayerInfoContainer : Discuz.Forum.PageBase
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (ConfigGlobal.PluginActive && ConfigGlobal.PluginContainerActive && ProfileUserID != -1)
            {
                //Generator the Style and Javascript
                Response.Write("document.write('<link href=\"../../App_Themes/Arsenalcn/clubsys.css\" type=\"text/css\" rel=\"stylesheet\" />');");
                Response.Write("document.write('<script type=\"text/javascript\" src=\"plugin/AcnClub/scripts/ClubSys.js\"></script>');");

                List<Club> clubs = ClubLogic.GetActiveUserClubs(ProfileUserID);
                if (clubs.Count != 0)
                {
                    Club club = clubs[0];

                    string cClubCSS = string.Empty;
                    string cClubTitle = string.Empty;
                    if (ConfigGlobal.ChampionsClubID > 0 && club.ID == ConfigGlobal.ChampionsClubID)
                    {
                        cClubCSS = "ClubSys_ShowTopicCrown";
                        cClubTitle = ConfigGlobal.ChampionsTitle;
                    }
                    else
                    {
                        cClubCSS = "ClubSys_ShowTopic";
                    }

                    string strClub = string.Format("<div class=\"{4}\" title=\"{5}\"><a href=\"plugin/AcnClub/ClubView.aspx?clubID={0}\" target=\"_blank\">{1}</a><em>LV:{2} | RPos:{3}</em></div>", club.ID.Value.ToString(), club.FullName, club.RankLevel.Value.ToString(), club.RankScore.Value.ToString(), cClubCSS, cClubTitle);
                    Response.Write(string.Format("document.write('{0}');", strClub.ToString()));
                }

                Gamer player = PlayerStrip.GetPlayerInfo(ProfileUserID);

                if (player != null && player.CurrentGuid != null)
                {
                    string strPlayer = string.Format("GenSwfObject('UserStrip', 'plugin/acnclub/swf/UserStrip.swf?XMLURL=plugin/acnclub/ServerXml.aspx%3FUserID={0}', '180', '120');", ProfileUserID.ToString());
                    //Response.Write(string.Format("document.write('<div style=\"text-align: center;\">');{0};document.write('</div>');", strPlayer.ToString()));
                    Response.Write(strPlayer);
                }

                //if (player != null && UserVideo.GetUserPublicVideo(ProfileUserID).Count > 0)
                //{
                //    string strVideo = string.Format("GenSwfObject('UserVideo', 'plugin/acnclub/swf/UserVideo.swf?XMLURL=plugin/acnclub/ServerXml.aspx%3FUserID={0}', '180', '200');", ProfileUserID.ToString());
                //    //Response.Write(string.Format("document.write('<div style=\"text-align: center;\">');{0};document.write('</div>');", strVideo.ToString()));
                //    Response.Write(strVideo);
                //}
            }

            Response.End();
        }

        public int ProfileUserID
        {
            get
            {
                if (Request.QueryString["ProfileUserID"] != null)
                {
                    try
                    {
                        return Convert.ToInt32(Request.QueryString["ProfileUserID"]);
                    }
                    catch
                    {
                        return -1;
                    }
                }
                else
                    return -1;
            }
        }
    }
}

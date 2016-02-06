using System;
using Arsenalcn.ClubSys.Entity;
using Arsenalcn.ClubSys.Service;
using Discuz.Forum;

namespace Arsenalcn.ClubSys.Web
{
    public partial class MyPlayerInfoContainer : PageBase
    {
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
                return -1;
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (ConfigGlobal.PluginActive && ConfigGlobal.PluginContainerActive && ProfileUserID != -1)
            {
                //Generator the Style and Javascript
                Response.Write(
                    "document.write('<link href=\"../../App_Themes/Arsenalcn/clubsys.css\" type=\"text/css\" rel=\"stylesheet\" />');");
                Response.Write(
                    "document.write('<script type=\"text/javascript\" src=\"plugin/AcnClub/scripts/ClubSys.js\"></script>');");

                var clubs = ClubLogic.GetActiveUserClubs(ProfileUserID);
                if (clubs.Count != 0)
                {
                    var club = clubs[0];

                    var cClubCSS = string.Empty;
                    var cClubTitle = string.Empty;
                    if (ConfigGlobal.ChampionsClubID > 0 && club.ID == ConfigGlobal.ChampionsClubID)
                    {
                        cClubCSS = "ClubSys_ShowTopicCrown";
                        cClubTitle = ConfigGlobal.ChampionsTitle;
                    }
                    else
                    {
                        cClubCSS = "ClubSys_ShowTopic";
                    }

                    var strClub =
                        string.Format(
                            "<div class=\"{4}\" title=\"{5}\"><a href=\"plugin/AcnClub/ClubView.aspx?clubID={0}\" target=\"_blank\">{1}</a><em>LV:{2} | RPos:{3}</em></div>",
                            club.ID.Value, club.FullName, club.RankLevel.Value, club.RankScore.Value, cClubCSS,
                            cClubTitle);
                    Response.Write($"document.write('{strClub}');");
                }

                var player = PlayerStrip.GetPlayerInfo(ProfileUserID);

                if (player != null && player.CurrentGuid != null)
                {
                    var strPlayer =
                        $"GenSwfObject('UserStrip', 'plugin/acnclub/swf/UserStrip.swf?XMLURL=plugin/acnclub/ServerXml.aspx%3FUserID={ProfileUserID}', '180', '120');";
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
    }
}
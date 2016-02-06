using System;
using Arsenalcn.ClubSys.Entity;
using Arsenalcn.ClubSys.Service;
using Arsenalcn.ClubSys.Web.Common;
using UserVideo = Arsenalcn.ClubSys.Service.UserVideo;

namespace Arsenalcn.ClubSys.Web
{
    public partial class ServerActivateCard : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var result = "false";
            var returnURL = string.Empty;
            var responseText = string.Empty;

            using (var conn = ConfigGlobal.SQLConnectionStrings)
            {
                conn.Open();
                var trans = conn.BeginTransaction();

                try
                {
                    if (userid != -1)
                    {
                        if (Request.Form["CardID"] != null)
                        {
                            var unID = -1;
                            var authKey = Request.Form["AuthKey"];

                            if (int.TryParse(Request.Form["CardID"], out unID))
                            {
                                if (PlayerStrip.ValidateAuthKey(unID.ToString(), string.Empty, authKey))
                                {
                                    if (PlayerStrip.CheckUserNumActiveCondition(userid, unID))
                                    {
                                        var un = PlayerStrip.GetUserNumber(unID);

                                        if (un.ArsenalPlayerGuid.HasValue)
                                        {
                                            //normal player card
                                            PlayerStrip.SetCardAcitve(userid, unID);

                                            PlayerLog.LogHistory(userid, username, PlayerHistoryType.ActivateCard,
                                                new ActivateCardDesc(un).Generate());

                                            result = "true";
                                        }
                                        else
                                        {
                                            //video card
                                            var guid = UserVideo.GetRandomVideo(userid, 1, 3, true);

                                            if (!guid.HasValue)
                                                throw new Exception("No Video Available.");

                                            var uv = new Entity.UserVideo();

                                            uv.UserID = userid;
                                            uv.UserName = username;
                                            uv.VideoGuid = guid.Value;
                                            uv.ActiveDate = DateTime.Now;
                                            uv.UserDesc = string.Empty;
                                            uv.IsPublic = false;

                                            uv.Insert(trans);

                                            trans.Commit();

                                            PlayerStrip.ActiveVideoCost(userid, unID);

                                            returnURL = Player.Cache.Load(
                                                Video.Cache.Load(guid.Value).GoalPlayerGuid.Value).PhotoURL;

                                            PlayerLog.LogHistory(userid, username, PlayerHistoryType.ActivateVideo,
                                                new ActivateVideoDesc(un).Generate());

                                            if (string.IsNullOrEmpty(returnURL))
                                            {
                                                result = "full";
                                            }
                                            else
                                            {
                                                result = "video";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {
                    trans.Rollback();

                    result = "error";
                }

                //conn.Close();
            }

            if (!string.IsNullOrEmpty(returnURL))
            {
                responseText = $"ServerMessage={result}&PhotoURL={returnURL}";
            }
            else
            {
                responseText = $"ServerMessage={result}";
            }

            Response.Write(responseText);
        }
    }
}
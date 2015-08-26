using System;
using System.Data.SqlClient;

using Arsenalcn.ClubSys.Entity;
using Arsenalcn.ClubSys.Service;


namespace Arsenalcn.ClubSys.Web
{
    public partial class ServerActivateCard : Common.BasePage
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
                                    if (PlayerStrip.CheckUserNumActiveCondition(this.userid, unID))
                                    {
                                        var un = PlayerStrip.GetUserNumber(unID);

                                        if (un.ArsenalPlayerGuid.HasValue)
                                        {
                                            //normal player card
                                            PlayerStrip.SetCardAcitve(this.userid, unID);

                                            PlayerLog.LogHistory(this.userid, this.username, PlayerHistoryType.ActivateCard, new ActivateCardDesc(un).Generate());

                                            result = "true";
                                        }
                                        else
                                        {
                                            //video card
                                            var guid = Service.UserVideo.GetRandomVideo(this.userid, 1, 3, true);

                                            if (!guid.HasValue)
                                                throw new Exception("No Video Available.");

                                            var uv = new Entity.UserVideo();

                                            uv.UserID = this.userid;
                                            uv.UserName = this.username;
                                            uv.VideoGuid = guid.Value;
                                            uv.ActiveDate = DateTime.Now;
                                            uv.UserDesc = string.Empty;
                                            uv.IsPublic = false;

                                            uv.Insert(trans);

                                            trans.Commit();

                                            PlayerStrip.ActiveVideoCost(this.userid, unID);

                                            returnURL = Player.Cache.Load(
                                                Video.Cache.Load(guid.Value).GoalPlayerGuid.Value).PhotoURL;

                                            PlayerLog.LogHistory(this.userid, this.username, PlayerHistoryType.ActivateVideo, new ActivateVideoDesc(un).Generate());

                                            if (String.IsNullOrEmpty(returnURL))
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

            if (!String.IsNullOrEmpty(returnURL))
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

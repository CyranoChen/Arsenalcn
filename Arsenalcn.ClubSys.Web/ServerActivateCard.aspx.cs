using System;

using Arsenalcn.ClubSys.Service;
using Arsenalcn.ClubSys.Entity;

namespace Arsenalcn.ClubSys.Web
{
    public partial class ServerActivateCard : Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string result = "false";
            string returnURL = string.Empty;
            string responseText = string.Empty;

            try
            {
                if (userid != -1)
                {
                    if (Request.Form["CardID"] != null)
                    {
                        int unID = -1;
                        string authKey = Request.Form["AuthKey"];

                        if (int.TryParse(Request.Form["CardID"], out unID))
                        {
                            if (PlayerStrip.ValidateAuthKey(unID.ToString(), string.Empty, authKey))
                            {
                                if (PlayerStrip.CheckUserNumActiveCondition(this.userid, unID))
                                {
                                    Card un = PlayerStrip.GetUserNumber(unID);

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
                                        returnURL = UserVideo.ActiveVideo(this.userid, this.username, unID);

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
            catch { result = "error"; }

            if (!String.IsNullOrEmpty(returnURL))
            {
                responseText = string.Format("ServerMessage={0}&PhotoURL={1}", result, returnURL);
            }
            else
            {
                responseText = string.Format("ServerMessage={0}", result);
            }

            Response.Write(responseText);
        }
    }
}

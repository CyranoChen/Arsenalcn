using System;
using System.Security.Cryptography;
using System.Text;
using Arsenalcn.ClubSys.Entity;
using Arsenalcn.ClubSys.Service;
using Arsenalcn.ClubSys.Web.Common;
using UserVideo = Arsenalcn.ClubSys.Service.UserVideo;

namespace Arsenalcn.ClubSys.Web
{
    public partial class ServerCardFusion : BasePage
    {
        private string responseResult = "-1";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (userid != -1)
                {
                    if (Request.Form["FusionResult"] == "start")
                    {
                        responseResult = userid.ToString();
                    }
                    else
                    {
                        //get request info
                        var leftCardID = Request.Form["LeftCardID"];
                        var rightCardID = Request.Form["RightCardID"];
                        var fusionResult = Request.Form["FusionResult"];
                        var authKey = Request.Form["AuthKey"];

                        if (!string.IsNullOrEmpty(leftCardID) && !string.IsNullOrEmpty(rightCardID) &&
                            !string.IsNullOrEmpty(fusionResult) && !string.IsNullOrEmpty(authKey))
                        {
                            int card1, card2, result;

                            if (int.TryParse(leftCardID, out card1) && int.TryParse(rightCardID, out card2) &&
                                int.TryParse(fusionResult, out result))
                            {
                                if (ValidateAuthKey(card1.ToString(), card2.ToString(), userid.ToString(), authKey))
                                {
                                    var un1 = PlayerStrip.GetUserNumber(card1);
                                    var un2 = PlayerStrip.GetUserNumber(card2);

                                    if (un1 != null && un2 != null && un1.UserID == userid && un2.UserID == userid)
                                    {
                                        if (!un1.IsActive && !un2.IsActive &&
                                            un1.ArsenalPlayerGuid.Value == un2.ArsenalPlayerGuid.Value)
                                        {
                                            //rule 1 -- two inactive cards of same player

                                            if (fusionResult == "1")
                                            {
                                                //remove two cards and insert 1 video

                                                UserVideo.ConsolidateCards(userid, username, card1, card2);
                                                PlayerLog.LogHistory(userid, username,
                                                    PlayerHistoryType.ConsolidateCards,
                                                    new ConsolidateCardsDesc(un1, un2).Generate());

                                                responseResult = "1";
                                            }
                                        }
                                        else if (un1.IsActive != un2.IsActive)
                                        {
                                            //rule 2 -- one active card and one inactive card

                                            if (fusionResult == "2")
                                            {
                                                //remove two cards and insert 1 video

                                                UserVideo.ConsolidateCards(userid, username, card1, card2);
                                                PlayerLog.LogHistory(userid, username,
                                                    PlayerHistoryType.ConsolidateCards,
                                                    new ConsolidateCardsDesc(un1, un2).Generate());

                                                responseResult = "1";
                                            }
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
            }

            Response.Write("ServerMessage=" + responseResult);
        }

        private static bool ValidateAuthKey(string card1, string card2, string userid, string authKey)
        {
            var authPrivateKey = ConfigGlobal.AuthPrivateKey;

            var originStr = card1 + card2 + userid + authPrivateKey;

            var bytes = Encoding.UTF8.GetBytes(originStr);

            var resultBytes = MD5.Create().ComputeHash(bytes);

            var sTemp = "";
            for (var i = 0; i < resultBytes.Length; i++)
            {
                sTemp += resultBytes[i].ToString("x").PadLeft(2, '0');
            }

            return sTemp.Substring(8, 16) == authKey;
        }
    }
}
using System;
using System.Text;

using Arsenalcn.ClubSys.Entity;
using Arsenalcn.ClubSys.DataAccess;

namespace Arsenalcn.ClubSys.Web
{
    public partial class ServerCardFusion : Common.BasePage
    {
        private string responseResult = "-1";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (this.userid != -1)
                {
                    if (Request.Form["FusionResult"] == "start")
                    {
                        responseResult = this.userid.ToString();
                    }
                    else
                    {
                        //get request info
                        string leftCardID = Request.Form["LeftCardID"];
                        string rightCardID = Request.Form["RightCardID"];
                        string fusionResult = Request.Form["FusionResult"];
                        string authKey = Request.Form["AuthKey"];

                        if (!string.IsNullOrEmpty(leftCardID) && !string.IsNullOrEmpty(rightCardID) && !string.IsNullOrEmpty(fusionResult) && !string.IsNullOrEmpty(authKey))
                        {
                            int card1, card2, result;

                            if (int.TryParse(leftCardID, out card1) && int.TryParse(rightCardID, out card2) && int.TryParse(fusionResult, out result))
                            {
                                if (ValidateAuthKey(card1.ToString(), card2.ToString(), this.userid.ToString(), authKey))
                                {
                                    UserNumber un1 = PlayerStrip.GetUserNumber(card1);
                                    UserNumber un2 = PlayerStrip.GetUserNumber(card2);

                                    if (un1 != null && un2 != null && un1.UserID == this.userid && un2.UserID == this.userid)
                                    {
                                        if (!un1.IsActive && !un2.IsActive && un1.ArsenalPlayerGuid.Value == un2.ArsenalPlayerGuid.Value)
                                        {
                                            //rule 1 -- two inactive cards of same player

                                            if (fusionResult == "1")
                                            {
                                                //remove two cards and insert 1 video

                                                UserVideo.ConsolidateCards(this.userid, this.username, card1, card2);
                                                PlayerLog.LogHistory(this.userid, this.username, PlayerHistoryType.ConsolidateCards, new ConsolidateCardsDesc(un1, un2).Generate());

                                                responseResult = "1";
                                            }
                                        }
                                        else if (un1.IsActive != un2.IsActive)
                                        {
                                            //rule 2 -- one active card and one inactive card

                                            if (fusionResult == "2")
                                            {
                                                //remove two cards and insert 1 video

                                                UserVideo.ConsolidateCards(this.userid, this.username, card1, card2);
                                                PlayerLog.LogHistory(this.userid, this.username, PlayerHistoryType.ConsolidateCards, new ConsolidateCardsDesc(un1, un2).Generate());

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

        private bool ValidateAuthKey(string card1, string card2, string userid, string authKey)
        {
            string authPrivateKey = ConfigGlobal.AuthPrivateKey;

            string originStr = card1 + card2 + userid + authPrivateKey;

            byte[] bytes = Encoding.UTF8.GetBytes(originStr);

            byte[] resultBytes = System.Security.Cryptography.MD5.Create().ComputeHash(bytes);

            string sTemp = "";
            for (int i = 0; i < resultBytes.Length; i++)
            {
                sTemp += resultBytes[i].ToString("x").PadLeft(2, '0');
            }

            return sTemp.Substring(8, 16) == authKey;
        }
    }
}

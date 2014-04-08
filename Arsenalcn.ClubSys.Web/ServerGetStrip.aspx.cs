using System;
using System.Collections.Generic;

using Arsenalcn.ClubSys.DataAccess;
using Arsenalcn.ClubSys.Entity;

using Discuz.Entity;
using Discuz.Forum;

namespace Arsenalcn.ClubSys.Web
{
    public partial class ServerGetStrip : Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string responseMessage = "-1";
            string userMoney = "0";
            int bingoHistoryID = 0;
            bool isGoogleAdv = false;

            try
            {
                if (userid != -1)
                {
                    if (Request.Form["StripResult"] != null)
                    {
                        if (Request.Form["StripResult"] == "start")
                        {
                            Boolean.TryParse(Request.Form["IsGoogleAdv"], out isGoogleAdv);

                            #region StartGetStrip
                            List<Club> clubs = ClubLogic.GetActiveUserClubs(userid);

                            if (clubs.Count != 0)
                            {
                                int clubID = clubs[0].ID.Value;

                                //check the user last time play
                                //TimeSpan ts = PlayerStrip.GetUserBingoTimeSpan(userid);

                                int count = PlayerStrip.GetUserBingoPlayCountThisHour(userid);
                                UserInfo info = AdminUsers.GetUserInfo(userid);

                                userMoney = ((int)info.Extcredits2).ToString();

                                if ((ConfigGlobal.BingoPlayCountPerHour > count) && (PlayerStrip.GetClubRemainingEquipment(clubID) > 0))
                                {
                                    if (isGoogleAdv && ConfigGlobal.GoogleAdvActive)
                                    {
                                        bingoHistoryID = PlayerStrip.InsertBingoStart(userid, username, clubID);

                                        responseMessage = (ConfigGlobal.BingoPlayCountPerHour - count).ToString();
                                    }
                                    else
                                    {
                                        //play cost
                                        int bingoCost = ConfigGlobal.BingoCost;

                                        if (info.Extcredits2 >= bingoCost)
                                        {
                                            info.Extcredits2 = info.Extcredits2 - bingoCost;

                                            if (AdminUsers.UpdateUserAllInfo(info))
                                            {
                                                bingoHistoryID = PlayerStrip.InsertBingoStart(userid, username, clubID);

                                                //return a rate based on user fortune

                                                //responseResult = PlayerStrip.CalcFlashRate(userid);

                                                //return userFortune

                                                responseMessage = (ConfigGlobal.BingoPlayCountPerHour - count).ToString();
                                            }
                                        }
                                        else
                                        {
                                            responseMessage = "-2";
                                        }
                                    }
                                }
                                else
                                {
                                    responseMessage = "0";
                                }
                            }
                            #endregion
                        }
                        else if (Request.Form["StripResult"] == "result")
                        {
                            string finalResult = Request.Form["FinalResult"];
                            string resultType = Request.Form["ResultType"];

                            string authKey = Request.Form["AuthKey"];
                            bingoHistoryID = int.Parse(Request.Form["ID"]);
                            Boolean.TryParse(Request.Form["IsGoogleAdv"], out isGoogleAdv);

                            UserInfo info = AdminUsers.GetUserInfo(userid);

                            if (PlayerStrip.ValidateAuthKey(bingoHistoryID.ToString(), this.userid.ToString(), authKey))
                            {
                                if (PlayerStrip.ValidateBingoResult(bingoHistoryID, userid))
                                {
                                    if (isGoogleAdv && ConfigGlobal.GoogleAdvActive)
                                    {
                                        #region GetStripResult
                                        BingoResult br = BingoUtil.AnalyzeFlashResult(userid, finalResult, resultType);
                                        responseMessage = PlayerStrip.UpdateBingoResult(userid, username, br, resultType);
                                        userMoney = ((int)info.Extcredits2).ToString("f0");
                                        #endregion
                                    }
                                    else
                                    {
                                        int bingoGetCost = ConfigGlobal.BingoGetCost;
                                        if (info.Extcredits2 >= bingoGetCost)
                                        {
                                            info.Extcredits2 = info.Extcredits2 - bingoGetCost;
                                            if (AdminUsers.UpdateUserAllInfo(info))
                                            {
                                                #region GetStripResult
                                                BingoResult br = BingoUtil.AnalyzeFlashResult(userid, finalResult, resultType);
                                                responseMessage = PlayerStrip.UpdateBingoResult(userid, username, br, resultType);
                                                userMoney = ((int)info.Extcredits2).ToString("f0");
                                                #endregion
                                            }
                                        }
                                        else
                                        {
                                            responseMessage = "-2";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                responseMessage = "-1";
                            }
                        }
                    }
                }
            }
            catch
            {
                responseMessage = "-1";
            }
            string responseText = string.Format("ServerMessage={0}&UserMoney={1}&ID={2}&UserID={3}", responseMessage, userMoney, bingoHistoryID, userid);
            Response.Write(responseText);
        }
    }
}
using System;
using Arsenalcn.ClubSys.Entity;
using Arsenalcn.ClubSys.Service;
using Arsenalcn.ClubSys.Web.Common;
using Discuz.Forum;

namespace Arsenalcn.ClubSys.Web
{
    public partial class ServerGetStrip : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var responseMessage = "-1";
            var userMoney = "0";
            var bingoHistoryID = 0;
            var isGoogleAdv = false;

            try
            {
                if (userid != -1)
                {
                    if (Request.Form["StripResult"] != null)
                    {
                        if (Request.Form["StripResult"] == "start")
                        {
                            bool.TryParse(Request.Form["IsGoogleAdv"], out isGoogleAdv);

                            #region StartGetStrip

                            var clubs = ClubLogic.GetActiveUserClubs(userid);

                            if (clubs.Count != 0)
                            {
                                var clubID = clubs[0].ID.Value;

                                //check the user last time play
                                //TimeSpan ts = PlayerStrip.GetUserBingoTimeSpan(userid);

                                var count = PlayerStrip.GetUserBingoPlayCountThisHour(userid);
                                var info = Users.GetUserInfo(userid);

                                userMoney = ((int) info.Extcredits2).ToString();

                                if ((ConfigGlobal.BingoPlayCountPerHour > count) &&
                                    (PlayerStrip.GetClubRemainingEquipment(clubID) > 0))
                                {
                                    if (isGoogleAdv && ConfigGlobal.GoogleAdvActive)
                                    {
                                        bingoHistoryID = PlayerStrip.InsertBingoStart(userid, username, clubID);

                                        responseMessage = (ConfigGlobal.BingoPlayCountPerHour - count).ToString();
                                    }
                                    else
                                    {
                                        //play cost
                                        var bingoCost = ConfigGlobal.BingoCost;

                                        if (info.Extcredits2 >= bingoCost)
                                        {
                                            info.Extcredits2 = info.Extcredits2 - bingoCost;

                                            if (AdminUsers.UpdateUserAllInfo(info))
                                            {
                                                bingoHistoryID = PlayerStrip.InsertBingoStart(userid, username, clubID);

                                                //return a rate based on user fortune

                                                //responseResult = PlayerStrip.CalcFlashRate(userid);

                                                //return userFortune

                                                responseMessage =
                                                    (ConfigGlobal.BingoPlayCountPerHour - count).ToString();
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
                            var finalResult = Request.Form["FinalResult"];
                            var resultType = Request.Form["ResultType"];

                            var authKey = Request.Form["AuthKey"];
                            bingoHistoryID = int.Parse(Request.Form["ID"]);
                            bool.TryParse(Request.Form["IsGoogleAdv"], out isGoogleAdv);

                            var info = Users.GetUserInfo(userid);

                            if (PlayerStrip.ValidateAuthKey(bingoHistoryID.ToString(), userid.ToString(), authKey))
                            {
                                if (PlayerStrip.ValidateBingoResult(bingoHistoryID, userid))
                                {
                                    if (isGoogleAdv && ConfigGlobal.GoogleAdvActive)
                                    {
                                        #region GetStripResult

                                        var br = BingoUtil.AnalyzeFlashResult(userid, finalResult, resultType);
                                        responseMessage = PlayerStrip.UpdateBingoResult(userid, username, br, resultType);
                                        userMoney = ((int) info.Extcredits2).ToString("f0");

                                        #endregion
                                    }
                                    else
                                    {
                                        var bingoGetCost = ConfigGlobal.BingoGetCost;
                                        if (info.Extcredits2 >= bingoGetCost)
                                        {
                                            info.Extcredits2 = info.Extcredits2 - bingoGetCost;
                                            if (AdminUsers.UpdateUserAllInfo(info))
                                            {
                                                #region GetStripResult

                                                var br = BingoUtil.AnalyzeFlashResult(userid, finalResult, resultType);
                                                responseMessage = PlayerStrip.UpdateBingoResult(userid, username, br,
                                                    resultType);
                                                userMoney = ((int) info.Extcredits2).ToString("f0");

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
            var responseText =
                $"ServerMessage={responseMessage}&UserMoney={userMoney}&ID={bingoHistoryID}&UserID={userid}";
            Response.Write(responseText);
        }
    }
}
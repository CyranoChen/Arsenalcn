using System;
using Arsenalcn.ClubSys.Entity;
using Discuz.Forum;

namespace Arsenalcn.ClubSys.Service
{
    public class UserClubLogic
    {
        public static void NominateManager(int clubID, int userID, string userName, string operatorUserName)
        {
            ChangeResponsibility(userID, userName, clubID, Responsibility.Manager, operatorUserName);
        }

        public static void ApplyJoinClub(int userID, string userName, int clubID)
        {
            var ah = new ApplyHistory();

            ah.Userid = userID;
            ah.UserName = userName;
            ah.ClubUid = clubID;
            ah.ApplyDate = DateTime.Now;

            ClubLogic.SaveApplyHistory(ah);

            ClubSysPrivateMessage.SendMessage(clubID, userName, ClubSysMessageType.ApplyJoinClub);
        }

        public static void ApproveJoinClub(int applyHistoryId, bool approved, string operatorUserName)
        {
            var ah = ClubLogic.GetApplyHistory(applyHistoryId);

            ah.IsAccepted = approved;

            ClubLogic.SaveApplyHistory(ah);

            var ch = new ClubHistory();
            ch.ClubID = ah.ClubUid;
            ch.ActionUserName = ah.UserName;
            ch.OperatorUserName = operatorUserName;
            ch.ActionDate = DateTime.Now;

            if (approved)
            {
                //insert user club
                var uc = new UserClub();

                uc.ClubUid = ah.ClubUid;
                uc.JoinClubDate = DateTime.Now;
                uc.FromDate = DateTime.Now;
                uc.IsActive = true;
                uc.Responsibility = (int) Responsibility.Member;
                uc.Userid = ah.Userid;
                uc.UserName = ah.UserName;

                ClubLogic.SaveUserClub(uc);

                ch.ActionType = ClubHistoryActionType.JoinClub.ToString();
                ch.ActionDescription = ClubLogic.BuildClubHistoryActionDesc(ClubHistoryActionType.JoinClub, null);
            }
            else
            {
                ch.ActionType = ClubHistoryActionType.RejectJoinClub.ToString();
                ch.ActionDescription = ClubLogic.BuildClubHistoryActionDesc(ClubHistoryActionType.RejectJoinClub, null);
            }

            ClubLogic.SaveClubHistory(ch);

            if (approved)
            {
                ClubSysPrivateMessage.SendMessage(ah.ClubUid, ah.UserName, ClubSysMessageType.ApproveJoinClub);
            }
            else
            {
                ClubSysPrivateMessage.SendMessage(ah.ClubUid, ah.UserName, ClubSysMessageType.RejectJoinClub);
            }
        }

        //for internal temp leave usage
        internal static bool LeaveClub(int userID, int clubID)
        {
            var uc = ClubLogic.GetActiveUserClub(userID, clubID);

            if (uc != null)
            {
                uc.ToDate = DateTime.Now;
                uc.IsActive = false;

                if (uc.Responsibility == (int) Responsibility.Manager)
                    return false;

                ClubLogic.SaveUserClub(uc);

                return true;
            }
            return false;
        }

        public static bool LeaveClub(int userID, int clubID, bool isKicked, string kickUserName)
        {
            var uc = ClubLogic.GetActiveUserClub(userID, clubID);

            if (uc != null)
            {
                uc.ToDate = DateTime.Now;
                uc.IsActive = false;

                if (uc.Responsibility == (int) Responsibility.Manager)
                    return false;

                ClubLogic.SaveUserClub(uc);

                var ch = new ClubHistory();
                ch.ClubID = clubID;
                ch.ActionUserName = uc.UserName;

                if (isKicked)
                {
                    ch.ActionType = ClubHistoryActionType.MandatoryLeaveClub.ToString();
                    ch.ActionDescription = ClubLogic.BuildClubHistoryActionDesc(
                        ClubHistoryActionType.MandatoryLeaveClub, null);
                    ch.OperatorUserName = kickUserName;
                }
                else
                {
                    ch.ActionType = ClubHistoryActionType.LeaveClub.ToString();
                    ch.ActionDescription = ClubLogic.BuildClubHistoryActionDesc(ClubHistoryActionType.LeaveClub, null);
                    ch.OperatorUserName = uc.UserName;
                }

                ClubLogic.SaveClubHistory(ch);

                if (isKicked)
                    ClubSysPrivateMessage.SendMessage(clubID, uc.UserName, ClubSysMessageType.MandatoryLeaveClub);
                else
                    ClubSysPrivateMessage.SendMessage(clubID, uc.UserName, ClubSysMessageType.LeaveClub);

                return true;
            }
            return false;
        }

        public static void ChangeResponsibility(int userID, string userName, int clubID, Responsibility res,
            string operatorUserName)
        {
            var userClub = ClubLogic.GetActiveUserClub(userID, clubID);
            if (userClub != null && userClub.Responsibility == (int) res)
                return;

            var club = ClubLogic.GetClubInfo(clubID);

            if (club != null)
            {
                if (res == Responsibility.Manager)
                {
                    #region Proceed previous manager user club info

                    var preManagerUid = club.ManagerUid.Value;
                    var preManagerName = club.ManagerUserName;
                    LeaveClub(preManagerUid, clubID);

                    var preManagerUc = ClubLogic.GetActiveUserClub(preManagerUid, clubID);

                    var preUC = new UserClub();
                    preUC.ClubUid = clubID;
                    preUC.JoinClubDate = preManagerUc.JoinClubDate;
                    preUC.FromDate = DateTime.Now;
                    preUC.IsActive = true;
                    preUC.Responsibility = (int) Responsibility.Member;
                    preUC.Userid = preManagerUid;
                    preUC.UserName = preManagerName;

                    ClubLogic.SaveUserClub(preUC);

                    #endregion

                    club.ManagerUid = userID;
                    club.ManagerUserName = userName;
                    club.UpdateDate = DateTime.Now;
                    ClubLogic.SaveClub(club);
                }

                LeaveClub(userID, clubID);

                var uc = new UserClub();
                uc.ClubUid = clubID;
                uc.JoinClubDate = userClub.JoinClubDate;
                uc.FromDate = DateTime.Now;
                uc.IsActive = true;
                uc.Userid = userID;
                uc.UserName = userName;


                var ch = new ClubHistory();
                ch.ClubID = clubID;
                ch.ActionUserName = userName;
                ch.OperatorUserName = operatorUserName;

                if (userClub.Responsibility.Value > (int) res)
                {
                    //nominate
                    ch.ActionType = ClubHistoryActionType.Nominated.ToString();
                    ch.ActionDescription = ClubLogic.BuildClubHistoryActionDesc(ClubHistoryActionType.Nominated,
                        ClubLogic.TranslateResponsibility(res));
                }
                else
                {
                    //dismiss
                    ch.ActionType = ClubHistoryActionType.Dismiss.ToString();
                    ch.ActionDescription = ClubLogic.BuildClubHistoryActionDesc(ClubHistoryActionType.Dismiss,
                        ClubLogic.TranslateResponsibility(res));
                }

                uc.Responsibility = (int) res;

                ClubLogic.SaveUserClub(uc);
                ClubLogic.SaveClubHistory(ch);
            }
            else
                throw new Exception("Club not exist.");
        }

        internal static bool CancelApplication(int userID, string userName, int clubID)
        {
            var ah = ClubLogic.GetActiveApplyHistoryByUserClub(userID, clubID);

            if (ah != null)
            {
                //proceed to cancel it
                ah.IsAccepted = false;

                ClubLogic.SaveApplyHistory(ah);

                return true;
            }
            return false;
        }

        internal static bool JoinClub(int userID, string userName, int clubID)
        {
            var ah = ClubLogic.GetActiveApplyHistoryByUserClub(userID, clubID);
            if (ah == null)
            {
                //proceed it
                ApplyJoinClub(userID, userName, clubID);
                return true;
            }
            return false;
        }

        public static bool UserClubAction(int userID, string userName, int clubID, UserClubStatus uct)
        {
            switch (uct)
            {
                case UserClubStatus.Applied:
                    //cancel an application
                    return CancelApplication(userID, userName, clubID);
                case UserClubStatus.Member:
                    //leave a club
                    return LeaveClub(userID, clubID, false, null);
                case UserClubStatus.No:
                    //apply to join a club
                    return JoinClub(userID, userName, clubID);
                default:
                    break;
            }

            return false;
        }

        public static void TransferMemberExtcredit(int clubID, int fromUserID, int toUserID, float extCredit,
            int extCreditType)
        {
            //UserClub ucFrom = ClubLogic.GetActiveUserClub(fromUserID, clubID);
            //UserClub ucTo = ClubLogic.GetActiveUserClub(toUserID, clubID);

            var userFrom = Users.GetUserInfo(fromUserID);
            var userTo = Users.GetUserInfo(toUserID);

            if (fromUserID != toUserID)
            {
                if (extCredit > Users.GetUserExtCredits(fromUserID, extCreditType))
                {
                    throw new Exception("Insufficient Founds");
                }

                var list = ClubLogic.GetUserManagedClubs(fromUserID);
                if (list == null || list.Count <= 0)
                {
                    throw new Exception("No privilege of tranfer");
                }

                // Transfer Logic

                Users.UpdateUserExtCredits(fromUserID, extCreditType, -extCredit);
                Users.UpdateUserExtCredits(toUserID, extCreditType, extCredit);

                // Club History Log & SMS

                var ch = new ClubHistory();
                ch.ClubID = clubID;
                ch.ActionUserName = userTo.Username.Trim();
                ch.ActionType = ClubHistoryActionType.TransferExtcredit.ToString();
                ch.ActionDescription = ClubLogic.BuildClubHistoryActionDesc(ClubHistoryActionType.TransferExtcredit,
                    userTo.Username.Trim(), extCredit.ToString(), "枪手币");
                ch.OperatorUserName = userFrom.Username.Trim();

                ClubLogic.SaveClubHistory(ch);

                ClubSysPrivateMessage.SendMessage(clubID, userTo.Username.Trim(), ClubSysMessageType.TransferExtcredit,
                    userFrom.Username.Trim(), extCredit.ToString("N0"), "枪手币");
            }
            else
            {
                throw new Exception("Can't transfer to yourself");
            }
        }

        public static void UserClubStatistics()
        {
            foreach (var club in ClubLogic.GetActiveClubs())
            {
                try
                {
                    var clubMemberFortune = 0;
                    var clubMemberCredit = 0;
                    var clubMemberLoyalty = 0;
                    var clubMemberRP = 0;

                    var members = ClubLogic.GetClubMembers(club.ID.Value);

                    foreach (var member in members)
                    {
                        try
                        {
                            var userInfo = Users.GetShortUserInfo(member.Userid.Value);

                            if (userInfo != null)
                            {
                                clubMemberFortune += (int) userInfo.Extcredits2;
                                clubMemberCredit += userInfo.Credits;
                                clubMemberLoyalty += (int) (DateTime.Now - member.JoinClubDate.Value).TotalDays;
                                clubMemberRP += (int) userInfo.Extcredits4;
                            }
                        }
                        catch
                        {
                        }
                    }

                    club.MemberFortune = clubMemberFortune;
                    club.MemberCredit = clubMemberCredit;
                    club.MemberLoyalty = clubMemberLoyalty;
                    club.MemberRP = clubMemberRP;

                    var ra = new RankAlgorithm(club);
                    club.RankScore = ra.SummaryRankPoint;

                    ClubLogic.SaveClub(club);
                }
                catch
                {
                }
            }
        }

        public static void CalcClubFortuneIncrement()
        {
            foreach (var club in ClubLogic.GetActiveClubs())
            {
                try
                {
                    var clubFortuneIncrement = 0;

                    var members = ClubLogic.GetClubMembers(club.ID.Value);

                    foreach (var member in members)
                    {
                        try
                        {
                            var userInfo = Users.GetShortUserInfo(member.Userid.Value);

                            if (userInfo != null)
                            {
                                clubFortuneIncrement += FortuneContributeAlgorithm.CalcContributeFortune(userInfo, true);

                                //int memberFortune = (int)userInfo.Extcredits2;
                                //int memberCredit = userInfo.Credits;
                                //float memberMana = userInfo.Extcredits1;//威望

                                //clubFortuneIncrement += (int)(Config.ClubFortuneIncrementVariable * Math.Pow(Math.Log10(memberCredit), 4)) + (int)((memberMana + 1) * Math.Log10(memberFortune));
                            }
                        }
                        catch
                        {
                        }
                    }

                    club.Fortune = club.Fortune.Value + clubFortuneIncrement;

                    var ra = new RankAlgorithm(club);
                    club.RankLevel = RankLevel.GetInstance().GetRank(club.Fortune.Value).ID;

                    ClubLogic.SaveClub(club);
                }
                catch
                {
                }
            }
        }
    }
}
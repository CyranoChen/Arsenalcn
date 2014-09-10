using System;
using System.Collections.Generic;
using System.Text;

using Arsenalcn.ClubSys.Entity;
using Discuz.Entity;
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
            ApplyHistory ah = new ApplyHistory();

            ah.Userid = userID;
            ah.UserName = userName;
            ah.ClubUid = clubID;
            ah.ApplyDate = DateTime.Now;

            ClubLogic.SaveApplyHistory(ah);

            ClubSysPrivateMessage.SendMessage(clubID, userName, ClubSysMessageType.ApplyJoinClub);
        }

        public static void ApproveJoinClub(int applyHistoryId, bool approved, string operatorUserName)
        {
            ApplyHistory ah = ClubLogic.GetApplyHistory(applyHistoryId);

            ah.IsAccepted = approved;

            ClubLogic.SaveApplyHistory(ah);

            ClubHistory ch = new ClubHistory();
            ch.ClubID = ah.ClubUid;
            ch.ActionUserName = ah.UserName;
            ch.OperatorUserName = operatorUserName;
            ch.ActionDate = DateTime.Now;

            if (approved)
            {
                //insert user club
                UserClub uc = new UserClub();

                uc.ClubUid = ah.ClubUid;
                uc.JoinClubDate = DateTime.Now;
                uc.FromDate = DateTime.Now;
                uc.IsActive = true;
                uc.Responsibility = (int)Responsibility.Member;
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
            UserClub uc = ClubLogic.GetActiveUserClub(userID, clubID);

            if (uc != null)
            {
                uc.ToDate = DateTime.Now;
                uc.IsActive = false;

                if (uc.Responsibility == (int)Responsibility.Manager)
                    return false;

                ClubLogic.SaveUserClub(uc);

                return true;
            }
            else
                return false;
        }

        public static bool LeaveClub(int userID, int clubID, bool isKicked, string kickUserName)
        {
            UserClub uc = ClubLogic.GetActiveUserClub(userID, clubID);

            if (uc != null)
            {
                uc.ToDate = DateTime.Now;
                uc.IsActive = false;

                if (uc.Responsibility == (int)Responsibility.Manager)
                    return false;

                ClubLogic.SaveUserClub(uc);

                ClubHistory ch = new ClubHistory();
                ch.ClubID = clubID;
                ch.ActionUserName = uc.UserName;

                if (isKicked)
                {
                    ch.ActionType = ClubHistoryActionType.MandatoryLeaveClub.ToString();
                    ch.ActionDescription = ClubLogic.BuildClubHistoryActionDesc(ClubHistoryActionType.MandatoryLeaveClub, null);
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
            else
                return false;
        }

        public static void ChangeResponsibility(int userID, string userName, int clubID, Responsibility res, string operatorUserName)
        {
            UserClub userClub = ClubLogic.GetActiveUserClub(userID, clubID);
            if (userClub != null && userClub.Responsibility == (int)res)
                return;

            Club club = ClubLogic.GetClubInfo(clubID);

            if (club != null)
            {
                if (res == Responsibility.Manager)
                {
                    #region Proceed previous manager user club info
                    int preManagerUid = club.ManagerUid.Value;
                    string preManagerName = club.ManagerUserName;
                    LeaveClub(preManagerUid, clubID);

                    UserClub preManagerUc = ClubLogic.GetActiveUserClub(preManagerUid, clubID);

                    UserClub preUC = new UserClub();
                    preUC.ClubUid = clubID;
                    preUC.JoinClubDate = preManagerUc.JoinClubDate;
                    preUC.FromDate = DateTime.Now;
                    preUC.IsActive = true;
                    preUC.Responsibility = (int)Responsibility.Member;
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

                UserClub uc = new UserClub();
                uc.ClubUid = clubID;
                uc.JoinClubDate = userClub.JoinClubDate;
                uc.FromDate = DateTime.Now;
                uc.IsActive = true;
                uc.Userid = userID;
                uc.UserName = userName;


                ClubHistory ch = new ClubHistory();
                ch.ClubID = clubID;
                ch.ActionUserName = userName;
                ch.OperatorUserName = operatorUserName;

                if (userClub.Responsibility.Value > (int)res)
                {
                    //nominate
                    ch.ActionType = ClubHistoryActionType.Nominated.ToString();
                    ch.ActionDescription = ClubLogic.BuildClubHistoryActionDesc(ClubHistoryActionType.Nominated, ClubLogic.TranslateResponsibility(res));
                }
                else
                {
                    //dismiss
                    ch.ActionType = ClubHistoryActionType.Dismiss.ToString();
                    ch.ActionDescription = ClubLogic.BuildClubHistoryActionDesc(ClubHistoryActionType.Dismiss, ClubLogic.TranslateResponsibility(res));
                }

                uc.Responsibility = (int)res;

                ClubLogic.SaveUserClub(uc);
                ClubLogic.SaveClubHistory(ch);
            }
            else
                throw new Exception("Club not exist.");
        }

        internal static bool CancelApplication(int userID, string userName, int clubID)
        {
            ApplyHistory ah = ClubLogic.GetActiveApplyHistoryByUserClub(userID, clubID);

            if (ah != null)
            {
                //proceed to cancel it
                ah.IsAccepted = false;

                ClubLogic.SaveApplyHistory(ah);

                return true;
            }
            else
                return false;
        }

        internal static bool JoinClub(int userID, string userName, int clubID)
        {
            ApplyHistory ah = ClubLogic.GetActiveApplyHistoryByUserClub(userID, clubID);
            if (ah == null)
            {
                //proceed it
                ApplyJoinClub(userID, userName, clubID);
                return true;
            }
            else
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

        public static void TransferMemberExtcredit(int clubID, int fromUserID, int toUserID, float extCredit, int extCreditType)
        {
            //UserClub ucFrom = ClubLogic.GetActiveUserClub(fromUserID, clubID);
            //UserClub ucTo = ClubLogic.GetActiveUserClub(toUserID, clubID);

            UserInfo userFrom = AdminUsers.GetUserInfo(fromUserID);
            UserInfo userTo = AdminUsers.GetUserInfo(toUserID);

            if (fromUserID != toUserID)
            {
                if (extCredit > AdminUsers.GetUserExtCredits(fromUserID, extCreditType))
                { throw new Exception("Insufficient Founds"); }

                List<Club> list = ClubLogic.GetUserManagedClubs(fromUserID);
                if (list == null || list.Count <= 0)
                { throw new Exception("No privilege of tranfer"); }

                // Transfer Logic

                AdminUsers.UpdateUserExtCredits(fromUserID, extCreditType, -extCredit);
                AdminUsers.UpdateUserExtCredits(toUserID, extCreditType, extCredit);

                // Club History Log & SMS

                ClubHistory ch = new ClubHistory();
                ch.ClubID = clubID;
                ch.ActionUserName = userTo.Username.Trim();
                ch.ActionType = ClubHistoryActionType.TransferExtcredit.ToString();
                ch.ActionDescription = ClubLogic.BuildClubHistoryActionDesc(ClubHistoryActionType.TransferExtcredit, userTo.Username.Trim(), extCredit.ToString(), "枪手币");
                ch.OperatorUserName = userFrom.Username.Trim();

                ClubLogic.SaveClubHistory(ch);

                ClubSysPrivateMessage.SendMessage(clubID, userTo.Username.Trim(), ClubSysMessageType.TransferExtcredit, userFrom.Username.Trim(), extCredit.ToString("N0"), "枪手币");
            }
            else
            {
                throw new Exception("Can't transfer to yourself");
            }
        }

        public static void UserClubStatistics()
        {
            foreach (Club club in ClubLogic.GetActiveClubs())
            {
                try
                {
                    int clubMemberFortune = 0;
                    int clubMemberCredit = 0;
                    int clubMemberLoyalty = 0;
                    int clubMemberRP = 0;

                    List<UserClub> members = ClubLogic.GetClubMembers(club.ID.Value);

                    foreach (UserClub member in members)
                    {
                        try
                        {
                            ShortUserInfo userInfo = Users.GetShortUserInfo(member.Userid.Value);

                            if (userInfo != null)
                            {
                                clubMemberFortune += (int)userInfo.Extcredits2;
                                clubMemberCredit += userInfo.Credits;
                                clubMemberLoyalty += (int)(DateTime.Now - member.JoinClubDate.Value).TotalDays;
                                clubMemberRP += (int)userInfo.Extcredits4;
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    club.MemberFortune = clubMemberFortune;
                    club.MemberCredit = clubMemberCredit;
                    club.MemberLoyalty = clubMemberLoyalty;
                    club.MemberRP = clubMemberRP;

                    RankAlgorithm ra = new RankAlgorithm(club);
                    club.RankScore = ra.SummaryRankPoint;

                    ClubLogic.SaveClub(club);
                }
                catch
                {
                    continue;
                }
            }
        }

        public static void CalcClubFortuneIncrement()
        {
            foreach (Club club in ClubLogic.GetActiveClubs())
            {
                try
                {
                    int clubFortuneIncrement = 0;

                    List<UserClub> members = ClubLogic.GetClubMembers(club.ID.Value);

                    foreach (UserClub member in members)
                    {
                        try
                        {
                            ShortUserInfo userInfo = Users.GetShortUserInfo(member.Userid.Value);

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
                            continue;
                        }
                    }

                    club.Fortune = club.Fortune.Value + clubFortuneIncrement;

                    RankAlgorithm ra = new RankAlgorithm(club);
                    club.RankLevel = RankLevel.GetInstance().GetRank(club.Fortune.Value).ID;

                    ClubLogic.SaveClub(club);
                }
                catch
                {
                    continue;
                }
            }
        }
    }
}

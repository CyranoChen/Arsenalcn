using System;
using System.Collections.Generic;
using System.Text;

using Arsenalcn.ClubSys.Entity;

using Discuz.Entity;
using Discuz.Forum;

namespace Arsenalcn.ClubSys.Service
{
    public class ClubSysPrivateMessage
    {
        private static readonly string ApplyClub_Message = "用户 {0} 提交了创建球会 {1} 的申请，请审核。";
        private static readonly string ApproveClub_Message = "您的球会{0}创建申请已{1}。";
        private static readonly string ApplyJoinClub_Message = "用户 {0} 申请加入球会 {1}，请审核。";
        private static readonly string ApproveJoinClub_Message = "您已通过审核加入球会 {0}。";
        private static readonly string LeaveClub_Message = "您已退出球会 {0}。";
        private static readonly string MandatoryLeaveClub_Message = "您已被强制解约出球会 {0}。";
        private static readonly string RejectJoinClub_Message = "你加入球会 {0}的申请已被驳回。";
        private static readonly string TransferExtcredit_Message = "{0} 转账给你 {2}:{1}";

        private static readonly string ApplyClub_Subject = "用户 {0} 创建球会申请通知（请勿回复此系统信息）";
        private static readonly string ApproveClub_Subject = "球会申请审核通知（请勿回复此系统信息）";
        private static readonly string ApplyJoinClub_Subject = "用户 {0} 申请加入球会通知（请勿回复此系统信息）";
        private static readonly string ApproveJoinClub_Subject = "加入球会通知（请勿回复此系统信息）";
        private static readonly string LeaveClub_Subject = "退会通知（请勿回复此系统信息）";
        private static readonly string MandatoryLeaveClub_Subject = "强制退会通知（请勿回复此系统信息）";
        private static readonly string RejectJoinClub_Subject = "加入球会申请被驳回通知（请勿回复此系统信息）";
        private static readonly string TransferExtcredit_Subject = "球会 {0} 转账通知";

        public static readonly string ClubSysAdminName = "球会系统管理员";

        public static void SendMessage(int clubID, string userName, ClubSysMessageType messageType, params string[] para)
        {
            int userID = AdminUsers.GetUserId(userName);
            Club club = ClubLogic.GetClubInfo(clubID);

            PrivateMessageInfo pm;

            switch (messageType)
            {
                case ClubSysMessageType.ApplyClub:
                    foreach (string aID in ConfigGlobal.PluginAdmin)
                    {
                        pm = new PrivateMessageInfo();

                        pm.Msgfrom = ClubSysAdminName;
                        pm.Msgfromid = 0;

                        pm.Folder = 0;
                        pm.Message = string.Format(ApplyClub_Message, userName, club.FullName);

                        pm.Msgtoid = Convert.ToInt32(aID);
                        pm.Msgto = AdminUsers.GetUserInfo(aID).Username.Trim();
                        pm.New = 1;
                        pm.Postdatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        pm.Subject = string.Format(ApplyClub_Subject, userName);

                        PrivateMessages.CreatePrivateMessage(pm, 0);
                    }
                    break;
                case ClubSysMessageType.ApplyJoinClub:
                    List<UserClub> users = ClubLogic.GetClubLeads(clubID);
                    foreach (UserClub userClub in users)
                    {
                        pm = new PrivateMessageInfo();

                        pm.Msgfrom = ClubSysAdminName;
                        pm.Msgfromid = 0;

                        pm.Folder = 0;
                        pm.Message = string.Format(ApplyJoinClub_Message, userName, club.FullName);
                        pm.Msgto = userClub.UserName;
                        pm.Msgtoid = userClub.Userid.Value;
                        pm.New = 1;
                        pm.Postdatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        pm.Subject = string.Format(ApplyJoinClub_Subject, userName);

                        PrivateMessages.CreatePrivateMessage(pm, 0);
                    }
                    break;
                case ClubSysMessageType.ApproveClub:
                    if (club != null)
                    {
                        string result = "通过";
                        if (!club.IsActive.Value)
                            result = "驳回";

                        pm = new PrivateMessageInfo();

                        pm.Msgfrom = ClubSysAdminName;
                        pm.Msgfromid = 0;

                        pm.Folder = 0;
                        pm.Message = string.Format(ApproveClub_Message, club.FullName, result);
                        pm.Msgto = club.CreatorUserName;
                        pm.Msgtoid = club.CreatorUid.Value;
                        pm.New = 1;
                        pm.Postdatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        pm.Subject = ApproveClub_Subject;

                        PrivateMessages.CreatePrivateMessage(pm, 0);
                    }
                    break;
                case ClubSysMessageType.ApproveJoinClub:
                    pm = new PrivateMessageInfo();

                    pm.Msgfrom = ClubSysAdminName;
                    pm.Msgfromid = 0;

                    pm.Folder = 0;
                    pm.Message = string.Format(ApproveJoinClub_Message, club.FullName);
                    pm.Msgto = userName;
                    pm.Msgtoid = userID;
                    pm.New = 1;
                    pm.Postdatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    pm.Subject = ApproveJoinClub_Subject;

                    PrivateMessages.CreatePrivateMessage(pm, 0);
                    break;
                case ClubSysMessageType.LeaveClub:
                    pm = new PrivateMessageInfo();

                    pm.Msgfrom = ClubSysAdminName;
                    pm.Msgfromid = 0;

                    pm.Folder = 0;
                    pm.Message = string.Format(LeaveClub_Message, club.FullName);
                    pm.Msgto = userName;
                    pm.Msgtoid = userID;
                    pm.New = 1;
                    pm.Postdatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    pm.Subject = LeaveClub_Subject;

                    PrivateMessages.CreatePrivateMessage(pm, 0);

                    break;
                case ClubSysMessageType.MandatoryLeaveClub:
                    pm = new PrivateMessageInfo();

                    pm.Msgfrom = ClubSysAdminName;
                    pm.Msgfromid = 0;

                    pm.Folder = 0;
                    pm.Message = string.Format(MandatoryLeaveClub_Message, club.FullName);
                    pm.Msgto = userName;
                    pm.Msgtoid = userID;
                    pm.New = 1;
                    pm.Postdatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    pm.Subject = MandatoryLeaveClub_Subject;

                    PrivateMessages.CreatePrivateMessage(pm, 0);

                    break;
                case ClubSysMessageType.RejectJoinClub:
                    pm = new PrivateMessageInfo();

                    pm.Msgfrom = ClubSysAdminName;
                    pm.Msgfromid = 0;

                    pm.Folder = 0;
                    pm.Message = string.Format(RejectJoinClub_Message, club.FullName);
                    pm.Msgto = userName;
                    pm.Msgtoid = userID;
                    pm.New = 1;
                    pm.Postdatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    pm.Subject = RejectJoinClub_Subject;

                    PrivateMessages.CreatePrivateMessage(pm, 0);

                    break;
                case ClubSysMessageType.TransferExtcredit:
                    pm = new PrivateMessageInfo();

                    pm.Msgfrom = para[0];
                    pm.Msgfromid = AdminUsers.GetUserId(para[0]);

                    pm.Folder = 0;
                    pm.Message = string.Format(TransferExtcredit_Message, para[0], para[1], para[2]);
                    pm.Msgto = userName;
                    pm.Msgtoid = userID;
                    pm.New = 1;
                    pm.Postdatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    pm.Subject = string.Format(TransferExtcredit_Subject, club.FullName);

                    PrivateMessages.CreatePrivateMessage(pm, 0);

                    break;
                default:
                    break;
            }
            
        }
    }

    public enum ClubSysMessageType
    {
        ApplyClub,
        ApproveClub,
        ApplyJoinClub,
        ApproveJoinClub,
        LeaveClub,
        MandatoryLeaveClub,
        RejectJoinClub,
        TransferExtcredit
    }
}

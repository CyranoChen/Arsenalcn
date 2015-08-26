using System;
using System.Web;
using Arsenalcn.ClubSys.Service;
using Arsenalcn.ClubSys.Entity;

namespace Arsenalcn.ClubSys.Web.Control
{
    public partial class ClubSysHeader : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (clubID > 0)
            {
                if (aManageClub.HRef.IndexOf("{0}") >= 0)
                    aManageClub.HRef = string.Format(aManageClub.HRef, clubID.ToString());

                var currentClub = ClubLogic.GetClubInfo(clubID);

                if (currentClub != null)
                {
                    imgClubLogo.ImageUrl = $"../UploadFiles/{currentClub.LogoName}";
                    imgClubLogo.ToolTip = currentClub.FullName;

                    ltrlClubFullName.Text = currentClub.FullName;

                    var clubDesc = HttpUtility.HtmlEncode(currentClub.Description);
                    if (clubDesc.Length > 80)
                        ltrlClubDesc.Text = $"<span title=\"{clubDesc}\">{clubDesc.Substring(0, 80)}...</span>";
                    else
                        ltrlClubDesc.Text = clubDesc;

                    divClubRank.Style.Add("Width", $"{currentClub.RankLevel*20}px");
                    divClubRank.Attributes.Add("Title", currentClub.RankScore.ToString());

                    if (userID == -1)
                    {
                        //anonymous user

                        aManageClub.Visible = false;

                        btnCancelApply.Visible = false;
                        btnJoinClub.Visible = false;
                        btnLeaveClub.Visible = false;

                        btnGetStrip.Visible = false;
                    }
                    else
                    {
                        var ucs = ClubLogic.GetUserClubStatus(userID, clubID);
                        switch (ucs)
                        {
                            case UserClubStatus.Applied:
                                btnCancelApply.Visible = true;
                                btnJoinClub.Visible = false;
                                btnLeaveClub.Visible = false;
                                break;
                            case UserClubStatus.Member:
                                btnCancelApply.Visible = false;
                                btnJoinClub.Visible = false;
                                btnLeaveClub.Visible = true;
                                break;
                            case UserClubStatus.No:
                                btnCancelApply.Visible = false;
                                btnJoinClub.Visible = true;
                                btnLeaveClub.Visible = false;
                                break;
                            default:
                                btnCancelApply.Visible = false;
                                btnJoinClub.Visible = false;
                                btnLeaveClub.Visible = false;
                                break;
                        }

                        //manager can not leave a club
                        if (currentClub.ManagerUid.Value == userID)
                            btnLeaveClub.Visible = false;

                        // the count of clubs which current user has joined exceed max quota, hide join action
                        if (ClubLogic.GetActiveUserClubs(userID).Count >= ConfigGlobal.SingleUserMaxClubCount && ucs != UserClubStatus.Member)
                        {
                            btnJoinClub.Visible = false;
                            btnCancelApply.Visible = false;
                        }

                        if (!currentClub.IsAppliable.Value || ClubLogic.GetClubMemberCount(clubID) >= ClubLogic.GetClubMemberQuota(clubID))
                        {
                            btnJoinClub.Visible = false;
                        }

                        var userClub = ClubLogic.GetActiveUserClub(userID, clubID);

                        if (userClub != null)
                        {
                            //current user is a member of the club

                            if (userClub.Responsibility == (int)Responsibility.Manager || userClub.Responsibility == (int)Responsibility.Executor)
                            {
                                aManageClub.Visible = true;
                            }
                            else
                            {
                                aManageClub.Visible = false;
                            }
                        }
                        else
                        {
                            //user is not a member of the club
                            aManageClub.Visible = false;

                            btnGetStrip.Visible = false;
                        }
                    }
                }
                else
                {
                    Response.Redirect("ClubPortal.aspx");
                }
            }
            else
            {
                Response.Redirect("ClubPortal.aspx");
            }
        }

        private int clubID = -1;
        public int ClubID
        {
            set
            {
                clubID = value;
            }
        }

        private int userID = -1;
        public int UserID
        {
            set
            {
                userID = value;
            }
        }

        private string userName = string.Empty;
        public string UserName
        {
            set
            {
                userName = value;
            }
        }

        protected void btnJoinClub_Click(object sender, EventArgs e)
        {
            var uct = ClubLogic.GetUserClubStatus(userID, clubID);

            if (uct != UserClubStatus.No)
                return;

            var script = string.Empty;

            if (UserClubLogic.UserClubAction(userID, userName, clubID, uct))
            {
                script = "window.alert('申请已提交！');";
            }
            else
            {
                script = "window.alert('状态异常！');";
            }

            btnJoinClub.Visible = false;
            btnCancelApply.Visible = true;

            this.Page.ClientScript.RegisterClientScriptBlock(typeof(string), "join", script, true);
        }

        protected void btnCancelApply_Click(object sender, EventArgs e)
        {
            var uct = ClubLogic.GetUserClubStatus(userID, clubID);

            if (uct != UserClubStatus.Applied)
                return;

            var script = string.Empty;

            if (UserClubLogic.UserClubAction(userID, userName, clubID, uct))
            {
                script = "window.alert('申请已取消！');";
            }
            else
            {
                script = "window.alert('状态异常！');";
            }

            btnCancelApply.Visible = false;
            btnJoinClub.Visible = true;

            this.Page.ClientScript.RegisterClientScriptBlock(typeof(string), "cancel", script, true);
        }

        protected void btnLeaveClub_Click(object sender, EventArgs e)
        {
            var uct = ClubLogic.GetUserClubStatus(userID, clubID);

            if (uct != UserClubStatus.Member)
                return;

            var script = string.Empty;

            if (UserClubLogic.UserClubAction(userID, userName, clubID, uct))
            {
                script = "window.alert('您已离开该球会！');";
            }
            else
            {
                script = "window.alert('状态异常！');";
            }

            btnJoinClub.Visible = true;
            btnLeaveClub.Visible = false;

            this.Page.ClientScript.RegisterClientScriptBlock(typeof(string), "leave", script, true);
        }
    }
}
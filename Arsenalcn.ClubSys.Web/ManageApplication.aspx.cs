using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Arsenalcn.ClubSys.Service;
using Arsenalcn.ClubSys.Entity;

using Discuz.Entity;
using Discuz.Forum;


namespace Arsenalcn.ClubSys.Web
{
    public partial class ManageApplication : Common.BasePage, ICallbackEventHandler
    {
        private int ClubID
        {
            get
            {
                int tmp;
                if (int.TryParse(Request.QueryString["ClubID"], out tmp))
                    return tmp;
                else
                {
                    Response.Redirect("ClubPortal.aspx");

                    return -1;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            #region SetControlProperty

            ctrlLeftPanel.UserID = this.userid;
            ctrlLeftPanel.UserName = this.username;
            ctrlLeftPanel.UserKey = this.userkey;

            ctrlFieldToolBar.UserID = this.userid;
            ctrlFieldToolBar.UserName = this.username;

            ctrlManageMenuTabBar.CurrentMenu = Arsenalcn.ClubSys.Web.Control.ManageClubMenuItem.ManageApplication;
            ctrlManageMenuTabBar.UserID = this.userid;

            #endregion

            #region Callback Reference
            string callbackReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "GetResult", "context");

            string callbackScript = string.Format("function ApproveJoin(arg, context){{ {0} }};", callbackReference);

            Page.ClientScript.RegisterClientScriptBlock(typeof(string), "action", callbackScript, true);

            #endregion

            Club club = ClubLogic.GetClubInfo(ClubID);

            if (club != null && this.Title.IndexOf("{0}") >= 0)
                this.Title = string.Format(this.Title, club.FullName);

            if (!IsPostBack)
            {
                LoadPageData();
            }
        }

        protected void LoadPageData()
        {
            // Administrators could enter this page
            if (ConfigAdmin.IsPluginAdmin(this.userid))
            {
                pnlInaccessible.Visible = false;
                phContent.Visible = true;

                //init gridview
                BindMemberList();
            }
            else
            {
                UserClub userClub = ClubLogic.GetActiveUserClub(this.userid, ClubID);

                if (userClub != null && userClub.Responsibility.HasValue)
                {
                    if (userClub.Responsibility.Value.Equals((int)Responsibility.Executor) || userClub.Responsibility.Value.Equals((int)Responsibility.Manager))
                    {
                        pnlInaccessible.Visible = false;
                        phContent.Visible = true;

                        //init gridview
                        BindMemberList();
                    }
                    else
                    {
                        pnlInaccessible.Visible = true;
                        phContent.Visible = false;

                    }
                }
                else
                {
                    pnlInaccessible.Visible = true;
                    phContent.Visible = false;
                }
            }
        }

        private List<ApplyHistory> clubApplicationList = null;
        private void BindMemberList()
        {
            if (clubApplicationList == null)
                clubApplicationList = ClubLogic.GetClubApplications(ClubID);

            gvClubMemberList.DataSource = clubApplicationList;
            gvClubMemberList.DataBind();
        }

        protected void gvClubMemberList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ApplyHistory ah = e.Row.DataItem as ApplyHistory;
                if (ah != null)
                {
                    UserInfo userInfo = AdminUsers.GetUserInfo(ah.Userid);
                    if (userInfo != null)
                    {
                        #region set avatar

                        Image imgAvatar = e.Row.FindControl("imgAvatar") as Image;

                        if (imgAvatar != null)
                        {
                            //if (userInfo.Avatar == string.Empty)
                            //{
                            //    imgAvatar.ImageUrl = "/avatars/common/0.gif";
                            //}
                            //else if (userInfo.Avatar.ToLower().IndexOf(@"/") == 0)
                            //{
                            //    imgAvatar.ImageUrl = userInfo.Avatar;
                            //}
                            //else if (userInfo.Avatar.ToLower().IndexOf("http") >= 0)
                            //{
                            //    imgAvatar.ImageUrl = userInfo.Avatar;
                            //}
                            //else
                            //{
                            //    imgAvatar.ImageUrl = string.Format("/{0}", userInfo.Avatar);
                            //}

                            string myAvatar = Avatars.GetAvatarUrl(ah.Userid, AvatarSize.Small);
                            imgAvatar.ImageUrl = myAvatar;

                            imgAvatar.AlternateText = userInfo.Username.Trim();
                        }

                        #endregion

                        #region set user group

                        Literal ltrlUserGroup = e.Row.FindControl("ltrlUserGroup") as Literal;
                        if (ltrlUserGroup != null)
                        {
                            UserGroupInfo groupInfo = UserGroups.GetUserGroupInfo(userInfo.Groupid);

                            if (groupInfo != null)
                                ltrlUserGroup.Text = groupInfo.Grouptitle;
                        }

                        #endregion

                        #region set user credits

                        Literal ltrlUserCredit = e.Row.FindControl("ltrlUserCredit") as Literal;
                        if (ltrlUserCredit != null)
                        {
                            ltrlUserCredit.Text = userInfo.Credits.ToString();
                        }

                        #endregion

                        #region set user fortune

                        Literal ltrlUserFortune = e.Row.FindControl("ltrlUserFortune") as Literal;
                        if (ltrlUserFortune != null)
                        {
                            ltrlUserFortune.Text = userInfo.Extcredits2.ToString();
                        }

                        #endregion

                        #region set user posts

                        Literal ltrlUserPosts = e.Row.FindControl("ltrlUserPosts") as Literal;
                        if (ltrlUserPosts != null)
                        {
                            ltrlUserPosts.Text = userInfo.Posts.ToString();
                        }

                        #endregion
                    }
                }
            }
        }

        protected void gvClubMemberList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvClubMemberList.PageIndex = e.NewPageIndex;

            BindMemberList();
        }

        #region ICallbackEventHandler Members

        public string GetCallbackResult()
        {
            if (applyHistoryID > 0)
            {
                ApplyHistory ah = ClubLogic.GetApplyHistory(applyHistoryID);
                if (ah != null && ah.IsAccepted == null)
                {
                    int count = ClubLogic.GetClubMemberCount(ClubID);
                    int quota = ClubLogic.GetClubMemberQuota(ClubID);

                    if (!approved)
                    {
                        UserClubLogic.ApproveJoinClub(ah.ID.Value, approved, this.username);
                        return "false";
                    }
                    else if (approved && count >= quota)
                        return string.Empty;

                    UserClubLogic.ApproveJoinClub(ah.ID.Value, approved, this.username);

                    //check if user joined clubs count has reached max count, if true, cancel all applications of this user
                    List<Club> myClubs = ClubLogic.GetActiveUserClubs(this.userid);
                    if (myClubs.Count >= ConfigGlobal.SingleUserMaxClubCount)
                    {
                        //cancel
                        List<ApplyHistory> applications = ClubLogic.GetActiveUserApplications(ah.Userid);

                        foreach (ApplyHistory apply in applications)
                        {
                            UserClubLogic.ApproveJoinClub(apply.ID.Value, false, ClubSysPrivateMessage.ClubSysAdminName);
                        }
                    }

                    return "true";
                }
                else
                    return string.Empty;
            }
            else
                return string.Empty;
        }

        int applyHistoryID = -1;
        bool approved = true;
        public void RaiseCallbackEvent(string eventArgument)
        {
            string[] args = eventArgument.Split(';');

            if (args.Length == 2)
            {
                int.TryParse(args[0], out applyHistoryID);
                bool.TryParse(args[1], out approved);
            }
        }

        #endregion
    }
}

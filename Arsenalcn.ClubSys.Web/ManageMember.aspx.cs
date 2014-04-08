using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using Arsenalcn.ClubSys.DataAccess;
using Arsenalcn.ClubSys.Entity;

using Discuz.Entity;
using Discuz.Forum;

namespace Arsenalcn.ClubSys.Web
{
    public partial class ManageMember : Common.BasePage, ICallbackEventHandler
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
            Club club = ClubLogic.GetClubInfo(ClubID);

            if (club != null && this.Title.IndexOf("{0}") >= 0)
                this.Title = string.Format(this.Title, club.FullName);

            #region SetControlProperty

            ctrlLeftPanel.UserID = this.userid;
            ctrlLeftPanel.UserName = this.username;
            ctrlLeftPanel.UserKey = this.userkey;

            ctrlFieldToolBar.UserID = this.userid;
            ctrlFieldToolBar.UserName = this.username;

            ctrlManageMenuTabBar.CurrentMenu = Arsenalcn.ClubSys.Web.Control.ManageClubMenuItem.ManageMember;
            ctrlManageMenuTabBar.UserID = this.userid;

            #endregion

            #region Callback Reference
            string callbackReference = Page.ClientScript.GetCallbackEventReference(this, "userID", "GetResult", "context");

            string callbackScript = string.Format("function KickMember(userID, context){{ {0} }};", callbackReference);

            Page.ClientScript.RegisterClientScriptBlock(typeof(string), "action", callbackScript, true);

            #endregion

            if (!IsPostBack)
            {
                LoadPageData();
            }
        }

        protected void LoadPageData()
        {
            #region Bind ddlGroup
            List<Club> list = ClubLogic.GetActiveClubs();
            if (list != null && list.Count > 0)
            {
                ddlClub.DataSource = list;
                ddlClub.DataTextField = "FullName";
                ddlClub.DataValueField = "ID";
                ddlClub.DataBind();

                ListItem item = new ListItem("--请选择球会--", string.Empty);
                ddlClub.Items.Insert(0, item);
            }
            else
                ddlClub.Visible = false;
            #endregion

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

        private void BindMemberList()
        {
            List<UserClub> clubMemberList;
            int currClubID = int.MinValue;

            if (!string.IsNullOrEmpty(ddlClub.SelectedValue) && int.TryParse(ddlClub.SelectedValue, out currClubID))
            {
                clubMemberList = ClubLogic.GetClubMembers(currClubID);
            }
            else
            {
                clubMemberList = ClubLogic.GetClubMembers(ClubID);
            }

            gvClubMemberList.DataSource = clubMemberList;
            gvClubMemberList.DataBind();
        }

        protected void gvClubMemberList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                UserClub uc = e.Row.DataItem as UserClub;
                if (uc != null)
                {
                    UserInfo userInfo = AdminUsers.GetUserInfo(uc.Userid.Value);
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

                            string myAvatar = Avatars.GetAvatarUrl(uc.Userid.Value, AvatarSize.Small);
                            imgAvatar.ImageUrl = myAvatar;

                            imgAvatar.AlternateText = userInfo.Username.Trim();
                        }

                        #endregion

                        #region set responsibility

                        Literal ltrlResponsibility = e.Row.FindControl("ltrlResponsibility") as Literal;
                        if (ltrlResponsibility != null)
                        {
                            if (uc.Responsibility.Value == (int)Responsibility.Member)
                                ltrlResponsibility.Text = string.Empty;
                            else
                                ltrlResponsibility.Text = ClubLogic.TranslateResponsibility(uc.Responsibility.Value);
                        }

                        #endregion

                        #region set user group

                        Literal ltrlUserGroup = e.Row.FindControl("ltrlUserGroup") as Literal;
                        if (ltrlUserGroup != null)
                        {
                            UserGroupInfo groupInfo = UserGroups.GetUserGroupInfo(userInfo.Groupid);

                            if (groupInfo != null)
                                ltrlUserGroup.Text = string.Format("<span title=\"积分:{0}\">{1}</span>", userInfo.Credits.ToString("N0"), groupInfo.Grouptitle);
                        }

                        #endregion

                        #region set user fortune

                        Literal ltrlUserFortune = e.Row.FindControl("ltrlUserFortune") as Literal;
                        if (ltrlUserFortune != null)
                        {
                            ltrlUserFortune.Text = userInfo.Extcredits2.ToString("N2");
                        }

                        #endregion

                        #region set user posts

                        Literal ltrlUserPosts = e.Row.FindControl("ltrlUserPosts") as Literal;
                        if (ltrlUserPosts != null)
                        {
                            ltrlUserPosts.Text = userInfo.Posts.ToString("N0");
                        }

                        #endregion

                        #region set user days

                        Literal ltrlDays = e.Row.FindControl("ltrlDays") as Literal;
                        if (ltrlDays != null)
                        {
                            ltrlDays.Text = ((int)((DateTime.Now - uc.JoinClubDate.Value).TotalDays)).ToString();
                        }

                        #endregion

                        #region contribute value

                        Literal ltrlContributeValue = e.Row.FindControl("ltrlContributeValue") as Literal;

                        try
                        {
                            ltrlContributeValue.Text = string.Format("<em>{0}</em>", FortuneContributeAlgorithm.CalcContributeFortune(userInfo, true).ToString());
                        }
                        catch { }

                        #endregion


                        //Literal ltrlButtonDisplay = e.Row.FindControl("ltrlButtonDisplay") as Literal;
                        //if (ltrlButtonDisplay != null && uc.Responsibility.Value == (int)Responsibility.Manager)
                        //{
                        //    ltrlButtonDisplay.Text = "none";
                        //}

                        LinkButton btnKick = e.Row.FindControl("btnKick") as LinkButton;
                        int currClubID = int.MinValue;

                        if (btnKick != null && int.TryParse(ddlClub.SelectedValue, out currClubID))
                        {
                            if (uc.Responsibility.Value != (int)Responsibility.Manager && ClubID.Equals(currClubID))
                                btnKick.OnClientClick = string.Format("KickButtonClicked({0});return false;", uc.Userid.Value.ToString());
                            else
                                btnKick.Visible = false;
                        }

                        HyperLink hlTransfer = e.Row.FindControl("hlTransfer") as HyperLink;
                        if (hlTransfer != null)
                        {
                            if (uc.Userid.Value != this.userid)
                                hlTransfer.NavigateUrl = string.Format("ManageExtcredit.aspx?clubID={0}&ToUID={1}", ClubID.ToString(), uc.Userid.Value.ToString());
                            else
                                hlTransfer.Visible = false;
                        }
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
            UserClub userClub = ClubLogic.GetActiveUserClub(this.userid, ClubID);

            if (userClub != null)
            {
                if (userClub.Responsibility == (int)Responsibility.Manager || userClub.Responsibility == (int)Responsibility.Executor)
                {
                    Club club = ClubLogic.GetClubInfo(ClubID);
                    if (club == null || club.ManagerUid == kickedUserID)
                    {
                        //return error
                        return string.Empty;
                    }
                    else
                    {
                        //kick user logic

                        UserClubLogic.LeaveClub(kickedUserID, ClubID, true, this.username);

                        return userClub.UserName;
                    }
                }
                else
                {
                    //return error
                    return string.Empty;
                }
            }
            else
            {
                //return error
                return string.Empty;
            }
        }

        int kickedUserID = -1;
        public void RaiseCallbackEvent(string eventArgument)
        {
            int userID = -1;

            int.TryParse(eventArgument, out userID);

            kickedUserID = userID;
        }

        #endregion

        protected void ddlClub_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvClubMemberList.PageIndex = 0;

            BindMemberList();
        }
    }
}

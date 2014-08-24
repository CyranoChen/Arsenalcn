using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using Arsenalcn.ClubSys.Service;
using Arsenalcn.ClubSys.Entity;

using Discuz.Entity;
using Discuz.Forum;

namespace Arsenalcn.ClubSys.Web
{
    public partial class ManageMember : Common.BasePage
    {
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

            if (!IsPostBack)
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
                    BindData();
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
                            BindData();
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
        }
        private int ClubID
        {
            get
            {
                int tmp;
                if (!string.IsNullOrEmpty(ddlClub.SelectedValue))
                {
                    return Convert.ToInt16(ddlClub.SelectedValue);
                }
                else if (int.TryParse(Request.QueryString["ClubID"], out tmp))
                {
                    return tmp;
                }
                else
                {
                    Response.Redirect("ClubPortal.aspx");

                    return -1;
                }
            }
        }

        private List<Club> CurrUserManagedClubs
        {
            get
            {
                List<Club> list = ClubLogic.GetUserManagedClubs(this.userid);

                if (list != null && list.Count > 0)
                {
                    return list;
                }
                else
                {
                    Response.Redirect("ClubPortal.aspx");

                    return null;
                }
            }
        }

        private void BindData()
        {
            List<UserClub> clubMemberList = ClubLogic.GetClubMembers(ClubID);

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

                        #region set User Info & Responsibility

                        Literal ltrlUserInfoResponsibility = e.Row.FindControl("ltrlUserInfoResponsibility") as Literal;
                        string _strUserInfo = string.Format("<a href=\"MyPlayerProfile.aspx?userID={0}\" target=\"_blank\">{1}</a>",
                            uc.Userid.ToString(), uc.UserName.Trim());

                        if (ltrlUserInfoResponsibility != null)
                        {
                            if (uc.Responsibility.HasValue && !uc.Responsibility.Value.Equals((int)Responsibility.Member))
                            {
                                ltrlUserInfoResponsibility.Text = string.Format("{0}<em>{1}</em>",
                                    _strUserInfo, ClubLogic.TranslateResponsibility(uc.Responsibility.Value));
                            }
                            else
                            {
                                ltrlUserInfoResponsibility.Text = _strUserInfo;
                            }
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
                        int _days = (int)((DateTime.Now - uc.JoinClubDate.Value).TotalDays);

                        if (ltrlDays != null)
                        {
                            ltrlDays.Text = string.Format("<em title=\"自{0}入会以来\">{1}天</em>",
                                uc.FromDate.ToString("yyyy-MM-dd"), _days.ToString());
                        }

                        #endregion

                        #region contribute value

                        Literal ltrlContributeValue = e.Row.FindControl("ltrlContributeValue") as Literal;

                        try
                        {
                            ltrlContributeValue.Text = string.Format("<em>{0}</em>",
                                FortuneContributeAlgorithm.CalcContributeFortune(userInfo, true).ToString("N2"));
                        }
                        catch { }

                        #endregion


                        //Literal ltrlButtonDisplay = e.Row.FindControl("ltrlButtonDisplay") as Literal;
                        //if (ltrlButtonDisplay != null && uc.Responsibility.Value == (int)Responsibility.Manager)
                        //{
                        //    ltrlButtonDisplay.Text = "none";
                        //}

                        LinkButton btnKick = e.Row.FindControl("btnKick") as LinkButton;

                        if (btnKick != null && ClubID > 0)
                        {
                            if (uc.Responsibility.Value != (int)Responsibility.Manager
                                && CurrUserManagedClubs.Exists(delegate(Club c) { return c.ID.Equals(ClubID); }))
                            { btnKick.CommandArgument = uc.Userid.ToString(); }
                            else
                            { btnKick.Visible = false; }
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

        protected void gvClubMemberList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "KickUser")
                {
                    int _kickUserID = Convert.ToInt32(e.CommandArgument.ToString());

                    Club club = ClubLogic.GetClubInfo(ClubID);
                    UserClub userClub = ClubLogic.GetActiveUserClub(this.userid, ClubID);

                    if (userClub != null && club != null)
                    {
                        if (userClub.Responsibility.HasValue &&
                            (userClub.Responsibility.Equals((int)Responsibility.Manager)
                            || userClub.Responsibility.Equals((int)Responsibility.Executor)))
                        {
                            if (club.ManagerUid == _kickUserID)
                            {
                                throw new Exception("您没有权限解约此会员");
                            }

                            //kick user logic
                            UserClubLogic.LeaveClub(_kickUserID, ClubID, true, this.username);

                            this.ClientScript.RegisterClientScriptBlock(typeof(string), "success", "alert('球会已与此会员成功解约');", true);

                            BindData();
                        }
                        else
                        {
                            throw new Exception("您没有权限解约此会员");
                        }
                    }
                    else
                    {
                        throw new Exception("该用户已不是该球会会员");
                    }
                }
            }
            catch (Exception ex)
            {
                this.ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}');", ex.Message.ToString()), true);
            }
        }

        protected void gvClubMemberList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvClubMemberList.PageIndex = e.NewPageIndex;

            BindData();
        }

        protected void ddlClub_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvClubMemberList.PageIndex = 0;

            BindData();
        }
    }
}

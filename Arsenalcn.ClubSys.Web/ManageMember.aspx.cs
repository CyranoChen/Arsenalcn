using System;
using System.Collections.Generic;
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
            var club = ClubLogic.GetClubInfo(ClubID);

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
                var list = ClubLogic.GetActiveClubs();
                if (list != null && list.Count > 0)
                {
                    ddlClub.DataSource = list;
                    ddlClub.DataTextField = "FullName";
                    ddlClub.DataValueField = "ID";
                    ddlClub.DataBind();

                    var item = new ListItem("--请选择球会--", string.Empty);
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
                    var userClub = ClubLogic.GetActiveUserClub(this.userid, ClubID);

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
                var list = ClubLogic.GetUserManagedClubs(this.userid);

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
            var clubMemberList = ClubLogic.GetClubMembers(ClubID);

            gvClubMemberList.DataSource = clubMemberList;
            gvClubMemberList.DataBind();
        }

        protected void gvClubMemberList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var uc = e.Row.DataItem as UserClub;
                if (uc != null)
                {
                    var userInfo = AdminUsers.GetUserInfo(uc.Userid.Value);
                    if (userInfo != null)
                    {
                        #region set avatar

                        var imgAvatar = e.Row.FindControl("imgAvatar") as Image;

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

                            var myAvatar = Avatars.GetAvatarUrl(uc.Userid.Value, AvatarSize.Small);
                            imgAvatar.ImageUrl = myAvatar;

                            imgAvatar.AlternateText = userInfo.Username.Trim();
                        }

                        #endregion

                        #region set User Info & Responsibility

                        var ltrlUserInfoResponsibility = e.Row.FindControl("ltrlUserInfoResponsibility") as Literal;
                        var _strUserInfo =
                            $"<a href=\"MyPlayerProfile.aspx?userID={uc.Userid.ToString()}\" target=\"_blank\">{uc.UserName.Trim()}</a>";

                        if (ltrlUserInfoResponsibility != null)
                        {
                            if (uc.Responsibility.HasValue && !uc.Responsibility.Value.Equals((int)Responsibility.Member))
                            {
                                ltrlUserInfoResponsibility.Text =
                                    $"{_strUserInfo}<em>{ClubLogic.TranslateResponsibility(uc.Responsibility.Value)}</em>";
                            }
                            else
                            {
                                ltrlUserInfoResponsibility.Text = _strUserInfo;
                            }
                        }

                        #endregion

                        #region set user group

                        var ltrlUserGroup = e.Row.FindControl("ltrlUserGroup") as Literal;
                        if (ltrlUserGroup != null)
                        {
                            var groupInfo = UserGroups.GetUserGroupInfo(userInfo.Groupid);

                            if (groupInfo != null)
                                ltrlUserGroup.Text =
                                    $"<span title=\"积分:{userInfo.Credits.ToString("N0")}\">{groupInfo.Grouptitle}</span>";
                        }

                        #endregion

                        #region set user fortune

                        var ltrlUserFortune = e.Row.FindControl("ltrlUserFortune") as Literal;
                        if (ltrlUserFortune != null)
                        {
                            ltrlUserFortune.Text = userInfo.Extcredits2.ToString("N2");
                        }

                        #endregion

                        #region set user posts

                        var ltrlUserPosts = e.Row.FindControl("ltrlUserPosts") as Literal;
                        if (ltrlUserPosts != null)
                        {
                            ltrlUserPosts.Text = userInfo.Posts.ToString("N0");
                        }

                        #endregion

                        #region set user days

                        var ltrlDays = e.Row.FindControl("ltrlDays") as Literal;
                        var _days = (int)((DateTime.Now - uc.JoinClubDate.Value).TotalDays);

                        if (ltrlDays != null)
                        {
                            ltrlDays.Text =
                                $"<em title=\"自{uc.FromDate.ToString("yyyy-MM-dd")}入会以来\">{_days.ToString()}天</em>";
                        }

                        #endregion

                        #region contribute value

                        var ltrlContributeValue = e.Row.FindControl("ltrlContributeValue") as Literal;

                        try
                        {
                            ltrlContributeValue.Text =
                                $"<em>{FortuneContributeAlgorithm.CalcContributeFortune(userInfo, true).ToString("N2")}</em>";
                        }
                        catch { }

                        #endregion


                        //Literal ltrlButtonDisplay = e.Row.FindControl("ltrlButtonDisplay") as Literal;
                        //if (ltrlButtonDisplay != null && uc.Responsibility.Value == (int)Responsibility.Manager)
                        //{
                        //    ltrlButtonDisplay.Text = "none";
                        //}

                        var btnKick = e.Row.FindControl("btnKick") as LinkButton;

                        if (btnKick != null && ClubID > 0)
                        {
                            if (uc.Responsibility.Value != (int)Responsibility.Manager
                                && CurrUserManagedClubs.Exists(delegate(Club c) { return c.ID.Equals(ClubID); }))
                            { btnKick.CommandArgument = uc.Userid.ToString(); }
                            else
                            { btnKick.Visible = false; }
                        }

                        var hlTransfer = e.Row.FindControl("hlTransfer") as HyperLink;

                        if (hlTransfer != null)
                        {
                            if (uc.Userid.Value != this.userid)
                                hlTransfer.NavigateUrl =
                                    $"ManageExtcredit.aspx?clubID={ClubID.ToString()}&ToUID={uc.Userid.Value.ToString()}";
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
                    var _kickUserID = Convert.ToInt32(e.CommandArgument.ToString());

                    var club = ClubLogic.GetClubInfo(ClubID);
                    var userClub = ClubLogic.GetActiveUserClub(this.userid, ClubID);

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
                this.ClientScript.RegisterClientScriptBlock(typeof(string), "failed",
                    $"alert('{ex.Message.ToString()}');", true);
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

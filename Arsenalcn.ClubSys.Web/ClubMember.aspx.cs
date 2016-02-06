using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Arsenalcn.ClubSys.Entity;
using Arsenalcn.ClubSys.Service;
using Arsenalcn.ClubSys.Web.Common;
using Arsenalcn.ClubSys.Web.Control;
using Discuz.Forum;

namespace Arsenalcn.ClubSys.Web
{
    public partial class ClubMember : BasePage
    {
        private long _totalContribution;

        private List<UserClub> clubMemberList;

        private int ClubID
        {
            get
            {
                int tmp;
                if (int.TryParse(Request.QueryString["ClubID"], out tmp))
                    return tmp;
                Response.Redirect("ClubPortal.aspx");

                return -1;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var club = ClubLogic.GetClubInfo(ClubID);

            if (club != null && Title.IndexOf("{0}") >= 0)
                Title = string.Format(Title, club.FullName);

            #region SetControlProperty

            ctrlLeftPanel.UserID = userid;
            ctrlLeftPanel.UserName = username;
            ctrlLeftPanel.UserKey = userkey;

            ctrlFieldToolBar.UserID = userid;
            ctrlFieldToolBar.UserName = username;

            ctrlMenuTabBar.CurrentMenu = ClubMenuItem.ClubMemeber;
            ctrlMenuTabBar.ClubID = ClubID;

            ctrlClubSysHeader.UserID = userid;
            ctrlClubSysHeader.ClubID = ClubID;
            ctrlClubSysHeader.UserName = username;

            #endregion

            BindMemberList();
        }

        private void BindMemberList()
        {
            if (clubMemberList == null)
                clubMemberList = ClubLogic.GetClubMembers(ClubID);

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
                    var userInfo = Users.GetUserInfo(uc.Userid.Value);
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

                        #region set responsibility

                        var ltrlResponsibility = e.Row.FindControl("ltrlResponsibility") as Literal;
                        if (ltrlResponsibility != null)
                        {
                            if (uc.Responsibility.Value == (int) Responsibility.Member)
                                ltrlResponsibility.Text = string.Empty;
                            else
                                ltrlResponsibility.Text =
                                    $"<em>({ClubLogic.TranslateResponsibility(uc.Responsibility.Value)})</em>";
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
                        if (ltrlDays != null)
                        {
                            ltrlDays.Text = ((int) ((DateTime.Now - uc.JoinClubDate.Value).TotalDays)).ToString();
                        }

                        #endregion

                        #region contribute value

                        var ltrlContributeValue = e.Row.FindControl("ltrlContributeValue") as Literal;

                        try
                        {
                            var contribution = FortuneContributeAlgorithm.CalcContributeFortune(userInfo, false);

                            var bonusRate = PlayerStrip.CalcPlayerContributionBonusRate(uc.Userid.Value);

                            if (bonusRate != 0)
                                ltrlContributeValue.Text = $"<em>{contribution}(*{1 + bonusRate}) 枪手币</em>";
                            else
                                ltrlContributeValue.Text = $"<em>{contribution} 枪手币</em>";

                            _totalContribution += (int) (contribution*(1 + bonusRate));
                        }
                        catch
                        {
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

        protected override void OnPreRender(EventArgs e)
        {
            ltrlMemberCount.Text = ClubLogic.GetClubMemberCount(ClubID).ToString();
            ltrlTotalContribution.Text = _totalContribution.ToString();

            base.OnPreRender(e);
        }
    }
}
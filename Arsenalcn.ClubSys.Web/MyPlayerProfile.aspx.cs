using System;
using System.Collections.Generic;

using Arsenalcn.ClubSys.Service;
using Arsenalcn.ClubSys.Entity;

using Discuz.Entity;
using Discuz.Forum;

namespace Arsenalcn.ClubSys.Web
{
    public partial class MyPlayerProfile : Common.BasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            AnonymousRedirect = true;
        }

        public int ProfileUserID
        {
            get
            {
                if (Request.QueryString["UserID"] != null)
                {
                    var profileUserID = -1;

                    if (int.TryParse(Request.QueryString["UserID"], out profileUserID))
                        return profileUserID;
                    else
                        return this.userid;
                }
                else
                    return this.userid;
            }
        }

        public string ProfileUserName
        {
            get
            {
                if (ProfileUserID == this.userid)
                    return this.username;
                else
                {
                    var sUser = AdminUsers.GetShortUserInfo(ProfileUserID);
                    return sUser.Username.Trim();
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

            ctrlPlayerHeader.UserID = this.userid;
            ctrlPlayerHeader.ProfileUserID = this.ProfileUserID;

            #endregion

            lblClubTip.Text = $"请点击下方的图片欣赏<em>{ProfileUserName}</em>的球员收藏。";

            btnVideoActive.ToolTip = $"点击进入{ProfileUserName}的集锦收藏";
            btnCardActive.ToolTip = $"点击进入{ProfileUserName}的卡片收藏";

            var queryStrUserID = string.Empty;

            if (ProfileUserID != this.userid)
                queryStrUserID = "&userid=" + ProfileUserID.ToString();

            //btnVideoActive.OnClientClick = string.Format("window.location.href='MyCollection.aspx?type=Video{0}';", queryStrUserID);
            //btnCardActive.OnClientClick = string.Format("window.location.href='MyCollection.aspx?type=Card{0}';", queryStrUserID);
            btnVideoActive.PostBackUrl = $"MyCollection.aspx?type=Video{queryStrUserID}";
            btnCardActive.PostBackUrl = $"MyCollection.aspx?type=Card{queryStrUserID}";

            //DataTable dtVideo = Service.UserVideo.GetUserVideo(ProfileUserID);
            lblVideoActiveCount.Text = Entity.UserVideo.GetUserVideosByUserID(ProfileUserID).Count.ToString();

            var items = PlayerStrip.GetMyNumbers(ProfileUserID);
            items.RemoveAll(delegate(Card un) { return un.ArsenalPlayerGuid.HasValue; });
            lblVideoCount.Text = items.Count.ToString();

            items = PlayerStrip.GetMyNumbers(ProfileUserID);
            items.RemoveAll(delegate(Card un) { return !un.IsActive; });
            lblCardActiveCount.Text = items.Count.ToString();

            items = PlayerStrip.GetMyNumbers(ProfileUserID);
            items.RemoveAll(delegate(Card un) { return un.IsActive || !un.ArsenalPlayerGuid.HasValue; });
            lblCardCount.Text = items.Count.ToString();
        }
    }
}

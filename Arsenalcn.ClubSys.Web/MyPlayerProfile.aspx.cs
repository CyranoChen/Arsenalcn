using System;
using Arsenalcn.ClubSys.Entity;
using Arsenalcn.ClubSys.Service;
using Arsenalcn.ClubSys.Web.Common;
using Discuz.Forum;
using UserVideo = Arsenalcn.ClubSys.Entity.UserVideo;

namespace Arsenalcn.ClubSys.Web
{
    public partial class MyPlayerProfile : BasePage
    {
        public int ProfileUserID
        {
            get
            {
                if (Request.QueryString["UserID"] != null)
                {
                    var profileUserID = -1;

                    if (int.TryParse(Request.QueryString["UserID"], out profileUserID))
                        return profileUserID;
                    return userid;
                }
                return userid;
            }
        }

        public string ProfileUserName
        {
            get
            {
                if (ProfileUserID == userid)
                    return username;
                var sUser = Users.GetShortUserInfo(ProfileUserID);
                return sUser.Username.Trim();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            AnonymousRedirect = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            #region SetControlProperty

            ctrlLeftPanel.UserID = userid;
            ctrlLeftPanel.UserName = username;
            ctrlLeftPanel.UserKey = userkey;

            ctrlFieldToolBar.UserID = userid;
            ctrlFieldToolBar.UserName = username;

            ctrlPlayerHeader.UserID = userid;
            ctrlPlayerHeader.ProfileUserID = ProfileUserID;

            #endregion

            lblClubTip.Text = $"请点击下方的图片欣赏<em>{ProfileUserName}</em>的球员收藏。";

            btnVideoActive.ToolTip = $"点击进入{ProfileUserName}的集锦收藏";
            btnCardActive.ToolTip = $"点击进入{ProfileUserName}的卡片收藏";

            var queryStrUserID = string.Empty;

            if (ProfileUserID != userid)
                queryStrUserID = "&userid=" + ProfileUserID;

            //btnVideoActive.OnClientClick = string.Format("window.location.href='MyCollection.aspx?type=Video{0}';", queryStrUserID);
            //btnCardActive.OnClientClick = string.Format("window.location.href='MyCollection.aspx?type=Card{0}';", queryStrUserID);
            btnVideoActive.PostBackUrl = $"MyCollection.aspx?type=Video{queryStrUserID}";
            btnCardActive.PostBackUrl = $"MyCollection.aspx?type=Card{queryStrUserID}";

            //DataTable dtVideo = Service.UserVideo.GetUserVideo(ProfileUserID);
            lblVideoActiveCount.Text = UserVideo.GetUserVideosByUserID(ProfileUserID).Count.ToString();

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
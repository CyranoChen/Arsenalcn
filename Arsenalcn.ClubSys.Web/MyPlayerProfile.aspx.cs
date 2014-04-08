using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

using Arsenalcn.ClubSys.DataAccess;
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
                    int profileUserID = -1;

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
                    ShortUserInfo sUser = AdminUsers.GetShortUserInfo(ProfileUserID);
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

            lblClubTip.Text = string.Format("请点击下方的图片欣赏<em>{0}</em>的球员收藏。", ProfileUserName);

            btnVideoActive.ToolTip = string.Format("点击进入{0}的集锦收藏", ProfileUserName);
            btnCardActive.ToolTip = string.Format("点击进入{0}的卡片收藏", ProfileUserName);

            string queryStrUserID = string.Empty;

            if (ProfileUserID != this.userid)
                queryStrUserID = "&userid=" + ProfileUserID.ToString();

            //btnVideoActive.OnClientClick = string.Format("window.location.href='MyCollection.aspx?type=Video{0}';", queryStrUserID);
            //btnCardActive.OnClientClick = string.Format("window.location.href='MyCollection.aspx?type=Card{0}';", queryStrUserID);
            btnVideoActive.PostBackUrl = string.Format("MyCollection.aspx?type=Video{0}",queryStrUserID);
            btnCardActive.PostBackUrl = string.Format("MyCollection.aspx?type=Card{0}", queryStrUserID);

            DataTable dtVideo = UserVideo.GetUserVideo(ProfileUserID);
            lblVideoActiveCount.Text = dtVideo.Rows.Count.ToString();

            List<UserNumber> items = PlayerStrip.GetMyNumbers(ProfileUserID);
            items.RemoveAll(delegate(UserNumber un) { return un.ArsenalPlayerGuid.HasValue; });
            lblVideoCount.Text = items.Count.ToString();

            items = PlayerStrip.GetMyNumbers(ProfileUserID);
            items.RemoveAll(delegate(UserNumber un) { return !un.IsActive; });
            lblCardActiveCount.Text = items.Count.ToString();

            items = PlayerStrip.GetMyNumbers(ProfileUserID);
            items.RemoveAll(delegate(UserNumber un) { return un.IsActive || !un.ArsenalPlayerGuid.HasValue; });
            lblCardCount.Text = items.Count.ToString();
        }
    }
}

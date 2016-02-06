using System;
using Arsenalcn.ClubSys.Web.Common;
using Arsenalcn.ClubSys.Web.Control;

namespace Arsenalcn.ClubSys.Web
{
    public partial class MyCollection : BasePage
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

            if (ProfileUserID != userid)
                ctrlTabBar.ProfileUserID = ProfileUserID;
            else
                ctrlTabBar.ProfileUserID = userid;

            #endregion

            CollectionBase collection = null;

            switch (ctrlTabBar.Current)
            {
                case CollectionTab.Card:
                    collection = ctrlCard;
                    break;
                case CollectionTab.InactiveCard:
                    collection = ctrlInvalidCard;
                    break;
                case CollectionTab.InactiveVideo:
                    collection = ctrlInvalidVideo;
                    break;
                case CollectionTab.Video:
                    collection = ctrlVideo;
                    break;
            }

            if (collection != null)
            {
                collection.Visible = true;
                collection.ProfileUserID = ProfileUserID;
                collection.CurrentUserID = userid;
            }
        }
    }
}
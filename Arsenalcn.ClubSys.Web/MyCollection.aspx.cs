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

using Arsenalcn.ClubSys.Service;
using Arsenalcn.ClubSys.Entity;

namespace Arsenalcn.ClubSys.Web
{
    public partial class MyCollection : Common.BasePage
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

            if (this.ProfileUserID != this.userid)
                ctrlTabBar.ProfileUserID = this.ProfileUserID;
            else
                ctrlTabBar.ProfileUserID = this.userid;

            #endregion

            Common.CollectionBase collection = null;

            switch (ctrlTabBar.Current)
            {
                case Arsenalcn.ClubSys.Web.Control.CollectionTab.Card:
                    collection = ctrlCard;
                    break;
                case Arsenalcn.ClubSys.Web.Control.CollectionTab.InactiveCard:
                    collection = ctrlInvalidCard;
                    break;
                case Arsenalcn.ClubSys.Web.Control.CollectionTab.InactiveVideo:
                    collection = ctrlInvalidVideo;
                    break;
                case Arsenalcn.ClubSys.Web.Control.CollectionTab.Video:
                    collection = ctrlVideo;
                    break;
            }

            if (collection != null)
            {
                collection.Visible = true;
                collection.ProfileUserID = ProfileUserID;
                collection.CurrentUserID = this.userid;
            }
        }
    }
}

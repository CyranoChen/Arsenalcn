using System;

namespace Arsenalcn.ClubSys.Web
{
    public partial class ClubCardFusion : Common.BasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            AnonymousRedirect = true;
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
            ctrlPlayerHeader.ProfileUserID = this.userid;

            #endregion

            //ctrlGoogleAdv.DisplayAdv = "none";
        }
    }
}

using System;
using Arsenalcn.ClubSys.Web.Common;

namespace Arsenalcn.ClubSys.Web
{
    public partial class ClubCardFusion : BasePage
    {
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
            ctrlPlayerHeader.ProfileUserID = userid;

            #endregion

            //ctrlGoogleAdv.DisplayAdv = "none";
        }
    }
}
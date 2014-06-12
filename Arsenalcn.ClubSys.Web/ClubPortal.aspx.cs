using System;


namespace Arsenalcn.ClubSys.Web
{
    public partial class ClubPortal : Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlLeftPanel.UserID = this.userid;
            ctrlLeftPanel.UserName = this.username;
            ctrlLeftPanel.UserKey = this.userkey;

            ctrlFieldToolBar.UserID = this.userid;
            ctrlFieldToolBar.UserName = this.username;
        }
    }
}

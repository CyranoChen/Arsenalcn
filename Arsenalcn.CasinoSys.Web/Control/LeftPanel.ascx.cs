using System;

using Discuz.Forum;

namespace Arsenalcn.CasinoSys.Web.Control
{
    public partial class LeftPanel : System.Web.UI.UserControl
    {
        public int UserID
        { get; set; }

        public string UserName
        { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (UserID <= 0)
            {
                pnlMyCasino.Visible = false;
            }
            else
            {   
                pnlMyCasino.Visible = true;

                ltrlCash.Text = new Entity.Gambler(UserID, null).Cash.ToString("N2");

                ltrlUserRP.Text = AdminUsers.GetUserExtCredits(UserID, 4).ToString();
            }

            #region HideCasinoSysNotice
            if (string.IsNullOrEmpty(Entity.ConfigGlobal.SysNotice))
            {
                pnlNotice.Visible = false;
            }
            #endregion

        }
    }
}
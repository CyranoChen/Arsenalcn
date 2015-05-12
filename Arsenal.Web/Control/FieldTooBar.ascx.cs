using System;

using Arsenal.Service;

namespace Arsenal.Web.Control
{
    public partial class FieldTooBar : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (UserID == -1)
                phAnonymous.Visible = true;
            else
                phAnonymous.Visible = false;

            if (ConfigGlobal.IsPluginAdmin(UserID))
                pnlFuncLink.Visible = true;
            else
                pnlFuncLink.Visible = false;
        }

        public int UserID
        { get; set; }
    }
}
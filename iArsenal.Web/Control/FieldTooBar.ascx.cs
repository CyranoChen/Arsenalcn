﻿using System;
using System.Web.UI;
using iArsenal.Service;

namespace iArsenal.Web.Control
{
    public partial class FieldTooBar : UserControl
    {
        public int UserID { get; set; }

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
    }
}
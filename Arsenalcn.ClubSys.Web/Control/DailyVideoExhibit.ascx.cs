using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using Arsenalcn.ClubSys.DataAccess;
using Arsenalcn.ClubSys.Entity;

namespace Arsenalcn.ClubSys.Web.Control
{
    public partial class DailyVideoExhibit : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((ConfigGlobal.DailyVideoActive != true) && (!string.IsNullOrEmpty(ConfigGlobal.DailyVideoGuid.ToString())))
            {
                pnlVideoExhibit.Visible = false;
                //btnSwfView.OnClientClick = "GenFrame('swf/ShowVideoRoom.swf?XMLURL=ServerXml.aspx%3FUserVideoID=" + VideoGuid.ToString() + "', '480', '300', true); return false";
            }
        }

        protected string VideoGuid
        {
            get
            {
                return ConfigGlobal.DailyVideoGuid.ToString();
            }
        }
    }
}
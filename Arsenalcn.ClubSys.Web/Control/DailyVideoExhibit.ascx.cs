using System;

using Arsenalcn.ClubSys.Entity;

namespace Arsenalcn.ClubSys.Web.Control
{
    public partial class DailyVideoExhibit : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (ConfigGlobal.DailyVideoActive)
                {
                    Guid guid = ConfigGlobal.DailyVideoGuid;

                    if (!guid.Equals(Guid.Empty))
                    {
                        ltrlVideo.Text = string.Format("<script type=\"text/javascript\">GenSwfObject('PlayerVideoActive', 'swf/PlayerVideoActive.swf?XMLURL=ServerXml.aspx%3FVideoGuid={0}&ShowEffect=true', '160', '200');</script>", guid.ToString());

                        btnSwfView.OnClientClick = string.Format("ShowVideoPreview('{0}'); return false", guid.ToString());

                        pnlVideoExhibit.Visible = true;
                    }
                    else
                    {
                        pnlVideoExhibit.Visible = false;
                    }
                    //btnSwfView.OnClientClick = "GenFlashFrame('swf/ShowVideoRoom.swf?XMLURL=ServerXml.aspx%3FUserVideoID=" + VideoGuid.ToString() + "', '480', '300', true); return false";
                }
                else
                {
                    pnlVideoExhibit.Visible = false;
                }
            }
            catch
            {
                pnlVideoExhibit.Visible = false;
            }
        }
    }
}
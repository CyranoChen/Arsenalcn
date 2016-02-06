using System;
using System.Web.UI;
using Arsenalcn.ClubSys.Entity;

namespace Arsenalcn.ClubSys.Web.Control
{
    public partial class DailyVideoExhibit : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (ConfigGlobal.DailyVideoActive)
                {
                    var guid = ConfigGlobal.DailyVideoGuid;

                    if (!guid.Equals(Guid.Empty))
                    {
                        ltrlVideo.Text =
                            $"<script type=\"text/javascript\">GenSwfObject('PlayerVideoActive', 'swf/PlayerVideoActive.swf?XMLURL=ServerXml.aspx%3FVideoGuid={guid}&ShowEffect=true', '160', '200');</script>";

                        btnSwfView.OnClientClick = $"ShowVideoPreview('{guid}'); return false";

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
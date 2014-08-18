using System;

using Arsenalcn.ClubSys.Entity;
using Arsenal.Entity;

namespace Arsenalcn.ClubSys.Web.Control
{
    public partial class DailyVideoExhibit : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (ClubSys.Entity.ConfigGlobal.DailyVideoActive)
                {
                    if (VideoGuid != null && !VideoGuid.Equals(Guid.Empty))
                    {
                        Video v = Video.Cache.Load(VideoGuid);

                        if (v.VideoType.Equals(VideoFileType.mp4))
                        {
                            string _strHtml = "<div class=\"SwfViewBtnLeft\" onclick=\"GenVideoFrame('{0}', '{1}', '{2}', true)\"></div>";

                            ltrlViewBtnLeft.Text = string.Format(_strHtml, v.VideoFilePath, v.VideoWidth.ToString(), v.VideoHeight.ToString());
                        }
                        else if (v.VideoType.Equals(VideoFileType.flv))
                        {
                            string _strHtml = "<div class=\"SwfViewBtnLeft\" onclick=\"GenFlashFrame('{0}', '{1}', '{2}', true)\"></div>";
                            string _swfUrl = string.Format("swf/ShowVideoRoom.swf?XMLURL=ServerXml.aspx%3FVideoGuid={0}", VideoGuid.ToString());

                            ltrlViewBtnLeft.Text = string.Format(_strHtml, _swfUrl, v.VideoWidth.ToString(), v.VideoHeight.ToString());
                        }
                        else
                        {
                            throw new Exception();
                        }

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

        protected Guid VideoGuid
        {
            get
            {
                return ClubSys.Entity.ConfigGlobal.DailyVideoGuid;
            }
        }
    }
}
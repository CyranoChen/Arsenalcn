using System;

using Arsenalcn.Core.Logger;

namespace iArsenal.Web
{
    public partial class AdminLogView : AdminPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = this.Username;

            if (!IsPostBack)
            {
                InitForm();
            }
        }

        private int LogID
        {
            get
            {
                int _logID;
                if (!string.IsNullOrEmpty(Request.QueryString["LogID"]) && int.TryParse(Request.QueryString["LogID"], out _logID))
                {
                    return _logID;
                }
                else
                    return int.MinValue;
            }
        }

        private void InitForm()
        {
            if (LogID > 0)
            {
                Log l = new Log();
                l.ID = LogID;
                l.Single();

                ltrlLogID.Text = string.Format("详细日志查 <em>({0})</em>", LogID.ToString());
                tbLogger.Text = l.Logger;
                tbCreateTime.Text = l.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                tbLevel.Text = l.Level.ToString();
                tbMessage.Text = l.Message;
                tbStackTrace.Text = l.StackTrace;
                tbThread.Text = l.Thread;
                tbMethod.Text = l.Method;
                tbUserID.Text = l.UserID.ToString();
                tbUserIP.Text = l.UserIP;
                tbUserBrowser.Text = l.UserBrowser;
                tbUserOS.Text = l.UserOS;
            }
            else
            {
                Response.Redirect("AdminLog.aspx");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (LogID > 0)
            {
                Response.Redirect("AdminLog.aspx?LogID=" + LogID.ToString());
            }
            else
            {
                Response.Redirect("AdminLog.aspx");
            }
        }
    }
}

using System;
using System.Linq;
using System.Web.UI.WebControls;

using Arsenal.Service;
using Arsenalcn.Core;
using Arsenalcn.Core.Logger;

namespace Arsenal.Web
{
    public partial class AdminLog : AdminPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = this.Username;
            ctrlCustomPagerInfo.PageChanged += new Control.CustomPagerInfo.PageChangedEventHandler(ctrlCustomPagerInfo_PageChanged);

            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(ddlLogger.SelectedValue))
                    ViewState["Logger"] = ddlLogger.SelectedValue;
                else
                    ViewState["Logger"] = string.Empty;

                BindData();
            }
        }

        private int _logID = int.MinValue;
        private int LogID
        {
            get
            {
                int _res;
                if (_logID == 0)
                    return _logID;
                else if (!string.IsNullOrEmpty(Request.QueryString["LogID"]) && int.TryParse(Request.QueryString["LogID"], out _res))
                    return _res;
                else
                    return int.MinValue;
            }
            set { _logID = value; }
        }

        private void BindData()
        {
            var list = Log.All().FindAll(x =>
                {
                    Boolean returnValue = true;
                    string tmpString = string.Empty;

                    if (ViewState["Logger"] != null)
                    {
                        tmpString = ViewState["Logger"].ToString();
                        if (!string.IsNullOrEmpty(tmpString))
                            returnValue = returnValue && x.Logger.ToString().Equals(tmpString, StringComparison.OrdinalIgnoreCase);
                    }

                    if (ViewState["Level"] != null)
                    {
                        tmpString = ViewState["Level"].ToString();
                        if (!string.IsNullOrEmpty(tmpString))
                            returnValue = returnValue && x.Level.ToString().Equals(tmpString, StringComparison.OrdinalIgnoreCase);
                    }

                    if (ViewState["Exception"] != null)
                    {
                        tmpString = ViewState["Exception"].ToString();
                        if (!string.IsNullOrEmpty(tmpString))
                            returnValue = returnValue && !string.IsNullOrEmpty(x.StackTrace).Equals(Convert.ToBoolean(tmpString));
                    }

                    if (ViewState["Method"] != null)
                    {
                        tmpString = ViewState["Method"].ToString();
                        if (!string.IsNullOrEmpty(tmpString) && tmpString != "--方法名称--")
                            returnValue = returnValue && x.Method.ToLower().Contains(tmpString.ToLower());
                    }

                    return returnValue;
                });

            #region set GridView Selected PageIndex
            if (LogID > 0)
            {
                int i = list.FindIndex(x => x.ID.Equals(LogID));
                if (i >= 0)
                {
                    gvLog.PageIndex = i / gvLog.PageSize;
                    gvLog.SelectedIndex = i % gvLog.PageSize;
                }
                else
                {
                    gvLog.PageIndex = 0;
                    gvLog.SelectedIndex = -1;
                }
            }
            else
            {
                gvLog.SelectedIndex = -1;
            }
            #endregion

            gvLog.DataSource = list;
            gvLog.DataBind();

            #region set Control Custom Pager
            if (gvLog.BottomPagerRow != null)
            {
                gvLog.BottomPagerRow.Visible = true;
                ctrlCustomPagerInfo.Visible = true;

                ctrlCustomPagerInfo.PageIndex = gvLog.PageIndex;
                ctrlCustomPagerInfo.PageCount = gvLog.PageCount;
                ctrlCustomPagerInfo.RowCount = list.Count;
                ctrlCustomPagerInfo.InitComponent();
            }
            else
            {
                ctrlCustomPagerInfo.Visible = false;
            }
            #endregion
        }

        protected void gvLog_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvLog.PageIndex = e.NewPageIndex;
            LogID = 0;

            BindData();
        }

        protected void ctrlCustomPagerInfo_PageChanged(object sender, Control.CustomPagerInfo.DataNavigatorEventArgs e)
        {
            if (e.PageIndex > 0)
            {
                gvLog.PageIndex = e.PageIndex;
                LogID = 0;
            }

            BindData();
        }

        protected void gvLog_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvLog.SelectedIndex != -1)
            {
                Response.Redirect(string.Format("AdminLogView.aspx?LogID={0}", gvLog.DataKeys[gvLog.SelectedIndex].Value.ToString()));
            }
        }

        protected void ddlLogger_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlLogger.SelectedValue))
                ViewState["Logger"] = ddlLogger.SelectedValue;
            else
                ViewState["Logger"] = string.Empty;

            LogID = 0;
            gvLog.PageIndex = 0;

            BindData();
        }

        protected void ddlLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlLevel.SelectedValue))
                ViewState["Level"] = ddlLevel.SelectedValue;
            else
                ViewState["Level"] = string.Empty;

            LogID = 0;
            gvLog.PageIndex = 0;

            BindData();
        }

        protected void ddlException_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlException.SelectedValue))
                ViewState["Exception"] = ddlException.SelectedValue;
            else
                ViewState["Exception"] = string.Empty;

            LogID = 0;
            gvLog.PageIndex = 0;

            BindData();
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbMethod.Text.Trim()))
                ViewState["Method"] = tbMethod.Text.Trim();
            else
                ViewState["Method"] = string.Empty;

            LogID = 0;
            gvLog.PageIndex = 0;

            BindData();
        }
    }
}
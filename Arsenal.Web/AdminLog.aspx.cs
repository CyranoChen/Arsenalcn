using System;
using System.Web.UI.WebControls;
using Arsenal.Web.Control;
using Arsenalcn.Core;
using Arsenalcn.Core.Logger;

namespace Arsenal.Web
{
    public partial class AdminLog : AdminPageBase
    {
        private readonly IRepository repo = new Repository();


        private int _logID = int.MinValue;

        private int LogID
        {
            get
            {
                int _res;
                if (_logID == 0)
                    return _logID;
                if (!string.IsNullOrEmpty(Request.QueryString["LogID"]) &&
                    int.TryParse(Request.QueryString["LogID"], out _res))
                    return _res;
                return int.MinValue;
            }
            set { _logID = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = Username;
            ctrlCustomPagerInfo.PageChanged += ctrlCustomPagerInfo_PageChanged;

            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(ddlException.SelectedValue))
                    ViewState["Exception"] = ddlException.SelectedValue;
                else
                    ViewState["Exception"] = string.Empty;

                BindData();
            }
        }

        private void BindData()
        {
            var list = repo.All<Log>().FindAll(x =>
            {
                var returnValue = true;
                var tmpString = string.Empty;

                if (ViewState["Logger"] != null)
                {
                    tmpString = ViewState["Logger"].ToString();
                    if (!string.IsNullOrEmpty(tmpString))
                        returnValue = returnValue &&
                                      x.Logger.ToString().Equals(tmpString, StringComparison.OrdinalIgnoreCase);
                }

                if (ViewState["Level"] != null)
                {
                    tmpString = ViewState["Level"].ToString();
                    if (!string.IsNullOrEmpty(tmpString))
                        returnValue = returnValue &&
                                      x.Level.ToString().Equals(tmpString, StringComparison.OrdinalIgnoreCase);
                }

                if (ViewState["Exception"] != null)
                {
                    tmpString = ViewState["Exception"].ToString();
                    if (!string.IsNullOrEmpty(tmpString))
                        returnValue = returnValue &&
                                      !string.IsNullOrEmpty(x.StackTrace).Equals(Convert.ToBoolean(tmpString));
                }

                if (ViewState["Method"] != null)
                {
                    tmpString = ViewState["Method"].ToString();
                    if (!string.IsNullOrEmpty(tmpString) && tmpString != "--方法名称--")
                        returnValue = returnValue && x.Method.ToLower().Contains(tmpString.ToLower());
                }

                if (ViewState["UserID"] != null)
                {
                    tmpString = ViewState["UserID"].ToString();
                    if (!string.IsNullOrEmpty(tmpString) && tmpString != "--ID--")
                        returnValue = returnValue && x.UserID.Equals(Convert.ToInt32(tmpString));
                }

                if (ViewState["UserIP"] != null)
                {
                    tmpString = ViewState["UserIP"].ToString();
                    if (!string.IsNullOrEmpty(tmpString) && tmpString != "--IP--")
                        returnValue = returnValue && x.UserIP.ToLower().Contains(tmpString.ToLower());
                }

                if (ViewState["UserBrowser"] != null)
                {
                    tmpString = ViewState["UserBrowser"].ToString();
                    if (!string.IsNullOrEmpty(tmpString) && tmpString != "--浏览器--")
                        returnValue = returnValue && x.UserBrowser.ToLower().Contains(tmpString.ToLower());
                }

                if (ViewState["UserOS"] != null)
                {
                    tmpString = ViewState["UserOS"].ToString();
                    if (!string.IsNullOrEmpty(tmpString) && tmpString != "--操作系统--")
                        returnValue = returnValue && x.UserOS.ToLower().Contains(tmpString.ToLower());
                }

                return returnValue;
            });

            if (ddlLogger.SelectedValue.Equals("UserLog"))
            {
                gvLog.Columns[5].Visible = false; // Method
                gvLog.Columns[6].Visible = true; // UserID
                gvLog.Columns[7].Visible = true; // UserIP
                gvLog.Columns[8].Visible = true; // UserBrowser
                gvLog.Columns[9].Visible = true; // UserOS
            }
            else
            {
                gvLog.Columns[5].Visible = true; // Method
                gvLog.Columns[6].Visible = false; // UserID
                gvLog.Columns[7].Visible = false; // UserIP
                gvLog.Columns[8].Visible = false; // UserBrowser
                gvLog.Columns[9].Visible = false; // UserOS
            }

            #region set GridView Selected PageIndex

            if (LogID > 0)
            {
                var i = list.FindIndex(x => x.ID.Equals(LogID));
                if (i >= 0)
                {
                    gvLog.PageIndex = i/gvLog.PageSize;
                    gvLog.SelectedIndex = i%gvLog.PageSize;
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

        protected void gvLog_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var l = e.Row.DataItem as Log;

                var ltrlException = e.Row.FindControl("ltrlException") as Literal;

                if (ltrlException != null && !string.IsNullOrEmpty(l.StackTrace))
                {
                    ltrlException.Text = string.Format("<em style=\"cursor: pointer\" title=\"{0}\">Exception</em>",
                        l.StackTrace);
                }
            }
        }

        protected void gvLog_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvLog.PageIndex = e.NewPageIndex;
            LogID = 0;

            BindData();
        }

        protected void ctrlCustomPagerInfo_PageChanged(object sender, CustomPagerInfo.DataNavigatorEventArgs e)
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
                Response.Redirect(string.Format("AdminLogView.aspx?LogID={0}", gvLog.DataKeys[gvLog.SelectedIndex].Value));
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

            if (!string.IsNullOrEmpty(tbUserID.Text.Trim()))
                ViewState["UserID"] = tbUserID.Text.Trim();
            else
                ViewState["UserID"] = string.Empty;

            if (!string.IsNullOrEmpty(tbUserIP.Text.Trim()))
                ViewState["UserIP"] = tbUserIP.Text.Trim();
            else
                ViewState["UserIP"] = string.Empty;

            if (!string.IsNullOrEmpty(tbUserBrowser.Text.Trim()))
                ViewState["UserBrowser"] = tbUserBrowser.Text.Trim();
            else
                ViewState["UserBrowser"] = string.Empty;

            if (!string.IsNullOrEmpty(tbUserOS.Text.Trim()))
                ViewState["UserOS"] = tbUserOS.Text.Trim();
            else
                ViewState["UserOS"] = string.Empty;

            LogID = 0;
            gvLog.PageIndex = 0;

            BindData();
        }
    }
}
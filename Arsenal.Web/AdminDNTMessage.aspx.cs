using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Discuz.Common;
using Discuz.Entity;
using Discuz.Forum;

namespace Arsenal.Web
{
    public partial class AdminDNTMessage : AdminPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = this.Username;
            ctrlCustomPagerInfo.PageChanged += new Control.CustomPagerInfo.PageChangedEventHandler(ctrlCustomPagerInfo_PageChanged);

            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            int _folder = 0;
            if (ViewState["Folder"] != null)
                _folder = Convert.ToInt16(ViewState["Folder"]);

            List<PrivateMessageInfo> list = PrivateMessages.GetPrivateMessageCollection(this.UID, _folder, int.MaxValue, 1, 2).FindAll(delegate(PrivateMessageInfo pm)
            {
                Boolean returnValue = true;
                string tmpString = string.Empty;

                if (ViewState["New"] != null)
                {
                    tmpString = ViewState["New"].ToString();
                    if (!string.IsNullOrEmpty(tmpString))
                        returnValue = returnValue && (pm.New == Convert.ToInt16(tmpString));
                }

                if (ViewState["Msgfrom"] != null)
                {
                    tmpString = ViewState["Msgfrom"].ToString();
                    if (!string.IsNullOrEmpty(tmpString) && tmpString != "--发送人--")
                        returnValue = returnValue && (pm.Msgfrom.IndexOf(tmpString) >= 0);
                }

                if (ViewState["Msgto"] != null)
                {
                    tmpString = ViewState["Msgto"].ToString();
                    if (!string.IsNullOrEmpty(tmpString) && tmpString != "--收件人--")
                        returnValue = returnValue && (pm.Msgto.ToLower().IndexOf(tmpString.ToLower()) >= 0);
                }

                if (ViewState["Subject"] != null)
                {
                    tmpString = ViewState["Subject"].ToString();
                    if (!string.IsNullOrEmpty(tmpString) && tmpString != "--标题--")
                        returnValue = returnValue && (pm.Subject.IndexOf(tmpString) >= 0);
                }

                return returnValue;
            });

            gvDNTMessage.DataSource = list;
            gvDNTMessage.DataBind();

            #region set Control Custom Pager
            if (gvDNTMessage.BottomPagerRow != null)
            {
                gvDNTMessage.BottomPagerRow.Visible = true;
                ctrlCustomPagerInfo.Visible = true;

                ctrlCustomPagerInfo.PageIndex = gvDNTMessage.PageIndex;
                ctrlCustomPagerInfo.PageCount = gvDNTMessage.PageCount;
                ctrlCustomPagerInfo.RowCount = list.Count;
                ctrlCustomPagerInfo.InitComponent();
            }
            else
            {
                ctrlCustomPagerInfo.Visible = false;
            }
            #endregion
        }

        protected void gvDNTMessage_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                PrivateMessageInfo pm = e.Row.DataItem as PrivateMessageInfo;

                Literal ltrlNew = e.Row.FindControl("ltrlNew") as Literal;
                Label lblPostdatetime = e.Row.FindControl("lblPostdatetime") as Label;
                HyperLink hlShowPM = e.Row.FindControl("hlShowPM") as HyperLink;
                
                if (pm.New == 1)
                    e.Row.CssClass = "SelectedRow";

                if (ltrlNew != null)
                {
                    if (pm.New == 1)
                        ltrlNew.Text = "<div class=\"ASC_New\" title=\"未读\"></div>";
                    else if (pm.New == 0)
                        ltrlNew.Text = "<div class=\"ASC_Old\" title=\"已读\"></div>";
                    else
                        ltrlNew.Text = string.Empty;
                }

                if (lblPostdatetime != null)
                    lblPostdatetime.Text = Convert.ToDateTime(pm.Postdatetime).ToString("yyyy-MM-dd HH:mm:ss");

                if (hlShowPM != null)
                    hlShowPM.NavigateUrl = string.Format("{0}usercpshowpm.aspx?pmid={1}", Request.ApplicationPath, pm.Pmid.ToString());
            }
        }

        protected void gvDNTMessage_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDNTMessage.PageIndex = e.NewPageIndex;

            BindData();
        }

        protected void ctrlCustomPagerInfo_PageChanged(object sender, Control.CustomPagerInfo.DataNavigatorEventArgs e)
        {
            if (e.PageIndex > 0)
            {
                gvDNTMessage.PageIndex = e.PageIndex;
            }

            BindData();
        }

        protected void ddlFolder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlFolder.SelectedValue))
                ViewState["Folder"] = ddlFolder.SelectedValue;
            else
                ViewState["Folder"] = "0";

            gvDNTMessage.PageIndex = 0;

            BindData();
        }

        protected void ddlNew_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlNew.SelectedValue))
                ViewState["New"] = ddlNew.SelectedValue;
            else
                ViewState["New"] = null;

            gvDNTMessage.PageIndex = 0;

            BindData();
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbMsgfrom.Text.Trim()))
                ViewState["Msgfrom"] = tbMsgfrom.Text.Trim();
            else
                ViewState["Msgfrom"] = string.Empty;

            if (!string.IsNullOrEmpty(tbMsgto.Text.Trim()))
                ViewState["Msgto"] = tbMsgto.Text.Trim();
            else
                ViewState["Msgto"] = string.Empty;

            if (!string.IsNullOrEmpty(tbSubject.Text.Trim()))
                ViewState["Subject"] = tbSubject.Text.Trim();
            else
                ViewState["Subject"] = string.Empty;

            gvDNTMessage.PageIndex = 0;

            BindData();
        }
    }
}

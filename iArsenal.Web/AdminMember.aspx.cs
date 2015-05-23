using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

using Arsenalcn.Core;
using iArsenal.Service;

namespace iArsenal.Web
{
    public partial class AdminMember : AdminPageBase
    {
        private readonly IRepository repo = new Repository();
        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = this.Username;
            ctrlCustomPagerInfo.PageChanged += new Control.CustomPagerInfo.PageChangedEventHandler(ctrlCustomPagerInfo_PageChanged);

            if (!IsPostBack)
            {
                BindData();
            }
        }

        private int _memberID = int.MinValue;
        private int MemberID
        {
            get
            {
                int _res;
                if (_memberID == 0)
                    return _memberID;
                else if (!string.IsNullOrEmpty(Request.QueryString["MemberID"]) && int.TryParse(Request.QueryString["MemberID"], out _res))
                    return _res;
                else
                    return int.MinValue;
            }
            set { _memberID = value; }
        }

        private void BindData()
        {
            var list = repo.All<Member>().FindAll(x =>
            {
                Boolean returnValue = true;
                string tmpString = string.Empty;

                if (ViewState["Name"] != null)
                {
                    tmpString = ViewState["Name"].ToString();
                    if (!string.IsNullOrEmpty(tmpString) && tmpString != "--会员姓名--")
                        returnValue = returnValue && x.Name.Contains(tmpString);
                }

                if (ViewState["AcnName"] != null)
                {
                    tmpString = ViewState["AcnName"].ToString();
                    if (!string.IsNullOrEmpty(tmpString) && tmpString != "--ACN会员名--")
                        returnValue = returnValue && x.AcnName.ToLower().Contains(tmpString.ToLower());
                }

                if (ViewState["Region"] != null)
                {
                    tmpString = ViewState["Region"].ToString();
                    if (!string.IsNullOrEmpty(tmpString) && tmpString != "--所在地区--")
                        returnValue = returnValue && x.RegionInfo.Contains(tmpString);
                }

                if (ViewState["Mobile"] != null)
                {
                    tmpString = ViewState["Mobile"].ToString();
                    if (!string.IsNullOrEmpty(tmpString) && tmpString != "--手机--")
                        returnValue = returnValue && x.Mobile.Equals(tmpString);
                }

                if (ViewState["MemberType"] != null)
                {
                    tmpString = ViewState["MemberType"].ToString();
                    if (!string.IsNullOrEmpty(tmpString) && tmpString != "0")
                        returnValue = returnValue && ((int)x.MemberType).ToString().Equals(tmpString);
                }

                return returnValue;
            });

            #region set GridView Selected PageIndex
            if (MemberID > 0)
            {
                int i = list.FindIndex(x => x.ID.Equals(MemberID));
                if (i >= 0)
                {
                    gvMember.PageIndex = i / gvMember.PageSize;
                    gvMember.SelectedIndex = i % gvMember.PageSize;
                }
                else
                {
                    gvMember.PageIndex = 0;
                    gvMember.SelectedIndex = -1;
                }
            }
            else
            {
                gvMember.SelectedIndex = -1;
            }
            #endregion

            gvMember.DataSource = list;
            gvMember.DataBind();

            #region set Control Custom Pager
            if (gvMember.BottomPagerRow != null)
            {
                gvMember.BottomPagerRow.Visible = true;
                ctrlCustomPagerInfo.Visible = true;

                ctrlCustomPagerInfo.PageIndex = gvMember.PageIndex;
                ctrlCustomPagerInfo.PageCount = gvMember.PageCount;
                ctrlCustomPagerInfo.RowCount = list.Count;
                ctrlCustomPagerInfo.InitComponent();
            }
            else
            {
                ctrlCustomPagerInfo.Visible = false;
            }
            #endregion
        }

        protected void gvMember_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMember.PageIndex = e.NewPageIndex;
            MemberID = 0;

            BindData();
        }

        protected void ctrlCustomPagerInfo_PageChanged(object sender, Control.CustomPagerInfo.DataNavigatorEventArgs e)
        {
            if (e.PageIndex > 0)
            {
                gvMember.PageIndex = e.PageIndex;
                MemberID = 0;
            }

            BindData();
        }

        protected void gvMember_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvMember.SelectedIndex != -1)
            {
                Response.Redirect(string.Format("AdminMemberView.aspx?MemberID={0}", gvMember.DataKeys[gvMember.SelectedIndex].Value.ToString()));
            }
        }

        protected void btnRefreshCache_Click(object sender, EventArgs e)
        {
            Member.Cache.RefreshCache();

            ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('更新缓存成功');window.location.href=window.location.href", true);
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbName.Text.Trim()))
                ViewState["Name"] = tbName.Text.Trim();
            else
                ViewState["Name"] = string.Empty;

            if (!string.IsNullOrEmpty(tbAcnName.Text.Trim()))
                ViewState["AcnName"] = tbAcnName.Text.Trim();
            else
                ViewState["AcnName"] = string.Empty;

            if (!string.IsNullOrEmpty(tbRegion.Text.Trim()))
                ViewState["Region"] = tbRegion.Text.Trim();
            else
                ViewState["Region"] = string.Empty;

            if (!string.IsNullOrEmpty(tbMobile.Text.Trim()))
                ViewState["Mobile"] = tbMobile.Text.Trim();
            else
                ViewState["Mobile"] = string.Empty;

            if (!string.IsNullOrEmpty(ddlMemberType.SelectedValue) && !ddlMemberType.SelectedValue.Equals("0"))
                ViewState["MemberType"] = ddlMemberType.SelectedValue;
            else
                ViewState["MemberType"] = string.Empty;

            MemberID = 0;
            gvMember.PageIndex = 0;

            BindData();
        }

        protected void gvMember_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Member m = e.Row.DataItem as Member;

                HyperLink hlName = e.Row.FindControl("hlName") as HyperLink;

                if (hlName != null)
                {
                    switch (m.Evalution)
                    {
                        case MemberEvalution.BlackList:
                            hlName.Text = string.Format("<em class=\"{1}\">{0}</em>",
                                m.Name, "asc_memberName_blackList");
                            break;
                        case MemberEvalution.WhiteList:
                            hlName.Text = string.Format("<em class=\"{1}\">{0}</em>",
                                m.Name, "asc_memberName_whiteList");
                            break;
                        default:
                            hlName.Text = string.Format("<em>{0}</em>", m.Name);
                            break;
                    }

                    hlName.NavigateUrl = string.Format("AdminOrder.aspx?MemberID={0}", m.ID);
                }

            }
        }
    }
}

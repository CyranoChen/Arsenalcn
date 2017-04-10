using System;
using System.Web.UI.WebControls;
using Arsenalcn.Core;
using Arsenalcn.Core.Dapper;
using iArsenal.Service;
using iArsenal.Web.Control;

namespace iArsenal.Web
{
    public partial class AdminMember : AdminPageBase
    {
        private readonly IRepository _repo = new Repository();

        private int _memberID = int.MinValue;

        private int MemberID
        {
            get
            {
                int res;
                if (_memberID == 0)
                    return _memberID;
                if (!string.IsNullOrEmpty(Request.QueryString["MemberID"]) &&
                    int.TryParse(Request.QueryString["MemberID"], out res))
                    return res;
                return int.MinValue;
            }
            set { _memberID = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = Username;
            ctrlCustomPagerInfo.PageChanged += ctrlCustomPagerInfo_PageChanged;

            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            var list = _repo.All<Member>().FindAll(x =>
            {
                var returnValue = true;
                string tmpString;

                if (ViewState["Name"] != null)
                {
                    tmpString = ViewState["Name"].ToString();
                    if (!string.IsNullOrEmpty(tmpString) && tmpString != "--会员姓名--")
                        returnValue = x.Name.Contains(tmpString);
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

                if (ViewState["Evalution"] != null)
                {
                    tmpString = ViewState["Evalution"].ToString();
                    if (!string.IsNullOrEmpty(tmpString) && tmpString != "0")
                        returnValue = returnValue && ((int)x.Evalution).ToString().Equals(tmpString);
                }

                if (ViewState["OfficialSync"] != null)
                {
                    tmpString = ViewState["OfficialSync"].ToString();
                    if (!string.IsNullOrEmpty(tmpString))
                        returnValue = returnValue && x.OfficialSync.Equals(tmpString);
                }

                return returnValue;
            });

            #region set GridView Selected PageIndex

            if (MemberID > 0)
            {
                var i = list.FindIndex(x => x.ID.Equals(MemberID));
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

        protected void ctrlCustomPagerInfo_PageChanged(object sender, CustomPagerInfo.DataNavigatorEventArgs e)
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
                var key = gvMember.DataKeys[gvMember.SelectedIndex];
                if (key != null)
                    Response.Redirect($"AdminMemberView.aspx?MemberID={key.Value}");
            }
        }

        protected void btnRefreshCache_Click(object sender, EventArgs e)
        {
            Member.Cache.RefreshCache();

            ClientScript.RegisterClientScriptBlock(typeof(string), "succeed",
                "alert('更新缓存成功');window.location.href=window.location.href", true);
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

            if (!string.IsNullOrEmpty(ddlMemberType.SelectedValue))
                ViewState["MemberType"] = ddlMemberType.SelectedValue;
            else
                ViewState["MemberType"] = string.Empty;

            if (!string.IsNullOrEmpty(ddlEvalution.SelectedValue))
                ViewState["Evalution"] = ddlEvalution.SelectedValue;
            else
                ViewState["Evalution"] = string.Empty;

            if (!string.IsNullOrEmpty(ddlOfficialSync.SelectedValue))
                ViewState["OfficialSync"] = ddlOfficialSync.SelectedValue;
            else
                ViewState["OfficialSync"] = string.Empty;

            MemberID = 0;
            gvMember.PageIndex = 0;

            BindData();
        }

        protected void gvMember_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var m = e.Row.DataItem as Member;

                var hlName = e.Row.FindControl("hlName") as HyperLink;

                if (hlName != null && m != null)
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
                            hlName.Text = $"<em>{m.Name}</em>";
                            break;
                    }

                    hlName.NavigateUrl = $"AdminOrder.aspx?MemberID={m.ID}";
                }
            }
        }
    }
}
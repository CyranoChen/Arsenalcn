using System;
using System.Web.UI.WebControls;
using Arsenal.Service;
using Arsenal.Web.Control;
using Arsenalcn.Core;
using Arsenalcn.Core.Dapper;

namespace Arsenal.Web
{
    public partial class AdminMembership : AdminPageBase
    {
        private readonly IRepository _repo = new Repository();

        private Guid? _userGuid;

        private Guid? UserGuid
        {
            get
            {
                if (_userGuid.HasValue && _userGuid == Guid.Empty)
                    return _userGuid;
                if (!string.IsNullOrEmpty(Request.QueryString["UserGuid"]))
                {
                    try
                    {
                        return new Guid(Request.QueryString["UserGuid"]);
                    }
                    catch
                    {
                        return Guid.Empty;
                    }
                }
                return null;
            }
            set { _userGuid = value; }
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
            var list = _repo.All<Membership>().FindAll(x =>
            {
                var returnValue = true;
                string tmpString;

                if (ViewState["UserName"] != null)
                {
                    tmpString = ViewState["UserName"].ToString();
                    if (!string.IsNullOrEmpty(tmpString) && tmpString != "--用户名--")
                        returnValue = x.UserName.ToLower().Contains(tmpString.ToLower());
                }

                if (ViewState["Mobile"] != null)
                {
                    tmpString = ViewState["Mobile"].ToString();
                    if (!string.IsNullOrEmpty(tmpString) && tmpString != "--手机--")
                        returnValue = x.Mobile.ToLower().Contains(tmpString.ToLower());
                }

                if (ViewState["Email"] != null)
                {
                    tmpString = ViewState["Email"].ToString();
                    if (!string.IsNullOrEmpty(tmpString) && tmpString != "--邮箱--")
                        returnValue = x.Email.ToLower().Contains(tmpString.ToLower());
                }

                return returnValue;
            });

            #region set GridView Selected PageIndex

            if (UserGuid.HasValue && !UserGuid.Value.Equals(Guid.Empty))
            {
                var i = list.FindIndex(x => x.ID.Equals(UserGuid));
                if (i >= 0)
                {
                    gvMembership.PageIndex = i / gvMembership.PageSize;
                    gvMembership.SelectedIndex = i % gvMembership.PageSize;
                }
                else
                {
                    gvMembership.PageIndex = 0;
                    gvMembership.SelectedIndex = -1;
                }
            }
            else
            {
                gvMembership.SelectedIndex = -1;
            }

            #endregion

            gvMembership.DataSource = list;
            gvMembership.DataBind();

            #region set Control Custom Pager

            if (gvMembership.BottomPagerRow != null)
            {
                gvMembership.BottomPagerRow.Visible = true;
                ctrlCustomPagerInfo.Visible = true;

                ctrlCustomPagerInfo.PageIndex = gvMembership.PageIndex;
                ctrlCustomPagerInfo.PageCount = gvMembership.PageCount;
                ctrlCustomPagerInfo.RowCount = list.Count;
                ctrlCustomPagerInfo.InitComponent();
            }
            else
            {
                ctrlCustomPagerInfo.Visible = false;
            }

            #endregion
        }

        protected void gvMembership_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMembership.PageIndex = e.NewPageIndex;
            UserGuid = null;

            BindData();
        }

        protected void ctrlCustomPagerInfo_PageChanged(object sender, CustomPagerInfo.DataNavigatorEventArgs e)
        {
            if (e.PageIndex > 0)
            {
                gvMembership.PageIndex = e.PageIndex;
                UserGuid = null;
            }

            BindData();
        }

        protected void gvMembership_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvMembership.SelectedIndex != -1)
            {
                var key = gvMembership.DataKeys[gvMembership.SelectedIndex];

                if (key != null)
                {
                    Response.Redirect($"AdminMembershipView.aspx?UserGuid={key.Value}");
                }
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbUserName.Text.Trim()))
                ViewState["UserName"] = tbUserName.Text.Trim();
            else
                ViewState["UserName"] = string.Empty;

            if (!string.IsNullOrEmpty(tbMobile.Text.Trim()))
                ViewState["Mobile"] = tbMobile.Text.Trim();
            else
                ViewState["Mobile"] = string.Empty;

            if (!string.IsNullOrEmpty(tbEmail.Text.Trim()))
                ViewState["Email"] = tbEmail.Text.Trim();
            else
                ViewState["Email"] = string.Empty;

            UserGuid = null;
            gvMembership.PageIndex = 0;

            BindData();
        }
    }
}
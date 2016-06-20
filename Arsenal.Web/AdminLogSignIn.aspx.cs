using System;
using System.Web.UI.WebControls;
using Arsenal.Service.Club;
using Arsenal.Web.Control;
using Arsenalcn.Core;

namespace Arsenal.Web
{
    public partial class AdminLogSignIn : AdminPageBase
    {
        private readonly IRepository _repo = new Repository();

        private int _logSignInID = int.MinValue;

        private int LogSignInID
        {
            get
            {
                int res;
                if (_logSignInID == 0)
                    return _logSignInID;
                if (!string.IsNullOrEmpty(Request.QueryString["LogSignInID"]) &&
                    int.TryParse(Request.QueryString["LogSignInID"], out res))
                    return res;
                return int.MinValue;
            }
            set { _logSignInID = value; }
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
            var list = _repo.All<LogSignIn>().FindAll(x =>
            {
                var returnValue = true;
                string tmpString;

                if (ViewState["UserName"] != null)
                {
                    tmpString = ViewState["UserName"].ToString();
                    if (!string.IsNullOrEmpty(tmpString) && tmpString != "--会员用户名--")
                        returnValue = x.UserName.ToLower().Contains(tmpString.ToLower());
                }

                return returnValue;
            });

            #region set GridView Selected PageIndex

            if (LogSignInID > 0)
            {
                var i = list.FindIndex(x => x.ID.Equals(LogSignInID));
                if (i >= 0)
                {
                    gvLogSignIn.PageIndex = i / gvLogSignIn.PageSize;
                    gvLogSignIn.SelectedIndex = i % gvLogSignIn.PageSize;
                }
                else
                {
                    gvLogSignIn.PageIndex = 0;
                    gvLogSignIn.SelectedIndex = -1;
                }
            }
            else
            {
                gvLogSignIn.SelectedIndex = -1;
            }

            #endregion

            gvLogSignIn.DataSource = list;
            gvLogSignIn.DataBind();

            #region set Control Custom Pager

            if (gvLogSignIn.BottomPagerRow != null)
            {
                gvLogSignIn.BottomPagerRow.Visible = true;
                ctrlCustomPagerInfo.Visible = true;

                ctrlCustomPagerInfo.PageIndex = gvLogSignIn.PageIndex;
                ctrlCustomPagerInfo.PageCount = gvLogSignIn.PageCount;
                ctrlCustomPagerInfo.RowCount = list.Count;
                ctrlCustomPagerInfo.InitComponent();
            }
            else
            {
                ctrlCustomPagerInfo.Visible = false;
            }

            #endregion
        }

        protected void gvLogSignIn_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvLogSignIn.PageIndex = e.NewPageIndex;
            LogSignInID = 0;

            BindData();
        }

        protected void ctrlCustomPagerInfo_PageChanged(object sender, CustomPagerInfo.DataNavigatorEventArgs e)
        {
            if (e.PageIndex > 0)
            {
                gvLogSignIn.PageIndex = e.PageIndex;
                LogSignInID = 0;
            }

            BindData();
        }

        protected void gvLogSignIn_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var key = (int)gvLogSignIn.DataKeys[e.RowIndex].Value;

            try
            {
                _repo.Delete<LogSignIn>(key);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", $"alert('{ex.Message}');", true);
            }

            BindData();
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbUserName.Text.Trim()))
                ViewState["UserName"] = tbUserName.Text.Trim();
            else
                ViewState["UserName"] = string.Empty;

            LogSignInID = 0;
            gvLogSignIn.PageIndex = 0;

            BindData();
        }
    }
}
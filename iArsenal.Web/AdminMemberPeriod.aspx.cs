using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using iArsenal.Entity;

namespace iArsenal.Web
{
    public partial class AdminMemberPeriod : PageBase.AdminPageBase
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

        private int _MemberPeriodID = int.MinValue;
        private int MemberPeriodID
        {
            get
            {
                int _res;
                if (_MemberPeriodID == 0)
                    return _MemberPeriodID;
                else if (!string.IsNullOrEmpty(Request.QueryString["MemberPeriodID"]) && int.TryParse(Request.QueryString["MemberPeriodID"], out _res))
                    return _res;
                else
                    return int.MinValue;
            }
            set { _MemberPeriodID = value; }
        }

        private void BindData()
        {
            List<MemberPeriod> list = MemberPeriod.GetMemberPeriods().FindAll(delegate(MemberPeriod mp)
            {
                Boolean returnValue = true;
                string tmpString = string.Empty;

                if (ViewState["MemberID"] != null)
                {
                    tmpString = ViewState["MemberID"].ToString();
                    if (!string.IsNullOrEmpty(tmpString) && tmpString != "--会员编号--")
                        returnValue = returnValue && mp.MemberID.Equals(Convert.ToInt32(tmpString));
                }

                if (ViewState["MemberName"] != null)
                {
                    tmpString = ViewState["MemberName"].ToString();
                    if (!string.IsNullOrEmpty(tmpString) && tmpString != "--会员姓名--")
                        returnValue = returnValue && mp.MemberName.Contains(tmpString);
                }

                if (ViewState["MemberCardNo"] != null)
                {
                    tmpString = ViewState["MemberCardNo"].ToString();
                    if (!string.IsNullOrEmpty(tmpString) && tmpString != "--会员卡号--")
                        returnValue = returnValue && mp.MemberCardNo.ToLower().Contains(tmpString.ToLower());
                }

                return returnValue;
            });

            #region set GridView Selected PageIndex
            if (MemberPeriodID > 0)
            {
                int i = list.FindIndex(delegate(MemberPeriod m) { return m.MemberPeriodID == MemberPeriodID; });
                if (i >= 0)
                {
                    gvMemberPeriod.PageIndex = i / gvMemberPeriod.PageSize;
                    gvMemberPeriod.SelectedIndex = i % gvMemberPeriod.PageSize;
                }
                else
                {
                    gvMemberPeriod.PageIndex = 0;
                    gvMemberPeriod.SelectedIndex = -1;
                }
            }
            else
            {
                gvMemberPeriod.SelectedIndex = -1;
            }
            #endregion

            gvMemberPeriod.DataSource = list;
            gvMemberPeriod.DataBind();

            #region set Control Custom Pager
            if (gvMemberPeriod.BottomPagerRow != null)
            {
                gvMemberPeriod.BottomPagerRow.Visible = true;
                ctrlCustomPagerInfo.Visible = true;

                ctrlCustomPagerInfo.PageIndex = gvMemberPeriod.PageIndex;
                ctrlCustomPagerInfo.PageCount = gvMemberPeriod.PageCount;
                ctrlCustomPagerInfo.RowCount = list.Count;
                ctrlCustomPagerInfo.InitComponent();
            }
            else
            {
                ctrlCustomPagerInfo.Visible = false;
            }
            #endregion
        }

        protected void gvMemberPeriod_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMemberPeriod.PageIndex = e.NewPageIndex;
            MemberPeriodID = 0;

            BindData();
        }

        protected void ctrlCustomPagerInfo_PageChanged(object sender, Control.CustomPagerInfo.DataNavigatorEventArgs e)
        {
            if (e.PageIndex > 0)
            {
                gvMemberPeriod.PageIndex = e.PageIndex;
                MemberPeriodID = 0;
            }

            BindData();
        }

        protected void gvMemberPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvMemberPeriod.SelectedIndex != -1)
            {
                Response.Redirect(string.Format("AdminMemberPeriodView.aspx?MemberPeriodID={0}", gvMemberPeriod.DataKeys[gvMemberPeriod.SelectedIndex].Value.ToString()));
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbMemberID.Text.Trim()))
                ViewState["MemberID"] = tbMemberID.Text.Trim();
            else
                ViewState["MemberID"] = string.Empty;

            if (!string.IsNullOrEmpty(tbMemberName.Text.Trim()))
                ViewState["MemberName"] = tbMemberName.Text.Trim();
            else
                ViewState["MemberName"] = string.Empty;

            if (!string.IsNullOrEmpty(tbMemberCardNo.Text.Trim()))
                ViewState["MemberCardNo"] = tbMemberCardNo.Text.Trim();
            else
                ViewState["MemberCardNo"] = string.Empty;

            MemberPeriodID = 0;
            gvMemberPeriod.PageIndex = 0;

            BindData();
        }
    }
}

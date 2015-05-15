using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

using iArsenal.Service;
using Arsenalcn.Core;

namespace iArsenal.Web
{
    public partial class AdminProduct : AdminPageBase
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

        private Guid? _productGuid = null;
        private Guid? ProductGuid
        {
            get
            {
                if (_productGuid.HasValue && _productGuid == Guid.Empty)
                    return _productGuid;
                else if (!string.IsNullOrEmpty(Request.QueryString["ProductGuid"]))
                {
                    try { return new Guid(Request.QueryString["ProductGuid"]); }
                    catch { return Guid.Empty; }
                }
                else
                    return null;
            }
            set { _productGuid = value; }
        }

        private void BindData()
        {
            var list = repo.All<Product>().ToList().FindAll(x =>
            {
                Boolean returnValue = true;
                string tmpString = string.Empty;

                if (ViewState["Code"] != null)
                {
                    tmpString = ViewState["Code"].ToString();
                    if (!string.IsNullOrEmpty(tmpString) && tmpString != "--编码--")
                        returnValue = returnValue && x.Code.ToLower().Contains(tmpString.ToLower());
                }

                if (ViewState["Name"] != null)
                {
                    tmpString = ViewState["Name"].ToString();
                    if (!string.IsNullOrEmpty(tmpString) && tmpString != "--名称--")
                        returnValue = returnValue && x.Name.ToLower().Contains(tmpString.ToLower());
                }

                if (ViewState["DisplayName"] != null)
                {
                    tmpString = ViewState["DisplayName"].ToString();
                    if (!string.IsNullOrEmpty(tmpString) && tmpString != "--译名--")
                        returnValue = returnValue && x.DisplayName.Contains(tmpString);
                }

                if (ViewState["ProductType"] != null)
                {
                    tmpString = ViewState["ProductType"].ToString();
                    if (!string.IsNullOrEmpty(tmpString))
                        returnValue = returnValue && ((int)x.ProductType).Equals(Convert.ToInt32(tmpString));
                }

                if (ViewState["IsActive"] != null)
                {
                    tmpString = ViewState["IsActive"].ToString();
                    if (!string.IsNullOrEmpty(tmpString))
                        returnValue = returnValue && x.IsActive.Equals(Convert.ToBoolean(tmpString));
                }

                return returnValue;
            });

            #region set GridView Selected PageIndex
            if (ProductGuid.HasValue && ProductGuid != Guid.Empty)
            {
                int i = list.FindIndex(x => x.ID.Equals(ProductGuid));
                if (i >= 0)
                {
                    gvProduct.PageIndex = i / gvProduct.PageSize;
                    gvProduct.SelectedIndex = i % gvProduct.PageSize;
                }
                else
                {
                    gvProduct.PageIndex = 0;
                    gvProduct.SelectedIndex = -1;
                }
            }
            else
            {
                gvProduct.SelectedIndex = -1;
            }
            #endregion

            gvProduct.DataSource = list;
            gvProduct.DataBind();

            #region set Control Custom Pager
            if (gvProduct.BottomPagerRow != null)
            {
                gvProduct.BottomPagerRow.Visible = true;
                ctrlCustomPagerInfo.Visible = true;

                ctrlCustomPagerInfo.PageIndex = gvProduct.PageIndex;
                ctrlCustomPagerInfo.PageCount = gvProduct.PageCount;
                ctrlCustomPagerInfo.RowCount = list.Count;
                ctrlCustomPagerInfo.InitComponent();
            }
            else
            {
                ctrlCustomPagerInfo.Visible = false;
            }
            #endregion
        }

        protected void gvProduct_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProduct.PageIndex = e.NewPageIndex;
            ProductGuid = Guid.Empty;

            BindData();
        }

        protected void ctrlCustomPagerInfo_PageChanged(object sender, Control.CustomPagerInfo.DataNavigatorEventArgs e)
        {
            if (e.PageIndex > 0)
            {
                gvProduct.PageIndex = e.PageIndex;
                ProductGuid = Guid.Empty;
            }

            BindData();
        }

        protected void gvProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvProduct.SelectedIndex != -1)
            {
                Response.Redirect(string.Format("AdminProductView.aspx?ProductGuid={0}", gvProduct.DataKeys[gvProduct.SelectedIndex].Value.ToString()));
            }
        }

        protected void btnRefreshCache_Click(object sender, EventArgs e)
        {
            Product.Cache.RefreshCache();

            ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('更新缓存成功');window.location.href=window.location.href", true);
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbCode.Text.Trim()))
                ViewState["Code"] = tbCode.Text.Trim();
            else
                ViewState["Code"] = string.Empty;

            if (!string.IsNullOrEmpty(tbName.Text.Trim()))
                ViewState["Name"] = tbName.Text.Trim();
            else
                ViewState["Name"] = string.Empty;

            if (!string.IsNullOrEmpty(tbDisplayName.Text.Trim()))
                ViewState["DisplayName"] = tbDisplayName.Text.Trim();
            else
                ViewState["DisplayName"] = string.Empty;

            if (!string.IsNullOrEmpty(ddlProductType.SelectedValue))
                ViewState["ProductType"] = ddlProductType.SelectedValue;
            else
                ViewState["ProductType"] = string.Empty;

            if (!string.IsNullOrEmpty(ddlIsActive.SelectedValue))
                ViewState["IsActive"] = ddlIsActive.SelectedValue;
            else
                ViewState["IsActive"] = string.Empty;

            ProductGuid = Guid.Empty;
            gvProduct.PageIndex = 0;

            BindData();
        }
    }
}

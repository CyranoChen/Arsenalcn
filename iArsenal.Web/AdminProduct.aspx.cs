using System;
using System.Web.UI.WebControls;
using Arsenalcn.Core;
using iArsenal.Service;
using iArsenal.Web.Control;

namespace iArsenal.Web
{
    public partial class AdminProduct : AdminPageBase
    {
        private readonly IRepository _repo = new Repository();

        private Guid? _productGuid;

        private Guid? ProductGuid
        {
            get
            {
                if (_productGuid.HasValue && _productGuid == Guid.Empty)
                    return _productGuid;
                if (!string.IsNullOrEmpty(Request.QueryString["ProductGuid"]))
                {
                    try
                    {
                        return new Guid(Request.QueryString["ProductGuid"]);
                    }
                    catch
                    {
                        return Guid.Empty;
                    }
                }
                return null;
            }
            set { _productGuid = value; }
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
            var list = _repo.All<Product>().FindAll(x =>
            {
                var returnValue = true;
                string tmpString;

                if (ViewState["Code"] != null)
                {
                    tmpString = ViewState["Code"].ToString();
                    if (!string.IsNullOrEmpty(tmpString) && tmpString != "--编码--")
                        returnValue = x.Code.ToLower().Contains(tmpString.ToLower());
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
                var i = list.FindIndex(x => x.ID.Equals(ProductGuid));
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

        protected void ctrlCustomPagerInfo_PageChanged(object sender, CustomPagerInfo.DataNavigatorEventArgs e)
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
                var key = gvProduct.DataKeys[gvProduct.SelectedIndex];
                if (key != null)
                { Response.Redirect($"AdminProductView.aspx?ProductGuid={key.Value}"); }
            }
        }

        protected void gvProduct_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var p = e.Row.DataItem as Product;

                var btnShowcase = e.Row.FindControl("btnShowcase") as LinkButton;

                if (p != null && btnShowcase != null)
                {
                    btnShowcase.Visible = p.ProductType == ProductType.Other && p.IsActive;

                    if (_repo.Any<Showcase>(x => x.ProductGuid == p.ID))
                    {
                        btnShowcase.Text = "已发布";
                        btnShowcase.Enabled = false;
                        btnShowcase.OnClientClick = "return false";
                    }

                    btnShowcase.CommandArgument = p.ID.ToString();
                }
            }
        }

        protected void gvProduct_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Showcase")
                {
                    var key = new Guid(e.CommandArgument.ToString());

                    var s = _repo.Single<Product>(key);

                    s?.Showcase();

                    ClientScript.RegisterClientScriptBlock(typeof(string), "success", "alert('加入橱窗成功');", true);

                    BindData();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", $"alert('{ex.Message}');", true);
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
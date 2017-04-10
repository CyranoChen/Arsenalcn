using System;
using System.Web.UI.WebControls;
using Arsenalcn.Core;
using Arsenalcn.Core.Dapper;
using iArsenal.Service;
using iArsenal.Web.Control;

namespace iArsenal.Web
{
    public partial class AdminShowcase : AdminPageBase
    {
        private readonly IRepository _repo = new Repository();

        private int _showcaseID = int.MinValue;

        private int ShowcaseID
        {
            get
            {
                int id;
                if (_showcaseID == 0)
                    return _showcaseID;
                if (!string.IsNullOrEmpty(Request.QueryString["ShowcaseID"]) &&
                    int.TryParse(Request.QueryString["ShowcaseID"], out id))
                    return id;
                return int.MinValue;
            }
            set { _showcaseID = value; }
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
            var list = _repo.All<Showcase>().FindAll(x =>
            {
                var returnValue = true;
                string tmpString;

                if (ViewState["Code"] != null)
                {
                    tmpString = ViewState["Code"].ToString();
                    if (!string.IsNullOrEmpty(tmpString) && tmpString != "--编码--")
                        returnValue = x.ProductCode.ToLower().Contains(tmpString.ToLower());
                }

                if (ViewState["Name"] != null)
                {
                    tmpString = ViewState["Name"].ToString();
                    if (!string.IsNullOrEmpty(tmpString) && tmpString != "--名称--")
                        returnValue = returnValue && Product.Cache.Load(x.ProductGuid).Name.ToLower().Contains(tmpString.ToLower());
                }

                if (ViewState["DisplayName"] != null)
                {
                    tmpString = ViewState["DisplayName"].ToString();
                    if (!string.IsNullOrEmpty(tmpString) && tmpString != "--译名--")
                        returnValue = returnValue && Product.Cache.Load(x.ProductGuid).DisplayName.Contains(tmpString);
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

            if (ShowcaseID > 0)
            {
                var i = list.FindIndex(x => x.ID.Equals(ShowcaseID));
                if (i >= 0)
                {
                    gvShowcase.PageIndex = i / gvShowcase.PageSize;
                    gvShowcase.SelectedIndex = i % gvShowcase.PageSize;
                }
                else
                {
                    gvShowcase.PageIndex = 0;
                    gvShowcase.SelectedIndex = -1;
                }
            }
            else
            {
                gvShowcase.SelectedIndex = -1;
            }

            #endregion

            gvShowcase.DataSource = list;
            gvShowcase.DataBind();

            #region set Control Custom Pager

            if (gvShowcase.BottomPagerRow != null)
            {
                gvShowcase.BottomPagerRow.Visible = true;
                ctrlCustomPagerInfo.Visible = true;

                ctrlCustomPagerInfo.PageIndex = gvShowcase.PageIndex;
                ctrlCustomPagerInfo.PageCount = gvShowcase.PageCount;
                ctrlCustomPagerInfo.RowCount = list.Count;
                ctrlCustomPagerInfo.InitComponent();
            }
            else
            {
                ctrlCustomPagerInfo.Visible = false;
            }

            #endregion
        }

        protected void gvShowcase_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvShowcase.PageIndex = e.NewPageIndex;
            ShowcaseID = 0;

            BindData();
        }

        protected void ctrlCustomPagerInfo_PageChanged(object sender, CustomPagerInfo.DataNavigatorEventArgs e)
        {
            if (e.PageIndex > 0)
            {
                gvShowcase.PageIndex = e.PageIndex;
                ShowcaseID = 0;
            }

            BindData();
        }

        protected void gvShowcase_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var s = e.Row.DataItem as Showcase;

                var ltrlProductName = e.Row.FindControl("ltrlProductName") as Literal;
                var ltrlProductPrice = e.Row.FindControl("ltrlProductPrice") as Literal;

                if (s != null && ltrlProductName != null && ltrlProductPrice != null)
                {
                    var p = Product.Cache.Load(s.ProductGuid);

                    if (p != null)
                    {
                        ltrlProductName.Text = $"{p.Name} ({p.DisplayName})";
                        ltrlProductPrice.Text = p.Sale.HasValue ?
                            $"<em>{p.SaleInfo}</em>(<span style=\"text-decoration:line-through\">{p.PriceInfo}</span>)"
                            : $"<em>{p.PriceInfo}</em>";
                    }
                }
            }
        }

        protected void gvShowcase_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            //var tbOrderNum = gvShowcase.Rows[gvShowcase.EditIndex].FindControl("tbOrderNum") as TextBox;
            //var tbIsActive = gvShowcase.Rows[gvShowcase.EditIndex].FindControl("tbIsActive") as TextBox;

            var tbOrderNum = gvShowcase.Rows[gvShowcase.EditIndex].Cells[1].Controls[0] as TextBox;
            var tbIsActive = gvShowcase.Rows[gvShowcase.EditIndex].Cells[5].Controls[0] as TextBox;

            var key = gvShowcase.DataKeys[gvShowcase.EditIndex];

            if (tbOrderNum != null && tbIsActive != null && key != null)
            {
                try
                {
                    var s = _repo.Single<Showcase>(key.Value);

                    int num;
                    if (int.TryParse(tbOrderNum.Text.Trim(), out num))
                    {
                        s.OrderNum = num;
                    }

                    bool active;
                    if (bool.TryParse(tbIsActive.Text.Trim(), out active))
                    {
                        s.IsActive = active;
                    }

                    _repo.Update(s);

                }
                catch (Exception ex)
                {
                    ClientScript.RegisterClientScriptBlock(typeof(string), "failed", $"alert('{ex.Message}');", true);
                }
            }

            gvShowcase.EditIndex = -1;

            BindData();
        }

        protected void gvShowcase_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var key = gvShowcase.DataKeys[e.RowIndex];

            if (key != null)
            {
                try
                {
                    _repo.Delete<Showcase>(key.Value);
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterClientScriptBlock(typeof(string), "failed", $"alert('{ex.Message}');", true);
                }
            }

            BindData();
        }

        protected void gvShowcase_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvShowcase.EditIndex = e.NewEditIndex;

            BindData();
        }

        protected void gvShowcase_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvShowcase.EditIndex = -1;

            BindData();
        }


        protected void btnRefreshCache_Click(object sender, EventArgs e)
        {
            Product.Cache.RefreshCache();
            Showcase.Cache.RefreshCache();

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

            if (!string.IsNullOrEmpty(ddlIsActive.SelectedValue))
                ViewState["IsActive"] = ddlIsActive.SelectedValue;
            else
                ViewState["IsActive"] = string.Empty;

            ShowcaseID = 0;
            gvShowcase.PageIndex = 0;

            BindData();
        }
    }
}
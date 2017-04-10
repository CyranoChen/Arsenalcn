using System;
using System.Linq;
using System.Web.UI.WebControls;
using Arsenalcn.Core;
using Arsenalcn.Core.Dapper;
using iArsenal.Service;
using iArsenal.Web.Control;

namespace iArsenal.Web
{
    public partial class AdminProductView : AdminPageBase
    {
        private readonly IRepository _repo = new Repository();

        private Guid ProductGuid
        {
            get
            {
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
                return Guid.Empty;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = Username;
            ctrlCustomPagerInfo.PageChanged += ctrlCustomPagerInfo_PageChanged;

            if (!IsPostBack)
            {
                InitForm();
            }
        }

        private void InitForm()
        {
            if (ProductGuid != Guid.Empty)
            {
                var p = _repo.Single<Product>(ProductGuid);

                tbProductGuid.Text = ProductGuid.ToString();
                tbCode.Text = p.Code;
                tbName.Text = p.Name;
                tbDisplayName.Text = p.DisplayName;
                ddlProductType.SelectedValue = ((int)p.ProductType).ToString();
                tbImageUrl.Text = p.ImageUrl;
                tbQrCodeUrl.Text = p.QrCodeUrl;
                tbMaterial.Text = p.Material;
                tbColour.Text = p.Colour;
                ddlSize.SelectedValue = p.Size.ToString();
                ddlCurrency.SelectedValue = p.Currency.ToString();
                tbPrice.Text = p.Price.ToString("f2");

                if (p.Sale.HasValue)
                    tbSale.Text = p.Sale.Value.ToString("f2");
                else
                    tbSale.Text = string.Empty;

                tbStock.Text = p.Stock.ToString();
                cbIsActive.Checked = p.IsActive;
                tbDescription.Text = p.Description;
                tbRemark.Text = p.Remark;

                // Only visible of ProductType.Other
                btnShowcase.Visible = p.ProductType == ProductType.Other;

                // Bind ProductOrder data of this Product
                BindItemData();
            }
            else
            {
                tbProductGuid.Text = Guid.NewGuid().ToString();
                gvProductOrder.Visible = false;
                btnShowcase.Visible = false;
            }
        }

        private void BindItemData()
        {
            var list = _repo.Query<OrderItem>(x => x.ProductGuid == ProductGuid);
            var query = _repo.All<Order>().FindAll(o => list.Exists(x => x.OrderID.Equals(o.ID)));

            gvProductOrder.DataSource = query.ToList();
            gvProductOrder.DataBind();

            #region set Control Custom Pager

            if (gvProductOrder.BottomPagerRow != null)
            {
                gvProductOrder.BottomPagerRow.Visible = true;
                ctrlCustomPagerInfo.Visible = true;

                ctrlCustomPagerInfo.PageIndex = gvProductOrder.PageIndex;
                ctrlCustomPagerInfo.PageCount = gvProductOrder.PageCount;
                ctrlCustomPagerInfo.RowCount = query.Count();
                ctrlCustomPagerInfo.InitComponent();
            }
            else
            {
                ctrlCustomPagerInfo.Visible = false;
            }

            #endregion
        }

        protected void gvProductOrder_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProductOrder.PageIndex = e.NewPageIndex;

            BindItemData();
        }

        protected void ctrlCustomPagerInfo_PageChanged(object sender, CustomPagerInfo.DataNavigatorEventArgs e)
        {
            if (e.PageIndex > 0)
            {
                gvProductOrder.PageIndex = e.PageIndex;
            }

            BindItemData();
        }

        protected void gvProductOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvProductOrder.SelectedIndex != -1)
            {
                var key = gvProductOrder.DataKeys[gvProductOrder.SelectedIndex];
                if (key != null)
                    Response.Redirect($"AdminOrderView.aspx?OrderID={key.Value}");
            }
        }

        protected void gvProductOrder_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var o = e.Row.DataItem as Order;

                var lblOrderStatus = e.Row.FindControl("lblOrderStatus") as Label;

                if (o != null && lblOrderStatus != null)
                {
                    lblOrderStatus.Text = o.Status.Equals(OrderStatusType.Confirmed) ? $"<em>{o.StatusInfo}</em>" : o.StatusInfo;
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                var p = new Product();

                if (!ProductGuid.Equals(Guid.Empty))
                {
                    p = _repo.Single<Product>(ProductGuid);
                }

                if (!string.IsNullOrEmpty(tbCode.Text.Trim()))
                    p.Code = tbCode.Text.Trim();
                else
                    throw new Exception("Product Code can't be empty");

                p.Name = tbName.Text.Trim();
                p.DisplayName = tbDisplayName.Text.Trim();
                p.ProductType = (ProductType)Enum.Parse(typeof(ProductType), ddlProductType.SelectedValue);
                p.ImageUrl = tbImageUrl.Text.Trim();
                p.QrCodeUrl = tbQrCodeUrl.Text.Trim();
                p.Material = tbMaterial.Text.Trim();
                p.Colour = tbColour.Text.Trim();

                if (!string.IsNullOrEmpty(ddlSize.SelectedValue))
                    p.Size = (ProductSizeType)Enum.Parse(typeof(ProductSizeType), ddlSize.SelectedValue);
                else
                    p.Size = ProductSizeType.None;

                p.Currency = (ProductCurrencyType)Enum.Parse(typeof(ProductCurrencyType), ddlCurrency.SelectedValue);
                p.Price = Convert.ToSingle(tbPrice.Text.Trim());

                if (!string.IsNullOrEmpty(tbSale.Text.Trim()))
                    p.Sale = Convert.ToSingle(tbSale.Text.Trim());
                else
                    p.Sale = null;

                p.CreateTime = DateTime.Now;
                p.Stock = Convert.ToInt32(tbStock.Text.Trim());
                p.IsActive = cbIsActive.Checked;
                p.Description = tbDescription.Text.Trim();
                p.Remark = tbRemark.Text.Trim();

                if (ProductGuid != Guid.Empty)
                {
                    _repo.Update(p);

                    Product.Cache.RefreshCache();

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed",
                        "alert('更新成功');window.location.href=window.location.href", true);
                }
                else
                {
                    if (_repo.All<Product>().Any(x => x.Code.ToLower().Equals(tbCode.Text.Trim().ToLower())))
                        throw new Exception("Product Code is already in use");

                    _repo.Insert(p);

                    Product.Cache.RefreshCache();

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed",
                        "alert('添加成功');window.location.href = 'AdminProduct.aspx'", true);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", $"alert('{ex.Message}')", true);
            }
        }

        protected void btnShowcase_Click(object sender, EventArgs e)
        {
            try
            {
                if (ProductGuid != Guid.Empty)
                {
                    var p = _repo.Single<Product>(ProductGuid);

                    p.Showcase();

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", $"alert('加入橱窗成功');window.location.href=window.location.href", true);

                }
                else
                {
                    throw new Exception("当前商品不存在");
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", $"alert('{ex.Message}')", true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (ProductGuid != Guid.Empty)
            {
                Response.Redirect("AdminProduct.aspx?ProductGuid=" + ProductGuid);
            }
            else
            {
                Response.Redirect("AdminProduct.aspx");
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (ProductGuid != Guid.Empty)
                {
                    _repo.Delete<Product>(ProductGuid);

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed",
                        "alert('删除成功');window.location.href='AdminProduct.aspx'", true);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", $"alert('{ex.Message}')", true);
            }
        }
    }
}
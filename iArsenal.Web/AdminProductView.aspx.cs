using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Linq;

using iArsenal.Entity;

namespace iArsenal.Web
{
    public partial class AdminProductView : PageBase.AdminPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = this.Username;
            ctrlCustomPagerInfo.PageChanged += new Control.CustomPagerInfo.PageChangedEventHandler(ctrlCustomPagerInfo_PageChanged);

            if (!IsPostBack)
            {
                InitForm();
            }
        }

        private Guid ProductGuid
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["ProductGuid"]))
                {
                    try { return new Guid(Request.QueryString["ProductGuid"]); }
                    catch { return Guid.Empty; }
                }
                else
                    return Guid.Empty;
            }
        }

        private void InitForm()
        {
            if (ProductGuid != Guid.Empty)
            {
                Product p = new Product();
                p.ProductGuid = ProductGuid;
                p.Select();

                tbProductGuid.Text = ProductGuid.ToString();
                tbCode.Text = p.Code;
                tbName.Text = p.Name;
                tbDisplayName.Text = p.DisplayName;
                ddlProductType.SelectedValue = ((int)p.ProductType).ToString();
                tbImageURL.Text = p.ImageURL;
                tbMaterial.Text = p.Material;
                tbColour.Text = p.Colour;

                if (p.Size == ProductSizeType.Null)
                    ddlSize.SelectedValue = string.Empty;
                else
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

                // Bind ProductOrder data of this Product
                BindItemData();
            }
            else
            {
                tbProductGuid.Text = Guid.NewGuid().ToString();
                gvProductOrder.Visible = false;
            }
        }

        private void BindItemData()
        {
            List<OrderItemBase> list = OrderItemBase.GetOrderItems().FindAll(oi => oi.ProductGuid.Equals(ProductGuid));
            List<OrderBase> oList = OrderBase.GetOrders().FindAll(o => list.Any(oi => oi.OrderID.Equals(o.OrderID)));

            gvProductOrder.DataSource = oList;
            gvProductOrder.DataBind();

            #region set Control Custom Pager
            if (gvProductOrder.BottomPagerRow != null)
            {
                gvProductOrder.BottomPagerRow.Visible = true;
                ctrlCustomPagerInfo.Visible = true;

                ctrlCustomPagerInfo.PageIndex = gvProductOrder.PageIndex;
                ctrlCustomPagerInfo.PageCount = gvProductOrder.PageCount;
                ctrlCustomPagerInfo.RowCount = list.Count;
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

        protected void ctrlCustomPagerInfo_PageChanged(object sender, Control.CustomPagerInfo.DataNavigatorEventArgs e)
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
                Response.Redirect(string.Format("AdminOrderView.aspx?OrderID={0}", gvProductOrder.DataKeys[gvProductOrder.SelectedIndex].Value.ToString()));
            }
        }

        protected void gvProductOrder_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string _strStatus = string.Empty;
                OrderBase o = e.Row.DataItem as OrderBase;

                Label lblOrderStatus = e.Row.FindControl("lblOrderStatus") as Label;

                if (lblOrderStatus != null)
                {
                    if (o.Status.Equals(OrderStatusType.Confirmed))
                        _strStatus = string.Format("<em>{0}</em>", o.StatusInfo);
                    else
                        _strStatus = o.StatusInfo;

                    lblOrderStatus.Text = _strStatus;
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Product p = new Product();

                if (!string.IsNullOrEmpty(tbCode.Text.Trim()))
                    p.Code = tbCode.Text.Trim();
                else
                    throw new Exception("Product Code can't be empty");

                p.Name = tbName.Text.Trim();
                p.DisplayName = tbDisplayName.Text.Trim();
                p.ProductType = (ProductType)Enum.Parse(typeof(ProductType), ddlProductType.SelectedValue);
                p.ImageURL = tbImageURL.Text.Trim();
                p.Material = tbMaterial.Text.Trim();
                p.Colour = tbColour.Text.Trim();

                if (!string.IsNullOrEmpty(ddlSize.SelectedValue))
                    p.Size = (ProductSizeType)Enum.Parse(typeof(ProductSizeType), ddlSize.SelectedValue);
                else
                    p.Size = ProductSizeType.Null;

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
                    p.ProductGuid = ProductGuid;
                    p.Update();
                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('更新成功');window.location.href=window.location.href", true);
                }
                else
                {
                    p.ProductGuid = new Guid(tbProductGuid.Text.Trim());

                    if (Product.GetProducts().FindIndex(delegate(Product tmpP) { return tmpP.Code.ToLower().Equals(tbCode.Text.Trim().ToLower()); }) >= 0)
                        throw new Exception("Product Code is already in use");

                    p.Insert();
                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('添加成功');window.location.href = 'AdminProduct.aspx'", true);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}')", ex.Message.ToString()), true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (ProductGuid != Guid.Empty)
            {
                Response.Redirect("AdminProduct.aspx?ProductGuid=" + ProductGuid.ToString());
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
                    Product p = new Product();
                    p.ProductGuid = ProductGuid;
                    p.Delete();

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('删除成功');window.location.href='AdminProduct.aspx'", true);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}')", ex.Message.ToString()), true);
            }
        }
    }
}
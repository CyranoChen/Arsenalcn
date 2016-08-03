using System;
using System.Web.UI.WebControls;
using iArsenal.Service;

namespace iArsenal.Web
{
    public partial class iArsenalOrder_Showcase : AcnPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //ctrlCustomPagerInfo.PageChanged += ctrlCustomPagerInfo_PageChanged;

            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            try
            {
                var showcases = Showcase.Cache.ShowcaseList.FindAll(x => x.IsActive);

                var list = Product.Cache.ProductList.FindAll(x => x.IsActive && x.ProductType == ProductType.Other &&
                    showcases.Exists(s => s.ProductGuid == x.ID));

                rptShowcase.DataSource = list;
                rptShowcase.DataBind();

                //#region set Control Custom Pager

                //if (gvMatch.BottomPagerRow != null)
                //{
                //    gvMatch.BottomPagerRow.Visible = true;
                //    ctrlCustomPagerInfo.Visible = true;

                //    ctrlCustomPagerInfo.PageIndex = gvMatch.PageIndex;
                //    ctrlCustomPagerInfo.PageCount = gvMatch.PageCount;
                //    ctrlCustomPagerInfo.RowCount = list.Count;
                //    ctrlCustomPagerInfo.InitComponent();
                //}
                //else
                //{
                //    ctrlCustomPagerInfo.Visible = false;
                //}

                //#endregion
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", $"alert('{ex.Message}')", true);
            }
        }

        protected void rptShowcase_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                var p = e.Item.DataItem as Product;

                var imgItemThumbnail = e.Item.FindControl("imgItemThumbnail") as Image;
                var ltrlProductName = e.Item.FindControl("ltrlProductName") as Literal;
                var ltrlProductInfo = e.Item.FindControl("ltrlProductInfo") as Literal;
                var ltrlProductPrice = e.Item.FindControl("ltrlProductPrice") as Literal;

                if (p != null && imgItemThumbnail != null && ltrlProductName != null && ltrlProductInfo != null &&
                    ltrlProductPrice != null)
                {
                    imgItemThumbnail.ImageUrl = Request.ApplicationPath + p.ImageUrl;
                    imgItemThumbnail.AlternateText = p.Code;

                    ltrlProductName.Text = $"<h3>{p.Name}</h3>";

                    ltrlProductInfo.Text = $"<h3><em>【{p.Code}】</em>{p.DisplayName}</h3>";

                    ltrlProductPrice.Text = p.Sale.HasValue ?
                        $"<p><em>{p.SaleInfo}</em><span style=\"text-decoration: line-through\">({p.PriceInfo})</span></p>"
                        : $"<p>{p.PriceInfo}<p>";
                }
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbTeamName.Text.Trim()))
                ViewState["TeamName"] = tbTeamName.Text.Trim();
            else
                ViewState["TeamName"] = string.Empty;


            BindData();
        }
    }
}
using System;

using iArsenal.Entity;

namespace iArsenal.Web
{
    public partial class AdminOrderItemView : PageBase.AdminPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = this.Username;

            if (!IsPostBack)
            {
                InitForm();
            }
        }

        private int OrderItemID
        {
            get
            {
                int _orderItemID;
                if (!string.IsNullOrEmpty(Request.QueryString["OrderItemID"]) && int.TryParse(Request.QueryString["OrderItemID"], out _orderItemID))
                {
                    return _orderItemID;
                }
                else
                    return int.MinValue;
            }
        }

        private void InitForm()
        {
            if (OrderItemID > 0)
            {
                OrderItemBase oi = new OrderItemBase();
                oi.OrderItemID = OrderItemID;
                oi.Select();

                lblOrderItemInfo.Text = string.Format("更新会员的许愿单 ID:<em>{0}</em>", OrderItemID.ToString());

                tbMemberID.Text = oi.MemberID.ToString();
                tbMemberName.Text = oi.MemberName;
                tbCreateTime.Text = oi.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");

                // Set Visible for the order back button for this item
                if (oi.OrderID > 0)
                    btnBackOrder.Visible = true;

                tbOrderID.Text = oi.OrderID.ToString();

                cbIsActive.Checked = oi.IsActive;
                tbCode.Text = oi.Code;
                tbProductGuid.Text = oi.ProductGuid.ToString();
                tbProductName.Text = oi.ProductName;
                tbSize.Text = oi.Size;
                tbUnitPrice.Text = oi.UnitPrice.ToString("f2");
                tbQuantity.Text = oi.Quantity.ToString("f0");

                if (oi.Sale.HasValue)
                    tbSale.Text = oi.Sale.Value.ToString("f2");
                else
                    tbSale.Text = string.Empty;

                if (oi.UnitPrice > 0 && oi.Quantity > 0)
                    lblPrice.Text = (oi.UnitPrice * oi.Quantity).ToString("f2");
                else
                    lblPrice.Text = "?";

                tbRemark.Text = oi.Remark;
            }
            else
            {
                lblOrderItemInfo.Text = "添加会员的许愿单";
                tbCreateTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                OrderItemBase oi = new OrderItemBase();

                if (!string.IsNullOrEmpty(tbMemberID.Text.Trim()))
                    oi.MemberID = Convert.ToInt32(tbMemberID.Text.Trim());
                else
                    throw new Exception("Member ID can't be empty");

                oi.MemberName = tbMemberName.Text.Trim();
                oi.CreateTime = DateTime.Parse(tbCreateTime.Text.Trim());
                oi.OrderID = Convert.ToInt32(tbOrderID.Text.Trim());
                oi.IsActive = cbIsActive.Checked;

                if (!string.IsNullOrEmpty(tbCode.Text.Trim()))
                    oi.Code = tbCode.Text.Trim();
                else
                    throw new Exception("OrderItem Code can't be empty");

                oi.ProductGuid = new Guid(tbProductGuid.Text.Trim());
                oi.ProductName = tbProductName.Text.Trim();
                oi.Size = tbSize.Text.Trim();
                oi.UnitPrice = Convert.ToSingle(tbUnitPrice.Text.Trim());
                oi.Quantity = Convert.ToInt32(tbQuantity.Text.Trim());

                if (!string.IsNullOrEmpty(tbSale.Text.Trim()))
                    oi.Sale = Convert.ToSingle(tbSale.Text.Trim());
                else
                    oi.Sale = null;

                oi.Remark = tbRemark.Text.Trim();

                if (OrderItemID > 0)
                {
                    oi.OrderItemID = OrderItemID;
                    oi.Update();
                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('更新成功');window.location.href=window.location.href", true);
                }
                else
                {
                    oi.Insert();
                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('添加成功');window.location.href = 'AdminOrderItem.aspx'", true);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}')", ex.Message.ToString()), true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (OrderItemID > 0)
            {
                Response.Redirect("AdminOrderItem.aspx?OrderItemID=" + OrderItemID.ToString());
            }
            else
            {
                Response.Redirect("AdminOrderItem.aspx");
            }
        }

        protected void btnBackOrder_Click(object sender, EventArgs e)
        {
            OrderItemBase oi = new OrderItemBase();
            oi.OrderItemID = OrderItemID;
            oi.Select();

            if (oi.OrderID > 0)
                Response.Redirect("AdminOrderView.aspx?OrderID=" + oi.OrderID.ToString());
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (OrderItemID > 0)
                {
                    OrderItemBase oi = new OrderItemBase();
                    oi.OrderItemID = OrderItemID;
                    oi.Delete();

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('删除成功');window.location.href='AdminOrderItem.aspx'", true);
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